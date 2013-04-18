using UnityEngine;
using System.Collections;

public class Pather{		//Pather is the class that helps me find the path!
	
	Transform[] targets;
	Transform[] nodes;
	int curTarget;
	Transform prevTarget;
	readonly Transform prey;
	readonly Transform predator;
	Vector3 lastKnownLocation;
	
	bool onToTheNextOne;
	bool hunting = true;
	int timesFailed = 0;
	int maxFailures = 9;
	int closeDistance = 20;
	int maxDist = 5000;
	
	int timer = 0;
	readonly int timing = 60;
	
	// Use this for initialization
	/// <summary>
	/// Initializes a new instance of the <see cref="Pather"/> class.
	/// </summary>
	/// <param name='predator'>
	/// Predator.
	/// </param>
	/// <param name='prey'>
	/// Prey.
	/// </param>
	public Pather(Transform predator, Transform prey){
		this.predator = predator;
		this.prey = prey;
		int counter = 0;
		GameObject[] Nodes = GameObject.FindGameObjectsWithTag("node");		//How many nodes are there? Make a Transform[] with the same length
		nodes = new Transform[Nodes.GetLength(0)];
		foreach (GameObject go in Nodes){
			nodes[counter] = go.transform;
			Debug.Log(go);
			counter ++;
		}
		targets = new Transform[nodes.GetLength(0)];
		Debug.Log("all the targets: " + nodes.GetLength(0));
	}
	/// <summary>
	/// Seek the specified target.
	/// </summary>
	/// <param name='target'>
	/// Target.
	/// </param>
	public Transform Seek(Transform target){
		timer ++;
		
		if (timer < timing){		//It's not TIME yet!
			if (Vector3.Distance(target.position, predator.position) < closeDistance && target != prey){
				onToTheNextOne = true;
				Debug.Log("I'm here now, going to find the next closest");
				
			}
			return target;
			
		}
		
		//If I'm in one of my checking frames...
		
		timer = 0;
		if (targets[0] == null){		//if this is my first time, my target is going to be the one given in the argument
			Debug.Log("My first target!");
			targets[0] = target;
			return targets[0];			//you know, to make sure that targets[0] is my first one
		}
		
		int mask = 1 << LayerMask.NameToLayer("collision");		//Cast a line to see if there's a collision in the way of my target
		RaycastHit hit;
		
		//first update to see if I can see my target now
		if (!Physics.Linecast(predator.position, prey.position, out hit, mask)){
			Debug.Log("There he is! I see him again!");
			lastKnownLocation = prey.position;
			timesFailed = 0;
			curTarget = 0;
			return prey;
		}
		
		
		if (Physics.Linecast(predator.position, target.position, out hit, mask)){
			Debug.Log("There's a collision in the way!" + hit.point);
			
			timesFailed ++;				//jump to 'timesFailed >= maxFailures'
			Debug.Log("fail! Add one to failures. Timestamp : " + Time.time.ToString());
		}
		else{
			Debug.Log("Found my current target");
			if (onToTheNextOne){
				timesFailed ++;
				onToTheNextOne = false;
				Debug.Log("I'm here now, going to find the next closest");
	
			}
			else return target;
		}
		
		if (timesFailed >= maxFailures){	//if I've failed too much, I'm no longer hunting
			hunting = false;
			Debug.Log("Failed too much... ;_;. Timestamp : " + Time.time.ToString());
		}
		
		if (!hunting){		//If I'm no longer hunting then I go back.
			curTarget --;
			Debug.Log("Not hunting, so just gonna go find sommat else");
			if (curTarget <= 0){
				return null;
			}
			return targets[Mathf.Max(curTarget, 0)];
		}
		
		curTarget ++;
		Debug.Log("Looking for a new target, I guess....  CurTarget = " + curTarget.ToString());
		//find a new node
		float shortestLength = maxDist;
		int shortestIndex = 0;
		int counter = -1;
		foreach (Transform node in nodes){
			counter ++;
			bool found = false;
			foreach(Transform t in targets){
				if (t == node){
					found = true;
					break;
				}
				Debug.Log("Seen him already");
			}
			if (found){
			
				continue;
			}
			else if(Physics.Linecast(predator.position, node.position, out hit, mask)){
				Debug.Log("no thanks there's something in the way");
				continue;
			}
			float dist = Vector3.Distance(lastKnownLocation, node.position);
			if (dist < shortestLength){
				shortestLength = dist;
				shortestIndex = counter;
				Debug.Log("This guy's not far: " + node.gameObject.name);
			}
		}
		if (shortestLength >= maxDist){ //great, no nodes are in range. I'll go back home then
			hunting = false;
			curTarget -= 2;
			Mathf.Max(curTarget, 0);
		}
		else{
			targets[Mathf.Max(curTarget, 0)] = nodes[shortestIndex];
			Debug.Log("New shortest! Yay!");
		}
		
		//is there a node nearer to the one I have?
		
		
		//is there something in the way of THAT node?
		return targets[curTarget];
	}
	
		
	
}

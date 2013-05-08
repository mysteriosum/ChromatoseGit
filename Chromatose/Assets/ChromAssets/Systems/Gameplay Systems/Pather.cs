using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	
	class Node{
		public int f;
		public int g = 0;
		public int h = 0;
		public Node parent;
		public readonly Transform t;
		public readonly Vector2 pos;
		public Node(Transform t, Node parent){
			this.t = t;
			this.parent = parent;
			pos = t.position;
		}
		
		public override string ToString(){
			return t.name + " at position " + pos.ToString();
		}
	}
	
	
	float timer = 0;
	readonly float timing = 0.5f;
	
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
			//Debug.Log(go);
			counter ++;
		}
		int nodesNumber = nodes.GetLength(0);
		targets = new Transform[nodesNumber];
		if (nodesNumber - 1 > maxFailures)
			maxFailures = nodesNumber - 1;
		//Debug.Log("all the targets: " + nodes.GetLength(0));
	}
	/// <summary>
	/// Seek the specified target.
	/// </summary>
	/// <param name='target'>
	/// Target.
	/// </param>
	public Transform Seek(Transform target){
		timer += Time.deltaTime;
		
		if (timer < timing){		//It's not TIME yet!
			if (Vector3.Distance(target.position, predator.position) < closeDistance && target != prey){
				onToTheNextOne = true;
				//Debug.Log("I'm here now, going to find the next closest");
				
			}
			return target;
			
		}
		
		//If I'm in one of my checking frames...
		
		timer = 0;
		if (targets[0] == null){		//if this is my first time, my target is going to be the one given in the argument
			//Debug.Log("My first target!");
			targets[0] = target;
			return targets[0];			//you know, to make sure that targets[0] is my first one
		}
		
		int mask = 1 << LayerMask.NameToLayer("collision");		//Cast a line to see if there's a collision in the way of my target
		RaycastHit hit;
		
		//first update to see if I can see my target now
		if (!Physics.Linecast(predator.position, prey.position, out hit, mask)){
			//Debug.Log("There he is! I see him again!");
			lastKnownLocation = prey.position;
			timesFailed = 0;
			curTarget = 0;
			return prey;
		}
		
		
		if (Physics.Linecast(predator.position, target.position, out hit, mask)){
			//Debug.Log("There's a collision in the way!" + hit.point);
			
			timesFailed ++;				//jump to 'timesFailed >= maxFailures'
			//Debug.Log("fail! Add one to failures. Timestamp : " + Time.time.ToString());
		}
		else{
			Debug.Log("Found my current target");
			if (onToTheNextOne){
				timesFailed ++;
				onToTheNextOne = false;
				//Debug.Log("I'm here now, going to find the next closest");
	
			}
			else return target;
		}
		
		if (timesFailed >= maxFailures){	//if I've failed too much, I'm no longer hunting
			hunting = false;
			//Debug.Log("Failed too much... ;_;. Timestamp : " + Time.time.ToString());
		}
		
		if (!hunting){		//If I'm no longer hunting then I go back.
			curTarget --;
			//Debug.Log("Not hunting, so just gonna go find sommat else");
			if (curTarget <= 0){
				return null;
			}
			return targets[Mathf.Max(curTarget, 0)];
		}
		
		curTarget ++;
		//Debug.Log("Looking for a new target, I guess....  CurTarget = " + curTarget.ToString());
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
			//	Debug.Log("Seen him already");
			}
			if (found){
			
				continue;
			}
			else if(Physics.Linecast(predator.position, node.position, out hit, mask)){
				//Debug.Log("no thanks there's something in the way");
				continue;
			}
			float dist = Vector3.Distance(lastKnownLocation, node.position);
			if (dist < shortestLength){
				shortestLength = dist;
				shortestIndex = counter;
				//Debug.Log("This guy's not far: " + node.gameObject.name);
			}
		}
		if (shortestLength >= maxDist){ //great, no nodes are in range. I'll go back home then
			hunting = false;
			curTarget -= 2;
			Mathf.Max(curTarget, 0);
		}
		else{
			targets[Mathf.Max(curTarget, 0)] = nodes[shortestIndex];
			//Debug.Log("New shortest! Yay!");
		}
		
		//is there a node nearer to the one I have?
		
		
		//is there something in the way of THAT node?
		return targets[curTarget];
	}
	
	public List<Transform> NewPath(Transform begin, Transform end){
	init:
		List<Node> openList = new List<Node>();
		List<Node> closedList = new List<Node>();
		List<Node> toAdd = new List<Node>();
		List<Transform> thePath = new List<Transform>();
		int smallestF;
		int mask = 1 << LayerMask.NameToLayer("collision");		//for teh linecasts
		RaycastHit hit;
		bool foundEnd = false;
		Node firstNode = new Node(begin, null);
		openList.Add(firstNode);
		Node underScrutiny = firstNode;
		int counter = 0;
		foreach (Transform t in nodes){			//Start by getting all the nodes I can see from the start
			if (!Physics.Linecast(t.position, (Vector3)underScrutiny.pos, out hit, mask)){
				Node newNode = new Node(t, underScrutiny);
				openList.Add(newNode);
				//Debug.Log(newNode);
			}
		}
		openList.Remove(underScrutiny);
		closedList.Add(underScrutiny);
	open:					//Here I find my lowest F score among things in the Open list
		smallestF = 10000000;
		//Debug.Log("There's still Node 24 in the openList : " + openList.Contains(underScrutiny) + " and as proof: " + underScrutiny);
		
		foreach (Node n in openList){
			n.g = gScore(n.pos, n.parent);
			n.h = hScore(n.pos, (Vector2)end.position);
			n.f = n.g + n.h;
			if (n.f < smallestF){
				smallestF = n.f;
				underScrutiny = n;
				//Debug.Log("Now under scrutiny: " + n);
			}
		}
		//Debug.Log("There has been " + underScrutiny + " in the openList : " + openList.Contains(underScrutiny));
		openList.Remove(underScrutiny);		//Move it from the open to the closed list
		closedList.Add(underScrutiny);
		//Debug.Log("There's still " + underScrutiny + " in the openList : " + openList.Contains(underScrutiny));
		
		foreach (Transform t in nodes){		//Check what nodes I can see from the current "UnderScrutiny"
			if (t == underScrutiny.t) continue;
			foreach (Node cn in closedList){
				if (cn.t == t){			//Nix it if it's on the closed list
					//Debug.Log(cn + " On closed list.");
					continue;
				}
			}
			if (!Physics.Linecast(t.position, (Vector3)underScrutiny.pos, out hit, mask)){
				//Debug.Log("There's " + openList.Count);
				bool found = false;
				foreach (Node on in openList){
					if (on.t == t){		//If it's on the open list, see if this is better
						//Debug.Log("Found " + on + " in the open list");
						if (gScore((Vector2)t.position, underScrutiny) < on.g){
							on.parent = underScrutiny;
							on.g = gScore(on.pos, underScrutiny);
							on.f = on.h + on.g;
							//Debug.Log("Turns out it's shorter");
							
						}
						else{
							//Debug.Log("It's not shorter!");
						}
						found = true;
						continue;
					}
				}
				if (found) continue;
				
				Node newNode = new Node(t, underScrutiny);	//if the t is not already a node, make a new one
				//Debug.Log("Ought to be adding a new node..." + newNode);
				openList.Add(newNode);
				if (t == end){		//if I can see the end node, start to wrap it up
					foundEnd = true;
					underScrutiny = newNode;
				}
			}
			else{
				//Debug.Log("Between " + t.name + " and " + underScrutiny + " there's " + hit.transform.name);
			}
		}
		
		if (!foundEnd){
			counter ++;
			if (counter < 150)
			goto open;
		}
		counter = 0;
		do {
			counter ++;
			if (counter > nodes.Length){
				//Debug.Log("I think the path is too long :S");
				break;
			}
			thePath.Add(underScrutiny.t);
			underScrutiny = underScrutiny.parent;
		}
		while(underScrutiny != firstNode);
		
		thePath.Reverse();
		//Debug.Log(underScrutiny + " and I did this " + counter + " times. There are " + thePath.Count + " nodes in the path");
		Debug.Log("Path is " + thePath.Count + " long");
		
		return thePath;
		
	}
	
	int hScore(Vector2 here, Vector2 endPlace){
		return (int)Mathf.Abs(here.x - endPlace.x) + (int)Mathf.Abs(here.y - endPlace.y);
	}
	int gScore(Vector2 here, Node parent){
		return parent.g + (int)(here - parent.pos).magnitude;
	}
}

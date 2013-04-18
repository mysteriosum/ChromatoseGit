using UnityEngine;
using System.Collections;

public class Npc : ColourBeing {
	
	Movement movement;
	Transform t;
	Transform target;
	Transform closestNode;
	Transform avatar;
	Destructible toDestroy;
	Buildable toBuild;
	
	
	bool losingColour = false;
	bool inMotion = false;
	bool building = false;
	bool breaking = false;
	int anarchyConsiderMin = 200;
	
	ColourBeing.Colour initColour;
	Pather myPather;
	
	public int detectRadius = 250;
	
	int closeRadius = 100;
	float checkTimer = 0;
	float checkTiming = 0.8f;
	
	// Use this for initialization
	void Start () {
		movement = GetComponent<Movement>();
		t = transform;
		if (movement.target){
			target = movement.target;
			
		}
		avatar = GameObject.FindGameObjectWithTag("avatar").transform;
		if (!target && colour.b > colourConsiderMin){
			target = avatar;
			
			
		}
		
		initColour = new ColourBeing.Colour();
		initColour.r = colour.r;
		initColour.g = colour.g;
		initColour.b = colour.b;
		
		Debug.Log("my init red " + initColour.r);
	}
	
	// Update is called once per frame
	void Update () {
		
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<---------------SETUP SECTION!--------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
		
		//.....................................................................tumbleweed
		if (building){
			if (toBuild){
				goto update;
			}
			else{
				building = false;
				
			}
			
		}
		
		if (breaking){
			if (toDestroy){
				goto update;
			}
			else{
				breaking = false;
			}
		}
		
		
		
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<----------------BLUE SECTION!--------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
	blue:
		if (colour.b > colourConsiderMin){
			
			if (!target){
				target = avatar;
			}
			
			Vector2 diff = (Vector2) target.position - (Vector2)t.position;
			
			if (target == avatar){
				if (diff.magnitude < detectRadius && CheckSameColour(target.GetComponent<ColourBeing>().colour)){
					inMotion = true;
					
				}
			}
			checkTimer += Time.deltaTime;
			if (checkTimer >= checkTiming){		//check for node every [checktiming] seconds
					
				checkTimer = 0;
				toBuild = FindClosestOfTag(t, "buildable").GetComponent<Buildable>();
				
				if (toBuild){
					Debug.Log("My target's name is " + toBuild.name);
					closestNode = FindClosestOfTag(t, "node", closeRadius);
					if (!closestNode) goto move;
					target = closestNode;
				}
			}
			
			if (target == closestNode){
				if (diff.magnitude < closeRadius){
					toBuild.AddOne();
					inMotion = false;
					building = true;
					target = null;
					closestNode = null;
					
				}
			}
				
		}//last bracket of 'if blue'
		
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<----------------RED SECTION!---------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	red:
		if (colour.r > anarchyConsiderMin){		//If I'm red enough to consider fighting
			if (!target){  //find a target, and the nearest one, ideally
				GameObject[] potentials = GameObject.FindGameObjectsWithTag("destructible");
				float closestDist = Mathf.Infinity;
				if (colour.b > colourConsiderMin){
					closestDist = detectRadius;
				}
				Transform closest = null;
				Debug.Log("Amount of destructibles = " + potentials);
				foreach (GameObject p in potentials){
					Vector2 tempDist = (Vector2)p.transform.position - (Vector2)t.position;
					if (tempDist.magnitude < closestDist && p.GetComponent<Destructible>() != null){
						closestDist = tempDist.magnitude;
						closest = p.transform;
					}
				}
				if (closest){		//There's a destructible thing, I'ma find the closest Node to that
					Debug.Log("Closest guy is " + closest.name);
					toDestroy = closest.GetComponent<Destructible>();
					closestNode = toDestroy.myNode;
					
					myPather = new Pather(t, closestNode);
					
					inMotion = true;
					target = closestNode;
				}
			}
			else{
				target = myPather.Seek(target);		//I have a target, so I'm going to look for it!
				float distToClosest = ((Vector2)closestNode.position - (Vector2)t.position).magnitude;
				if (distToClosest < closeRadius){
					toDestroy.AddOne();
					target = null;
					closestNode = null;
					losingColour = true;
					inMotion = false;
					breaking = true;
					colour.r = initColour.r;
					Debug.Log("I have a new red! It's " + colour.r);
				}
			}
			
			
		}
		else if (colour.r > 0){ //Absorb colour from Avatar. Don't want to do it if I have NO colour, just... little colour.
			Vector2 diff = (Vector2) avatar.position - (Vector2)t.position;
			if (diff.magnitude < detectRadius && CheckSameColour(avatar.GetComponent<ColourBeing>().colour)){
				colour.r = 255;
				
			}
		}
	move:
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<----------------MOVE SECTION!--------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
		if (inMotion && target){
			movement.target = target;
			
			Vector2 diff = (Vector2) target.position - (Vector2)t.position;
		
			float angle = VectorFunctions.PointDirection(diff);
			
			float zGoodFormat = VectorFunctions.Convert360(transform.rotation.eulerAngles.z);
			
			t.Rotate(0, 0, angle - zGoodFormat);
			Vector2 disp = movement.Displace(true);
			
			transform.position += new Vector3(disp.x, disp.y, 0);
		}
		else{
			movement.SlowToStop();
		}
		
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<--------------UPDATE SECTION!--------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	update:
		if (losingColour){
			colour.r = colour.r > initColour.r? colour.r - 1 : colour.r;
		}
		//end :)
	}
	
	/// <summary>
	/// Finds the closest object with the specified tag.
	/// </summary>
	/// <returns>
	/// The closest of tag.
	/// </returns>
	/// <param name='mainTarget'>
	/// Main target.
	/// </param>
	/// <param name='tag'>
	/// Tag.
	/// </param>
	/// <param name='maxDistance'>
	/// Max distance.
	/// </param>
	Transform FindClosestOfTag(Transform mainTarget, string tag, int maxDistance){
		GameObject[] nodes = GameObject.FindGameObjectsWithTag(tag);
		float closestDist = maxDistance;
		Transform closestHere = null;
		foreach (GameObject n in nodes){
			float tempDist = ((Vector2) mainTarget.position - (Vector2)n.transform.position).magnitude;
			
			if (tempDist < closestDist){
				closestDist = tempDist;
				closestHere = n.transform;
			}
		}
		
		return closestHere;
		
	}
	Transform FindClosestOfTag(Transform mainTarget, string tag){
		return FindClosestOfTag(mainTarget, tag, 10000000);
	}
	
	override public void Trigger(){
		
	}
}

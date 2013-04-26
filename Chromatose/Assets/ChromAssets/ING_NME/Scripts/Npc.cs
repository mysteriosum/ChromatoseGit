using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Movement))]
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
	
	bool stoppingForever = false;
	bool addingGreen;
	bool waitingForMaxGreen;
	
	
	ColourBeing.Colour initColour;
	Pather myPather;
	int currentNode = 0;
	
	public int detectRadius = 250;
	
	int closeRadius = 100;
	int mask;
	RaycastHit hit;
	float checkTimer = 0;
	float checkTiming = 0.8f;
	List<Transform> myPath;
	Avatar avatarScript;
	
	// Use this for initialization
	void Start () {
		
		mask = 1 << LayerMask.NameToLayer("collision");		//for teh linecasts
		movement = GetComponent<Movement>();
		t = transform;
		spriteInfo = GetComponent<tk2dSprite>();
		if (movement.target){
			target = movement.target;
			
		}
		avatar = GameObject.FindGameObjectWithTag("avatar").transform;
		if (!target && colour.b > colourConsiderMin){
			target = avatar;
			
			
		}
		if (avatar){
			avatarScript = avatar.GetComponent<Avatar>();
		}
		else{
			Debug.Log("Can't find the Avatar! He ain't here I tells ya!");
		}
		
		initColour = new ColourBeing.Colour(colour.r, colour.g, colour.b);
		
		shownColour = new Color((float)initColour.r/255f, (float)initColour.g/255f, (float)initColour.b/255f, spriteInfo.color.a);
	}
	
	// Update is called once per frame
	void Update () {
		
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<---------------SETUP SECTION!--------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
		
		//.....................................................................tumbleweed
		
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<--------------CHECKS SECTION!--------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
		
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
				losingColour = true;
			}
		}
		
		if (stoppingForever){
			goto move;
		}
		
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<----------------BLUE SECTION!--------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
	blue:
		if (colour.b > colourConsiderMin){
			if (colour.r > anarchyConsiderMin) goto red;
			if (!target){
				target = avatar;
			}
			
			Vector2 diff = (Vector2) target.position - (Vector2)t.position;
			
			if (target == avatar){
				
				if (diff.magnitude < detectRadius && avatar.GetComponent<Avatar>().colour.Blue && !inMotion){
					inMotion = true;
					//Debug.Log("Should be in motion");
				}
				else if(inMotion){
					inMotion = false;
				}
			}
			checkTimer += Time.deltaTime;
			if (checkTimer >= checkTiming){		//check for node every [checktiming] seconds
					
				checkTimer = 0;
				Transform closestBuildable = FindClosestOfTag(t, "buildable");
				if (!closestBuildable) goto red;
				toBuild = closestBuildable.GetComponent<Buildable>();
				 
				if (toBuild){
					/*
					closestNode = FindClosestOfTag(t, "node", closeRadius);
					if (!closestNode) goto move;
					target = closestNode;*/
					closestNode = toBuild.myNode;
					if ((closestNode.position - t.position).magnitude < closeRadius){
						target = closestNode;
					}
				}
			}
			
			if (target == closestNode){
				if (diff.magnitude < closeRadius){
					//Debug.Log("Now at build node");
					bool isValid = toBuild.AddOne(this);
					if (!isValid){
						target = avatar;
						goto red;
					}
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
			
			if (target == null || target == avatar){  //find a target, and the nearest one, ideally
				GameObject[] potentials = GameObject.FindGameObjectsWithTag("destructible");
				float closestDist = Mathf.Infinity;
				/*if (colour.b > colourConsiderMin){
					closestDist = detectRadius;
				}*/
				Transform closest = null;
				//Debug.Log("Amount of destructibles = " + potentials.Length);
				foreach (GameObject p in potentials){
					Vector2 tempDist = (Vector2)p.transform.position - (Vector2)t.position;
					Destructible pScript = p.GetComponent<Destructible>();
					if (tempDist.magnitude < closestDist && pScript != null){
						if (!pScript.CheckName(name)) continue;				//If the Destructible is picky, it might not want you
						closestDist = tempDist.magnitude;
						closest = p.transform;
					}
				}
				if (closest){		//There's a destructible thing, I'ma find the closest Node to that
					//Debug.Log("Closest guy is " + closest.name);
					toDestroy = closest.GetComponent<Destructible>();
					closestNode = toDestroy.myNode;
					
					myPather = new Pather(t, closestNode);
					myPath = myPather.NewPath(t, closestNode);
					inMotion = true;
					target = myPath[0];
				}
			}
			else if (target){
				//Debug.Log("I should have a target...");
				//0target = myPather.Seek(target);		//I have a target, so I'm going to look for it!
				float distToTarget = ((Vector2)target.position - (Vector2)t.position).magnitude;
				if (distToTarget < closeRadius){
					if (target == closestNode){
						toDestroy.AddOne(this);
						target = null;
						closestNode = null;
						inMotion = false;
						breaking = true;
						colour.r = initColour.r;
					}
					else{
						currentNode ++;
						target = myPath[currentNode];
					}
				}
			}
			
			
		}
		else if (colour.r > 0){ //Absorb colour from Avatar. Don't want to do it if I have NO colour, just... little colour.
			Vector2 diff = (Vector2) avatar.position - (Vector2)t.position;
			if (diff.magnitude < detectRadius && avatarScript.colour.Red && !Physics.Linecast(avatar.position, t.position, out hit, mask)){
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
			if (stoppingForever){
				inMotion = false;
				goto update;
			}
			else{
				//Debug.Log("Transform!");
				transform.position += new Vector3(disp.x, disp.y, 0);
			}
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
			shownColour.r -= 1f/255f;
			
			if ((float)colour.r /255f > (float)shownColour.r + 2f/255f){
				losingColour = false;
			}
		}
		if ((float)colour.r /255f > (float)shownColour.r + 2f/255f){
			shownColour.r += 1f/255f;
		}
		
		if (addingGreen){
			Debug.Log("Adding green!");
			colour.g ++;
			shownColour.g += 1f/255f;
			if (colour.g > 254){
				addingGreen = false;
				waitingForMaxGreen = false;
			}
		}
		
		if (stoppingForever){
			//movement.SlowToStop();
			if (movement.GetVelocity().magnitude < 5 && !waitingForMaxGreen){
				this.enabled = false;
			}
		}
		
		
		
		spriteInfo.color = shownColour;
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
	
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<============SPECIFIC FUNCTIONS!!============>
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	public void StopAndDisable(){
		stoppingForever = true;
		Debug.Log("Rawr, stop and disable!");
	}
	
	public void AddGreenUntilMax(){
		waitingForMaxGreen = true;
		addingGreen = true;
	}
	
	public void SetNewTarget(Transform newTarget){
		target = newTarget;
		
	}
	
	Transform FindClosestOfTag(Transform mainTarget, string tag){
		return FindClosestOfTag(mainTarget, tag, 10000000);
	}
	
	public void MaxRed(){
		colour.r = 255;
	}
	
	override public void Trigger(){
		
	}
}

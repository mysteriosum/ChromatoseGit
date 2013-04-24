using UnityEngine;
using System.Collections;
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
	
	public int detectRadius = 250;
	
	int closeRadius = 100;
	float checkTimer = 0;
	float checkTiming = 0.8f;
	
	// Use this for initialization
	void Start () {
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
			if (!avatar){
				Debug.Log("Can't find the Avatar! He ain't here I tells ya!");
			}
		
		initColour = new ColourBeing.Colour(colour.r, colour.g, colour.b);
		
		shownColour = new Color((float)initColour.r/255f, (float)initColour.g/255f, (float)initColour.b/255f, 1f);
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
			
			if (!target){
				target = avatar;
			}
			Vector2 diff = (Vector2) target.position - (Vector2)t.position;
			
			if (target == avatar){
				if (diff.magnitude < detectRadius && avatar.GetComponent<Avatar>().CheckIsBlue() && !inMotion){
					inMotion = true;
					
				}
				else if(inMotion){
					inMotion = false;
				}
			}
			checkTimer += Time.deltaTime;
			if (checkTimer >= checkTiming){		//check for node every [checktiming] seconds
					
				checkTimer = 0;
				toBuild = FindClosestOfTag(t, "buildable").GetComponent<Buildable>();
				 
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
			if (target != null){  //find a target, and the nearest one, ideally
				GameObject[] potentials = GameObject.FindGameObjectsWithTag("destructible");
				float closestDist = Mathf.Infinity;
				if (colour.b > colourConsiderMin){
					closestDist = detectRadius;
				}
				Transform closest = null;
				Debug.Log("Amount of destructibles = " + potentials.Length);
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
					Debug.Log("Closest guy is " + closest.name);
					toDestroy = closest.GetComponent<Destructible>();
					closestNode = toDestroy.myNode;
					
					myPather = new Pather(t, closestNode);
					
					inMotion = true;
					target = closestNode;
				}
			}
			else if (target){
				Debug.Log("I should have a target...");
				target = myPather.Seek(target);		//I have a target, so I'm going to look for it!
				float distToClosest = ((Vector2)closestNode.position - (Vector2)t.position).magnitude;
				if (distToClosest < closeRadius){
					toDestroy.AddOne(this);
					target = null;
					closestNode = null;
					inMotion = false;
					breaking = true;
					colour.r = initColour.r;
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
	
	override public void Trigger(){
		
	}
}

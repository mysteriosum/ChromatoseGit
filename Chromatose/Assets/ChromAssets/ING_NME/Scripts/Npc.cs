using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Movement))]
public class Npc : ColourBeing {
	
	private Movement movement;
	private Transform t;
	private Transform target;
	private Transform closestNode;
	private Transform avatar;
	private Destructible toDestroy;
	private Buildable toBuild;
	
	
	private bool losingColour = false;
	private bool inMotion = false;
	
	private bool building = false;
	private bool breaking = false;
	private int anarchyConsiderMin = 200;
	
	//for the cyan guy
	private bool stoppingForever = false;
	private bool addingGreen;
	private bool waitingForMaxGreen;
	private bool saidOK = false;
	private bool saidFollow = false;
	
	
	private ColourBeing.Colour initColour;
	private Pather myPather;
	private int currentNode = 0;
	
	
	private int closeRadius = 100;
	private int mask;
	private RaycastHit hit;
	private float checkTimer = 0;
	private float checkTiming = 0.8f;
	private List<Transform> myPath;
	private Avatar avatarScript;
	
	//elbow room variables
	private Transform[] allNPCs;
	private int elbowRoom = 25;
	private float elbowSpeed = 10f;
	private bool elbowing = false;
	private Vector3 elbowVector;
	private Transform elbowee;
	
	//about fucking off and shit
	private bool fuckingOff = false;
	
	
	
	
	public class SpeechBubble{				//THE BUBBLE AND ITS DECLARATION!
		//variables of various types
		public GameObject go;
		public tk2dSprite spriteInfo;
		private Transform t;
		private Renderer r;
		private Transform parent;
		private Vector2 offset = new Vector2(30, 30);
		private float timer;
		private tk2dTextMesh myNumber;
		private Vector3 digitOffset = new Vector3(10, 3, -2);
		public string Digit{
			get{ return myNumber.text;}
			set{ myNumber.text = value;
				 myNumber.Commit();}
		}
		
		//getters & setters
		public bool Showing {
			get{ return r.enabled; }
			set{ r.enabled = value;
				 myNumber.renderer.enabled = value;
				 if (value == false){
					Digit = "";
				}
			}
		}
		
		public string SpriteName{
			get{ return spriteInfo.CurrentSprite.name; }
			set{ spriteInfo.SetSprite(value); }
		}
		
					//constructors
		
		public SpeechBubble(Transform toFollow){
			
			go = new GameObject(toFollow.name + "Bubble");
			Debug.Log(go);
			parent = toFollow;
			tk2dSprite.AddComponent(go, ChromatoseManager.manager.bubbleCollection, 0);
			spriteInfo = go.GetComponent<tk2dSprite>();
			
			r = go.renderer;
			t = go.transform;
			GameObject numberObj = new GameObject(go.name + "Number");
			myNumber = numberObj.AddComponent<tk2dTextMesh>();
			myNumber.font = ChromatoseManager.manager.chromatoseFont;
			myNumber.anchor = TextAnchor.MiddleCenter;
			myNumber.maxChars = 1;
			myNumber.Commit();
		}
		
		public void Main(){
			if (timer > 0){
				timer -= Time.deltaTime;
			}
			else if (r.enabled){
				r.enabled = false;
				Blend(Color.white);
			}
			t.position = parent.position + (Vector3)offset;
			myNumber.transform.position = t.position + digitOffset;
			
		}
		
		public void ShowBubbleFor(string bubble, float time){
			SpriteName = bubble;
			timer = time;
			r.enabled = true;
		}
		
		public void Blend(Color color){
			spriteInfo.color = color;
		}
		
		public override string ToString(){
			return SpriteName;
		}
		
		
	}
	
	private Npc.SpeechBubble myBubble;
												//PRIVATE VARS TO DO WITH THE BUBBLE
	private string followName = "ImFollowing";
	private string destroyName = "bubbleOK";
	private string blueNeedNPCs = "level4GoalNeed4NPC";
	private string redNeedNPCs = "level1GoalIndicator";
	
	private bool announcingExtrasNeeded = false;
	
												//ALL THE PUBLIC VARIABLES
	public int detectRadius = 250;
	public bool beginBySaying = false;
	public int initialSpeechRange = 400;
	public string bubbleSpriteName = "";
	public bool announceOtherNPCsRequired = false;
	
	public Vector2 fuckOffReference = new Vector2(1, 1);
	
	void Awake () {
		tag = "npc";
	}
	// Use this for initialization
	void Start () {
		
	//	tk2dSprite.AddComponent<tk2dSprite>(myBubble.go, ChromatoseManager.manager.bubbleCollection, bubbleName);
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
		
		
		//My bubble!
		myBubble = new Npc.SpeechBubble(this.transform);
		//Debug.Log(myBubble);
		
		//allNPCs = FindSceneObjectsOfType(typeof(Npc)) as Transform[];
		GameObject[] allTheNPCs = GameObject.FindGameObjectsWithTag("npc");
		List<Transform> npcList = new List<Transform>();
		
		foreach (GameObject go in allTheNPCs){
			npcList.Add(go.transform);
		}
		
		allNPCs = npcList.ToArray();
		
		
		Debug.Log(allNPCs);
	}
	
	// Update is called once per frame
	void Update () {
		
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<---------------SETUP SECTION!--------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
		myBubble.Main();
		//.....................................................................tumbleweed
		
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<--------------CHECKS SECTION!--------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
		
		if (building){
			if (toBuild){
				if (announceOtherNPCsRequired){
					announcingExtrasNeeded = true;
					myBubble.Showing = true;
					myBubble.SpriteName = blueNeedNPCs;
					myBubble.Digit = toBuild.NPCsNeeded.ToString();
				}
				myBubble.Showing = true;
				
				goto update;
			}
			else{
				announcingExtrasNeeded = false;
				myBubble.Showing = false;
				building = false;
				
			}
			
		}
		
		if (breaking){
			if (toDestroy){
				
				if (announceOtherNPCsRequired){
					announcingExtrasNeeded = true;
					myBubble.Showing = true;
					myBubble.SpriteName = redNeedNPCs;
					myBubble.Digit = toDestroy.NPCsNeeded.ToString();
				}
				
				goto update;
			}
			else{
				announcingExtrasNeeded = false;
				myBubble.Showing = false;
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
		if (colour.b > colourConsiderMin){			//What do I do if I'm blue enough to be considered blue by the blues?
			if (colour.r > anarchyConsiderMin) goto red;
			if (!target){
				target = avatar;
			}
			
			Vector2 diff = (Vector2) target.position - (Vector2)t.position;
			
			if (target == avatar){
												//THIS IS WHERE I START FOLLOWING THE AVATAR
				
				if (diff.magnitude < detectRadius && avatar.GetComponent<Avatar>().colour.Blue && !inMotion){
					inMotion = true;
					if (!saidFollow){
						beginBySaying = false;
						myBubble.ShowBubbleFor(followName, 4f);
						myBubble.Blend(spriteInfo.color);
						/*GameObject followBubble = ChromatoseManager.manager.OneShotAnim("bubbleIFollowYou", 4f, t.position);
													//follow avatar here
						followBubble.SetParent(gameObject);
						followBubble.transform.localPosition = new Vector3(20, 20, 0);*/
						saidFollow = true;
					}
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
				MaxRed();
				
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
										//move me out of the way of another NPC
		
		
		
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<--------------UPDATE SECTION!--------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	update:
		
							//Don't want you NPCs all on top of each other and everything!
		
		if (!elbowing){
			foreach (Transform tr in allNPCs){
				if (tr == t) continue;
				Vector3 dist = t.position - tr.position;
				if (dist.magnitude < elbowRoom){
					elbowVector = dist.normalized * Time.deltaTime * elbowSpeed;
					elbowee = tr;
					elbowing = true;
				}
			}
		}
		else{
			t.Translate(elbowVector, Space.World);
			Vector3 dist = elbowee.position - transform.position;
			if (dist.magnitude > elbowRoom){
				elbowing = false;
				elbowee = null;
				elbowVector = Vector3.zero;
			}
		}
							//Was I red and now I'm not?
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
		
		
							//Am I getting green? ...what is this for again? Hmm.... 
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
		
		if (beginBySaying){
			Vector3 diff = avatar.position - t.position;
			if (diff.magnitude < initialSpeechRange){
				myBubble.Showing = true;
				myBubble.SpriteName = bubbleSpriteName;
			}
		}
		
		if (fuckingOff){
			shownColour = new Color(shownColour.r, shownColour.g, shownColour.b, shownColour.a * 0.95f);
			if (shownColour.a < 0.01){
				Dead = true;
				Gone = true;
				StopAndDisable();
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
		if (saidOK) return;
		saidOK = true;
		myBubble.ShowBubbleFor(destroyName, 2.5f);
		//ok.SetParent(gameObject);
		//ok.transform.localPosition = new Vector3(20, 20, 0);
	}
	
	override public void Trigger(){
		
	}
	
	public void FuckOff(){
		fuckingOff = true;
		target = new GameObject(name + "Target").GetComponent<Transform>();
		target.position = t.position + (Vector3)fuckOffReference;
		inMotion = true;
	}
	
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position + (Vector3)fuckOffReference, 10);
	}
}

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
	private float cantSeeTimer = 0f;
	private float cantSeeTiming = 2f;
	
	//elbow room variables
	private Transform[] allNPCs;
	private int elbowRoom = 25;
	private float elbowSpeed = 10f;
	private bool elbowing = false;
	private Vector3 elbowVector;
	private Transform elbowee;
	
	private bool announceOtherNPCsRequired = false;
		
	public bool AnnounceOtherNPCsRequired {
		get { return announceOtherNPCsRequired; }
		set { announceOtherNPCsRequired = value; }
	}
	
	//about fucking off and shit
	private bool fuckingOff = false;
	
	
											//vars about my animations and what they DO
	private string greyToRed = "rNPC_greyToRed";
	private string[] redFreakouts = new string[] {"rNPC_bounce", "rNPC_nervousPale"};
	
	//prolly gon' put some stuff here
	
	private GameObject miscPart;
	private tk2dAnimatedSprite miscPartAnim;
	
	private bool showingMiscPart;
	private bool turningRed = false;
	
	
	public class SpeechBubble{				//THE BUBBLE AND ITS DECLARATION!
		//variables of various types
		public GameObject go;
		public tk2dSprite spriteInfo;
		private Transform t;
		private Renderer r;
		private Transform parent;
		private Vector2 offset = new Vector2(64, 25);
		private float timer;
		private GameObject myCollectible;
		private Vector2 collectibleOffset = new Vector2(10, 10);
		
		private string collectibleBubbleName = "redBubble_veryHappy";
		public string CollectibleBubbleName{ get { return collectibleBubbleName; } }
		
		private tk2dTextMesh myNumber;
		private Vector3 digitOffset = new Vector3(10, 3, -2);
		private int myDigit = 0;
		
		public int Digit{
			get{ return myDigit; }
			set{ myDigit = value; }
		}
		
		//getters & setters
		public bool Showing {
			get{ return r.enabled; }
			set{ r.enabled = value;
				 myNumber.renderer.enabled = value;
				 if (value == false){
					Digit = 0;
				}
			}
		}
		
		public string SpriteName{
			get{ return spriteInfo.CurrentSprite.name; }
			set{ spriteInfo.SetSprite(value); }
		}
		
		public SpeechBubble(Transform toFollow){  //constructor!
			
			go = new GameObject(toFollow.name + "Bubble");
			parent = toFollow;
			tk2dSprite.AddComponent(go, ChromatoseManager.manager.bubbleCollection, 0);
			spriteInfo = go.GetComponent<tk2dSprite>();
			
			r = go.renderer;
			t = go.transform;
			t.parent = parent;
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
				if (spriteInfo.CurrentSprite.name == collectibleBubbleName){
					collectibleBubbleName = "";
					myCollectible = GameObject.Instantiate(Resources.Load("pre_redCollectible"), t.position + (Vector3)collectibleOffset, Quaternion.identity) as GameObject;
					parent.SendMessage("NoMoreCollectible");
				}
			}
			//t.position = parent.position + (Vector3)offset;
			myNumber.transform.position = t.position + digitOffset;
			
		}
		
		public void ShowBubbleFor(string bubble, float time, int digit){
			string myDigitString = digit > 0? digit.ToString() : "";
			SpriteName = bubble + myDigitString;
			timer = time;
			r.enabled = true;
		}
		
		public void ShowBubbleFor(string bubble, float time){
			ShowBubbleFor(bubble, time, 0);
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
	private string followName = "partNPC_following";
	private string destroyName = "redBubble_happy";
	private string blueNeedNPCs = "blueBubble_x";
	private string redNeedNPCs = "redBubble_x";
	private Vector3 followPartOffset = new Vector3(27, 17, -2);
	private Vector3 goalPartOffset = Vector3.zero;
												//ALL THE PUBLIC VARIABLES
	public int detectRadius = 250;
	public bool beginBySaying = false;
	public bool alwaysSayWhenIdle = false;
	public int initialSpeechRange = 400;
	public string bubbleSpriteName = "";
	public bool waitForMessage = false;
	//public bool hasShadowBG = false;
	public int shadowSpriteIndex;
	public bool hasRedCol = false;
	
	
	private bool poopingCol = false;
	
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
		anim = GetComponent<tk2dAnimatedSprite>();
		
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
		
		
		
		//My bubble!
		myBubble = new Npc.SpeechBubble(this.transform);
		
		//allNPCs = FindSceneObjectsOfType(typeof(Npc)) as Transform[];
		GameObject[] allTheNPCs = GameObject.FindGameObjectsWithTag("npc");
		List<Transform> npcList = new List<Transform>();
		
		foreach (GameObject go in allTheNPCs){
			npcList.Add(go.transform);
		}
		
		allNPCs = npcList.ToArray();
		float animOffset = Random.Range(0f, 1f);
		
		int highestColour = 0;
		if (colour.r > highestColour){
			highestColour = colour.r;
		}
		if (colour.g > highestColour){
			highestColour = colour.g;
		}
		if (colour.b > highestColour){
			highestColour = colour.b;
		}
		if (highestColour > 220){
			anim.Play(animOffset);
		}
		
		//Particle:  instantiate it 
		miscPart = new GameObject(name + "Shadow");
		tk2dAnimatedSprite.AddComponent<tk2dAnimatedSprite>(miscPart, anim.Collection, 0);
		miscPartAnim = miscPart.GetComponent<tk2dAnimatedSprite>();
		miscPart.SetParent(gameObject);
		miscPart.transform.position += followPartOffset;
		miscPartAnim.anim = anim.anim;
		miscPartAnim.Play(followName);
		miscPartAnim.renderer.enabled = false;
		
		
		
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
				miscPart.renderer.enabled = true;
				miscPartAnim.Play("partNPC_onGoal");
				miscPart.transform.position = t.position + goalPartOffset;
				if (announceOtherNPCsRequired){
					myBubble.ShowBubbleFor(blueNeedNPCs, -1f, toBuild.NPCsNeeded);
				}
				//myBubble.Showing = true;
				
				goto update;
			}
			else{
				building = false;
				
				miscPart.renderer.enabled = false;
			}
			
		}
		
		if (breaking){
			if (toDestroy){
				
				miscPart.renderer.enabled = true;
				miscPartAnim.Play("partNPC_onGoal");
				miscPart.transform.position = t.position + goalPartOffset;
				if (announceOtherNPCsRequired){
					myBubble.ShowBubbleFor(redNeedNPCs, 0.5f, toDestroy.NPCsNeeded);
				}
				
				goto update;
			}
			else{
				myPath.Clear();
				inMotion = false;
				target = null;
				miscPart.renderer.enabled = false;
				
				breaking = false;
				if (!poopingCol){
					
					
				}
			}
		}
		
		if (poopingCol){
			goto move;
		}
		
		if (stoppingForever){
			goto move;
		}
		
		if (waitForMessage){
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
				bool cantSee = Physics.Linecast(avatar.position, t.position, out hit, mask);
				if (cantSee){
					cantSeeTimer += Time.deltaTime;
				}
				else{
					cantSeeTimer = 0f;
				}
				if 	(diff.magnitude < detectRadius && avatar.GetComponent<Avatar>().colour.Blue
					 && cantSeeTimer <= cantSeeTiming && !cantSee
					){
					inMotion = true;
					showingMiscPart = true;
					miscPartAnim.Play(followName);
					miscPart.transform.position = t.position + followPartOffset;
					miscPart.renderer.enabled = true;
					if (!saidFollow){
						beginBySaying = false;
						//myBubble.ShowBubbleFor(followName, 4f);
						saidFollow = true;
					}
				}
				else if(inMotion){
					miscPart.renderer.enabled = false;
					Debug.Log("Here");
					inMotion = false;
					cantSeeTimer = 0f;
				}
				
			}
			checkTimer += Time.deltaTime;
			if (checkTimer >= checkTiming){		//check for node every [checktiming] seconds
					
				checkTimer = 0;
				Transform closestBuildable = FindClosestOfTag(t, "buildable");
				if (!closestBuildable) goto red;
				toBuild = closestBuildable.GetComponent<Buildable>();
				 
				if (toBuild){
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
					Debug.Log("tHere");
					miscPart.renderer.enabled = false;
				}
			}
				
		}//last bracket of 'if blue'
		
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<----------------RED SECTION!---------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	red:
		if (colour.r > anarchyConsiderMin && !turningRed){		//If I'm red enough to consider fighting
			
			if (target == null || target == avatar){  //find a target, and the nearest one, ideally
				GameObject[] potentials = GameObject.FindGameObjectsWithTag("destructible");
				float closestDist = Mathf.Infinity;
				/*if (colour.b > colourConsiderMin){
					closestDist = detectRadius;
				}*/
				Transform closest = null;
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
					
					toDestroy = closest.GetComponent<Destructible>();
					closestNode = toDestroy.myNode;
					
					myPather = new Pather(t, closestNode);
					myPath = myPather.NewPath(t, closestNode);
					/*foreach (Transform targ in myPath){
						Debug.Log(targ.name);
					}*/
					inMotion = true;
					target = myPath[0];
				}
			}
			else if (target){
				//target = myPather.Seek(target);		//I have a target, so I'm going to look for it!
				float distToTarget = ((Vector2)target.position - (Vector2)t.position).magnitude;
				if (distToTarget < closeRadius){
					if (target == closestNode){
						if (toDestroy){
							toDestroy.AddOne(this);
						}
						target = null;
						closestNode = null;
						inMotion = false;
						breaking = true;
						movement.SetVelocity(Vector2.zero);
						myPath.Clear();
						currentNode = 0;
					}
					else{
						currentNode ++;
						if (currentNode >= myPath.Count){
							Debug.Log("Count = " + myPath.Count + " and node = " + currentNode);
							foreach (Transform targ in myPath){
								Debug.Log(targ.name);
							}
						}
						target = myPath[currentNode];
					}
				}
			}
			
			
		}
		else if (colour.r > 0 && !turningRed){ //Absorb colour from Avatar. Don't want to do it if I have NO colour, just... little colour.
			Vector2 diff = (Vector2) avatar.position - (Vector2)t.position;
			if (diff.magnitude < detectRadius && avatarScript.colour.Red && !Physics.Linecast(avatar.position, t.position, out hit, mask)){
				TurnRed();
				avatarScript.GiveColourTo(t, avatar);
			}
		}
	move:
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<----------------MOVE SECTION!--------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
		if (inMotion && target){
			movement.target = target;
			
										//gotta make sure my alpha stays the same if I'm moving, you know?
			if (spriteInfo.color.a > 0.7f && !fuckingOff){
				spriteInfo.SendMessage("FadeAlpha", 1f);
			}
			
			if (target == avatar){
				movement.SetNewMoveStats(avatarScript.movement.thruster.MaxSpeed - 2, movement.thruster.accel, movement.rotator.rotationRate);
			}
			
			Vector2 diff = (Vector2) target.position - (Vector2)t.position;
		
			float angle = VectorFunctions.PointDirection(diff);
			
			float zGoodFormat = VectorFunctions.Convert360(transform.rotation.eulerAngles.z);
			
			//t.Rotate(0, 0, angle - zGoodFormat);
			Vector2 disp = movement.Displace(true, angle * Mathf.Deg2Rad);
			//Vector2 disp = diff.normalized * movement.thruster.velocity.magnitude;
			if (stoppingForever){
				inMotion = false;
				goto update;
			}
			else{
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
		
		
							//this is for if I'm showing my particles
		/*
		if (showingMiscPart){
			miscPart.renderer.enabled = true;
			miscPart.transform.position = t.position + Vector3.forward;
			miscPart.transform.rotation = t.rotation;
			miscPartAnim.color = spriteInfo.color;
		}*/
		
							//Am I getting green? ...what is this for again? Hmm.... 
		
		
		if (stoppingForever){
			//movement.SlowToStop();
			if (movement.GetVelocity().magnitude < 5 && !waitingForMaxGreen){
				this.enabled = false;
			}
		}
		
		if (beginBySaying || alwaysSayWhenIdle && movement.GetVelocity().magnitude < 1){
			Vector3 diff = avatar.position - t.position;
			if (diff.magnitude < initialSpeechRange){
				myBubble.Showing = true;
				myBubble.SpriteName = bubbleSpriteName;
			}
		}
		
		if (fuckingOff){
			target.position = t.position + (Vector3)fuckOffReference;
			//Debug.Log("Fuck Off");
			spriteInfo.color = new Color(spriteInfo.color.r, spriteInfo.color.g, spriteInfo.color.b, spriteInfo.color.a * 0.95f);
			if (spriteInfo.color.a < 0.01){
				Dead = true;
				Gone = true;
				StopAndDisable();
			}
		}
		
		
		
		//end :)
	}
	
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
	}
	
	public void AddGreenUntilMax(){
		waitingForMaxGreen = true;
		addingGreen = true;
	}
	
	public void SetNewTarget(Transform newTarget){
		target = newTarget;
		
	}
	
	private Transform FindClosestOfTag(Transform mainTarget, string tag){
		return FindClosestOfTag(mainTarget, tag, 10000000);
	}
	
	public void MaxRed(tk2dAnimatedSprite sprite, int index){
		colour.r = 255;
		anim.Play(anim.GetClipByName(redFreakouts[Random.Range(0, 1)]), 0);
		turningRed = false;
		if (saidOK) return;
		saidOK = true;
		myBubble.ShowBubbleFor(destroyName, 2.5f);
	}
	
	public void TurnRed(){
		anim.Play(anim.GetClipByName(greyToRed), 0);
		anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
		anim.animationCompleteDelegate = MaxRed;
		turningRed = true;
		//ok.SetParent(gameObject);
		//ok.transform.localPosition = new Vector3(20, 20, 0);
	}
	
	override public void Trigger(){
		
	}
	
	public void FuckOff(){
		toBuild = null;
		toDestroy = null;
		fuckingOff = true;
		target = new GameObject(name + "Target").GetComponent<Transform>();
		target.position = t.position + (Vector3)fuckOffReference;
		inMotion = true;
	}
	
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position + (Vector3)fuckOffReference, 10);
	}
	
	public void PoopCollectible(){
		if (!hasRedCol) return;
		myBubble.ShowBubbleFor(myBubble.CollectibleBubbleName, 1f);
		poopingCol = true;
	}
	
	public void NoMoreCollectible(){
		hasRedCol = false;
		poopingCol = false;
	}
	
	void BackToGrey(){
		anim.Play(anim.GetClipByName("rNPC_redToGrey"), 0);
		losingColour = true;
		colour.r = initColour.r;
		anim.animationCompleteDelegate = null;
	}
}

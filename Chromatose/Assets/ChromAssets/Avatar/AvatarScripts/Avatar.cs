using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Avatar : ColourBeing
{
	private bool debug = false;
	
	private float loseRate = 6f;
	private float loseTimer = 0f;
	
	protected Vector2 velocity;
	private Vector2 previousVelocity;
	protected int direction;
	protected float[] angles = new float[16];
	
	
	[System.NonSerializedAttribute]
	public Movement movement;
	protected float basicTurnSpeed;
	protected float basicMaxSpeed;
	protected float basicAccel;
	
	public tk2dSpriteCollectionData normalCollection;
	public tk2dSpriteCollectionData paleCollection;
	public tk2dSpriteCollectionData afterImageCollection;
	public tk2dSpriteCollectionData particleCollection;
	public tk2dAnimatedSprite givePart;
	
	public tk2dSpriteAnimation partAnimations;
	
	private RefillColourParticle refillPart;
	private LoseAllColourParticle loseAllColourPart;
	private List<MovementLines> accelParts = new List<MovementLines>();
	private TurboParticle turboPart = null;
	private List<LoseColourParticle> loseColourPart = new List<LoseColourParticle>();
		private int loseColourPartDrop = 10;
		private int loseColourPartCounter = 0;
		private int partDropMin = 5;
		private int partDropMax = 13;
	
	private List<GiveColourParticle> giveColourParts = new List<GiveColourParticle>();

	public float accelPartTimer = 0f;
	public float accelPartTiming = 0.3f;
	public float accelPartTimingBase = 0.3f;
	
	
	//inputs. Up, left and right will also work, but getW seems intuitive to me
	protected bool getW;
	protected bool getA;
	protected bool getS;	//This is there for solidarity
	protected bool getD;
	private bool getSpace;
	
	private int currentSubimg;
	private string spritePrefix = "Player";
	private int rotCounter = 0;
	private int rotAnimTiming = 10;
	
	private int noRotSubimg = 1;
	private int ccw1 = 2;
	private int ccw2 = 3;
	private int cw1 = 4;
	private int cw2 = 5;
	
	
	
	private bool hurt;				//other properties! Getting hurt and stuff
	public bool Hurt{
		get{ return hurt;}
		set{ hurt = value;}
	}
	private int hurtTimer = 0;
	private int hurtTiming = 60;
	private int blinkOffAt = 20;
	private int blinkOnAt = 10;
	private List<GameObject> blobsHit = new List<GameObject>();
	private bool invisible = false;
	public int Invisible{
		get{ return invisible ? 0 : 1;}
	}
	private bool inBlueLight = false;
	public bool InBlueLight{
		get { return inBlueLight; }
		set { inBlueLight = value; }
	}
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<--------------Speed boosts!!-------------->
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	private GameObject[] speedBoosts;
	private Rect[] speedBoostAreas;
	private bool canControl = true;
	private int speedBoostDist = 55;
	private float maxSpeedMod = 2.0f;
	private float speedBoostMod = 2.0f;
	private float speedBoostCur = 1f;
	private int speedBoostCounter = 0;
	private int speedBoostMax = 50;
	
	
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<-----------Dependent objects!!------------>
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	[System.NonSerializedAttribute]			//my transform and renderer and material info
	public Transform t;
	private Renderer r;
	private Material mat;
	private Shader s;
	
	
	private GameObject outline;				//my after-image (outline) info (and refill)
	private GameObject outlinePointer;
	private string outlineName = "Player1";
	private string outlinePointerName = "Player6";
	private int teleportCost = 2;
	private int refillCost = 5;
	private bool hasOutline = false;
	
	private GameObject[] allTheFaders;
	
	[System.NonSerializedAttribute]
	public Texture avatarOutlineTexture;
											//Keeping track of where I get knocked back to
	private Transform myKnockTarget;
	
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<------------Particle classes!!------------>
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	private class MovementLines {
		float baseSpeed = 50f;
		float fadeRate = 0.05f;
		float fadeAfter = 0.7f;
		Vector3 velocity;
		
		int offset = 15;
		
		GameObject go;
		Transform t;
		tk2dAnimatedSprite spriteInfo;
		Transform avatarT;
		
		List<Avatar.MovementLines> theList;
		
		public MovementLines(Transform avatarT, Vector3 direction, float speedModifier, tk2dSpriteCollectionData spriteData, tk2dSpriteAnimation anim, List<Avatar.MovementLines> theList){
			velocity = direction.normalized * baseSpeed * speedModifier * Time.deltaTime;
			go = new GameObject("MovementLine");
			t = go.transform;
			this.avatarT = avatarT;
			t.position = avatarT.position + direction.normalized * offset;
			
			t.rotation = avatarT.rotation;
			tk2dAnimatedSprite.AddComponent<tk2dAnimatedSprite>(go, spriteData, "part_avatarAccel0001");
			spriteInfo = go.GetComponent<tk2dAnimatedSprite>();
			spriteInfo.anim = anim;
			spriteInfo.clipId = spriteInfo.GetClipIdByName("clip_avatarAccel");
			spriteInfo.Play();
			spriteInfo.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
			this.theList = theList;
		}
		
		public bool Main(){
			if (t)
				t.Translate(velocity, Space.World);
			
			if (fadeAfter > 0){
				fadeAfter -= Time.deltaTime;
			}
			else{
				spriteInfo.color = new Color(spriteInfo.color.r, spriteInfo.color.g, spriteInfo.color.b, spriteInfo.color.a - fadeRate);
				if (spriteInfo.color.a <= 0){
					GameObject.Destroy(go);
					return true;
				}
			}	
			return false;
		}
	}
	
	private class TurboParticle {
		GameObject go = new GameObject("TurboPart");
		Transform t;
		Transform avatarT;
		tk2dAnimatedSprite spriteInfo;
		
		public TurboParticle(tk2dSpriteCollectionData colData, tk2dSpriteAnimation animData, Transform avatarT){
			t = go.transform;
			this.avatarT = avatarT;
			tk2dAnimatedSprite.AddComponent<tk2dAnimatedSprite>(go, colData, "part_avatarTurbo0001");
			spriteInfo = go.GetComponent<tk2dAnimatedSprite>();
			spriteInfo.anim = animData;
			spriteInfo.clipId = spriteInfo.GetClipIdByName("clip_avatarTurbo");
			spriteInfo.Play();
			spriteInfo.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
		}
		
		public void Go(){
			t.position = avatarT.position;
			go.SetActive(true);
		}
		
		public void Stop(){
			go.SetActive(false);
		}
	}
	
	private class GiveColourParticle {
		List<GameObject> gos = new List<GameObject>();
		Transform target;
		Transform origin;
		Vector3 velocity;
		//tk2dAnimatedSprite spriteInfo;
		int shotsLeft = 4;
		float timer = 0f;
		float frequency = 0.07f;
		float speed = 400f;
		tk2dSpriteCollectionData colData;
		
		
		public GiveColourParticle(tk2dSpriteCollectionData colData, Transform target, Transform origin){
			this.colData = colData;
			this.target = target;
			this.origin = origin;
			this.velocity = (target.position - origin.position).normalized * speed;
		}
		
		public bool Main(){
			timer += Time.deltaTime;
			if (timer >= frequency && shotsLeft > 0){
				Shoot();
				shotsLeft --;
				timer = 0;
			}
			
			GameObject toDestroy = null;
			foreach (GameObject go in gos){
				
				go.transform.rotation = Quaternion.LookRotation(Vector3.forward, target.position - go.transform.position);
				go.transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
				if ((go.transform.position - target.position).magnitude < 10){
					toDestroy = go;
				}
			}
			if (toDestroy != null){
				gos.Remove(toDestroy);
				Destroy(toDestroy);
				if (gos.Count == 0)
					return true;
			}
			return false;
		}
		
		public void Shoot(){
			
				GameObject newGuy = new GameObject("colourGivePart" + shotsLeft);
				gos.Add(newGuy);
				tk2dSprite.AddComponent<tk2dSprite>(newGuy, colData, "part_avatarGiveColor");
				
				tk2dSprite spriteInfo = newGuy.GetComponent<tk2dSprite>();
				
				newGuy.transform.position = origin.position + Vector3.forward;
				spriteInfo.color = Color.red;
		}
	}
	
	private class RefillColourParticle {
		List<GameObject> gos = new List<GameObject>();
		Transform avatarT;
		//tk2dAnimatedSprite spriteInfo;
		int incrementMin = 42;
		int incrementMax = 47;
		int maxParts = 8;
		int offset = 25;
		tk2dSpriteCollectionData colData;
		tk2dSpriteAnimation animData;
		
		
		public RefillColourParticle(tk2dSpriteCollectionData colData, tk2dSpriteAnimation animData, Transform avatarT){
			this.colData = colData;
			this.animData = animData;
			this.avatarT = avatarT;
		}
		
		public void Emit(Color colour){
			for (int i = 0; i < maxParts; i ++){
				GameObject newGuy = new GameObject("RefillPart" + i);
				gos.Add(newGuy);
				tk2dAnimatedSprite.AddComponent<tk2dAnimatedSprite>(newGuy, colData, "part_avatarRefillColor0001");
				
				tk2dAnimatedSprite spriteInfo = newGuy.GetComponent<tk2dAnimatedSprite>();
				
				spriteInfo.anim = animData;
				 
				spriteInfo.Play(spriteInfo.GetClipByName("clip_avatarRefillColour"), 0);
				spriteInfo.color = colour;
				spriteInfo.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
				
				newGuy.transform.position = avatarT.position;
				newGuy.transform.rotation = Quaternion.identity;
				newGuy.transform.Rotate(new Vector3(0, 0, i * Random.Range(incrementMin, incrementMax)));
				newGuy.transform.Translate(Vector3.right * offset);
				
				if (i == 0){
					spriteInfo.animationCompleteDelegate = End;
				}
					
			}
		}
		
		public void End(tk2dAnimatedSprite sprite, int ClipId){
			foreach (GameObject go in gos){
				GameObject.Destroy(go);
			}
		}
		
	}
	
	private class LoseAllColourParticle {
		GameObject go = new GameObject("LoseColourPart");
		Transform t;
		Transform avatarT;
		tk2dAnimatedSprite spriteInfo;
		int offset = 17;
		float fadeRate = 0.05f;
		float fadeAfter = 1.2f;
		
		public LoseAllColourParticle(tk2dSpriteCollectionData colData, tk2dSpriteAnimation anim, Transform avatarT, Color blendColor){
			t = go.transform;
			this.avatarT = avatarT;
			t.position = avatarT.position + (-avatarT.right) * offset + (Vector3)Random.insideUnitCircle * offset;
			
			
			string spriteName = "part_avatarLosingAllColor0001";
			Debug.Log(spriteName + " is my name");
			tk2dAnimatedSprite.AddComponent<tk2dAnimatedSprite>(go, colData, spriteName);
			spriteInfo = go.GetComponent<tk2dAnimatedSprite>();
			spriteInfo.anim = anim;
			spriteInfo.Play(spriteInfo.GetClipByName("clip_avatarLoseAllColour"), 0);
			spriteInfo.color = blendColor;
			spriteInfo.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
		}
		public bool Fade(){
			if (fadeAfter > 0){
				fadeAfter -= Time.deltaTime;
			}
			else{
				Color newCol = new Color(spriteInfo.color.r, spriteInfo.color.g, spriteInfo.color.b, spriteInfo.color.a - fadeRate);
				spriteInfo.color = newCol;
				if (spriteInfo.color.a <= 0){
					GameObject.Destroy(go);
					return true;
				}
			}	
			return false;
		}
	}
	
	private class LoseColourParticle {
		GameObject go = new GameObject("LoseColourPart");
		Transform t;
		Transform avatarT;
		tk2dSprite spriteInfo;
		int offset = 17;
		float fadeRate = 0.05f;
		float fadeAfter = 1.2f;
		int maxNumber = 13;
		
		
		public LoseColourParticle(tk2dSpriteCollectionData colData, Transform avatarT, Color blendColor){
			t = go.transform;
			this.avatarT = avatarT;
			t.position = avatarT.position + (-avatarT.right) * offset + (Vector3)Random.insideUnitCircle * offset + Vector3.forward;
			
			
			int index = Random.Range(1, maxNumber);
			string spriteName = "part_avatarLosingColor00" + (index < 10? "0" + index.ToString() : index.ToString());
			tk2dSprite.AddComponent<tk2dSprite>(go, colData, spriteName);
			spriteInfo = go.GetComponent<tk2dSprite>();
			
			spriteInfo.color = blendColor;
		}
		
		public bool Fade(){
			if (fadeAfter > 0){
				fadeAfter -= Time.deltaTime;
			}
			else{
				spriteInfo.color = new Color(spriteInfo.color.r, spriteInfo.color.g, spriteInfo.color.b, spriteInfo.color.a - fadeRate);
				if (spriteInfo.color.a <= 0){
					GameObject.Destroy(go);
					return true;
				}
			}	
			return false;
		}
		
	}
	
	
	// Use this for initialization
	void Start ()
	{
		
		
		
		movement = GetComponent<Movement>();
		basicTurnSpeed = movement.rotator.rotationRate;
		basicAccel = movement.thruster.accel;
		basicMaxSpeed = movement.thruster.maxSpeed;
		
		for (int i = 0; i < angles.Length; i++){
			angles[i] = i * 22.5f;
		}
		t = this.transform;
		r = this.renderer;
		mat = this.renderer.materials[0];
		s = mat.shader;
												//initializing my particle objects, as necessary
		spriteInfo = GetComponent<tk2dSprite>();
		turboPart = new TurboParticle(particleCollection, partAnimations, t);
		accelPartTiming = basicAccel * Time.deltaTime;
		refillPart = new RefillColourParticle(particleCollection, partAnimations, t);
		
		/*
		outlinePointer = new GameObject("OutlinePointer");		//make my outline pointer thing	TODO remove this and other references to it. Martine hates it a lot... <_<
		tk2dSprite.AddComponent<tk2dSprite>(outlinePointer, spriteInfo.Collection, outlinePointerName);
		outlinePointer.renderer.enabled = false;*/
		//outlinePointer.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
		
		speedBoosts = GameObject.FindGameObjectsWithTag("SpeedBoost");
		speedBoostAreas = new Rect[speedBoosts.Length];
		int counter = 0;
		foreach (GameObject sb in speedBoosts){
			List<float> xs = new List<float>();
			List<float> ys = new List<float>();
			float w;
			float h;
			Transform[] nodes = sb.GetAllComponentsInChildren<Transform>();
			foreach (Transform n in nodes){
				xs.Add(n.transform.position.x);
				ys.Add(n.transform.position.y);
			}
		
			xs.Sort();
			ys.Sort();
			int offset = 125;
			speedBoostAreas[counter] = new Rect(xs[0] - offset, ys[0] - offset, xs[xs.Count - 1] - xs[0] + offset * 2, ys[ys.Count - 1] - ys[0] + offset * 2);		//make a rectangle encasing the speed boost areas
			
			counter ++;
		}
		
													//I WILL NOW FIND THE NEAREST KNOCKBACK TARGET!
		myKnockTarget = VectorFunctions.FindClosestOfTag(t.position, "knockTarget", 1000000);
		
		allTheFaders = GameObject.FindGameObjectsWithTag("spriteFader");
		
		
		
	}
	
	// Update is called once per frame
	void Update ()
	{
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<----------Handling Colour Blend!---------->
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
		
		int highestColour = Mathf.Max(colour.r, colour.g, colour.b);		//Decide what colour the avatar is
		//Debug.Log("Highest colour is " + highestColour);
		float r = 255; float g = 255; float b = 255;
		Color partColor = Color.white;
		if (highestColour == colour.r){
			g -= colour.r;
			b -= colour.r;
			partColor = Color.red;
		}
		if (highestColour == colour.g){
			r -= colour.g;
			b -= colour.g;
			partColor = Color.green;
		}
		if (highestColour == colour.b){
			g -= colour.b;
			r -= colour.b;
			partColor = Color.blue;
		}
		
		if (inBlueLight){
			r = colour.r;
			g = colour.g;
		}
		
		shownColour = new Color(r/255f, g/255f, b/255f, Invisible);		//TODO : proper colour on 
		
		if (inBlueLight){
			partColor = shownColour;
		}
		//Debug.Log("I'm showing the colour " + shownColour);
		spriteInfo.color = shownColour;
		
					//Check for inputs: WAD or Up, Left Right
		if (highestColour > 0){
			loseTimer += Mathf.Min(velocity.magnitude, basicMaxSpeed * Time.deltaTime);
		}
		if (loseTimer >= loseRate){
			loseTimer = 0f;
			colour.r = tempColour.r >= 0 ? colour.r : colour.r - 1;
			colour.g = tempColour.g >= 0 ? colour.g : colour.g - 1;
			colour.b = tempColour.b >= 0 ? colour.b : colour.b - 1;
			loseColourPartCounter ++;
			
		}
		if (loseColourPartCounter >= loseColourPartDrop){
			loseColourPartCounter = 0;
			loseColourPartDrop = Random.Range(partDropMin, partDropMax);
			loseColourPart.Add(new LoseColourParticle(particleCollection, t, partColor));
			
		}
		
		colour.r = Mathf.Clamp(colour.r, 0, 255);
		colour.g = Mathf.Clamp(colour.g, 0, 255);
		colour.b = Mathf.Clamp(colour.b, 0, 255);
		
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<---------Check for speed boosts!!--------->
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		int rectCounter = 0;
		bool detectedSB = false;
		foreach (Rect rect in speedBoostAreas){
			
			if (rect.Contains((Vector2)t.position)){
				//Debug.Log("Found my speed boost! number " + rectCounter);
				//Debug.Log("The speed boost itself is called " + speedBoosts[rectCounter]);
				foreach (Transform node in speedBoosts[rectCounter].GetComponentsInChildren<Transform>()){
					if (node == speedBoosts[rectCounter].transform) continue;
					if (Vector2.Distance((Vector2)t.position, (Vector2)node.position) < speedBoostDist){
						detectedSB = true;
						//Debug.Log("The one I found is " + node.name);
					}
				}
			}
			
			rectCounter ++;
		}
		
		if (detectedSB){
			speedBoostCounter = Mathf.Min(speedBoostCounter + 1, speedBoostMax);
			turboPart.Go();
			movement.SetNewMoveStats(Mathf.Min(basicMaxSpeed + speedBoostMod * speedBoostCounter, basicMaxSpeed * speedBoostMod), basicAccel * speedBoostMod, basicTurnSpeed / speedBoostMod * 2);
		}
		else if (speedBoostCounter > 0){
			speedBoostCounter --;
			movement.SetNewMoveStats(Mathf.Max(basicMaxSpeed + speedBoostMod * speedBoostCounter, basicMaxSpeed), basicAccel, basicTurnSpeed);
			turboPart.Go();
		}
		else{
			movement.SetNewMoveStats(basicMaxSpeed, basicAccel, basicTurnSpeed);
			turboPart.Stop();
		}
		
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<-------------Getting Inputs!-------------->
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		if (canControl){
			getW = Input.GetKey(KeyCode.W);
			if (Input.GetKey (KeyCode.UpArrow)){
				getW = true;
				
			}
			
			getA = Input.GetKey(KeyCode.A);
			if (Input.GetKey (KeyCode.LeftArrow)){
				getA = true;
			}
			
			getD = Input.GetKey(KeyCode.D);
			if (Input.GetKey (KeyCode.RightArrow)){
				getD = true;
			}
			
			getS = Input.GetKeyDown(KeyCode.S);
			if (Input.GetKeyDown(KeyCode.DownArrow)){
				getS = true;
			}
			
			getSpace = Input.GetKeyDown(KeyCode.Space);
			if (getSpace){
				bool enoughCols = ChromatoseManager.manager.GetCollectibles(Couleur.white) >= 5;
				if (!enoughCols){
					getSpace = false;
					Debug.Log("Not enough collectibles for refill!"); 			//TODO: Make this into a real function! Like an anim or something
				}
			}
				
		}
			
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<------------Handling Movement!------------>
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
		
						//Translating the inputs to movement functions
		TranslateInputs(speedBoostCur);
		
		
		
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<------------Other fun things!!------------>
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
		if (hurt && Time.timeScale > 0){		//Am I hurt? Blink the sprite appropriately
			hurtTimer ++;
			
			if (hurtTimer % blinkOnAt == 0){
				
				invisible = false;
			}
			
			if (hurtTimer % blinkOffAt == 0){
				invisible = true;
			}
			
			if (hurtTimer >= hurtTiming){
				hurt = false;
				invisible = false;
				hurtTimer = 0;
			}
		}
		
												//Self-made checkpoints! Or whatever you want to call it
		
		if (getS){
			if (ChromatoseManager.manager.GetCollectibles(Couleur.white) < teleportCost && !hasOutline){
				Debug.Log("NOT ENOUGH COLLECTIBLES FOR AFTER_IMAGE!!!");		//	TODO Make this into an actual function! Play an animation or something, yano?
				goto end;
			}
			
			if (!hasOutline){
				outline = new GameObject("Outline");
				outline.transform.rotation = t.rotation;
				outline.transform.position = t.position;
				tk2dSprite.AddComponent<tk2dSprite>(outline, afterImageCollection, spriteInfo.CurrentSprite.name);
				
				ChromatoseManager.manager.DropCollectibles(ChromatoseManager.manager.WhiteCollectibles, teleportCost, outline.transform.position);
				
				hasOutline = true;
				foreach (GameObject go in allTheFaders){
					go.SendMessage("SaveState");
				}
				//spriteInfo.SwitchCollectionAndSprite()
			}
			else{
				
				t.position = outline.transform.position;
				t.rotation = outline.transform.rotation;
				ChromatoseManager.manager.GrabHeldWhiteCols();
				ChromatoseManager.manager.RemoveCollectibles(Couleur.white, teleportCost, outline.transform.position);
				Destroy(outline);
				hasOutline = false;
				//velocity = Vector2.zero;				//TEST For now I like the idea of keeping your current movement for when you go back. 
				//movement.SetVelocity(velocity);		// But should we have you facing  the same direction?
				
				foreach (GameObject go in allTheFaders){
					go.SendMessage("LoadState");
				}
			}
		}
		
					//Do I refill my colour? =O
		
		if (getSpace && !colour.White){
			if (colour.r > colour.g && colour.r > colour.b){
				colour.r = 255;
				ChromatoseManager.manager.RemoveCollectibles(Couleur.white, refillCost, t.position);
			}
			if (colour.b > colour.g && colour.b > colour.r){
				colour.b = 255;
				ChromatoseManager.manager.RemoveCollectibles(Couleur.white, refillCost, t.position);
			}
			if (colour.g > colour.r && colour.r > colour.b){
				colour.g = 255;
				ChromatoseManager.manager.RemoveCollectibles(Couleur.white, refillCost, t.position);
			}
			
			refillPart.Emit(partColor);
		}
		
					//Update my little pointer man!  TODO put this guy at the edge of the screen if the shadow is off-screen
		/*
		if (!hasOutline && outlinePointer.renderer.enabled){
			outlinePointer.renderer.enabled = false;
		}
		else if (hasOutline && outline){
			Vector3 direction = outline.transform.position - t.position;
			if (direction.magnitude > 30){
				outlinePointer.renderer.enabled = true;
				outlinePointer.transform.position = t.position + direction.normalized * 30;
				Vector3 lookDirection = VectorFunctions.ConvertLookDirection(direction);
				outlinePointer.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 1), direction);
				
			}
		}
		*/
		
		
		
															//drop particles where necessary
		if (getW){
			accelPartTimer += Time.deltaTime;
			if (accelPartTimer >= accelPartTiming){
				//accelParts.Add(new MovementLines())
				accelParts.Add(new MovementLines(t,-t.right, 1f, particleCollection, partAnimations, accelParts));
				accelPartTimer = 0;
				accelPartTiming = velocity.magnitude *accelPartTimingBase;
			}
		}
		else{
			accelPartTimer = 0f;
			accelPartTiming = basicAccel * Time.deltaTime;
		}
		
															//running the main method on each particle
		MovementLines lineToRemove = null;
		
		foreach (MovementLines m in accelParts){
			bool removeLine = m.Main();
			if (removeLine)
				lineToRemove = m;
		}
		
		if (lineToRemove != null){
			accelParts.Remove(lineToRemove);
		}
		
		LoseColourParticle blotchToRemove = null;
		
		foreach (LoseColourParticle m in loseColourPart){
			bool removeBlotch = m.Fade();
			if (removeBlotch)
				blotchToRemove = m;
		}
		
		if (blotchToRemove != null){
			accelParts.Remove(lineToRemove);
		}
		
		if (loseAllColourPart != null){
			bool dead = loseAllColourPart.Fade();
			if (dead){
				loseAllColourPart = null;
			}
		}
		GiveColourParticle giveToRemove = null;
		foreach (GiveColourParticle part in giveColourParts){
			part.Main();
		}
		
		
		
	end:
		if (previousVelocity != velocity)
			previousVelocity = velocity;
		t.position = new Vector3(t.position.x, t.position.y, 0);
		if (debug)
			Debug.Log("Yea dawg, verily this be the end");
		
	}
	
	protected void TranslateInputs(float multiplier){
		
		if (Time.timeScale == 0) return;
		bool gonnaRotate = false;
		bool clockwise = false;
		bool gonnaThrust = false;
		if (getW){
			gonnaThrust = true;
		}
		
		if (getA){
			rotCounter --;
			gonnaRotate = true;
			clockwise = false;
		}
		
		if (getD){
			rotCounter ++;
			if (getA){
				gonnaRotate = false;
				getA = false;
				getD = false;
			}
			else{
				gonnaRotate = true;
				clockwise = true;
			}
		}
		
		rotCounter = Mathf.Clamp(rotCounter, -rotAnimTiming, rotAnimTiming);
		
		
		if (!getA && !getD && currentSubimg != noRotSubimg){
			int newCounter = Mathf.Abs(rotCounter) - 1;
			rotCounter = Mathf.Clamp(rotCounter, -newCounter, newCounter);
			if (rotCounter == 0){
				currentSubimg = noRotSubimg;
				spriteInfo.SetSprite(spritePrefix + currentSubimg.ToString());
			}
		}
		
		if (rotCounter > 0 && rotCounter < rotAnimTiming && currentSubimg != cw1){				//turn animation: clockwise
			currentSubimg = cw1;
			spriteInfo.SetSprite(spritePrefix + currentSubimg.ToString());
		}
		
		if (rotCounter >= rotAnimTiming && currentSubimg != cw2){
			currentSubimg = cw2;
			spriteInfo.SetSprite(spritePrefix + currentSubimg.ToString());
		}
		
		if (rotCounter < 0 && rotCounter > -rotAnimTiming && currentSubimg != ccw1){			//turn animation: counter-clockwise
			currentSubimg = ccw1;
			spriteInfo.SetSprite(spritePrefix + currentSubimg.ToString());
		}
		
		if (rotCounter <= -rotAnimTiming && currentSubimg != ccw2){
			currentSubimg = ccw2;
			spriteInfo.SetSprite(spritePrefix + currentSubimg.ToString());
		}
		
		
		if (gonnaRotate){																		//applying rotation and translation
			t.Rotate(this.movement.Rotate(clockwise));
			//t.rotation.eulerAngles = movement.rotator.Rotate(t.rotation.eulerAngles, clockwise);
		}
		velocity = this.movement.Displace(gonnaThrust);
		t.position += new Vector3(velocity.x, velocity.y, 0) * multiplier;
		//Debug.Log(t.rotation.eulerAngles.z);
		
	}
	protected void TranslateInputs(){
		TranslateInputs(1f);
	}
	
															//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
															//<---------Knockback Management!--------->
															//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	void OnTriggerEnter(Collider box){
		if (box.tag == "knockTarget"){
			myKnockTarget = box.transform;
		}
	}
	
	
															//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
															//<--------Get/Setter functions----------->
															//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	
	
	public void CannotControlFor(float t){
		canControl = false;
			
		Invoke("CanControl", t);
	}
	
	public bool CheckIsBlue(){
		return colour.b > 0;
	}
	
	public void CanControl(){
		canControl = true;
	}
	
	override public void Trigger(){
		
	}
	
	public void CancelOutline(){
		Destroy(outline);
		hasOutline = false;
	}
	
	public void LoseAllColour(){
		int highestColour = Mathf.Max(colour.r, colour.g, colour.b);		//Decide what colour the avatar is
		Color blendColour = new Color(colour.r - highestColour + 1, colour.g - highestColour + 1, colour.b - highestColour + 1, 1);
		loseAllColourPart = new LoseAllColourParticle(particleCollection, partAnimations, t, blendColour);
		colour.r = 0;
		colour.g = 0;
		colour.b = 0;
	}
	
	public void GiveColourTo(Transform target, Transform origin){
		giveColourParts.Add(new GiveColourParticle(particleCollection, target, origin));
	}
	
	
															//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
															//<-----------PAIN AND PUSHING!----------->
															//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	
	private void Ouch(GameObject go){
		
		if (hurt){
			hurtTimer = 0;
		}
		else{
			if (ChromatoseManager.manager.WhiteCollectibles.Count == 0){
				canControl = false;
				ChromatoseManager.manager.Death();
				return;
			}
			ChromatoseManager.manager.RemoveCollectibles(Couleur.white, 1, t.position);
			hurt = true;
			CannotControlFor(0.5f);
			invisible = true;
		}
	}
	
	public void Jolt(float amount){
		if (!myKnockTarget){
			Debug.LogWarning("There's no knockback targets in this level! NOOOOO!");
			return;
		}
		Vector3 direction = myKnockTarget.position - t.position;
		t.position += direction.normalized * amount;
	}
	
	public void Push(float amount){
		
		if (!myKnockTarget){
			Debug.LogWarning("There's no knockback targets in this level! NOOOOO!");
			return;
		}
		Vector2 diff = (Vector2)myKnockTarget.position - (Vector2)t.position;
		movement.SetVelocity(diff.normalized * amount);
		Debug.Log("Diff is " + diff);
	}
	
}


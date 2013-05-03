using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Avatar : ColourBeing
{
	private bool debug = false;
	
	private float loseRate = 6f;
	private float loseTimer = 0f;
	
	protected Vector2 velocity;
	protected int direction;
	protected float[] angles = new float[16];
	
	
	[System.NonSerializedAttribute]
	public Movement movement;
	public float basicTurnSpeed;
	public float basicMaxSpeed;
	public float basicAccel;
	
	public tk2dSpriteCollectionData normalCollection;
	public tk2dSpriteCollectionData paleCollection;
	public tk2dSpriteCollectionData afterImageCollection;
	public tk2dSpriteCollectionData particleCollection;
	public tk2dAnimatedSprite accelPart;
	public tk2dAnimatedSprite turboPart;
	public tk2dAnimatedSprite givePart;
	public tk2dAnimatedSprite loseColourPart;
	public tk2dAnimatedSprite loseAllColourPart;
	
	
	
	
	public float accelPartTimer = 0f;
	public float accelPartTiming = 0.5f;
	
	
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
	
	
	
	private bool hurt;
	public bool Hurt{
		get{ return hurt;}
		set{ hurt = value;}
	}
	private int hurtTimer = 0;
	private int hurtTiming = 60;
	private int blinkOffAt = 20;
	private int blinkOnAt = 10;
	private bool invisible = false;
	public int Invisible{
		get{ return invisible ? 0 : 1;}
	}
	
	private GameObject[] speedBoosts;
	private Rect[] speedBoostAreas;
	private bool canControl = true;
	private int speedBoostDist = 40;
	private float maxSpeedMod = 2.0f;
	private float speedBoostMod = 2.0f;
	private float speedBoostCur = 1f;
	
	
	[System.NonSerializedAttribute]
	public Transform t;
	private Renderer r;
	private Material mat;
	private Shader s;
	private GameObject outline;
	private GameObject outlinePointer;
	private string outlineName = "Player1";
	private string outlinePointerName = "Player6";
	private int teleportCost = 2;
	private int refillCost = 5;
	private bool hasOutline = false;
	
	[System.NonSerializedAttribute]
	public Texture avatarOutlineTexture;
	
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
		spriteInfo = GetComponent<tk2dSprite>();
		
		accelPartTiming = basicAccel * Time.deltaTime;
		
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
			
			Debug.Log("biggest, smallest X values: " + xs[xs.Count - 1] + ", " + xs[0]);
			counter ++;
		}
		
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
		if (highestColour == colour.r){
			g -= colour.r;
			b -= colour.r;
		}
		if (highestColour == colour.g){
			r -= colour.g;
			b -= colour.g;
		}
		if (highestColour == colour.b){
			g -= colour.b;
			r -= colour.b;
		}
		shownColour = new Color(r/255f, g/255f, b/255f, Invisible);		//TODO : proper colour on 
		//Debug.Log("I'm showing the colour " + shownColour);
		spriteInfo.color = shownColour;
		
					//Check for inputs: WAD or Up, Left Right
		loseTimer += velocity.magnitude;
		
		if (loseTimer >= loseRate){
			loseTimer = 0f;
			colour.r = tempColour.r >= 0 ? colour.r : colour.r - 1;
			colour.g = tempColour.g >= 0 ? colour.g : colour.g - 1;
			colour.b = tempColour.b >= 0 ? colour.b : colour.b - 1;
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
				
				foreach (Transform node in speedBoosts[rectCounter].GetComponentsInChildren<Transform>()){
					if (Vector2.Distance((Vector2)t.position, (Vector2)node.position) < speedBoostDist){
						detectedSB = true;
					}
				}
			}
			
			rectCounter ++;
		}
		if (detectedSB){
			movement.SetNewMoveStats(basicMaxSpeed * speedBoostMod, basicAccel * speedBoostMod, basicTurnSpeed / speedBoostMod * 2);
		}
		else{
			movement.SetNewMoveStats(basicMaxSpeed, basicAccel, basicTurnSpeed);
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
			}
		}
		
												//Self-made checkpoints! Or whatever you want to call it
		
		if (getS){
			if (ChromatoseManager.manager.GetCollectibles(Couleur.white) < teleportCost){
				Debug.Log("NOT ENOUGH COLLECTIBLES FOR AFTER_IMAGE!!!");		//	TODO Make this into an actual function! Play an animation or something, yano?
				goto end;
			}
			
			if (!hasOutline){
				outline = new GameObject("Outline");
				outline.transform.rotation = t.rotation;
				outline.transform.position = t.position;
				//tk2dSprite.AddComponent<tk2dSprite>(outline, spriteInfo.Collection, outlineName);
				
				ChromatoseManager.manager.DropCollectibles(ChromatoseManager.manager.WhiteCollectibles, teleportCost, outline.transform.position);
				
				hasOutline = true;
				
				//spriteInfo.SwitchCollectionAndSprite()
			}
			else{
				t.position = outline.transform.position;
				t.rotation = outline.transform.rotation;
				ChromatoseManager.manager.RemoveCollectibles(Couleur.white, teleportCost, outline.transform.position);
				Destroy(outline);
				hasOutline = false;
				//velocity = Vector2.zero;				//TEST For now I like the idea of keeping your current movement for when you go back. 
				//movement.SetVelocity(velocity);		// But should we have you facing  the same direction?
			}
		}
		
					//Do I refill my colour? =O
		
		if (getSpace){
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
				
				
				accelPartTimer = 0f;
				accelPartTiming = velocity.magnitude * 0.3f;
			}
		}
		else{
			accelPartTimer = 0f;
			
		}
	end:
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
	//<--------Get/Setter functions----------->
	//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	public void CannotControlFor(float t){
		canControl = false;
			
		Invoke("CanControl", t);
	}
	
	void Ouch(){
		hurt = true;
		CannotControlFor(0.5f);
		invisible = true;
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
	
	
	
}


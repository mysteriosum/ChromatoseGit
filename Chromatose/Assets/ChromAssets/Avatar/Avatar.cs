using UnityEngine;
using System.Collections;

public class Avatar : ColourBeing
{
	
	private float loseRate = 6f;
	private float loseTimer = 0f;
	
	protected Vector2 velocity;
	protected int direction;
	protected float[] angles = new float[16];
	
	
	[System.NonSerializedAttribute]
	public Movement movement;
	
	//inputs. Up, left and right will also work, but getW seems intuitive to me
	protected bool getW;
	protected bool getA;
	protected bool getS;	//This is there for solidarity
	protected bool getD;
	
	
	private bool hurt;
	public bool Hurt{
		get{ return hurt;}
		set{ hurt = value;}
	}
	int hurtTimer = 0;
	int hurtTiming = 60;
	int blinkOffAt = 20;
	int blinkOnAt = 10;
	bool invisible = false;
	int Invisible{
		get{ return invisible ? 0 : 1;}
	}
	
	
	private bool canControl = true;
	
	
	[System.NonSerializedAttribute]
	public Transform t;
	private Renderer r;
	private Material mat;
	private Shader s;
	private GameObject outline;
	private GameObject outlinePointer;
	private string outlineName = "Player1";
	private string outlinePointerName = "Player6";
	
	
	[System.NonSerializedAttribute]
	public Texture avatarOutlineTexture;
	private bool hasOutline = false;
	
	// Use this for initialization
	void Start ()
	{
		
		
		
		movement = GetComponent<Movement>();
		
		
		for (int i = 0; i < angles.Length; i++){
			angles[i] = i * 22.5f;
		}
		t = this.transform;
		r = this.renderer;
		mat = this.renderer.materials[0];
		s = mat.shader;
		spriteInfo = GetComponent<tk2dSprite>();
		
		outlinePointer = new GameObject("OutlinePointer");		//make my outline pointer thing
		tk2dSprite.AddComponent<tk2dSprite>(outlinePointer, spriteInfo.Collection, outlinePointerName);
		outlinePointer.renderer.enabled = false;
		//outlinePointer.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
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
								//<------------Handling Movement!------------>
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		if (canControl){
			getW = Input.GetKey (KeyCode.W);
			if (Input.GetKey (KeyCode.UpArrow)){
				getW = true;
				
			}
			
			getA = Input.GetKey (KeyCode.A);
			if (Input.GetKey (KeyCode.LeftArrow)){
				getA = true;
			}
			
			getD = Input.GetKey (KeyCode.D);
			if (Input.GetKey (KeyCode.RightArrow)){
				getD = true;
			}
		}
			
						//Translating the inputs to movement functions
		TranslateInputs();
		
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
		
					//TEST : Making copies of myself!
		
		if (Input.GetKeyDown(KeyCode.Space)){
			if (!hasOutline){
				outline = new GameObject("Outline");
				outline.transform.rotation = t.rotation;
				outline.transform.position = t.position;
				tk2dSprite.AddComponent<tk2dSprite>(outline, spriteInfo.Collection, outlineName);
				hasOutline = true;
			}
			else{
				t.position = outline.transform.position;
				t.rotation = outline.transform.rotation;
				Destroy(outline);
				hasOutline = false;
				//velocity = Vector2.zero;				//TEST For now I like the idea of keeping your current moment for when you go back
				//movement.SetVelocity(velocity);
			}
		}
		
					//Update my little pointer man!
		
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
		
	}
	
	protected void TranslateInputs(float multiplier){
		bool gonnaRotate = false;
		bool clockwise = false;
		bool gonnaThrust = false;
		if (getW){
			gonnaThrust = true;
		}
		
		if (getA){
			gonnaRotate = true;
			clockwise = false;
		}
		
		if (getD){
			if (getA){
				gonnaRotate = false;
			}
			else{
				gonnaRotate = true;
				clockwise = true;
			}
		}
		
		if (gonnaRotate){
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
	
	
}


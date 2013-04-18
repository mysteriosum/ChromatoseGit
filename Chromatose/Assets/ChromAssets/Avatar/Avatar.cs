using UnityEngine;
using System.Collections;

public class Avatar : ColourBeing
{
	
	private float loseRate = 15f;
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
	
	
	private bool canControl = true;
	
	
	[System.NonSerializedAttribute]
	public Transform t;
	private Renderer r;
	private Material mat;
	
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
	}
	
	// Update is called once per frame
	void Update ()
	{
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<------------Handling Movement!------------>
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
		
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
		
		//mat.color = new Color(colour.r, colour.g, colour.b, 1);
		mat.SetColor("_Color", new Color(colour.r, colour.g, colour.b));
	}
	
	protected void TranslateInputs(){
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
		t.position += new Vector3(velocity.x, velocity.y, 0);
		//Debug.Log(t.rotation.eulerAngles.z);
	}
	
	
	//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
	//<--------Get/Setter functions----------->
	//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	public void CannotControlFor(float t){
		canControl = false;
			
		Invoke("CanControl", t);
	}
	
	public void CanControl(){
		canControl = true;
	}
	
	override public void Trigger(){
		
	}
	
	
}


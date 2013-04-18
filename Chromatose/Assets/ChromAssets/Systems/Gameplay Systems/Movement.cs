using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
	//<-----------DEFINING MY VARIABLES!----------->
	//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	
	public bool humanControlled;
	public Transform target;
		
	public Thruster thruster = new Thruster();		//my class variables! :D
	public Rotator rotator = new Rotator();
	public Collider2d collider2d = new Collider2d();
	
	[System.NonSerializedAttribute]
	public Transform t;
	//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
	//<------------DEFINING MY CLASSES!------------>
	//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	[System.Serializable]
	public class Thruster{			//This is where I do all of my Translation
		
		public float maxSpeed = 100;
		public float accel = 5;
		[System.NonSerialized]
		public float magnitude;
		[System.NonSerialized]
		public Vector2 velocity;
		//private bool isThrusting = false;
		
		
		
	}
	
	[System.Serializable]
	public class Rotator{			//ROTATOR CLASS OH YEAH
		
		public bool rotates;
		public float rotationRate;
		[System.NonSerialized]
		public float rotationIncrement = 22.5f;
		[System.NonSerialized]
		public float rotationTimer = 0;
		[System.NonSerialized]
		public bool prevClockwise;
		
		
		
		
		
	}
	
	void OnCollisionEnter(Collision collision){			//COLLISION CLASS! I'MA COLLIDE YOUR FACE!
		
		if (collision.gameObject.tag == "collision"){
			ContactPoint point = collision.contacts[0];
			
			thruster.velocity = collider2d.Collide(point, thruster.velocity);
		}
	}
	
	//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
	//<-----------TRANSLATION FUNCTIONS!----------->
	//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	
	public Vector2 Displace(bool thrust){								//Displacement : Thrust, accel, stuff
		if (thrust){
			
			float zRotR = t.rotation.eulerAngles.z * Mathf.Deg2Rad;
			Vector2 displacement = new Vector2(Mathf.Cos(zRotR) * thruster.accel, Mathf.Sin(zRotR) * thruster.accel);
			thruster.velocity += displacement;	
			//Debug.Log(thruster.velocity);
			if (thruster.velocity.magnitude > thruster.maxSpeed){
				thruster.velocity = thruster.velocity.normalized * thruster.maxSpeed;
			}
		}
		return thruster.velocity * Time.deltaTime;
	}
	
	public void SlowToStop(){
		if (thruster.velocity != Vector2.zero){
			thruster.velocity = Vector2.Lerp(thruster.velocity, Vector3.zero, 0.4f);
			Debug.Log("Slowing...");
		}
	}
	
	public void SetVelocity(Vector2 newVel){
		thruster.velocity = newVel;
	}
	
	//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
	//<------------ROTATION FUNCTIONS!!------------>
	//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	
	
	public Vector3 Rotate (bool clockwise){								//Rotate me; rotate me, my friend.
		if (!rotator.rotates){
			return Vector3.zero;
		}
		
		
		if (clockwise == rotator.prevClockwise){
			rotator.rotationTimer += Time.deltaTime;
		}
		else{
			rotator.rotationTimer = 1;
		}
		
		rotator.prevClockwise = clockwise;
		
		if (rotator.rotationTimer >= rotator.rotationRate){
			rotator.rotationTimer = 0;
			Vector3 rotAmount;
			if (clockwise){
				rotAmount = new Vector3(0, 0, -rotator.rotationIncrement);
			}
			else{
				
				rotAmount = new Vector3(0, 0, rotator.rotationIncrement);
			}
			
			return rotAmount;
			
		}
		else{
			return Vector3.zero;
		}
		
	}
	
	//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
	//<-------------COLLIDING ALL DAY!------------->
	//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	
	public class Collider2d{
		
		private float frictionFactor = 2;
		
		
		public Vector2 Collide(ContactPoint point, Vector2 velocity){
			
			Vector2 normal = point.normal;
			Debug.Log(point.normal);
			normal = new Vector2(-normal.y, normal.x);
			Vector2 newVel = 2 * normal * (normal.x * velocity.x + normal.y * velocity.y) - velocity;
			
			
			newVel /= frictionFactor;
			/*
			 *     xN = sin(degtorad(157.5));
				    yN = cos(degtorad(157.5));
				    if (canCol3){
				        tempx = x1 / magnitude;
				        tempy = y1 / magnitude;
				        
				        x1 = 2 * xN * (xN * tempx + yN * tempy) - tempx;
				        x1 *= magnitude / 2;
				        y1 = 2 * yN * (xN * tempx + yN * tempy) - tempy;
				        y1 *= magnitude / 2;
				        
				        canCol3 = false;
				    }  
				    else{
				        x += xN * timer3 / eighthPushModifier;
				        y -= yN * timer3 / eighthPushModifier;
				    }
			*/
			return newVel;
		
		}
	}
	
	//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
	//<------BORING UNITY'S BORING FUNCTIONS!------>
	//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>				
	
	
	// Use this for initialization
	void Start () {
		t = GetComponent<Transform>();
	}
	
	// Update is called once per frame, but doesn't want to call my other ones. I'm gonna make it!
	void Update () {
		
	}
	
	void OnGUI(){
		float x = Screen.width;
		float y = Screen.height;
		GUI.TextArea(new Rect(0, 0, 128, 64), "Resolution: " + x.ToString () + " x " + y.ToString());
	}
	
}

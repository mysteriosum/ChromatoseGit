using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAvatar : Avatar {
	
	int wTiming;
	int aTiming;
	int dTiming;
	
	
	public Transform prey;
	
	private Transform target;
	public bool guardDuty;
	public Transform guardPost;
	public int detectionRadius = 450;
	public bool patrol;
	public Transform[] patrolRoute;
	
	[System.NonSerialized]
	public GameObject questionBubble;
	bool returning;
	bool hunting;
	float checkTiming = 0.5f;
	float timer = 0f;
	
	List<Transform> myPath = new List<Transform>();
	int curNode = 0;
	bool onPath = false;
	int mask;		//for teh linecasts
	RaycastHit hit;
	
	ColourBeing colourTarget;
	Pather pather;
	
	// Use this for initialization
	void Start () {
		
		if (guardDuty){
			target = guardPost;
			if (!target){
				Debug.LogWarning("There's no post assigned to this guard! Fix thiiiiis");
				guardDuty = false;
			}
			if (!prey){
				GameObject avatarObject = GameObject.Find("Avatar");
				Debug.Log(avatarObject);
				prey = avatarObject.transform;
				colourTarget = prey.GetComponent<ColourBeing>();
			}
		}
		
		movement = GetComponent<Movement>();
		
		for (int i = 0; i < angles.Length; i++){
			angles[i] = i * 22.5f;
		}
		
		t = this.transform;
		
		
		getW = false;
		getA = false;
		getD = false;
		if (prey){
			pather = new Pather(t, prey);
		}
		
		mask = 1 << LayerMask.NameToLayer("collision");		//for teh linecasts
	}
	
	override public void Trigger(){
		Debug.Log("BOOM!");
		guardDuty = true;
		if (!target){
			prey = GameObject.FindGameObjectWithTag("avatar").transform;
			target = prey;
			pather = new Pather(t, prey);
		}
	}
	
	// Update is called once per frame
	void Update(){
		timer += Time.deltaTime;
		float multiplier = 1f;
		if (target != null){								//Here I'm going to figure out where I should be pointing, 
			float curRot = VectorFunctions.Convert360(t.rotation.eulerAngles.z); 	//how to turn there, and whether I should be accelerating
			Vector2 pointA = t.position;
			Vector2 pointB = target.position;
			Vector2 dist = pointB - pointA;
			float angle = VectorFunctions.PointDirection(dist);
			
			//float pointDir = Mathf.Rad2Deg * Mathf.Asin(dist.y / dist.magnitude);
			
			float diffAngle = curRot - angle;
			float targetAngle = diffAngle * 2;
			getD = targetAngle > -20;
			getA = targetAngle < 20;
			getW = true;
			if (target == guardPost && dist.magnitude < 75){
				multiplier = 0.2f;
			}
		}
		
		if (returning && target){
			Debug.Log("Returning, for some reason");
			if (Vector3.Distance(target.position, t.position) < 50){
				if (target == guardPost){
					returning = false;
				}
				else{
					curNode ++;
					target = myPath[curNode];
				}
				
			}
			
		}
		
		if (guardDuty){
			bool colourAppropriate = true;
			if (colourTarget){
				colourAppropriate = !colourTarget.colour.White;
			}
			if ((prey.position - t.position).magnitude < detectionRadius && colourAppropriate && !Physics.Linecast(prey.position, t.position, out hit, mask)){
				target = prey;
				hunting = true;
				returning = false;
				//Debug.Log("In ragne. GetW = " + getW + " getD = " + getD + " getA = " + getA);
			}
			
		}
		
		
		
		if (guardDuty && target && timer >= checkTiming && hunting){		//Update: is my target still in view?
			timer = 0;
			
			
			if (Physics.Linecast(target.position, t.position, out hit, mask)){
				target = null;
				Debug.Log("Something in the way of " + name);
				//Instantiate(questionBubble, t.position, Quaternion.identity);  TODO Implement!
				Invoke("ReturnToPost", 2f);
			}
		}
		if (target == null){
			getW = false;
			getA = false;
			getD = false;
			movement.SlowToStop();
		}
		//Debug.Log("GetW = " + getW + " getD = " + getD + " getA = " + getA);
	
		TranslateInputs(multiplier);		//From parent script: avatar
		
		
		
	}
	
	
	void OnCollisionEnter(Collision collider){
		if (collider.gameObject.tag == "avatar"){
			Avatar avatar = collider.gameObject.GetComponent<Avatar>();
			ReturnToPost();
			avatar.SetColour(0, 0, 0);
		}
	}
	
	public void NewPrey(GameObject newPrey){
		prey = newPrey.transform;
		colourTarget = prey.GetComponent<ColourBeing>();
		pather = new Pather(t, newPrey.transform);
	}
	
	void ReturnToPost(){
		returning = true;
		myPath = pather.NewPath(t, guardPost);
		curNode = 0;
		target = myPath[curNode];
		onPath = true;
	}
	
}
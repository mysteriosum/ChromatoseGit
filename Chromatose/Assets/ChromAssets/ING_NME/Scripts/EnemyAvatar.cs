using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0414 
public class EnemyAvatar : Avatar {
	
	
	public enum _BehaviourEnum{
		idle, patrol, idle_Follow, patrol_Follow, bossFight
	}
	
	public _BehaviourEnum behaviours;
	
	
	int wTiming;
	int aTiming;
	int dTiming;
	
	
	public Transform prey;
	
	private Transform target;
	public bool guardDuty;
	public Transform guardPost;
	public int detectionRadius = 450;
	public Transform[] patrolRoute;
	
	[System.NonSerialized]
	public GameObject questionBubble;
	private bool _CanFollow = false;
	private bool returning;
	private bool hunting;
	private float checkTiming = 0.5f;
	private float timer = 0f;
	
	private Vector3 initPosition;
	private Quaternion initRotation;
	
	
	private List<Transform> myPath = new List<Transform>();
	private int curNode = 0;
	private bool onPath = false;
	private int mask;		//for teh linecasts
	private RaycastHit hit;
	
	ColourBeing colourTarget;
	Pather pather;
	
	// Use this for initialization
	void Start () {
		
		
		switch(behaviours){
		case _BehaviourEnum.idle:
			
			break;
		case _BehaviourEnum.idle_Follow:
			
			break;
		case _BehaviourEnum.patrol:
			
			
			target = patrolRoute[0];
			
			break;
		case _BehaviourEnum.patrol_Follow:
			
			break;
		case _BehaviourEnum.bossFight:
			
			break;
			
		}
		
		
		spriteInfo = GetComponent<tk2dSprite>();
		movement = GetComponent<Movement>();
		t = this.transform;
		
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
				basicAccel = movement.thruster.accel;
				basicMaxSpeed = movement.thruster.maxSpeed;
				basicTurnSpeed = movement.rotator.rotationRate;
				movement.SetNewMoveStats(1f, 0.5f, basicTurnSpeed);
		}
		
		
		for (int i = 0; i < angles.Length; i++){
			angles[i] = i * 22.5f;
		}
		
		
		
		getW = false;
		getA = false;
		getD = false;
		if (prey){
			pather = new Pather(t, prey);
		}
		
		
		
		initPosition = t.position;
		initRotation = t.rotation;
		
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
		float multiplier = 1f;		//<--Rendre la variable solide pour eviter la depense du garbage Coellector
		
		switch(behaviours){
		case _BehaviourEnum.idle:
			
			bool colourAppropriate = true;
			if (colourTarget){
				colourAppropriate = !colourTarget.colour.White;
			}
			if ((prey.position - t.position).magnitude < detectionRadius && colourAppropriate && !Physics.Linecast(prey.position, t.position, out hit, mask)){
				target = prey;
				hunting = true;
				returning = false;
				movement.SetNewMoveStats(basicMaxSpeed, basicAccel, basicTurnSpeed);
				//Debug.Log("In ragne. GetW = " + getW + " getD = " + getD + " getA = " + getA);
			}
			
			break;
		case _BehaviourEnum.idle_Follow:
			
			break;
		case _BehaviourEnum.patrol:
			
			if (Vector3.Distance(target.position, t.position) < 50){
				if (target == guardPost){
					returning = false;
				}
				else{
					curNode ++;
					if (curNode == patrolRoute.Length) curNode = 0;
					target = patrolRoute[curNode];
					//Debug.Log("Shavatar targets " + curNode);
				}
				
			}
			
			break;
		case _BehaviourEnum.patrol_Follow:
			
			break;
		case _BehaviourEnum.bossFight:
			
			break;
			
		}
		

		if (target != null){								//Here I'm going to figure out where I should be pointing, 
			float curRot = VectorFunctions.Convert360(t.rotation.eulerAngles.z); 	//how to turn there, and whether I should be accelerating
			
			Vector2 pointA = t.position;
			Vector2 pointB = target.position;
			Vector2 dist = pointB - pointA;
			float angle = VectorFunctions.PointDirection(dist);
			
			//float pointDir = Mathf.Rad2Deg * Mathf.Asin(dist.y / dist.magnitude);
			
			float diffAngle = curRot - angle;
			float targetAngle = diffAngle * 2;
			
			getD = targetAngle > -25 || targetAngle < -550; 	//positive difference = getD. negative difference = getA.
			getA = targetAngle < 25 || targetAngle > 550;
			getW = true;
			if (target == guardPost && dist.magnitude < 75){
				multiplier = 0.2f;
			}
		}
		
		if (returning && target){
			
			if (Vector3.Distance(target.position, t.position) < 50){
				if (target == guardPost){
					returning = false;
					movement.SetNewMoveStats(1f, 0.5f, basicTurnSpeed);
				}
				else{
					curNode ++;
					target = myPath[curNode];
				}
			}
		}
		
		
		if (guardDuty){
			
			
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
			if (guardDuty) ReturnToPost();
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
	
	void CheckAvatarColor(){
		if(prey.GetComponent<Avatar>().AvatarColor != Color.white){
			_CanFollow = true;
		}
		else{
			_CanFollow= false;
		}
	}
	
	void OnDrawGizmosSelected(){
		if (behaviours != _BehaviourEnum.patrol || behaviours != _BehaviourEnum.patrol_Follow) return;
		int maxIndex = patrolRoute.Length;
		if (maxIndex <= 1) return;
		int index = 0;
		
		int nextIndex = 1;
		for(int i = 0; i < maxIndex; i++){
			Gizmos.DrawLine(patrolRoute[index].position, patrolRoute[nextIndex].position);
			
			index = (index + 1) % maxIndex;
			nextIndex = (index + 1) % maxIndex;
		}
	}
	
}
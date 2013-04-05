using UnityEngine;
using System.Collections;

public class EnemyAvatar : Avatar {
	int wTimer;			//TEST so don't keep it forever
	int aTimer;
	int dTimer;
	
	int wTiming;
	int aTiming;
	int dTiming;
	
	
	public Transform prey;
	private Transform target;
	public bool seekingTarget;
	Pather pather;
	
	// Use this for initialization
	void Start () {
		
		
		
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
			target = prey;		//TODO: make it selective; if I can't find my prey, I'll look for a node. 
		}
		
		/*
		wTimer = Random.Range(5, 15);
		wTiming = wTimer;
		
		aTimer = 0;
		aTiming = Random.Range(10, 30);
		
		dTimer = 0;
		dTiming = Random.Range(10, 30);
		*/
	}
	override public void Trigger(){
		Debug.Log("BOOM!");
		seekingTarget = true;
		if (!target){
			prey = GameObject.FindGameObjectWithTag("avatar").transform;
			target = prey;
			pather = new Pather(t, prey);
		}
	}
	
	// Update is called once per frame
	void Update(){
		
		if (seekingTarget){								//Here I'm going to figure out where I should be pointing, 
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
		}
		
		if (seekingTarget && pather != null){
			target = pather.Seek(target);
			if (target == null){
				seekingTarget = false;
				getW = false;
				getA = false;
				getD = false;
				movement.SlowToStop();
			}
		}
		
		TranslateInputs();		//From parent script: avatar
		
		
		
	}
	
	
	void OnCollisionEnter(Collision collider){
		if (collider.gameObject.tag == "avatar"){
			Avatar avatar = collider.gameObject.GetComponent<Avatar>();
			
			avatar.SetColour(0, 0, 0);
		}
	}
	
}
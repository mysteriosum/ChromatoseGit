using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shavatar : MonoBehaviour {

	public enum _BehaviourEnum{
		idle, patrol, idle_Follow, patrol_Follow, bossFight
	}
	
	public _BehaviourEnum behaviours;
	public bool canCharge = false;
	public bool backToIdle = false;
	public int zoneDetectionFollow = Mathf.Clamp(300, 100, 500);
	public int zoneDetectionCharge = Mathf.Clamp(100, 50, 250);
	public float shavatarSpeed = Mathf.Clamp(1, -1, 3);
	
	public Transform[] patrolPath;
	
	private Vector3 _Target;
	private Transform t;
	private Transform ShavatarT;
	private GameObject _Avatar;
	
	private Transform _InitTransform;
	private Vector3 _InitPosition;
	private Quaternion _InitRotation;
	
	private ColourBeing _ColourTarget;
	private Movement _Movement;
	private tk2dSprite _SpriteInfo;
	private LayerMask _ColLayerMask;
	private RaycastHit _Hit;
	private RaycastHit _Hit2;
	private bool colourAppropriate = false;
	
	
	private Vector3 targetPoint;
	private Quaternion rotation;
	private float _AccelSpeed = 1;
	private bool _OnCollision = false;
	public bool OnCollision{
		get{return _OnCollision;}
		set{_OnCollision = value;}
	}
	
	//Variable "Charge"
	private bool _InCharge = false;
	private float _InitSpeed = 1f;
	private float _SpeedInCharge = 2.5f;
	private float _ChargeTime = 1.0f;
	private float _DistanceToStop = 75f;
	
	//Variable Patrol
	private int _CurIndex = 0;
	private float _DistToTurn = 10;
	private float rotCounter = 0;
	private bool gonnaRotate = false;
	private bool clockwise;
	
	//Famille
	public SphereCollider _DetectionCollider;
	public SphereCollider _CollisionDetectionCollider;
	private ShavatarDetectionScript _DetectionScript;
	

	
	void Start () {

		Setup ();
		
		#region Switch Init
		switch(behaviours){
		case _BehaviourEnum.idle:
			if(canCharge){
				canCharge = false;
				Debug.LogWarning("MSG DU CHU; Tu peux pas Charge en Idle, juste en Follow ou FollowPatrol");
			}
			break;
			
		case _BehaviourEnum.idle_Follow:
			break;
			
		case _BehaviourEnum.patrol:
			if(patrolPath == null){
				behaviours = _BehaviourEnum.idle;
			}
			if(canCharge){
				canCharge = false;
				Debug.LogWarning("MSG DU CHU; Tu peux pas Charge en IdlePatrol, juste en FollowPatrol ou Follow");
			}
			
			_Target = patrolPath[_CurIndex].position;
			
			break;
			
		case _BehaviourEnum.patrol_Follow:
			if(patrolPath == null){
				behaviours = _BehaviourEnum.idle;
			}
			_Target = patrolPath[_CurIndex].position;
			
			break;
			
		case _BehaviourEnum.bossFight:
			break;
		}
		#endregion
		
		//eyeOfTheChu = new Eye(this.transform, eyeCollection);
		
	}
	void Update () {
		
		#region Idle
		switch(behaviours){
		case _BehaviourEnum.idle:
			
			if(!_DetectionScript.AvatarDetected)return;
			
			LookAtAvatar();
	
			break;
		#endregion
			
		#region IdleFollow Update			
		case _BehaviourEnum.idle_Follow:
			
			bool colourAppropriate = true;
			if (_ColourTarget){
				colourAppropriate = _Avatar.GetComponent<Avatar>().curColor != Color.white;
			}
			
			
			if(!_DetectionScript.AvatarDetected && Vector2.Distance(t.position, _InitPosition) < 20)return;
			else{
				if(!_DetectionScript.AvatarDetected && Vector2.Distance(t.position, _InitPosition) > 25){
					if(backToIdle){
						BackToIdlePos();
						if (Vector2.Distance(t.position, _InitPosition) < 25){
							_InitPosition = t.position;
						}
					}
					else{
						_InitPosition = t.position;
					}
				}
			}
			
			if(colourAppropriate){
				if(!_InCharge && _DetectionScript.AvatarDetected){
					LookAtAvatar();
				}
				
				if(!Physics.Linecast(_Avatar.transform.position, t.position, out _Hit, _ColLayerMask)){
					Forward();
					if(canCharge){
						if(Vector2.Distance(_Avatar.transform.position, t.position) < zoneDetectionCharge && !_InCharge){
							_InCharge = true;
							StartCoroutine(ResetSpeedAfterCharge(_ChargeTime, shavatarSpeed));
							shavatarSpeed = _SpeedInCharge;
						}
					}
				}
			}
			else{
				if(Vector2.Distance(t.position, _InitPosition) > 25){
					if(backToIdle){
						BackToIdlePos();
						if (Vector2.Distance(t.position, _InitPosition) < 25){
							_InitPosition = t.position;
						}
					}
					else{
						_InitPosition = t.position;
					}
				}
			}
			
			break;
			#endregion
			
		#region Patrol
		case _BehaviourEnum.patrol:
			
			LookAtTarget();
			Forward();
						
			break;
		#endregion
			
		#region PatrolFollow
		case _BehaviourEnum.patrol_Follow:
			
			
			colourAppropriate = true;
			if (_ColourTarget){
				colourAppropriate = _Avatar.GetComponent<Avatar>().curColor != Color.white;
			}
			
			
		//	if(colourAppropriate){}
				
					
			
			if(!_DetectionScript.AvatarDetected || !colourAppropriate){
				LookAtTarget();
				Forward();			
			}
			else{
				if(!_InCharge){
					LookAtAvatar();
				}
			
				if(!Physics.Linecast(_Avatar.transform.position, t.position, out _Hit, _ColLayerMask)){
					Forward();
					if(canCharge){
						if(Vector2.Distance(_Avatar.transform.position, t.position) < zoneDetectionCharge && !_InCharge){
							_InCharge = true;
							StartCoroutine(ResetSpeedAfterCharge(_ChargeTime, shavatarSpeed));
							shavatarSpeed = _SpeedInCharge;
						}
					}
				}				
			}				
			
			break;
		#endregion
			
		case _BehaviourEnum.bossFight:
			break;
		}
	}
	
	
	void Setup(){
		_Avatar = GameObject.FindGameObjectWithTag("avatar");
		t = gameObject.transform;
		//_ShavatarT
		_ColourTarget = _Avatar.GetComponent<ColourBeing>();
		_SpriteInfo = GetComponentInChildren<tk2dSprite>();
		_Movement = GetComponent<Movement>();
		
		_DetectionScript = _DetectionCollider.GetComponent<ShavatarDetectionScript>();
		_DetectionScript.DetectionZone = zoneDetectionFollow;
		
		//Setup des Pos/RotatorScript Init.
		_InitTransform = t;
		_InitPosition = t.position;
		_InitRotation = t.rotation;
		_InitSpeed = shavatarSpeed;
		
		_ColLayerMask = 1 << LayerMask.NameToLayer("collision");		//for teh linecasts
				
	}
	
	/*
	void CalculAngle(){
		_CurRotAngle = VectorFunctions.Convert360(t.rotation.eulerAngles.z); 	//how to turn there, and whether I should be accelerating
		_PointA = t.position;
		_PointB = _Avatar.transform.position;
		_Dist = _PointB - _PointA;
		_Angle = VectorFunctions.PointDirection(_Dist);
		
		_DiffAngle = _CurRotAngle - _Angle;
		_TargetAngle = _DiffAngle * 2;
	}*/
	
	void BackToIdlePos(){
		//TODO Refaire le StopFollowing du SHAVATAR
		_Target = _InitPosition;
		LookAtTarget();
		Forward();
		
	}
	
	void Accel(){
		//TODO Faire Acceleration pour le mouvement
	}
	
	void LookAtTarget(){

		float curRot = VectorFunctions.Convert360(t.rotation.eulerAngles.z); 	//how to turn there, and whether I should be accelerating
			
		Vector2 pointA = t.position;
		Vector2 pointB = _Target;
		Vector2 dist = pointB - pointA;
		float angle = VectorFunctions.PointDirection(dist);
		
		
		float diffAngle = curRot - angle;
		float targetAngle = diffAngle * 2;
		
		Vector3 temprel = transform.InverseTransformPoint(_Target);
		
		Debug.Log (transform.InverseTransformPoint(_Target).x > 0);
		
		
		
		//Debug.Log(targetAngle);
		/*
		if(targetAngle > -25 || targetAngle < -550){
			transform.Rotate(new Vector3(0, 0, -1f));
			//transform.Rotate(_Movement.Rotate(false));
		}
		if(targetAngle < 25 || targetAngle > 550){
			transform.Rotate(new Vector3(0, 0, 1f));
			//transform.Rotate(_Movement.Rotate(true));
		}*/
		
		bool getD = targetAngle > -25 || targetAngle < -550;
		bool getA = targetAngle < 25 || targetAngle > 550;
	
		
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
		
		if (gonnaRotate){
			t.Rotate(this._Movement.Rotate(clockwise));
		}
		
		if(getA){
			_SpriteInfo.SetSprite("Player5");
			rotCounter--;
		}
		if(getD){
			_SpriteInfo.SetSprite("Player3");
			rotCounter++;
		}
	
		gonnaRotate = false;
		
		/*
		
		if(getA){
			transform.Rotate(new Vector3(0, 0, -0.5f));
		}
		else if(getD){
			transform.Rotate(new Vector3(0, 0, 0.5f));
		}*/
		
		targetPoint = _Target;
		//targetPoint.z = transform.position.z;
		rotation = Quaternion.LookRotation(targetPoint - t.position);
		rotation.x = 0;
		rotation.y = 0;
		//Quaternion realRot = Quaternion.Euler(rotation.eulerAngles.x, , 0f);
		//t.rotation = Quaternion.Slerp(t.rotation, rotation, Time.deltaTime * 5.0f);
		//transform.LookAt(targetPoint);
		
		
		if(Vector2.Distance(transform.position, _Target) < 10){
			_Target = patrolPath[_CurIndex].position;
			_CurIndex++;
			if (_CurIndex == patrolPath.Length){
				_CurIndex = 0;
			}
		}
	}
	
	
	void LookAtAvatar(){
		targetPoint = _Avatar.transform.position;
		targetPoint.z = transform.position.z;
		transform.LookAt(targetPoint);
	}
	
	void Forward(){
		if(_InCharge && _OnCollision){return;}		
		transform.Translate(Vector3.right * shavatarSpeed * _AccelSpeed, Space.Self);
		//transform.position = Vector2.Lerp(transform.position, _Target, Time.deltaTime);
	}
	
	
	
	void OnCollisionEnter(Collision collider){
		if(collider.gameObject.tag == "avatar"){
			//_Avatar.GetComponent<Avatar>().LoseAllColour();
			_Avatar.GetComponent<Avatar>().EmptyingBucket();
			StopAllCoroutines();
			shavatarSpeed = _InitSpeed;
			_InCharge = false;
				
		}
	}

	
	IEnumerator ResetSpeedAfterCharge(float delay, float initSpeed){
		yield return new WaitForSeconds(delay);
		shavatarSpeed = initSpeed;
		StartCoroutine(DelaiCanCharge());
	}
	IEnumerator DelaiCanCharge(){
		yield return new WaitForSeconds(2f);
		_InCharge = false;
	}
}

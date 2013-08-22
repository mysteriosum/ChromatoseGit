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
				colourAppropriate = !_ColourTarget.colour.White;
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
				colourAppropriate = !_ColourTarget.colour.White;
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
		_ColourTarget = _Avatar.GetComponent<ColourBeing>();
		_SpriteInfo = GetComponent<tk2dSprite>();
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
		targetPoint = _Target;
		targetPoint.z = transform.position.z;
		transform.LookAt(targetPoint);
		
		if(Vector2.Distance(transform.position, _Target) < _DistToTurn){
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
		transform.Translate(Vector3.forward * shavatarSpeed * _AccelSpeed, Space.Self);
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shavatar1 : MonoBehaviour {

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
	
	private Transform _InitTransform;
	private Vector3 _InitPosition;
	private Quaternion _InitRotation;
	
	private Color _ColourTarget;
	private Movement _Movement;
	private tk2dAnimatedSprite _MainAnim;
	private LayerMask _ColLayerMask;
	private RaycastHit _Hit;
	private RaycastHit _Hit2;
	private bool colourAppropriate = false;
	private bool setuped = false;
	
	
	private Vector3 targetPoint;
	private float _AccelSpeed = 1;
	private bool _OnCollision = false;
	private bool alreadyTurn = false;
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
	
	//Management
	private GameObject _Avatar;
	private Avatar _AvatarScript;
	
	void Start () {
		
		t = gameObject.transform;
		_MainAnim = GetComponentInChildren<tk2dAnimatedSprite>();
		_Movement = GetComponent<Movement>();
		
		_DetectionScript = _DetectionCollider.GetComponent<ShavatarDetectionScript>();
		_DetectionScript.DetectionZone = zoneDetectionFollow;
		
		//Setup des Pos/RotatorScript Init.
		_InitTransform = t;
		_InitPosition = t.position;
		_InitRotation = t.rotation;
		_InitSpeed = shavatarSpeed;
		
		_ColLayerMask = 1 << LayerMask.NameToLayer("collision");		//for teh linecasts
		
		
	
		StartCoroutine(Setup());
		
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
	}
	void FixedUpdate () {
	
		if(!GameObject.FindGameObjectWithTag("avatar"))return;
		
		if(!_Avatar){
			_Avatar = GameObject.FindGameObjectWithTag("avatar");
			_AvatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		}
		
		#region Idle
		switch(behaviours){
		case _BehaviourEnum.idle:
			
			if(_DetectionScript && !_DetectionScript.AvatarDetected)return;
			
			LookAtAvatar();
	
			break;
		#endregion
			
		#region IdleFollow Update			
		case _BehaviourEnum.idle_Follow:
			
			
			if(_Avatar.GetComponent<Avatar>().curColor != Color.white){
				colourAppropriate = true;
			}
			else{
				colourAppropriate = false;
			}
			
			
			if(_DetectionScript && !_DetectionScript.AvatarDetected && Vector2.Distance(t.position, _InitPosition) < 20)return;
			else{
				if(_DetectionScript && !_DetectionScript.AvatarDetected && Vector2.Distance(t.position, _InitPosition) > 25){
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
				if(t){
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
			
			if(_Avatar.GetComponent<Avatar>().curColor != Color.white){
				colourAppropriate = true;
			}
			else{
				colourAppropriate = false;
			}
			if(_DetectionScript!=null){
				if(!_DetectionScript.AvatarDetected || !colourAppropriate){
					LookAtTarget();
					Forward();			
				}
				else{
					if(!_InCharge){
						LookAtAvatar();
					}
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
	
	IEnumerator Setup(){
		yield return new WaitForSeconds(0.1f);
		_ColourTarget = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().curColor;
		
		switch(_AvatarScript.avaTypeAccess){
		case _AvatarTypeEnum.avatar:
			_MainAnim.Play("Shavatar");
			break;
		case _AvatarTypeEnum.shavatar:
			_MainAnim.Play("Avatar");
			break;
		}
		
		setuped = true;
	}
	
	public void ReSetupAnim(){
		switch(_AvatarScript.avaTypeAccess){
		case _AvatarTypeEnum.avatar:
			_MainAnim.Play("Shavatar");
			break;
		case _AvatarTypeEnum.shavatar:
			_MainAnim.Play("Avatar");
			break;
		}
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
		
		if(Vector2.Distance(transform.position, _Target) < _DistToTurn + 5){
			if(!alreadyTurn){
				PlayTurnAnim();
				alreadyTurn = true;
				StartCoroutine(DelaiResetTurn());
			}
		}
		
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
	}
	
	
	
	void OnCollisionEnter(Collision collider){
		if(collider.gameObject.tag == "avatar"){
			_Avatar.GetComponent<Avatar>().EmptyingBucket();
			StopAllCoroutines();
			shavatarSpeed = _InitSpeed;
			_InCharge = false;
			if(!alreadyTurn){
				PlayTurnAnim();
				alreadyTurn = true;
				StartCoroutine(DelaiResetTurn());
			}
		}
	}
	
	void PlayTurnAnim(){
		switch(_AvatarScript.avaTypeAccess){
		case _AvatarTypeEnum.avatar:
			if(!_MainAnim.IsPlaying("ShavaRotation")){
				_MainAnim.SetFrame(0);
				_MainAnim.Play("ShavaRotation");
				_MainAnim.animationCompleteDelegate = ReturnToIdleAnim;
			}
			break;
		case _AvatarTypeEnum.shavatar:
			if(!_MainAnim.IsPlaying("AvataRotation")){
				_MainAnim.SetFrame(0);
				_MainAnim.Play("AvataRotation");
				_MainAnim.animationCompleteDelegate = ReturnToIdleAnim;
			}
			break;
		}
	}
	
	void ReturnToIdleAnim(tk2dAnimatedSprite sprite, int clipId){
		switch(_AvatarScript.avaTypeAccess){
		case _AvatarTypeEnum.avatar:
			_MainAnim.SetFrame(0);
			_MainAnim.Play("Shavatar");
			break;
		case _AvatarTypeEnum.shavatar:
			_MainAnim.SetFrame(0);
			_MainAnim.Play("Avatar");
			break;
		}
	}
	
	IEnumerator ResetSpeedAfterCharge(float delay, float initSpeed){
		yield return new WaitForSeconds(delay);
		shavatarSpeed = initSpeed;
		StartCoroutine(DelaiCanCharge());
		StartCoroutine(DelaiTOChargeTurn());
	}
	IEnumerator DelaiCanCharge(){
		yield return new WaitForSeconds(2f);
		_InCharge = false;
	}
	IEnumerator DelaiResetTurn(){
		yield return new WaitForSeconds(1.0f);
		alreadyTurn = false;
	}
	IEnumerator DelaiTOChargeTurn(){
		yield return new WaitForSeconds(0.25f);
		if(!alreadyTurn){
			PlayTurnAnim();
			alreadyTurn = true;
			StartCoroutine(DelaiResetTurn());
			Debug.Log("Play");
		}
	}
}

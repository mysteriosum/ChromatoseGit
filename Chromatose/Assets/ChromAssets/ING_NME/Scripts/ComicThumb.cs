using UnityEngine;
using System.Collections;


#pragma warning disable 0414 // private field assigned but not used.

//TODO Faire menage dans le script ComicThumb
//Mettre pragma en commentaire pour voir
public class ComicThumb : MonoBehaviour {
	
	public int myIndex;
	
	private ChromatoseManager _Manager;
	private Camera _Camera;
	private GameObject _Moi = null;
	
	private tk2dAnimatedSprite anim;
	private tk2dAnimatedSprite myCircle;
	
	private float _TargetXOffset = 50f;
	private float _TargetYOffset = 20f;
	private float _MaxVitesseDelta = 150f;
	private float _GoBiggerRate = 0.05f;
	private float _GoSmallerRate = -0.05f;
	private float _Distance = 0f;
	private float _DistanceConverti = 0f;
	private float _DistanceRemaining = 0f;
	private float _DistanceDepart = 0f;
	
	
	private bool _CanGoBig = false;
	private bool _CanGoToHUD = false;
	private bool _InTheTrigger = false;
	private bool _AnimationComplete = false;
	private bool _Alive = true;
	private bool _CanGoSmall = false;
	private bool _Destroyed = false;
	private bool _FollowHUD = false;
	private bool _DejaTiers = false;
	
	private Vector3 _MyPos = new Vector3(0,0,0);
	private Vector3 _HUDPos = new Vector3(0,0,0);
	private Vector3 _MyStartPos = new Vector3(0,0,0);
	
	//Slerp Variable
	private float _StartTime = 0;
	private float journeyTime = 25.0F;
	private float _SlerpSpeedRate = 1.0f;

	void Start () {
		_Manager = ChromatoseManager.manager;
		_Moi = this.gameObject;
		_Camera = Camera.mainCamera;
		
		anim = GetComponent<tk2dAnimatedSprite>();
		anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
		myCircle = (Instantiate(Resources.Load("animref_ing2")) as GameObject).GetComponent<tk2dAnimatedSprite>();
		myCircle.Play("comicThumb_pickedUp");
		myCircle.Stop();
		myCircle.transform.position = transform.position;
		myCircle.renderer.enabled = false;
		
		_Alive = true;
		_CanGoBig = false;
		_CanGoSmall = false;
		_CanGoToHUD = false;
	}

	void FixedUpdate () {
				
		if(_CanGoBig){ThumbGoBigger();}
		if(_CanGoSmall){ThumbGoSmaller();}
		
		if(_FollowHUD){
			CalculatePosition();
			CalculateHUDPosition();
			CalculeDistance();
			
			Vector3 center = (_MyPos + _HUDPos) * 0.45F;
	        center -= new Vector3(0, 1f, 0);
	        Vector3 riseRelCenter = _MyPos - center;
	        Vector3 setRelCenter = _HUDPos - center;
	        float fracComplete = (Time.time - _StartTime) / journeyTime;
	        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete * _SlerpSpeedRate);
	        transform.position += center;
			
			anim.MakePixelPerfect(); 	
		}
		
		if(Vector3.Distance(_MyPos, _HUDPos) < (Vector3.Distance(_MyStartPos, _HUDPos)/2) && _CanGoBig){
			_CanGoBig = false;
			_CanGoSmall = true;
		}
		else{ 
	
		}
		
		if(transform.localScale.x < 0.1){
			StartCoroutine(DestroyWhenImBig(0f));
		}
		
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag != "avatar") {return;}
		else{
			_InTheTrigger = true;
			if(!_CanGoBig && !_CanGoSmall && !_CanGoToHUD){
				myCircle.renderer.enabled = true;
				myCircle.Play("comicThumb_pickedUp");
				myCircle.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
				myCircle.animationCompleteDelegate = Trigger;
			}
		}
	}
	
	void OnTriggerExit(Collider other){
		if (other.tag != "avatar") {return;}
		else{
			_InTheTrigger = false;
			myCircle.renderer.enabled = false;
			myCircle.StopAndResetFrame();
		}
	}
	
	void Trigger(tk2dAnimatedSprite spranim, int index){
		
		//TOFIX Corriger le probleme que certains thumbs reussissent a s'ajouter avant la fin de l'anim
		
		//ChromatoseManager.manager.FindComic(myIndex);
		
		myCircle.renderer.enabled = false;
		
		ThumbStartGoBig();
	}
	
	void ThumbStartGoBig(){
		
		transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		
		CalculatePosition();
		CalculateHUDPosition();
		CalculateStartPosition();
		
		_StartTime = Time.time;
		_FollowHUD = true;
		
		_CanGoToHUD = true;
		_CanGoBig = true;
	}
	void ThumbGoBigger(){
		transform.localScale = new Vector3(transform.localScale.x + _GoBiggerRate, transform.localScale.y + _GoBiggerRate, transform.localScale.z);
	}
	void ThumbGoSmaller(){
		transform.localScale = new Vector3(transform.localScale.x + _GoSmallerRate, transform.localScale.y + _GoSmallerRate, transform.localScale.z);
	}
	void CalculatePosition(){
		_MyPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
	}
	void CalculateStartPosition(){
		_MyStartPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		CalculeDistanceDepart();
	}
	void CalculateHUDPosition(){
		_HUDPos = _Camera.GetComponent<ChromatoseCamera>()._HUDHelper_Comics.transform.position;
	}
	void CalculeDistance(){
		_DistanceRemaining = Vector3.Distance(_MyPos, _HUDPos);
	}	
	void CalculeDistanceDepart(){
		_DistanceDepart = Vector3.Distance(_MyStartPos, _HUDPos);
	}	
	IEnumerator DestroyWhenImBig(float _time){
		yield return new WaitForSeconds(_time);
		_FollowHUD = false;
		_CanGoSmall = false;
		
		if(!_Destroyed){
			_Moi.transform.Translate(Vector3.forward * -3000);	
			ChromatoseManager.manager.FindComic(myIndex);
			_Destroyed = true;
		}
	}
	
}

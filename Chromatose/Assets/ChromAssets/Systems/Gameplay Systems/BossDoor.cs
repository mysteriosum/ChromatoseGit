using UnityEngine;
using System.Collections;

public class BossDoor : MonoBehaviour {

	private int requiredPayment = 0;
	private int minBefOpenShop = 47;
	
	public Vector3 positionOffSet;
	public Vector3 rotationOffSet;
	
	public BoxCollider myCollider;
	public BoxCollider myTriggerZone;
	protected Transform colliderT;
	protected Transform avatarT;
	protected ChromatoseManager chroManager;
	protected Transform collisionChild;
	protected tk2dAnimatedSprite anim;
	protected Color myColor;
	
	private tk2dAnimatedSprite indicator;
	private string inString;
	private string outString;
	private int avatarCloseDist = 350;
	private bool isOut = false;
	private bool isIn = true;
	private bool setuped = false;
	private bool inZone = false;
	
	protected bool triggered = false;
	protected bool waiting = false;
	// Use this for initialization
	void Start () {
		myColor = Color.red;
		inString = "bossFlagIn_47";
		outString = "bossFlagOut_47";

		//Debug.Log("Got a collider " + myCollider.name + " and a transform: " + colliderT.name);
		
		anim = GetComponent<tk2dAnimatedSprite>();
		
		Quaternion indicRotation = Quaternion.identity;
		indicRotation.eulerAngles = new Vector3(0 + rotationOffSet.x, 0 + rotationOffSet.y, -90 + rotationOffSet.z);
		
		indicator = (Instantiate(Resources.Load("pre_shopIndicator"), transform.position + new Vector3(-25 + positionOffSet.x, 0 + positionOffSet.y, 0 + positionOffSet.z - 1), indicRotation) as GameObject).GetComponent<tk2dAnimatedSprite>();
		indicator.renderer.enabled = false;
		
	}
	
	void Setup(){
		chroManager = ChromatoseManager.manager;
		avatarT = GameObject.FindGameObjectWithTag("avatar").GetComponent<Transform>();
		setuped = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!setuped)Setup ();
		inZone = myTriggerZone.GetComponent<ShopDetection>().onDetectZone;
		
		if(avatarT != null){
			if (triggered) return;

			float dist = Vector3.Distance(avatarT.position, transform.position);
			if (inZone && isIn){
				StartOut();
				if(StatsManager.redCollDisplayed >= minBefOpenShop){
					StartIn();
					waiting = true;
					triggered = true;
					MusicManager.soundManager.PlaySFX(46);
				}
			}
			else if (!inZone && isOut){
				HUDManager.hudManager.OffAction ();
				StartIn();
			}
		}
	}

	
	void Animate(){
		anim.Play();
		anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
	}
	
	void StartOut(){
		indicator.Play(outString);
		indicator.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
		isIn = false;
		indicator.animationCompleteDelegate = Out;
		indicator.renderer.enabled = true;
	}
	
	virtual protected void StartIn(){
		indicator.Play(inString);
		indicator.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
		isOut = false;
		indicator.animationCompleteDelegate = In;
	}
	
	void Out(tk2dAnimatedSprite sprite, int index){
		isOut = true;
	}
	
	void In(tk2dAnimatedSprite sprite, int index){
		isIn = true;
		indicator.renderer.enabled = false;
		if (triggered){
			waiting = false;
			//collisionChild.gameObject.SetActive(false);
			HUDManager.hudManager.OffAction ();
			myCollider.enabled = false;
			if (anim)
				Animate();
		}
	}
}

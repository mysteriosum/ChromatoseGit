using UnityEngine;
using System.Collections;

public class whiteTollBooth : MonoBehaviour {
	public int requiredPayment = 1;
	
	protected BoxCollider myCollider;
	protected Transform colliderT;
	protected Transform avatarT;
	protected ChromatoseManager chroManager;
	protected Transform collisionChild;
	protected tk2dAnimatedSprite anim;
	protected Color myColor;
	
	private tk2dAnimatedSprite indicator;
	private string inString;
	private string outString;
	private int avatarCloseDist = 150;
	private bool isOut = false;
	private bool isIn = true;
	
	protected bool triggered = false;
	protected bool waiting = false;
	// Use this for initialization
	void Start () {
		myColor = Color.white;
		
		Setup();
	}
	
	protected void Setup(){
		chroManager = ChromatoseManager.manager;
		avatarT = GameObject.FindGameObjectWithTag("avatar").GetComponent<Transform>();
		//Debug.Log("Did I find avatar? " + avatarT.name);
		BoxCollider[] chilluns = gameObject.GetAllComponentsInChildren<BoxCollider>();
		foreach (BoxCollider c in chilluns){
			if (c.isTrigger){
				myCollider = c;
			}
			
			if (c.gameObject.layer == LayerMask.NameToLayer("collision")){
				collisionChild = c.transform;
			}
		}
		colliderT = myCollider.transform;
		//Debug.Log("Got a collider " + myCollider.name + " and a transform: " + colliderT.name);
		
		anim = GetComponent<tk2dAnimatedSprite>();
		
		Quaternion indicRotation = Quaternion.identity;
		indicRotation.eulerAngles = new Vector3(0, 0, -90);
		
		indicator = (Instantiate(Resources.Load("pre_tollIndicator"), transform.position + new Vector3(-25, 0, 0), indicRotation) as GameObject).GetComponent<tk2dAnimatedSprite>();
		indicator.renderer.enabled = false;
		inString = "boothWhiteIn_" + requiredPayment.ToString();
		outString = "boothWhiteOut_" + requiredPayment.ToString();
		
	}
	
	// Update is called once per frame
	void Update () {
		Check(Actions.whitePay);
		
		if (triggered) return;
		
		//string clipName = indicator.CurrentClip == null? "" : indicator.CurrentClip.name;
		float dist = Vector3.Distance(avatarT.position, transform.position);
		if (dist < avatarCloseDist && isIn){
			StartOut();
		}
		else if (dist > avatarCloseDist && isOut){
			StartIn();
		}
		
	}
	
	protected bool Check(Actions action){
		if (triggered){
			
			return false;
		}
		if (myCollider.bounds.Contains(avatarT.position)){
			chroManager.UpdateAction(action, whitePay);
			return true;
		}
		return false;
	}
	
	void whitePay(){
		if (chroManager.GetCollectibles(myColor) >= requiredPayment){
			
			StartIn();
			waiting = true;
			triggered = true;
		
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
			collisionChild.gameObject.SetActive(false);
			chroManager.RemoveCollectibles(Color.white, requiredPayment, avatarT.position);
			if (anim)
				Animate();
		}
	}
}

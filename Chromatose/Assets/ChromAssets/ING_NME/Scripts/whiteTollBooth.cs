using UnityEngine;
using System.Collections;

public class whiteTollBooth : MonoBehaviour {
	public int requiredPayment = 1;
	
	public AudioClip locked;
	public AudioClip unlocked;
	public AudioClip open;
	
	public Vector3 positionOffSet;
	public Vector3 rotationOffSet;
	
	protected BoxCollider myCollider;
	protected Transform colliderT;
	protected Transform avatarT;
	protected ChromatoseManager chroManager;
	protected Transform collisionChild;
	protected tk2dAnimatedSprite anim;
	protected Color myColor;
	
	private tk2dAnimatedSprite indicator;
	private AudioSource sfxPlayer;
	private string inString;
	private string outString;
	private int avatarCloseDist = 150;
	private bool isOut = false;
	private bool isIn = true;
	private bool setuped = false;
	
	protected bool triggered = false;
	protected bool waiting = false;
	// Use this for initialization
	void Start () {
		myColor = Color.white;
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
		indicRotation.eulerAngles = new Vector3(0 + rotationOffSet.x, 0 + rotationOffSet.y, -90 + rotationOffSet.z);
		
		indicator = (Instantiate(Resources.Load("pre_tollIndicator"), transform.position + new Vector3(-25 + positionOffSet.x, 0 + positionOffSet.y, 0 + positionOffSet.z - 1), indicRotation) as GameObject).GetComponent<tk2dAnimatedSprite>();
		indicator.renderer.enabled = false;
		inString = "boothWhiteIn_" + requiredPayment.ToString();
		outString = "boothWhiteOut_" + requiredPayment.ToString();
	}
	
	void Setup(){
		chroManager = ChromatoseManager.manager;
		sfxPlayer = GetComponent<AudioSource>();
		avatarT = GameObject.FindGameObjectWithTag("avatar").GetComponent<Transform>();
		setuped = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!setuped)Setup ();

		if(avatarT != null){
			Check(Actions.WhitePay);
			
			if (triggered) return;

			float dist = Vector3.Distance(avatarT.position, transform.position);
			if (dist < avatarCloseDist && isIn){
				StartOut();
			}
			else if (dist > avatarCloseDist && isOut){
				StartIn();
			}
		}
	}
	
	protected bool Check(Actions action){
		if (triggered){
			return false;
		}
		if (myCollider.bounds.Contains(avatarT.position)){
			HUDManager.hudManager.UpdateAction(action, whitePay);
			return true;
		}
		return false;
	}
	
	void whitePay(){
		if (chroManager.GetCollectibles(myColor) >= requiredPayment){
			
			StartIn();
			waiting = true;
			triggered = true;
			sfxPlayer.clip = unlocked;
			sfxPlayer.Play();
		}
		else{
			sfxPlayer.clip = locked;
			sfxPlayer.Play();
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

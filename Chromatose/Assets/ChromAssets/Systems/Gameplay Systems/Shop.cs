using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {
	
	public enum requiredCollTypeEnum{
		White, Blue
	}
	
	public requiredCollTypeEnum requiredCollType;
	
	public Vector3 positionOffSet;
	public Vector3 rotationOffSet;
	
	public int requiredPayment;
	public int minCollRequBefOpenShop;
	
	private bool inZone;
	private BoxCollider blocker;
	private tk2dAnimatedSprite indicator;
	private string inString;
	private string outString;
	private int avatarCloseDist = 150;
	private bool isOut = false;
	private bool isIn = true;
	
	protected Transform collisionChild;
	protected tk2dAnimatedSprite anim;
	protected Color myColor;
	private BoxCollider myCollider;
	//protected Transform colliderT;
	protected Transform avatarT;
	protected ChromatoseManager chroManager;
	
	protected bool triggered = false;
	protected bool waiting = false;
	
	void Start () {
		StartCoroutine(LateSetup(0.1f));
		
		blocker = GetComponentInChildren<BoxCollider>();
		anim = GetComponent<tk2dAnimatedSprite>();
		Quaternion indicRotation = Quaternion.identity;
		indicRotation.eulerAngles = new Vector3(0 + rotationOffSet.x, 0 + rotationOffSet.y, -90 + rotationOffSet.z);
		
		indicator = (Instantiate(Resources.Load("pre_shopIndicator"), transform.position + new Vector3(-25 + positionOffSet.x, 0 + positionOffSet.y, 0 + positionOffSet.z - 1), indicRotation) as GameObject).GetComponent<tk2dAnimatedSprite>();
		indicator.renderer.enabled = false;
		/*
		BoxCollider[] chilluns = gameObject.GetAllComponentsInChildren<BoxCollider>();
		foreach (BoxCollider c in chilluns){
			if (c.isTrigger){
				myCollider = c;
			}
			
			if (c.gameObject.layer == LayerMask.NameToLayer("collision")){
				collisionChild = c.transform;
			}
		}*/
		
		switch(requiredCollType){
		case requiredCollTypeEnum.White:
			inString = "shopFlag" + requiredPayment.ToString() + "W_in";
			outString = "shopFlag" + requiredPayment.ToString() + "W_out";
			myColor = Color.white;
			break;
		case requiredCollTypeEnum.Blue:
			inString = "shopFlag" + requiredPayment.ToString() + "B_in";
			outString = "shopFlag" + requiredPayment.ToString() + "B_out";
			myColor = Color.blue;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		switch(requiredCollType){
		case requiredCollTypeEnum.White:
			Check(Actions.WhitePay);
			break;
		case requiredCollTypeEnum.Blue:
			Check(Actions.Pay);
			break;
		}
		if (triggered) return;
				
		inZone = GetComponentInChildren<ShopDetection>().onDetectZone;	
	
		switch(requiredCollType){
		case requiredCollTypeEnum.White:
			
			if(inZone && StatsManager.whiteCollDisplayed < minCollRequBefOpenShop){
					//Affiche le bubble qui indique le nb de coll necesaire avant que le shop soit disponible
				Debug.Log("Il vous faut minimum " + minCollRequBefOpenShop + " Collectibles blancs avant de pouvoir activer le shop");
			}
			else if(inZone && StatsManager.whiteCollDisplayed >= minCollRequBefOpenShop && StatsManager.whiteCollDisplayed < (minCollRequBefOpenShop + requiredPayment)){
					
					//Affiche le bubble qui indique cmb de coll pour acheter l'ouverture
				float dist = Vector3.Distance(avatarT.position, transform.position);
				if (dist < avatarCloseDist && isIn){
					StartOut();
				}
				else if (dist > avatarCloseDist && isOut){
					StartIn();
				}		
				
				Debug.Log(requiredPayment + " Collectibles blancs sont requis pour ouvrir");
			}
			else if (inZone && StatsManager.whiteCollDisplayed >= (minCollRequBefOpenShop + requiredPayment)){
					//Affiche le bubble qui indique qu'on peut effectuer l'ouverture
				float dist = Vector3.Distance(avatarT.position, transform.position);
				if (dist < avatarCloseDist && isIn){
					StartOut();
				}
				else if (dist > avatarCloseDist && isOut){
					StartIn();
				}
				
				Debug.Log("Ouvrez la barriere avec P");
			}
			
			
			break;
		case requiredCollTypeEnum.Blue:
			
			if(inZone && StatsManager.blueCollDisplayed < minCollRequBefOpenShop){
					//Affiche le bubble qui indique le nb de coll necesaire avant que le shop soit disponible
				Debug.Log("Il vous faut minimum " + minCollRequBefOpenShop + " Collectibles bleus avant de pouvoir activer le shop");
			}
			else if(inZone && StatsManager.blueCollDisplayed >= minCollRequBefOpenShop && StatsManager.blueCollDisplayed < (minCollRequBefOpenShop + requiredPayment)){
					//Affiche le bubble qui indique cmb de coll pour acheter l'ouverture
				float dist = Vector3.Distance(avatarT.position, transform.position);
				if (dist < avatarCloseDist && isIn){
					StartOut();
				}
				else if (dist > avatarCloseDist && isOut){
					StartIn();
				}
				
				Debug.Log(requiredPayment + " Collectibles bleus sont requis pour ouvrir");
			}
			else if (inZone && StatsManager.blueCollDisplayed >= (minCollRequBefOpenShop + requiredPayment)){
					//Affiche le bubble qui indique qu'on peut effectuer l'ouverture
				float dist = Vector3.Distance(avatarT.position, transform.position);
				if (dist < avatarCloseDist && isIn){
					StartOut();
				}
				else if (dist > avatarCloseDist && isOut){
					StartIn();
				}
				
				Debug.Log("Ouvrez la barriere avec P");
			}
			
			break;
		}
	}
	
	protected bool Check(Actions action){
		if (triggered){
			return false;
		}
		if (isIn){
			switch(action){
			case Actions.WhitePay:
				HUDManager.hudManager.UpdateAction(action, Payment);
				break;
			case Actions.Pay:
				HUDManager.hudManager.UpdateAction(action, Payment);
				break;
			}
			return true;
		}
		return false;
	}
	
	void Payment(){
		
		if (ChromatoseManager.manager.GetCollectibles(myColor) < requiredPayment){
			return;
		}
		StartIn();
		waiting = true;
		triggered = true;
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
			//blocker.enabled = false;
			blocker.gameObject.SetActive(false);
			ChromatoseManager.manager.RemoveCollectibles(myColor, requiredPayment, avatarT.position);
			if (anim)
				Animate();
		}
	}
	
	void Animate(){
		anim.Play();
		anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
	}
	
	IEnumerator LateSetup(float delai){
		yield return new WaitForSeconds(delai);
		avatarT = GameObject.FindGameObjectWithTag("avatar").GetComponent<Transform>();
		Debug.Log("SetupDuShop");
	}
	
}

using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {
	
	public enum requiredCollTypeEnum{
		White, Red, Blue
	}
	
	public requiredCollTypeEnum requiredCollType;
	
	public int requiredPayment;
	public int minCollRequBefOpenShop;
	
	public Vector3 positionOffSet;
	public Vector3 rotationOffSet;
	
	private bool inZone;
	private bool _AlreadyPaid = false;
	
	private BoxCollider blocker;
	private tk2dSprite _SpriteInfo;
	private tk2dAnimatedSprite _MainAnim;
	private tk2dAnimatedSprite indicator;
	private string inString;
	private string outString;
	private bool isOut = false;
	private bool isIn = true;
	protected bool triggered = false;
	protected bool waiting = false;
	
	
	void Start () {
		blocker = GetComponentInChildren<BoxCollider>();
		switch(requiredCollType){
		case requiredCollTypeEnum.White:
			_MainAnim = GetComponent<tk2dAnimatedSprite>();
			break;
		case requiredCollTypeEnum.Red:
			_SpriteInfo = GetComponent<tk2dSprite>();
			break;
		case requiredCollTypeEnum.Blue:
			_SpriteInfo = GetComponent<tk2dSprite>();
			break;
		}
		
		
		Quaternion indicRotation = Quaternion.identity;
		indicator = (Instantiate(Resources.Load("pre_shopIndicator"), transform.position + new Vector3(-25 + positionOffSet.x, 0 + positionOffSet.y, 0 + positionOffSet.z - 1), indicRotation) as GameObject).GetComponent<tk2dAnimatedSprite>();
		indicator.renderer.enabled = false;
		
		switch(requiredCollType){
		case requiredCollTypeEnum.White:
			inString = "wshopFlagIn_" + requiredPayment.ToString();
			outString = "wshopFlagOut_" + requiredPayment.ToString();
			break;
		case requiredCollTypeEnum.Red:
			inString = "bossFlagIn_47";
			outString = "bossFlagOut_47";
			break;
		case requiredCollTypeEnum.Blue:
			inString = "bshopFlagIn_" + requiredPayment.ToString();
			outString = "bshopFlagOut_" + requiredPayment.ToString();
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		inZone = GetComponentInChildren<ShopDetection>().onDetectZone;	
		
		
	
		switch(requiredCollType){
		case requiredCollTypeEnum.White:
			if(_AlreadyPaid)return;
			
			if(inZone && StatsManager.whiteCollDisplayed < minCollRequBefOpenShop && !_AlreadyPaid){
					//Affiche le bubble qui indique le nb de coll necesaire avant que le shop soit disponible
				Debug.Log("Il vous faut minimum " + minCollRequBefOpenShop + " Collectibles blancs avant de pouvoir activer le shop");
			}
			else if(inZone && StatsManager.whiteCollDisplayed >= minCollRequBefOpenShop && StatsManager.whiteCollDisplayed < (minCollRequBefOpenShop + requiredPayment) && !_AlreadyPaid){
					//Affiche le bubble qui indique cmb de coll pour acheter l'ouverture
				Check(Actions.WhitePay);
				StartOut();
				
				Debug.Log(requiredPayment + " Collectibles blancs sont requis pour ouvrir");
			}
			else if(!inZone && StatsManager.whiteCollDisplayed >= minCollRequBefOpenShop && StatsManager.whiteCollDisplayed < (minCollRequBefOpenShop + requiredPayment) && !_AlreadyPaid){
				StartIn();
				HUDManager.hudManager.OffAction ();
			}
			else if (inZone && StatsManager.whiteCollDisplayed >= (minCollRequBefOpenShop + requiredPayment) && !_AlreadyPaid){
					//Affiche le bubble qui indique qu'on peut effectuer l'ouverture
				Check(Actions.WhitePay);
				StartOut();
				_AlreadyPaid = true;
				Debug.Log("Ouvrez la barriere avec P");
			}
			
			
			
			
			
			break;
		case requiredCollTypeEnum.Red:
			
			if(inZone && StatsManager.redCollDisplayed < minCollRequBefOpenShop && !_AlreadyPaid){
					//Affiche le bubble qui indique le nb de coll necesaire avant que le shop soit disponible
				Debug.Log("Il vous faut minimum " + minCollRequBefOpenShop + " Collectibles rouges avant de pouvoir activer le shop");
			}
			else if(inZone && StatsManager.redCollDisplayed >= minCollRequBefOpenShop && StatsManager.redCollDisplayed < (minCollRequBefOpenShop + requiredPayment) && !_AlreadyPaid){
					//Affiche le bubble qui indique cmb de coll pour acheter l'ouverture
				Check(Actions.Release);
				StartOut();
				Debug.Log(requiredPayment + " Collectibles rouges sont requis pour ouvrir");
			}
			else if(!inZone && StatsManager.redCollDisplayed >= minCollRequBefOpenShop && StatsManager.redCollDisplayed < (minCollRequBefOpenShop + requiredPayment) && !_AlreadyPaid){
				StartIn();
				HUDManager.hudManager.OffAction ();
			}
			else if (inZone && StatsManager.redCollDisplayed >= (minCollRequBefOpenShop + requiredPayment) && !_AlreadyPaid){
					//Affiche le bubble qui indique qu'on peut effectuer l'ouverture
				Check(Actions.Release);
				StartOut();
				_AlreadyPaid = true;
				Debug.Log("Ouvrez la barriere avec P");
			}
			
			break;
		case requiredCollTypeEnum.Blue:
			
			if(inZone && StatsManager.blueCollDisplayed < minCollRequBefOpenShop && !_AlreadyPaid){
					//Affiche le bubble qui indique le nb de coll necesaire avant que le shop soit disponible
				Debug.Log("Il vous faut minimum " + minCollRequBefOpenShop + " Collectibles bleus avant de pouvoir activer le shop");
			}
			else if(inZone && StatsManager.blueCollDisplayed >= minCollRequBefOpenShop && StatsManager.blueCollDisplayed < (minCollRequBefOpenShop + requiredPayment) && !_AlreadyPaid){
					//Affiche le bubble qui indique cmb de coll pour acheter l'ouverture
				Check(Actions.Pay);
				StartOut();
				Debug.Log(requiredPayment + " Collectibles bleus sont requis pour ouvrir");
			}
			else if(!inZone && StatsManager.blueCollDisplayed >= minCollRequBefOpenShop && StatsManager.blueCollDisplayed < (minCollRequBefOpenShop + requiredPayment) && !_AlreadyPaid){
				StartIn();
				HUDManager.hudManager.OffAction ();
			}
			else if (inZone && StatsManager.blueCollDisplayed >= (minCollRequBefOpenShop + requiredPayment) && !_AlreadyPaid){
					//Affiche le bubble qui indique qu'on peut effectuer l'ouverture
				Check(Actions.Pay);
				StartOut();
				_AlreadyPaid = true;
				Debug.Log("Ouvrez la barriere avec P");
			}
			
			break;
		}		
	}
	
	
	void OpenGate(){
		StartIn();
		//_SpriteInfo.enabled = false;
		//blocker.enabled = false;
		waiting = true;
		triggered = true;
		//ChromatoseManager.manager.RemoveCollectibles(Color.white, requiredPayment, GameObject.FindGameObjectWithTag("avatar").transform.position);
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
	
	protected bool Check(Actions action){
		if (triggered){
			return false;
		}
		if (inZone){
			HUDManager.hudManager.UpdateAction(action, OpenGate);			
			return true;
		}
		return false;
	}
	
	void Out(tk2dAnimatedSprite sprite, int index){
		isOut = true;
	}
	
	void In(tk2dAnimatedSprite sprite, int index){
		isIn = true;
		indicator.renderer.enabled = false;
		if (triggered){
			waiting = false;
			switch(requiredCollType){
			case requiredCollTypeEnum.White:
				ChromatoseManager.manager.RemoveCollectibles(Color.white, requiredPayment, GameObject.FindGameObjectWithTag("avatar").transform.position);
				break;
			case requiredCollTypeEnum.Red:
				ChromatoseManager.manager.RemoveCollectibles(Color.red, requiredPayment, GameObject.FindGameObjectWithTag("avatar").transform.position);
				break;
			case requiredCollTypeEnum.Blue:
				ChromatoseManager.manager.RemoveCollectibles(Color.blue, requiredPayment, GameObject.FindGameObjectWithTag("avatar").transform.position);
				break;
			}
			HUDManager.hudManager.OffAction ();
			switch(requiredCollType){
			case requiredCollTypeEnum.White:
				_MainAnim.Play();
				break;
			case requiredCollTypeEnum.Red:
				gameObject.SetActive(false);
				break;
			case requiredCollTypeEnum.Blue:
				gameObject.SetActive(false);
				break;
			}
		}
	}
}

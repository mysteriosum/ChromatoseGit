using UnityEngine;
using System.Collections;

public class TransitionTrigger : MonoBehaviour {
	
	public enum transitionVers{
		 NextLevel, LocalTarget, DynamicComic, TimeTrial, ReturnMainMenu
	}
	public transitionVers _TransitEnum;
	public bool _DontAddRoom = false;
	
	//public bool 
	
	private Avatar _AvatarScript;
	private Transform avatarT;
	private ChromatoseManager _Manager;
	private ChromaRoomManager _RoomManager;
	private ChromatoseCamera _Cam;
	
	private bool _Popped = false;
	private bool _Lightning = false;
	private bool _OnStaticComic = false;
	private bool _FadeIn = false;
	private bool _FadeOut = false;
	
	private float _LightCounter = 0f;
	
	private delegate void TriggerMethod();
	private TriggerMethod myTrigger;
	
	public Texture blackBox;
	public Transform localTarget;
	public Texture staticComic;			//<--Devra peut-etre etre changer dependant comment il faut transitionner vers sa
	public Transform dynamicComicTarget;
	
	void Start(){
				
		switch (_TransitEnum){
		case transitionVers.NextLevel:
			myTrigger = NextLevel;
			break;
			
		case transitionVers.LocalTarget:
			if(localTarget != null){
				myTrigger = ToTarget;
			}
			else{
				myTrigger = NextLevel;
			}
			break;
			
		case transitionVers.TimeTrial:
			myTrigger = FinishTimeTrialChallenge;
			break;
			
		case transitionVers.DynamicComic:
			if(dynamicComicTarget != null){
				myTrigger = ToDynamicComic;
			}
			else{
				myTrigger = NextLevel;
			}
			break;
			
		case transitionVers.ReturnMainMenu:
			myTrigger = ReturnMainMenu;
			break;
		}
		StartCoroutine(Setup());
	}	

	void FixedUpdate () {
		
		switch (_TransitEnum){
		case transitionVers.NextLevel:
			
			if (!_Popped) return;
		
			if(_LightCounter < 1){_LightCounter += 0.05f;}
			else{
				myTrigger();
			}
			
			break;
		case transitionVers.LocalTarget:
			
			if (!_Popped) return;
			
				
			if (_LightCounter < 1 && _FadeIn){_LightCounter += 0.05f;}
			if (_LightCounter > 0 && _FadeOut){_LightCounter -= 0.015f;}
			if (_LightCounter > 1){
				_FadeIn = false; 
				_FadeOut = true; 
				ChromatoseManager.manager.ResetComicCounter(); 
				ChromatoseManager.manager.ResetColl(); 
				myTrigger();
			}
				
			break;
		case transitionVers.TimeTrial:
			
			if (!_Popped) return;
			myTrigger();
			
			break;
		case transitionVers.DynamicComic:
			
			if (!_Popped) return;
			
			if (_LightCounter < 1 && _FadeIn){_LightCounter += 0.05f;}
			if (_LightCounter > 0 && _FadeOut){_LightCounter -= 0.015f;}
			if (_LightCounter > 1){_FadeIn = false; _FadeOut = true; _Cam.SwitchCamType(); myTrigger();}
			
			break;
			
		case transitionVers.ReturnMainMenu:
			if (!_Popped) return;
			
			if (_LightCounter < 1 && _FadeIn){_LightCounter += 0.05f;}
			if (_LightCounter > 0 && _FadeOut){_LightCounter -= 0.015f;}
			if (_LightCounter > 1){_FadeIn = false; _FadeOut = true; myTrigger();}
			break;
		}
	}
	
	void OnTriggerEnter(Collider other){
		if (other.name == "Avatar"){
			_FadeIn = true;
			_Popped = true;
		}
	}
	
	void OnGUI(){
		
		if(!_OnStaticComic){
		
			if (!_Popped) return;
			
			GUI.color = new Color(0, 0, 0, _LightCounter);
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBox);
		
		}
		else{
			//Affiche le Comic dans le GUI Directment, pensez a faire le lien avec les bouton et le reste du HUD
			Rect comicRect = new Rect(0, 0, staticComic.width, staticComic.height);
			GUI.DrawTexture(comicRect, staticComic);
		}
	}
	
	/// <summary>
	/// Appelle la Transition de l'exterieur
	/// </summary>
	public void ExternalCall(){
		myTrigger();
	}
	public void ResetBool(){
		_Popped = false;
		_FadeIn = true;
		_FadeOut = false;
		_LightCounter = 0;
	}
	
	
#region Methodes de Transition
	
	void NextLevel(){
		if(_AvatarScript.HasOutline){
			_AvatarScript.CancelOutline();
		}
		MainManager._MainManager.UnlockNextLevel();
		Application.LoadLevel(Application.loadedLevel + 1);
	}
	
	void ToTarget(){
		avatarT.position = localTarget.position;
		avatarT.rotation = localTarget.rotation;
		avatarT.SendMessage ("SetVelocity", Vector2.zero);
		//Vector3 dataPos = localTarget.position;
		//StatsManager.lastSpawnPos = dataPos;
		Debug.Log("Last target Changed");
		
		if(_RoomManager.roomType == ChromaRoomManager._RoomTypeEnum.WhiteRoom && !_DontAddRoom){
			_RoomManager.NextLilRoom();
			ChromatoseManager.manager.CheckPointHere(localTarget);
			//Debug.Log("NextLilRoom");
		}
		
		ChromatoseManager.manager.ResetComicCounter();
		ChromatoseManager.manager.ResetColl();

		//ResetBool();
		
		if(GameObject.FindGameObjectWithTag("OptiManager") != null){
			OptiManager tempOptiManager = GameObject.FindGameObjectWithTag("OptiManager").GetComponent<OptiManager>();
			tempOptiManager.OptimizeZone();
		}
		
		_AvatarScript.CallFromFar();
		_AvatarScript.LoseAllColourHidden();		
		if(_AvatarScript.HasOutline){
			_AvatarScript.CancelOutline();
		}
		HUDManager.hudManager.afterComic = false;
	}
	
	void FinishTimeTrialChallenge(){/*
		if(_Manager.timeTrialClass.StopChallenge()){
			//Faire afficher la fenetre de reussite du TimeTrial
			_Manager.timeTrialClass.DisplayWinWindows = true;
		}
		StartCoroutine(DelaiToDisplay());*/
	}
	
	void ToDynamicComic(){
		avatarT.position = dynamicComicTarget.position;
		avatarT.rotation = dynamicComicTarget.rotation;
		avatarT.SendMessage ("SetVelocity", Vector2.zero);
		HUDManager.hudManager.canFlash = false;
		HUDManager.hudManager.afterComic = true;
		HUDManager.hudManager.onFlash = false;
		ResetBool();

	}
	
	void ReturnMainMenu(){
		Application.LoadLevel(0);
	}
	
#endregion	
	
#region CoRoutine	
	IEnumerator DelaiToDisplay(){
		yield return new WaitForSeconds(0.5f);
		//_Manager.timeTrialClass.DisplayScore = true;
	}
	IEnumerator DelaiToFadeOut(){
		yield return new WaitForSeconds(1f);
		_FadeOut = true;
	}

	IEnumerator Setup(){
		yield return new WaitForSeconds(0.1f);
		_RoomManager = ChromaRoomManager.roomManager;
		_AvatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		avatarT = GameObject.FindWithTag("avatar").transform;
		_Cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ChromatoseCamera>();
		/*	
		if(_Manager.TimeTrialMode) {
			_TransitEnum = transitionVers.TimeTrial;
		}*/
	}
}
#endregion
using UnityEngine;
using System.Collections;

[SerializeAll]
public class MainManager : MonoBehaviour {
	
	//ENUM
	public enum _WhiteRoomEnum{
		Tuto, ModuleBlanc_2, ModuleBlanc_3, ModuleBlanc_4, None
	}
	public enum _RoomTypeEnum{
		Menu, WhiteRoom, RedRoom, BlueRoom, RedAndBlueRoom, FinalBoss, None
	}
	public enum _KeyboardTypeEnum{
		QWERTY, AZERTY
	}
	
	//ENUM ASSIGNATION
	private static _WhiteRoomEnum _WhiteRoom;
	public _WhiteRoomEnum whiteRoom { get { return _WhiteRoom; } }
	
	private static _RoomTypeEnum _RoomType;
	public _RoomTypeEnum roomType { get { return _RoomType; } }	
	
	private static _KeyboardTypeEnum _KeyboardType;
	public _KeyboardTypeEnum keyboardType { get { return _KeyboardType; } set { _KeyboardType = value; } }
	
	//PUBLIC VARIABLES -- INGAME USED
	public static int currentLevel = 0;
	public static int currentRoomInt = 0;
	public static string currentRoomString = "room00";
	public static GameObject startPoint;
	
	public static bool _CanControl = false;
	
	protected bool getForward;
	protected bool getLeft;
	protected bool getRight;
	
	
	//PRIVATE VARIABLES -- INMANAGER USED
	private bool lvlSetuped = false;
	private bool childrenLinked = false;
		
	//VARIABLE STATIC STATS		
	public static int currentLevelUnlocked = 0;
	
	public static int redCollCollected = 0;
	public static int blueCollCollected = 0;
	public static int whiteCollCollected = 0;
	public static int comicThumbCollected = 0;
	
	public static int redCollDisplayed = 0;
	public static int blueCollDisplayed = 0;
	public static int whiteCollDisplayed = 0;
	public static int comicThumbDisplayed = 0;
	
	public static int totalComicViewed = 0;
	public static float playedTime = 0;
	
	public static bool doneOneTime = false;
	public static bool versionPirate = false;
	
	
	//VARIABLE SOUND
	public static bool _MusicMute = false;
	public static bool _SFXMute = false;
	public static float _MusicVolume = Mathf.Clamp(80, 0, 100);
	public static float _SFXVolume = Mathf.Clamp(80, 0, 100);
	
	//VARIABLE D'INTERFACE DYNAMIC
	public static float _RotStartButton = 0;
	public static bool _RotUp = false;
	public static int _LoadCounter = 0;
	
	//VARIABLE TRIAL (VERSION DEMO)
	public static bool _ADADAD = true;
	
	//VARIABLE GAMEMODE
	public static bool _ExtraModeUnlocked = false;
	public static bool _TimeTrialActive = false;
	public static bool _NoDeathModeActive = false;
	
	//VARIABLE LOADING SCREEN
	public static float _LoadProgress = 0;
	public static AsyncOperation async = null;
	public static bool _loop2 = false;
	public static float loopingCounter = 0;
	
	
	
	//VARIABLE HIGHSCORE
	
	
	
	//MANAGER & VIP OBJECT
	public static MainManager _MainManager;
	public static ChromatoseManager _ChroManager;
	public static HUDManager _HudManager;
	public static ChromaRoomManager _RoomManager;
	public static OptiManager _OptiManager;
	public static RoomInstancier _RoomInstancier;	
	public static GameObject _Avatar;
	public static Avatar _AvatarScript;
	public static GameObject _Chromera;
	public static ChromatoseCamera _ChromeraScript;
		
	
	//GET/SET ACCESSOR
		
	
	
	//START, SETUP & INIT
	void Awake(){
		DontDestroyOnLoad(transform.gameObject);
		
		_MainManager = this;
		_ChroManager = GetComponent<ChromatoseManager>();
		_HudManager = GetComponent<HUDManager>();
		_RoomManager = GetComponent<ChromaRoomManager>();
		_OptiManager = GetComponent<OptiManager>();
		_RoomInstancier = GetComponent<RoomInstancier>();
	}
	void OnLevelWasLoaded(){

	}	
	void Start () {
		
		//A EFFACER
		_CanControl = true;
		
		
		if(_KeyboardType == null){
			_KeyboardType = _KeyboardTypeEnum.QWERTY;
		}
	
		CheckWhereIAm();
		LinkChildren();
		SetupLvl();
		
		Debug.LogWarning("MainManager-Start_log - Started in Lvl " + Application.loadedLevel + ". Also, the keyboard is a " + _KeyboardType + " type."
			
							/*", HudManager is : " + (_HudManager!=null) + ", ChroManager is : " + (_ChroManager!=null) + 
							", RoomManager is : " + (_RoomManager!=null) + ", OptiManager is : " + (_OptiManager!=null) + 
							", RoomInstancier is : " + (_RoomInstancier!=null) + ". Also, the Room was a : " + _RoomType + 
							" which contains " + GetComponent<OptiManager>().roomList.Length + " room(s). The keyboard is " + _KeyboardType + "."*/);
	}
	
	void Update(){
		Debug.Log("Forward est a : " + getForward);
	}

	
	//PRIVATE FUNCTION
	private void LinkChildren(){
		
	}
	
	public void SetupLvl(){
		if(GameObject.FindGameObjectWithTag("StartPoint")){
			startPoint = GameObject.FindGameObjectWithTag("StartPoint");
			if(startPoint != null){
				Debug.Log("StartPoint Finded");
			}	
		}
		
		switch(Application.loadedLevel){
		case 0:
			_RoomType = _RoomTypeEnum.Menu;
			_WhiteRoom = _WhiteRoomEnum.None;
			break;
		case 1:
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.Tuto;
			break;
		case 2:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			break;
		case 3: 
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.ModuleBlanc_2;
			break;
		case 4:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			break;
		case 5:
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.ModuleBlanc_3;
			break;
		case 6:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			break;
		case 7:
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.ModuleBlanc_4;
			break;
		case 8:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			break;
		}
				
		if(!GameObject.FindGameObjectWithTag("avatar") && Application.loadedLevel != 0){
			_Avatar = Instantiate(Resources.Load("Avatar"), startPoint.transform.position, Quaternion.identity)as GameObject;
			
			if(_Avatar != null){
				Debug.Log("Avatar Created");
			}
			_Avatar.name = "Avatar";
			_AvatarScript = _Avatar.GetComponent<Avatar>();
		}
		
		if(!GameObject.FindGameObjectWithTag("MainCamera")){
			Vector3 camPos = new Vector3(0, 0, -25);
			_Chromera = Instantiate(Resources.Load("Chromera"), camPos, Quaternion.identity)as GameObject;
			_ChromeraScript = _Chromera.GetComponent<ChromatoseCamera>();
		}	
		StartCoroutine(SetupRoom());
	}
	
	
	//PUBLIC FUNCTION
	public int CheckWhereIAm(){
		return currentLevel = Application.loadedLevel;
	}
	public void Pause(){
		GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().InverseControlRight();
		
		if(Time.timeScale==1){
			GetComponent<HUDManager>().guiState = GUIStateEnum.Pause;
			Time.timeScale = 0;
		}
		else{
			GetComponent<HUDManager>().guiState = GUIStateEnum.Interface;
			Time.timeScale = 1; 
		}
	}
	
	
	
	
	//INHERITANCE COROUTINE
	public IEnumerator LoadALevel(int levelInt){
		async = Application.LoadLevelAsync(levelInt);
		yield return async;
	}
	
	IEnumerator ResetSetupCheck(){
		yield return new WaitForSeconds(5.0f);
		lvlSetuped = false;
	}
	
	IEnumerator SetupRoom(){
		yield return new WaitForSeconds(0.1f);
		int curLevel = Application.loadedLevel;
		
		switch(curLevel){
		//MAIN MENU
		case 0:
			_RoomType = _RoomTypeEnum.Menu;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 0;
			break;
			
		//TUTORIAL
		case 1:
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.Tuto;
			currentLevel = 1;
			break;
			
		//LEVEL 1 - MODULE_BLANC_1
		case 2:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 2;
			break;
			
		//LEVEL 2 - MODULE_1_SCENE_1
		case 3:
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.ModuleBlanc_2;
			currentLevel = 3;
			break;
			
		//LEVEL 3 - MODULE_BLANC_2
		case 4:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 4;
			break;
			
		//LEVEL 4 = MODULE_1_SCENE_2
		case 5:
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.ModuleBlanc_3;
			currentLevel = 5;
			break;
			
		//LEVEL 5 - MODULE_BLANC_3
		case 6:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 6;
			break;
			
		//LEVEL 6 - MODULE_1_SCENE_3
		case 7:
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.ModuleBlanc_4;
			currentLevel = 7;
			break;
			
		//LEVEL 7 - MODULE_BLANC_4
		case 8:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 8;
			break;
			
		//LEVEL 8 - MODULE_1_SCENE_4
		case 9:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 9;
			break;
			
		//LEVEL 9 - MODULE_1_SCENE_5
		case 10:
			_RoomType = _RoomTypeEnum.FinalBoss;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 10;
			break;
		}
	}
}

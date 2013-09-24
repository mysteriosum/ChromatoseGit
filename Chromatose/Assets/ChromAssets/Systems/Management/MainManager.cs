using UnityEngine;
using System.Collections;

[SerializeAll]
public class MainManager : MonoBehaviour {
	
	//Commentaire de Test Perforce
	
	//ENUM
	public enum _WhiteRoomEnum{
		Tuto, ModuleBlanc_2, ModuleBlanc_3, ModuleBlanc_4, None
	}
	public enum _RoomTypeEnum{
		Menu, WhiteRoom, RedRoom, BlueRoom, RedAndBlueRoom, FinalBoss, GYM, None
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
	public static string currentRoomString = "room00";
	public static GameObject startPoint;
	
	public static bool _CanControl = false;
	
	
	//PRIVATE VARIABLES -- INMANAGER USED
	private bool lvlSetuped = false;
	private bool childrenLinked = false;
	
	private bool gone;
	public bool Gone{
		get{
			return gone;
		}
		set{
			gone = value;
			transform.position = gone? new Vector3(_Avatar.transform.position.x, _Avatar.transform.position.y, -3000)
									 : new Vector3(_Avatar.transform.position.x, _Avatar.transform.position.y, 0);
		}
	}
	private string gameName = "ChromaSavedRoom";
		
	//VARIABLE STATIC STATS		
	/*
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
	*/
	
	//VARIABLE SOUND
	public static bool _MusicMute = false;
	public static bool _SFXMute = false;
	public static float _MusicVolume = 0.80f; public float musicVolume { get { return _MusicVolume; } set { _MusicVolume = value; } } 
	public static float _SFXVolume = 0.80f; public float sfxVolume { get { return _SFXVolume ; } set { _SFXVolume = value; } }
	
	
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
	public static StatsManager _StatsManager;
	public static GameObject _SpeechBubble;
	public static Camera menuCam;
	
	//GET/SET ACCESSOR
		
	
	
	//START, SETUP & INIT
	void Awake(){
		if(Application.isEditor){
			//Debug.Log("LEVEL : " + Application.loadedLevel);
		}
		
		DontDestroyOnLoad(transform.gameObject);
		
		_MainManager = this;
		_ChroManager = GetComponent<ChromatoseManager>();
		_HudManager = GetComponent<HUDManager>();
		_RoomManager = GetComponent<ChromaRoomManager>();
		_OptiManager = GetComponent<OptiManager>();
		_RoomInstancier = GetComponent<RoomInstancier>();
		_StatsManager = GetComponentInChildren<StatsManager>();
	}
	void OnLevelWasLoaded(){
		print("Level Loaded");
		CheckWhereIAm();
		StartCoroutine(SetupRoom());
		ChromatoseManager.manager.CheckNewfaderList();
		SetupAvatarAndCam();		
	}	
	void Start () {
		
		if(menuCam == null){
			menuCam = Camera.mainCamera;
		}
		
		Debug.LogWarning("MainManager-Start_log - Started in Lvl " + Application.loadedLevel + ". Also, the keyboard is a " +
							_KeyboardType + " type. Already own " + StatsManager.whiteCollCollected + " whiteCollectibles but only " +
							StatsManager.whiteCollDisplayed + " will be displayed. Already own " + StatsManager.redCollCollected + " redCollectibles but only " +
							StatsManager.redCollDisplayed + " will be displayed. Already own " + StatsManager.blueCollCollected + " blueCollectibles but only " +
							StatsManager.blueCollDisplayed + " will be displayed. " );
			
						
		
		//A EFFACER
		_CanControl = true;
		
		if(_KeyboardType == null){
			_KeyboardType = _KeyboardTypeEnum.QWERTY;
		}
		CheckWhereIAm();
		StartCoroutine(SetupRoom());
		ChromatoseManager.manager.CheckNewfaderList();
		SetupAvatarAndCam();
	}
	
	
	void SetupAvatarAndCam(){
			//DETERMINE LA POSITION DU SPAWN DE DEPART SELON SI C'EST UNE NEW GAME OU UNE GAME LOADER
		//if(StatsManager.lastSpawnPos==Vector3.zero){
			if(GameObject.FindGameObjectWithTag("StartPoint")){
				startPoint = GameObject.FindGameObjectWithTag("StartPoint");
				if(_Avatar){
					_Avatar.transform.position = startPoint.transform.position;
				}
			}			
		//}
		//else{
			//startPoint.transform.position = StatsManager.lastSpawnPos;
		//}
		
			//S'ASSURE QU'IL N'Y A PAS D'AVATAR, PUIS EN CREE UN
		if(!GameObject.FindGameObjectWithTag("avatar") && Application.loadedLevel != 0){
			_Avatar = Instantiate(Resources.Load("Avatar"), startPoint.transform.position, Quaternion.identity)as GameObject;
			_Avatar.name = "Avatar";
			_AvatarScript = _Avatar.GetComponent<Avatar>();
			ChromatoseManager.manager.Setup();
			_SpeechBubble = GameObject.FindGameObjectWithTag("speechBubble");
			//StatsManager.lastSpawnPos = startPoint.transform.position;
		}

			//S'ASSURE QU'IL N'Y AI PAS DE CAMERA, PUIS EN CREE UNE	
		if(!GameObject.FindGameObjectWithTag("MainCamera")){
			Vector3 camPos = new Vector3(0, 0, -25);
			_Chromera = Instantiate(Resources.Load("Chromera"), camPos, Quaternion.identity)as GameObject;
			_ChromeraScript = _Chromera.GetComponent<ChromatoseCamera>();
		}
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
	
	
	public void LoadALevel(int levelInt){
		HUDManager.hudManager.guiState = GUIStateEnum.MainMenu;
		HUDManager.hudManager.menuWindows = _MenuWindowsEnum.LoadingScreen;
		HUDManager.hudManager.movieLoad2.Play();
		
		if(levelInt == 0){			
			if(_Avatar){
				_Avatar.SetActive(false);
				if(_SpeechBubble == null){
					_SpeechBubble = GameObject.FindGameObjectWithTag("speechBubble");
				}
				_SpeechBubble.SetActive(false);
			}
		}
		else{
			if(_Avatar){
				_Avatar.SetActive(true);
				_SpeechBubble.SetActive(true);
			}
			HUDManager.hudManager.ResetAllMovie();
		}
		StartCoroutine(DelayinLoadLevel(levelInt));
	}
	
	public void SaveRoom(){
		LevelSerializer.Checkpoint();
		
		//LevelSerializer.SaveGame(gameName);
	}
	
	public void LoadRoom(){
		LevelSerializer.Resume();
		
		//LevelSerializer.LoadSavedLevel(gameName);
	}
	
	public void UnlockNextLevel(){
		
		for(int i = 0; i < StatsManager.levelUnlocked.Length; i++){
			if(!StatsManager.levelUnlocked[i]){
				StatsManager.levelUnlocked[i] = true;
				return;
			}
		}
	}
	
	public void LockLastLevel(){
		
		for(int i = 0; i < StatsManager.levelUnlocked.Length; i++){
			if(!StatsManager.levelUnlocked[i]){
				StatsManager.levelUnlocked[i - 1] = false;
				return;
			}
		}
	}
	
	
	//INHERITANCE COROUTINE
	public IEnumerator LoadALevelAsync(int levelInt){
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
			
			//TUTORIAL - MODULE_BLANC_1
		case 1:
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.Tuto;
			currentLevel = 1;
			break;
			
			//LEVEL 2 - MODULE_1_SCENE_1
		case 2:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 2;
			break;
			
			//LEVEL 3 - MODULE_BLANC_2
		case 3:
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.ModuleBlanc_2;
			currentLevel = 3;
			break;
			
			//LEVEL 4 = MODULE_1_SCENE_2
		case 4:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 4;
			break;
			
			//LEVEL 5 - MODULE_BLANC_3
		case 5:
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.ModuleBlanc_3;
			currentLevel = 5;
			break;
			
			//LEVEL 6 - MODULE_1_SCENE_3
		case 6:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 6;
			break;
			
			//LEVEL 7 - MODULE_BLANC_4
		case 7:
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.ModuleBlanc_4;
			currentLevel = 7;
			break;
			
			//LEVEL 8 - MODULE_1_SCENE_4
		case 8:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 8;
			break;
			
			//LEVEL 9 - MODULE_1_SCENE_6
		case 9:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 9;
			break;
			
			//LEVEL 10 - MODULE_1_SCENE_7
		case 10:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 10;
			break;
			
			//LEVEL 11 - MODULE_1_SCENE_8
		case 11:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 11;
			break;
			
			//LEVEL 12 - FINAL BOSS
		case 12:
			_RoomType = _RoomTypeEnum.FinalBoss;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 12;
			break;
			//GYM DU CHU
		case 13:
			_RoomType = _RoomTypeEnum.GYM;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 13;
			break;
		}
	}
	IEnumerator DelayinLoadLevel(int levelInt){
		yield return new WaitForSeconds(1.0f);
		Application.LoadLevel(levelInt);
	}
}

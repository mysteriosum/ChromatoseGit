using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

//comms pour le push

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414


#region Variables et Data
public enum Actions{
	Back,
	Absorb,
	Destroy,
	Build,
	Pay,
	whitePay,
	Release,
	
	Nothing,
	
}

public enum GUIStateEnum{
	OnStart, EndResult, EndLevel, Interface, Pause, InComic, Nothing
}

[SerializeAll]
public class ChromatoseManager : MonoBehaviour {
	
	
	//Variables Assignation Public
	public GameObject comicCompleteAnim;
	public GameObject oneShotSpritePrefab;
	public tk2dSpriteCollectionData bubbleCollection;
	public tk2dFontData chromatoseFont;
	public GameObject rewardsGuy;
	
	
	//Variables Instance Enum
	private GUIStateEnum _GUIState;
	
	//Avatar Data
	private Avatar avatar;
	private AvatarPointer avatarP;
	private Vector3 _AvatarStartingPos;
	
	//Class & Manager
	public static ChromatoseManager manager; 
	private ChromaRoomManager _RoomManager;
	private RoomInstancier _RoomSaver;
	private AudioSource sfxPlayer;
	private tk2dSprite spriteInfo;
	public ChromHUD hud = new ChromHUD();
	public TimeTrialTimes timeTrialClass = new TimeTrialTimes();
	public PrefabCollection prefab = new PrefabCollection();
	private GUISkin skin;
	private string _GameName = "CheckpointSave";
	
	//Variables Bool
	private bool _OnPause = false;
	private bool _CanShowAction = false;
	private bool actionPressed;
	private bool showingAction;
	private bool checkedComicStats = false;
	public bool playedCompleteFlourish = false;
	public bool playedSecretFlourish = false;
	public bool givenCols = false;
	public bool animsReady = false;
		public bool AnimsReady{
			set { animsReady = value; }
		}
	private bool inComic = false;
		public bool InComic{
			get{ return inComic; }
			
		}
	public bool _TimeTrialModeActivated = false;
		public bool TimeTrialMode{
			get{return _TimeTrialModeActivated;}
		}
	
	public bool _NoDeathModeActivated = false;
		public bool NoDeathMode{
			get{return _NoDeathModeActivated;}
		}	
	
	
	//Variable ActionWindows
	private Actions currentAction = Actions.Nothing;
	private Texture actionTexture;
	private Texture shownActionTexture;
	public delegate void ActionDelegate();
	public ActionDelegate actionMethod;
	private float actionSlideSpeed = 10f;

	public void UpdateAction(Actions action, ActionDelegate method){
		if (action <= currentAction || action == Actions.Nothing){
			currentAction = action;
			actionMethod = method;
			showingAction = true;
		}
	}
	public void UpdateActionTexture(){
		shownActionTexture = actionTexture;
	}
	
	
	//Variables Collectible
	private bool _CollAlreadyAdded = false;
		public bool CollAlreadyAdded{
			get{return _CollAlreadyAdded;}
			set{_CollAlreadyAdded = value;}
		}

	
	private int _WhiteCollected = 0;
		public int wCollected{
			get{return _WhiteCollected;}
		}
	private int _RedCollected = 0;
		public int rCollected{
			get{return _RedCollected;}
		}
	private int _BlueCollected = 0;
		public int bCollected{
			get{return _BlueCollected;}
		}
	private int _ComicThumbCollected = 0;
		public int comicCollected{
			get{return _ComicThumbCollected;}
		}
	
	private int _TotalWhiteColl = 0;
	private int _TotalRedColl = 0;
	private int _TotalBlueColl = 0;
	private int _TotalComicThumb = 0;
	
	
	//Variables CheckPoint
	private bool _NewLevel = false;
	private bool _FirstLevelCPDone = false;
		public bool FirstLevelCPDone {
			get{return _FirstLevelCPDone;}
			set{_FirstLevelCPDone = value;}
		}
	private Transform curCheckpoint;
	
	
	//Variables Listes
	private SpriteFader[] _FaderList;
	
	
	//Variables Room
	private int curRoom;
		public int CurRoom{
			get{ return curRoom; }
		}
	
	


	private float aX;
	
	private int rN;
	private int gN;
	private int bN;
	private int wN;
	private int cN;
	
	private int barX;
	private int barY;
	private int barMinY;
	private int barMaxY;
	
	private bool flashyBar = false;
	private int flashyBarTimer = 0;
	private int flashyBarTiming = 20;
	
	
	
	public void Healed(){
		flashyBar = true;
		flashyBarTimer = flashyBarTiming;
		Debug.Log("HEYR");
	}
	
	private int tankX = 1219;
	private int tankY = 28;
	
	
	private Vector2 textOffset = new Vector2 (55f, 8);
	
	//private Couleur[] colCouleurs = {Couleur.red, Couleur.green, Couleur.blue, Couleur.white, Couleur.black};		//the black is for the comics
	
	//private bool[] showingCol = {false, false, false, false, false};
	private int[] colTimers = {0, 0, 0, 0, 0};

	
	private int showTiming = 1;
	private float defaultSpeed = 1.2f;
	private int refreshTiming = 75;
	
	private float[] track;
	public AudioClip[] sfx;

	[System.Serializable]
	public class ChromHUD {
		
		public bool _GlowComicNb;
		public bool _CanFlash;
		public bool _OnFlash;
		public bool _AfterComic;
		public float flashTimer;
		
		public Texture mainBox;
		public Texture smallBox;
		public Texture[] energyTank;
		//public Texture energyBar;
		public Texture actionButton;
		public Texture absorbAction;
		public Texture buildAction;
		public Texture destroyAction;
		public Texture payAction;
		public Texture whitePayAction;
		public Texture releaseAction;
		public Texture returnAction;
		//public Texture energyBarFlash;
		
		
		public Texture redCollectible;
		public Texture greenCollectible;
		public Texture blueCollectible;
		public Texture whiteCollectible;
		public Texture comicCounter, comicCounter2;
		
		public Texture _TimeTrialBox;
		
		public Texture[] pauseButton;
		public Texture pauseWindows;
		public Texture blackBG;
		public Texture endResultWindows;
		
		public Texture _AvatarLoadingLoop1, _AvatarLoadingLoop2, _BulleLoadingLoop1, _BulleLoadingLoop2;
		public GUISkin _PlayButtonSkin, _GreenlightSkin;
		public bool _loop2 = false;
		public float loopingCounter;
		
		//TEXTURE POUR WEBPLAYER
		public GUISkin skinSansBox;
		
		public Font chromFont;
		
		private tk2dTextMesh rColMesh;
		private tk2dTextMesh gColMesh;
		private tk2dTextMesh bColMesh;
	}
	
	[System.Serializable]
	public class PrefabCollection{
		
		public GameObject collectible;
		public GameObject checkPoint;
		
	}
	
#endregion
	
#region TimeTrial Data & Methods
	[System.Serializable]
	public class TimeTrialTimes {
		
		private ChromatoseManager _Manager;
		
		private float _TimerTime = 0f;
		private float _StartTimerTime = 0f;
		private float _SecCounter = 0f;
		private float _MinCounter = 0f;
		private float _FractionCounter = 0f;
		private float _TimeToDisplay = 0;
		private float _FinalLevelTimes = 0;
		
		private bool _DisplayScore = false;
		public bool DisplayScore{
			get{return _DisplayScore;}
			set{_DisplayScore = value;}
		}
		private bool _DisplayWinWindows = false;
		public bool DisplayWinWindows{
			get{return _DisplayWinWindows;}
			set{_DisplayWinWindows = value;}
		}
		
		public bool _TimerOnPause = false; 		//<--- A Remettre private
			public bool TimerOnPause{
				get{return _TimerOnPause;}
			}
		
		private float _TotalPauseTime = 0f;
		private float _PauseTime = 0f;
		private float _StartPauseTime = 0f;
		private float _EndPauseTime = 0f;

		public static string _TimeString = "";
			public string TimeString{
				get{return _TimeString;}
				set{_TimeString = value;}
			}

		public static List<float> _TimesList;// = new List<float>(10);
			public List<float> TimeList{
				get{return _TimesList;}
			}
		
		public float _Tuto_Time2Beat = 0;
		public float _Lev1_Time2Beat = 0;
		public float _Lev2_Time2Beat = 120;
		public float _Lev3_Time2Beat = 0;
		public float _Lev4_Time2Beat = 0;
		public float _Lev5_Time2Beat = 0;
		public float _Lev6_Time2Beat = 0;
		public float _Lev7_Time2Beat = 0;
		public float _Lev8_Time2Beat = 0;
		public float _Lev9_Time2Beat = 0;
		
		
		[XmlAttribute("_RecordsList")]
		public static List<float> _RecordsList;// = new List<float>(10);
			public List<float> RecordsList{
				get{return _RecordsList;}
			}
		
		private float _NEW_Tuto_Time2Beat = 0;
		private float _NEW_Lev1_Time2Beat = 134.25f;
		private float _NEW_Lev2_Time2Beat = 134.77f;
		private float _NEW_Lev3_Time2Beat = 0;
		private float _NEW_Lev4_Time2Beat = 0;
		private float _NEW_Lev5_Time2Beat = 0;
		private float _NEW_Lev6_Time2Beat = 0;
		private float _NEW_Lev7_Time2Beat = 0;
		private float _NEW_Lev8_Time2Beat = 0;
		private float _NEW_Lev9_Time2Beat = 0;
		
		private float _Min2Beat = 0;
		private float _Sec2Beat = 0;
		private float _Fraction2Beat = 000;
		
		
		private static List<float> _NewRecordList;
		public List<float> NewRecordList{
			get{return _NewRecordList;}
		}
		
		private float _NewRecordTimes_Tuto = 0;
		private float _NewRecordTimes_Lev1 = 0;
		private float _NewRecordTimes_Lev2 = 0;
		private float _NewRecordTimes_Lev3 = 0;
		private float _NewRecordTimes_Lev4 = 0;
		private float _NewRecordTimes_Lev5 = 0;
		private float _NewRecordTimes_Lev6 = 0;
		private float _NewRecordTimes_Lev7 = 0;
		private float _NewRecordTimes_Lev8 = 0;
		private float _NewRecordTimes_Lev9 = 0;
		
		
		public static string _Time2BeatString = "";
			public string Time2BeatString{
				get{return _Time2BeatString;}
				set{_Time2BeatString = value;}
			}
		
		
		public void SetupTTT(){
		
		
		if(LevelSerializer.IsDeserializing)return;
			_Manager = ChromatoseManager.manager;
			
			//TODO Finir remplir les tableau contenant les temps a battre	
			if (_TimesList == null){
				_TimesList = new List<float>(10){ 	_Tuto_Time2Beat, _Lev1_Time2Beat, _Lev2_Time2Beat, 
													_Lev3_Time2Beat, _Lev4_Time2Beat, _Lev5_Time2Beat, 
													_Lev6_Time2Beat, _Lev7_Time2Beat, _Lev8_Time2Beat, _Lev9_Time2Beat };
			}	
			if (_RecordsList == null){
				_RecordsList = new List<float>(10){ _NEW_Tuto_Time2Beat, _NEW_Lev1_Time2Beat, _NEW_Lev2_Time2Beat, 
													_NEW_Lev3_Time2Beat, _NEW_Lev4_Time2Beat, _NEW_Lev5_Time2Beat, 
													_NEW_Lev6_Time2Beat, _NEW_Lev7_Time2Beat, _NEW_Lev8_Time2Beat, _NEW_Lev9_Time2Beat };
			}
			if (_NewRecordList == null){
				_NewRecordList = new List<float>(10){ _NewRecordTimes_Tuto, _NewRecordTimes_Lev1, _NewRecordTimes_Lev2,
													_NewRecordTimes_Lev3, _NewRecordTimes_Lev4, _NewRecordTimes_Lev5,
													_NewRecordTimes_Lev6, _NewRecordTimes_Lev7, _NewRecordTimes_Lev8, _NewRecordTimes_Lev9 };
			}
		}
		
		public string DisplayTimes2Beat(){
		
			
			_TimeToDisplay = (_TimesList[_Manager.CurRoom] > _RecordsList[_Manager.CurRoom]? _TimesList[_Manager.CurRoom] : _RecordsList[_Manager.CurRoom]);
			
			_Min2Beat = Mathf.Floor(_TimeToDisplay/60f);		
			_Sec2Beat = Mathf.RoundToInt(_TimeToDisplay % 60f);
			_Fraction2Beat = (_TimeToDisplay * 1000)%1000;
			
			_Time2BeatString = _Min2Beat + ":" + _Sec2Beat + ":" + _Fraction2Beat;
			_Time2BeatString = string.Format("{00:00}:{1:00}:{2:000}",_Min2Beat,_Sec2Beat,_Fraction2Beat);
			
			return _Time2BeatString;
		}
										//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
										//<-------------TIME TRIAL COUNTER------------->
										//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		public void TimeTrialCounter(){
				
			_TimerTime = Time.timeSinceLevelLoad - _TotalPauseTime - _StartTimerTime;		// - starttime
	
			_MinCounter = Mathf.Floor(_TimerTime/60f);
			_SecCounter = Mathf.RoundToInt(_TimerTime % 60f);
			_FractionCounter = (_TimerTime * 1000)%1000;			//TOFIX Glitch dans le compteur, se reset au retour de la pause au niveau des fraction
			_TimeString = _MinCounter + ":" + _SecCounter + ":" + _FractionCounter;
			
			_TimeString = string.Format("{00:00}:{1:00}:{2:000}",_MinCounter,_SecCounter,_FractionCounter);	
		
		}
		
		public bool CheckForNewRecords(){
			bool newRecord = false;
			//TODO Faire CheckUp pour verifier si le joueur a tous les collectible et comics
			if(_TimerTime < _TimesList[_Manager.CurRoom]){
				_RecordsList[_Manager.CurRoom] = _TimerTime;
				
			
				newRecord = true;
			}	
			return newRecord;
		}
		
		//TODO Faire Fonction Save/Load les records sur un XML externe
		

		
		public bool StopChallenge(){
			
			bool winGame = false;
			_DisplayScore = true;
			_FinalLevelTimes = _TimerTime;
			
			//Stop le Manager et le Jeu
			_Manager.avatar.CannotControlFor(false, 0f);
			Time.timeScale = 0;
			
			//Verifie si le temps du Joueur bats le Temps a battre
			if (_FinalLevelTimes < _TimeToDisplay){
				winGame = true;
				return winGame;
			}
			return winGame;
		}
		
		/// Mets le Compteur a Pause
		public void PauseTimer(){
			_TimerOnPause = true;
			_StartPauseTime = Time.fixedTime;
			Debug.Log("Pause start at : " + _StartPauseTime);
		}
		public void UnpauseTimer(){
			_TimerOnPause = false;
			_EndPauseTime = Time.fixedTime;
			_TotalPauseTime += _EndPauseTime - _StartPauseTime;
			Debug.Log("Pause ends at : " + _EndPauseTime);
			Debug.Log("Total de la Pause : " + _TotalPauseTime);
		}
		public void ResetTimer(){
			_StartTimerTime = Time.fixedTime;
			//TODO remettre les variable pour la endresultWindows a defaut
		}
		
		public void RestartLevel(){
			
			manager.ResetPos();
			ResetTimer();	
			DisplayScore = false;
			_DisplayWinWindows = false;
			Time.timeScale = 1;
			_Manager.avatar.CanControl();
		}
	}
#endregion

	
#region Initialisation et Setup	(Start)
	// Use this for initialization
	void Awake () {
		manager = this;
	}
	
	void Start(){
		
		_RoomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<ChromaRoomManager>();
		_RoomSaver = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<RoomInstancier>();
		_TotalComicThumb = _RoomManager.UpdateTotalComic();
		_FaderList = FindObjectsOfType(typeof(SpriteFader)) as SpriteFader[];
		avatar = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		sfxPlayer = GetComponent<AudioSource>();
		
		_GUIState = GUIStateEnum.OnStart;
		
		CheckStartPos();
		
		CalculeCollectiblesInLevel();
		
		if(_TimeTrialModeActivated){
			timeTrialClass.SetupTTT();
			timeTrialClass.DisplayTimes2Beat();
		}
		
		//Initialise pour la premiere la security pour le FirstCP
		_FirstLevelCPDone = false;
		
		//DontDestroyOnLoad(manager);
		if (!hud.mainBox){
			Debug.LogWarning("There are some missing textures....");
		}

		
						//HACK Getting a HUD_skin from the Resources folder
		skin = Resources.Load("Menus/HUD_skin") as GUISkin;

		
		aX = hud.absorbAction.width * 1.5f;
		
		actionTexture = hud.absorbAction;
		shownActionTexture = actionTexture;
		
		rewardsGuy = GameObject.FindWithTag("rewardsGuy");
		if (!rewardsGuy){
			//Debug.LogWarning("Hey doofus! There's no rewards guy in this level! Is there even a comic!?");
		}
		else{
			
		}
	}

	void ResetStats(){
		/*roomStats = new RoomStats[14]{	new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), 
										new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), 
										new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats()};*/
	}
	
	//TODO Faire Fonction qui Save/Load les RoomStats, faire en XML pour pouvoir l'utiliser avec les checkpoint et pesistant

#endregion	
	
#region Update & LateUpdate
	void Update () {
		
		
		if (Input.GetKeyDown(KeyCode.PageUp)){
			if (Application.loadedLevel == 0) return;
			Application.LoadLevel(Application.loadedLevel - 1);
		}
		
		if (Input.GetKeyDown(KeyCode.PageDown)){
			if (Application.loadedLevel == Application.levelCount - 1) return;
			Application.LoadLevel(Application.loadedLevel + 1);
		}
		
		if (Input.GetKeyDown(KeyCode.Escape)){
			ManagerPause();
		}
		
		if (inComic){
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.P)){
				//comicTransition.Return();
			}
		}
				
		
		//TODEL KeyTouch M pour pause a Deleter lorsque les tests seront fini
		/*
		if(Input.GetKeyDown(KeyCode.M)){
			ManagerPause();
		}*/
		
		switch(_GUIState){
		case GUIStateEnum.OnStart:
			if(Input.GetKeyDown(KeyCode.Space)){
				_GUIState = GUIStateEnum.Interface;
				avatar.CanControl();
				sfxPlayer.PlayOneShot(sfx[6]);
			}
			break;
		}
		
	
		//Appel du timeTrial
		if(_TimeTrialModeActivated && !timeTrialClass.TimerOnPause){
			timeTrialClass.TimeTrialCounter();
		}
	}
	
	void LateUpdate(){
		
										//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
										//<--------------ACTION BUTTON!---------------->
										//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
								
		switch (currentAction){
		case Actions.Back:
			actionTexture = hud.returnAction;
			break;
		case Actions.Absorb:
			actionTexture = hud.absorbAction;
			break;
		case Actions.Destroy:
			actionTexture = hud.destroyAction;
			break;
		case Actions.Build:
			actionTexture = hud.buildAction;
			break;
		case Actions.Pay:
			actionTexture = hud.payAction;
			break;
		case Actions.whitePay:
			actionTexture = hud.whitePayAction;
			break;
		case Actions.Release:
			actionTexture = hud.releaseAction;
			break;
			
		default:
			actionMethod = null;
			break;
		}
		

		
		if (currentAction == Actions.Nothing && aX < hud.absorbAction.width * 1.5f){
			aX -= actionSlideSpeed;
			if (aX < -hud.absorbAction.width * 1.5f){
				aX = Mathf.Abs(aX);
			}
		}
		if (!showingAction){
			currentAction = Actions.Nothing;
		}
		
		if (showingAction && aX != 0 && shownActionTexture == actionTexture){
			
			aX -= actionSlideSpeed;
			if (aX > -9 && aX < 9) aX = 0;
			if (aX < -hud.absorbAction.width * 1.5f){
				aX = Mathf.Abs(aX);
			}
		}
		
		if (showingAction && shownActionTexture != actionTexture){
			Debug.Log("Should be working to update the action texture");
			if (aX >= hud.absorbAction.width * 1.5f){
				
				UpdateActionTexture();
				
			}
			else{
				aX -= actionSlideSpeed;
			}
		}
		
		
		//Effectue l'action si une action peut etre effectuer et si la touche P est presser
		if (Input.GetKeyDown(KeyCode.P) && currentAction > 0 && actionMethod != null && !inComic){
			actionMethod();
		}
		showingAction = false;
		
	}
#endregion
		
#region OnGUI (HUD & Windows)
	void OnGUI(){
		
		float horizRatio = Screen.width / 1280.0f;
		float vertiRatio = Screen.height / 960.0f;
		
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,new Vector3(horizRatio, vertiRatio, 1f));
		
		
			//BackUp de la Matrix Initiale
			Matrix4x4 matrixBackup = GUI.matrix;
			
			Rect bgRect = new Rect(0, 0, 1280, 960);
			
			Rect mainRect = new Rect(1090, -180, hud.mainBox.width * 1.5f + 10, hud.mainBox.height * 1.5f);
			Rect comicRect = new Rect(1090, 175, hud.comicCounter.width * 1.5f + 10, hud.comicCounter.height * 1.5f);
			Rect wColRect = new Rect(1090, 255, hud.whiteCollectible.width * 1.5f + 10, hud.whiteCollectible.height * 1.5f);
			Rect rColRect = new Rect(1090, 335, hud.redCollectible.width * 1.5f + 10, hud.redCollectible.height * 1.5f);
			Rect bColRect = new Rect(1090, 415, hud.blueCollectible.width * 1.5f + 10, hud.blueCollectible.height * 1.5f);
		
			Rect actionRect = new Rect(1145, 30, 60 * 1.5f, 52 * 1.5f);
			Rect timeTrialRect = new Rect(25, 20, hud._TimeTrialBox.width + 100f, hud._TimeTrialBox.height + 10f);
			Rect pauseWindowsRect = new Rect (128, 100, 700, 360);
			Rect endResultRect = new Rect (0, 0, Screen.width, Screen.height);
		
			/*NE SEMBLE PAS SERVIR
			Rect energyRect = new Rect(barX, barMinY, hud.energyBar.width, hud.energyBar.height);
			Rect flashyRect = new Rect(barX - 7, barMinY - 8, hud.energyBarFlash.width, hud.energyBarFlash.height);
			Rect tankRect = new Rect(tankX, tankY, 80, 128);*/
		
		
		#region OnGUI OnStart
		switch(_GUIState){
		case GUIStateEnum.OnStart:
			GUI.skin = hud.skinSansBox;
			
			avatar.CannotControlFor(false, 0);
			
			//Black BackGround
			GUI.DrawTexture(bgRect, hud.blackBG);
			GUI.skin = hud._PlayButtonSkin;
			GUI.skin.button.fontSize = 78;
			if(GUI.Button(new Rect(442, 70, 402, 402), "")){
				_GUIState = GUIStateEnum.Interface;
				avatar.CanControl();
				sfxPlayer.PlayOneShot(sfx[6]);
			}
			
			//IMAGE AVATAR
				
				hud.loopingCounter += Time.deltaTime;
				if(hud.loopingCounter > 1){
					hud.loopingCounter = 0;
					hud._loop2 = !hud._loop2;
				}
				if(!hud._loop2){
					GUI.DrawTexture(new Rect(333, 610, 225, 225), hud._AvatarLoadingLoop1);
				}
				else{
					GUI.DrawTexture(new Rect(333, 610, 225, 225), hud._AvatarLoadingLoop2);
				}
				
				
				if(!hud._loop2){
					GUI.DrawTexture(new Rect(505, 480, 410, 410), hud._BulleLoadingLoop1);
				}
				else{
					GUI.DrawTexture(new Rect(505, 480, 410, 410), hud._BulleLoadingLoop2);
				}
				
				
									
				GUI.skin = hud._GreenlightSkin;
					if(GUI.Button(new Rect(578, 545, 273, 273), "")){
						Application.OpenURL("http://store.steampowered.com/");
					}
			
			break;
			#endregion
			
		#region OnGUI Interface
		case GUIStateEnum.Interface:

			GUI.skin = hud._PlayButtonSkin;
			
			GUI.DrawTexture(mainRect, hud.mainBox);
			
			if(_TimeTrialModeActivated){
				GUI.BeginGroup(timeTrialRect);												//TimeTrial Counter
					GUI.skin.textArea.normal.textColor = Color.black;
					GUI.DrawTexture(new Rect(0, 0, hud._TimeTrialBox.width + 100f, hud._TimeTrialBox.height + 10f), hud._TimeTrialBox);
					GUI.TextArea(new Rect(textOffset.x - 20f, textOffset.y, 250, 40), "Time To Beat : " + "'" + timeTrialClass.DisplayTimes2Beat() +"'");
					GUI.TextArea(new Rect(textOffset.x - 20f, textOffset.y + 30f, 250, 40), "Your Time : " + "'" + timeTrialClass.TimeString + "'");
				GUI.EndGroup();
			}
			
			GUI.BeginGroup(comicRect);										//comic counter		
				//GUI.skin = skin;
				GUI.skin.textArea.normal.textColor = Color.black;
				if(_ComicThumbCollected >= _TotalComicThumb && _TotalComicThumb != 0 && !hud._AfterComic){
					hud._CanFlash = true;
				}
				if(hud._CanFlash){
					hud.flashTimer += Time.deltaTime;
					if(hud.flashTimer >= 1){
						hud.flashTimer = 0;
						hud._OnFlash = !hud._OnFlash;
					}
				}
				if(!hud._OnFlash || hud._AfterComic){
					GUI.DrawTexture(new Rect(0, 0, hud.comicCounter.width * 1.5f + 10, hud.comicCounter.height * 1.5f), hud.comicCounter);
				}
				else{
					GUI.DrawTexture(new Rect(0, 0, hud.comicCounter.width * 1.5f + 10, hud.comicCounter.height * 1.5f), hud.comicCounter2);
				}
				GUI.skin.textArea.fontSize = 35;
				GUI.skin.textArea.normal.textColor = Color.black;
				GUI.TextArea(new Rect(textOffset.x * 1.5f, textOffset.y * 1.5f, 100, 50), _ComicThumbCollected + " /" + _TotalComicThumb);
				
				
			GUI.EndGroup();
			
			GUI.BeginGroup(wColRect);										//white collectible
				GUI.skin.textArea.normal.textColor = Color.white;
				GUI.DrawTexture(new Rect(0, 0, hud.whiteCollectible.width * 1.5f + 10, hud.whiteCollectible.height * 1.5f), hud.whiteCollectible);
				GUI.TextArea(new Rect(textOffset.x * 1.5f + 10, textOffset.y * 1.5f, 100, 50), _WhiteCollected.ToString());// + " / " + _TotalWhiteColl.ToString());
				
			GUI.EndGroup();
			
			GUI.BeginGroup(rColRect);										//red collectible
				GUI.skin.textArea.normal.textColor = Color.red;
				GUI.DrawTexture(new Rect(0, 0, hud.redCollectible.width * 1.5f + 10, hud.redCollectible.height * 1.5f), hud.redCollectible);
				GUI.TextArea(new Rect(textOffset.x * 1.5f + 10, textOffset.y * 1.5f, 100, 50), _RedCollected.ToString());// + " / " + _TotalRedColl.ToString());
				
			GUI.EndGroup();

			
			GUI.BeginGroup(bColRect);										//blue collectible
				GUI.skin.textArea.normal.textColor = Color.blue;
				GUI.DrawTexture(new Rect(0, 0, hud.blueCollectible.width * 1.5f + 10, hud.blueCollectible.height * 1.5f), hud.blueCollectible);
				GUI.TextArea(new Rect(textOffset.x * 1.5f + 10, textOffset.y * 1.5f, 100, 50), _BlueCollected.ToString());// + " / " + _TotalBlueColl.ToString());
				
			GUI.EndGroup();
			
			
			GUI.BeginGroup(actionRect);										//Action icon
				
				GUI.DrawTexture(new Rect(aX, 0, hud.absorbAction.width * 1.5f, hud.absorbAction.height * 1.5f), actionTexture);
			
			GUI.EndGroup();			
			
			break;
			#endregion
			
		#region OnGUI Pause
		case GUIStateEnum.Pause:
			
			GUI.skin = hud.skinSansBox;
			
			GUIUtility.RotateAroundPivot(-12f, new Vector2(320, 215));
			GUI.BeginGroup(pauseWindowsRect);
				//PAUSE WINDOWS
				GUI.DrawTexture(new Rect(pauseWindowsRect.width*0.05f, pauseWindowsRect.height*0.05f, pauseWindowsRect.width*0.9f, pauseWindowsRect.height*0.9f), hud.pauseWindows);
				
				//TEXTE PAUSE
				GUI.skin.textArea.fontSize = 76;
				//GUI.skin.textArea.alignment = TextAnchor.UpperCenter;
				GUI.TextArea(new Rect(pauseWindowsRect.width*0.35f, pauseWindowsRect.height*0.23f, pauseWindowsRect.width*0.4f, pauseWindowsRect.height*0.23f), "PAUSE");
				//GUI.skin.textArea.alignment = TextAnchor.UpperLeft;
			
				//BOUTON PAUSE
				GUI.skin.button.fontSize = 54;
				if(GUI.Button(new Rect(pauseWindowsRect.width*0.25f, pauseWindowsRect.height*0.46f, pauseWindowsRect.width*0.5f, pauseWindowsRect.height*0.18f), new GUIContent("RESUME", "Hover"))){
					ManagerPause();
					_GUIState = GUIStateEnum.Interface;
					sfxPlayer.PlayOneShot(sfx[6]);
					if(Event.current.type == EventType.Repaint){
						if(GUI.tooltip == "Hover"){
							sfxPlayer.PlayOneShot(sfx[5]);
						}					
					}
				}
			
				if(GUI.Button(new Rect(pauseWindowsRect.width*0.25f, pauseWindowsRect.height*0.65f, pauseWindowsRect.width*0.5f, pauseWindowsRect.height*0.18f), "MAIN MENU")){
					_GUIState = GUIStateEnum.EndLevel;
					Time.timeScale = 1;
					Application.LoadLevelAsync(0);
				
				//TODO Si en TimeTrial Challenge, annuler le challenge
				
				}
			GUI.EndGroup();
			
			break;
			#endregion
			
		#region OnGUI EndLevel
		case GUIStateEnum.EndLevel:
			
			break;
			#endregion
			
		#region OnGUI EndResult
		case GUIStateEnum.EndResult:
			
			if(_TimeTrialModeActivated && timeTrialClass.DisplayScore){
				if(!timeTrialClass.DisplayWinWindows){
					//Affichage du Lose result
					GUI.BeginGroup(endResultRect);
						if (GUI.Button(new Rect(390, 360, hud.pauseButton[0].width, hud.pauseButton[0].height), hud.pauseButton[0])){
							
						}
						GUI.skin.textArea.normal.textColor = Color.red;
						GUI.DrawTexture(new Rect(0, 0, hud.endResultWindows.width, hud.endResultWindows.height), hud.endResultWindows);
						GUI.TextArea(new Rect (textOffset.x, textOffset.y, 500, 50), "YOU LOSE, SORRY !");
						GUI.TextArea(new Rect (515, 230, 500, 50), timeTrialClass.Time2BeatString);
					
						GUI.Button(new Rect(890, 360, 300, 100), "NEXT LEVEL");
					GUI.EndGroup();
				}
				else{
					//Affichage du win result
					GUI.BeginGroup(endResultRect);
						GUI.skin.textArea.normal.textColor = Color.red;
						GUI.DrawTexture(new Rect(0, 0, hud.endResultWindows.width, hud.endResultWindows.height), hud.endResultWindows);
						GUI.TextArea(new Rect (540, 235, 500, 50), "YOU WIN !");
						GUI.TextArea(new Rect (515, 285, 500, 50), "NEW TIME 2 BEAT");
						GUI.TextArea(new Rect (515, 335, 500, 50), timeTrialClass.TimeString);
						
						GUI.TextArea(new Rect (390, 530, 500, 50), "RETRY");
						if (GUI.Button(new Rect(390, 560, hud.pauseButton[0].width, hud.pauseButton[0].height), hud.pauseButton[0])){
							timeTrialClass.RestartLevel();							
						}
					GUI.EndGroup();					
				}
			}
		
			
			break;
			#endregion
			
		#region OnGUI InComic
		case GUIStateEnum.InComic:
			
			break;
			#endregion
			
			
		case GUIStateEnum.Nothing:
			
			break;
			
		}
	}
#endregion	
	
#region Collectibles Methods	
	public void AddCollectible(Color col){
		
		if(!_CollAlreadyAdded){
			_CollAlreadyAdded = true;
			StartCoroutine(ResetCanGrabCollectibles(0.05f));
			
			if(col == Color.white){
				_WhiteCollected++;
			}
			else if(col == Color.red){
				_RedCollected++;
			}
			else if(col == Color.blue){
				_BlueCollected++;
			}
			else{
				Debug.LogWarning("Not a real collectible.");
			}
		}
	}

	public int GetCollectibles(Color color){
		
		if(color == Color.white){
			return _WhiteCollected;
		}
		else if(color == Color.red){
			return _RedCollected;
		}
		else if(color == Color.blue){
			return _BlueCollected;
		}
		else{
			Debug.Log("CANT CHECK THIS COLOR?");
			return 0;
		}
	}
	
	public void RemoveCollectibles(Color color, int amount, Vector3 pos){
		
		
		if(color == Color.white){
			_WhiteCollected -= amount;
			BlowWhiteColl(amount, pos);			
		}
		else if(color == Color.red){
			_RedCollected -= amount;
			ShootRedCollOnMini(amount, pos);
			Debug.Log("Remove "+amount);
		}
		else if(color == Color.blue){
			_BlueCollected -= amount;
			BlowBlueColl(amount, pos);
		}
		else{
			Debug.Log("Cant delete this collectable, check color");
		}
	}
#endregion	
	
#region Comics Stuff
																			//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
																			//<-------------COMICS AND STUFF!-------------->
																			//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	public void AddComicThumb(){
		_ComicThumbCollected++;
	}

#endregion	
	
#region Fonctions Diverses
	Avatar.DeathAnim danim;		//avatar's death animation
	public void Death(){
		
		if(!_NoDeathModeActivated){
			sfxPlayer.PlayOneShot(sfx[11]);
			danim = new Avatar.DeathAnim();
			danim.PlayDeath(Reset);
			//avatar.SendMessage("FadeAlpha", 0f);
			avatar.movement.SetVelocity(Vector2.zero);
			StartCoroutine(OnDeath(0.15f));
			StartCoroutine(RestartRoom());
			
			avatar.CancelOutline();
			avatar.Gone = true;
		}
		else{
			sfxPlayer.PlayOneShot(sfx[11]);
			danim = new Avatar.DeathAnim();
			danim.PlayDeath(Reset);
			//avatar.SendMessage("FadeAlpha", 0f);
			avatar.movement.SetVelocity(Vector2.zero);
			StartCoroutine(OnDeath(0.15f));
			avatar.CancelOutline();
			avatar.Gone = true;
			Application.LoadLevel(2);
			ResetStats();
		}
		
		//avatar.renderer.enabled = false;
	}
	
	public void Reset(tk2dAnimatedSprite anim, int index){
		Destroy(danim.go);
		//avatar.renderer.enabled = true;
		avatar.Gone = false;
		avatar.transform.position = curCheckpoint.transform.position;
		avatar.transform.rotation = Quaternion.identity;
		avatar.movement.SetVelocity(Vector2.zero);
		//avatar.SetColour(0, 0, 0);
	}
	
	void TriggerQuestionMark(){
		
		GameObject question = GameObject.FindWithTag("questionMark");
		if (question)
			question.SendMessage("Trigger");
	}
#endregion	
	
#region Fonctions CheckPoint
	public void NewFirstCheckPoint(Transform cp){
		curCheckpoint = cp;
		foreach(SpriteFader _SFader in _FaderList){
			_SFader.SaveState();
		}
	}
	
	public void NewCheckpoint(Transform cp){
		curCheckpoint = cp;
		sfxPlayer.PlayOneShot(sfx[10]);
		GameObject[] cps = GameObject.FindGameObjectsWithTag("checkpoint");
		foreach (GameObject check in cps){
			Checkpoint script = check.GetComponent<Checkpoint>();
			if (check.transform == cp){
				script.Active = true;
			}
			else{
				script.Active = false;
			}
		}
		
		foreach(SpriteFader _SFader in _FaderList){
			_SFader.SaveState();
			//Debug.Log("SaveState in CP");
		}
	}
	
	public void SaveRoom(){
		LevelSerializer.SaveGame(_GameName);
		Debug.Log("Save");
	}
	public void LoadRoom(){
		foreach(var sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName]) { 
         	LevelSerializer.LoadNow(sg.Data);
			Time.timeScale = 1;
    	} 
		Debug.Log("Load");
	}
	
#endregion	
	
#region Static Functions	
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<-------------STATIC FUNCTIONS!-------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	
	
	public GameObject OneShotAnim( string animName, float time, Vector3 callerPosition){
		
		GameObject newGuy = GameObject.Instantiate(oneShotSpritePrefab, callerPosition, Quaternion.identity) as GameObject;
		
		spriteInfo = newGuy.GetComponent<tk2dSprite>();
		
			spriteInfo.SetSprite(spriteInfo.Collection.GetSpriteIdByName(animName));
		GameObject.Destroy(newGuy, time);
		return newGuy;
		
	}
#endregion	
	
#region A Classer
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<-------------FONCTION DU CHU!--------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>	
	
	void CheckStartPos(){
		_AvatarStartingPos = avatar.transform.position;
	}
	
	void CalculeCollectiblesInLevel(){

		foreach (GameObject col in GameObject.FindGameObjectsWithTag("collectible")){
			if(col.GetComponent<Collectible2>().colorCollectible == Collectible2._ColorCollectible.White){
				_TotalWhiteColl++;
			}
			else if(col.GetComponent<Collectible2>().colorCollectible == Collectible2._ColorCollectible.Red){
				_TotalRedColl++;
			}
			else if(col.GetComponent<Collectible2>().colorCollectible == Collectible2._ColorCollectible.Blue){
				_TotalBlueColl++;
			}
			else{
				Debug.Log ("A Collectible doesn't have a Color or a Script");
			}
		}	
	}
	
	void BlowWhiteColl(int amount, Vector3 pos){
		
		for(int i = 0; i < amount; i++){
			Vector2 randomVelocity = Random.insideUnitCircle.normalized * Random.Range(40, 65);
			Vector3 randomPos = avatar.transform.position + (Vector3)randomVelocity;
			randomPos.z = -5;
			GameObject wCol = Instantiate(prefab.collectible, randomPos, Quaternion.identity)as GameObject;
			wCol.GetComponent<Collectible2>().effect = true;
			wCol.GetComponent<Collectible2>().colorCollectible = Collectible2._ColorCollectible.White;
			avatar.sfxPlayer.PlayOneShot(sfx[13]);
			StartCoroutine(DelaiToBlowColl(0.5f, wCol));
		}
	}
	
	void BlowBlueColl(int amount, Vector3 pos){
		for(int i = 0; i < amount; i++){
			Vector2 randomVelocity = Random.insideUnitCircle.normalized * Random.Range(40, 65);
			Vector3 randomPos = avatar.transform.position + (Vector3)randomVelocity;
			GameObject bCol = Instantiate(prefab.collectible, randomPos, Quaternion.identity)as GameObject;
			bCol.GetComponent<Collectible2>().effect = true;
			bCol.GetComponent<Collectible2>().colorCollectible = Collectible2._ColorCollectible.Blue;
			avatar.sfxPlayer.PlayOneShot(sfx[7]);
			StartCoroutine(DelaiToBlowColl(0.5f, bCol));
		}
	}
	void ShootRedCollOnMini(int amount, Vector3 pos){
		Debug.Log("Shoot");
		for(int i = 0; i < amount; i++){
			StartCoroutine(ShootRed(i*0.5f, pos));
			
		}
	}
	
	void ManagerPause(){
		avatar.Pause();
	
		if(Time.timeScale==1){
			Time.timeScale = 0;
		}
		else{
			Time.timeScale = 1; 
		}
		
		if(!timeTrialClass.TimerOnPause){
			timeTrialClass.PauseTimer();
			_GUIState = GUIStateEnum.Pause;
		}
		else{
			timeTrialClass.UnpauseTimer();
			_GUIState = GUIStateEnum.Interface;
		}
	}
	
	public void ResetComicCounter(){
		_TotalComicThumb = _RoomManager.UpdateTotalComic();
		_ComicThumbCollected = 0;
	}
	public void ResetPos(){
		avatar.transform.position = _AvatarStartingPos;
	}
	public void SwitchGUIToBlank(){
		_GUIState = GUIStateEnum.Nothing;
	}
	public void SwitchGUIToWait(){
		_GUIState = GUIStateEnum.OnStart;
	}

	
#endregion	
		
#region CoRoutine
	IEnumerator ShootRed(float delai, Vector3 pos){
		yield return new WaitForSeconds(delai);
		GameObject rCol = Instantiate(prefab.collectible, avatar.transform.position, Quaternion.identity)as GameObject;
		Vector3 newPos = new Vector3(pos.x + Random.Range(-50, 50), pos.y + Random.Range(-50, 50), 0);
		rCol.GetComponent<Collectible2>().redCollectorPos = newPos;
		rCol.GetComponent<Collectible2>().effect = true;
		rCol.GetComponent<Collectible2>().colorCollectible = Collectible2._ColorCollectible.Red;
	}
	IEnumerator OnDeath(float _wait){
		yield return new WaitForSeconds(_wait);
		
		foreach(SpriteFader _SFader in _FaderList){
			_SFader.ResetState();
		}
		
		//LoadRoom();
	}	
	IEnumerator ResetCanGrabCollectibles(float _wait){
		yield return new WaitForSeconds(_wait);
		//Debug.Log("collectiblesResetStart");
		_CollAlreadyAdded = false;
	}
	IEnumerator DelaiToAddComic(float _delai){
		yield return new WaitForSeconds(_delai);
		cN ++;
	}
	IEnumerator DelaiToBlowColl(float delai, GameObject col){
		yield return new WaitForSeconds(delai);
		col.GetComponent<Collectible2>().PayEffect();
	}
	IEnumerator RestartRoom(){
		yield return new WaitForSeconds(0.25f);
		_RoomSaver.LoadRoom();
	}
#endregion
	
}

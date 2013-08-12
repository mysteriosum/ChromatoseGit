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
	Release,
	
	Nothing,
	
}

public enum TankStates{
	None,
	Empty,
	Full,
	Flashing,
}

public enum GUIStateEnum{
	OnStart, EndResult, EndLevel, Interface, Pause, InComic
}

[SerializeAll]
public class ChromatoseManager : MonoBehaviour {
	private Avatar avatar;
	private AvatarPointer avatarP;
	private Vector3 _AvatarStartingPos;
	public static ChromatoseManager manager; 
	public ChromHUD hud = new ChromHUD();
	private GUISkin skin;
	private string _GameName = "Chromatose";
	
										//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
										//<--------------ACTION BUTTON!---------------->
										//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	private bool actionPressed;
	
	private bool _OnPause = false;
	private bool _CanShowAction = false;
	
	private Actions currentAction = Actions.Nothing;
	private GUIStateEnum _GUIState;
	
	private Texture actionTexture;
	private Texture shownActionTexture;
	
	public delegate void ActionDelegate();
	public ActionDelegate actionMethod;
	
	private SpriteFader[] _FaderList;
	
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
	
	
	private bool showingAction;
	private float actionSlideSpeed = 10f;
	
	private bool _CollAlreadyAdded = false;
	public bool CollAlreadyAdded{
		get{return _CollAlreadyAdded;}
		set{_CollAlreadyAdded = value;}
	}
	private bool _NewLevel = false;
	private bool _FirstLevelCPDone = false;
	public bool FirstLevelCPDone {
		get{return _FirstLevelCPDone;}
		set{_FirstLevelCPDone = value;}
	}
	
	private string[] roomNames = 
		{
		"Tutorial", "Module1_Scene1", "Module1_Scene2", "Module1_Scene3", "Module1_Scene4", "Module1_Scene5,",
		"Module1_Scene6", "Module1_Scene7", "Module1_Scene8", "Module1_Scene9", "ModuleBlanc_2", "ModuleBlanc_3", "ModuleBlanc_4"};
	public static RoomStats[] roomStats;
	private int curRoom;
		public int CurRoom{
			get{ return curRoom; }
		}
	private Transform curCheckpoint;
	
	private CollectiblesManager collectibles = new CollectiblesManager();
	public static CollectiblesManager statCols;
	
	public GameObject comicCompleteAnim;
	public GameObject oneShotSpritePrefab;
	public tk2dSpriteCollectionData bubbleCollection;
	public tk2dFontData chromatoseFont;
	
	private bool checkedComicStats = false;
	
	public bool playedCompleteFlourish = false;
	public bool playedSecretFlourish = false;
	
	public bool givenCols = false;
	public GameObject rewardsGuy;
	
	public bool animsReady = false;
	public bool AnimsReady{
		set { animsReady = value; }
	}
	
	public Texture backButton;
												//COMICS AND HOW TO USE THEM
	private bool inComic = false;
	public bool InComic{
		get{ return inComic; }
		
	}
	
	
												//GETTERS & SETTERS
	public List<Collectible> WhiteCollectibles{
		get{ return collectibles.w; }
	}
										//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
										//<----------------DIFFICULTY------------------>
										//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	public bool _TimeTrialModeActivated = false;
	public bool TimeTrialMode{
		get{return _TimeTrialModeActivated;}
	}
	
	public bool _NoDeathModeActivated = false;
	public bool NoDeathMode{
		get{return _NoDeathModeActivated;}
	}
	
	private float[] colX = {0, 0, 0, 0, 0};
	private float[] colY = {0, 0, 0, 0, 0};
	
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
	
	private Couleur[] colCouleurs = {Couleur.red, Couleur.green, Couleur.blue, Couleur.white, Couleur.black};		//the black is for the comics
	
	private bool[] showingCol = {false, false, false, false, false};
	private int[] colTimers = {0, 0, 0, 0, 0};

	
	private int showTiming = 1;
	private float defaultSpeed = 1.2f;
	private int refreshTiming = 75;
	
	private float[] track;
	
	private int rTimer = 0;
	private float rSpeed = 0;
	private int gTimer = 0;
	private float gSpeed = 0;
	private int bTimer = 0;
	private float bSpeed = 0;
	private int wTimer = 0;
	private float wSpeed = 0;
	
	[System.Serializable]
	//[SerializeAll]
	public class ChromHUD {
		
		public Texture mainBox;
		public Texture smallBox;
		public Texture[] energyTank;
		public Texture energyBar;
		public Texture actionButton;
		public Texture absorbAction;
		public Texture buildAction;
		public Texture destroyAction;
		public Texture payAction;
		public Texture releaseAction;
		public Texture returnAction;
		public Texture energyBarFlash;
		
		
		public Texture redCollectible;
		public Texture greenCollectible;
		public Texture blueCollectible;
		public Texture whiteCollectible;
		public Texture comicCounter;
		
		public Texture _TimeTrialBox;
		
		public Texture[] pauseButton;
		public Texture pauseWindows;
		public Texture blackBG;
		public Texture endResultWindows;
		
		
		//TEXTURE POUR WEBPLAYER
		
		public GUISkin skinSansBox;
		
		public Font chromFont;
		
		private tk2dTextMesh rColMesh;
		private tk2dTextMesh gColMesh;
		private tk2dTextMesh bColMesh;
	}
	
	
#endregion
	
#region TimeTrial Data & Methods
										//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
										//<------------TIME TRIAL CHALLENGE------------>
										//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	//[System.Serializable]
	//[SerializeAll]
	//public class TimeTrialTimes {
		
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
			
			if (_NewRecordList == null){
				_NewRecordList = new List<float>(10){ _NewRecordTimes_Tuto, _NewRecordTimes_Lev1, _NewRecordTimes_Lev2,
													_NewRecordTimes_Lev3, _NewRecordTimes_Lev4, _NewRecordTimes_Lev5,
													_NewRecordTimes_Lev6, _NewRecordTimes_Lev7, _NewRecordTimes_Lev8, _NewRecordTimes_Lev9 };
				
			}
			
			/*
			if (_TimesList[_Manager.CurRoom] >= _RecordsList[_Manager.CurRoom]){
				_TimesList[_Manager.CurRoom] = RecordsList[_Manager.CurRoom];
				}*/
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
		
		public void TTChallengeWinWindow(){
		//TODO Faire Fonction lorsque le joueur reussi le TimeTrial [Affichage = Fenetre Reussite + Resultat + Bouton Continuer + Bouton Recommencer]
		
			
		}
		
		public void TTChallengeLoseWindow(){
		//TODO Faire Fonction lorsque le joueur echoue le TimeTrial [Affichage = Fenetre Echec + Resultat + Temps a Battre pour Reussite + Bouton Recommencer]
		
			
		}
		
		public void TTChallengeCloseWindow(){
		//TODO Faire Fonction qui ferme la fenetre de resultat du TimeTrial
			
			
		}
		
		public bool StopChallenge(){
			
			bool winGame = false;
			
			//Assignation
			_DisplayScore = true;
			
			
			//Sauvegarde le Temps
			_FinalLevelTimes = _TimerTime;
			
			//Stop le Manager et le Jeu
			//_Manager.ManagerPause();
			_Manager.avatar.CannotControlFor(false, 0f);
			Time.timeScale = 0;
			
			//Verifie si le temps du Joueur bats le Temps a battre
			if (_FinalLevelTimes < _TimeToDisplay){
				winGame = true;
				//_DisplayWinWindows = true;
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
			
			
			avatar.transform.position = _AvatarStartingPos;
			ResetTimer();	
			DisplayScore = false;
			_DisplayWinWindows = false;
			Time.timeScale = 1;
			_Manager.avatar.CanControl();
			
			
		}
		
		
	//}
#endregion

#region Data Tracking
										//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
										//<--------------DATA TRACKING!---------------->
										//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>

	
	private int _CurrentThumbOnAdd = 0;
	
	public class CollectiblesManager{
		public List<Collectible> w = new List<Collectible>();
		public List<Collectible> r = new List<Collectible>();
		public List<Collectible> g = new List<Collectible>();
		public List<Collectible> b = new List<Collectible>();
		public List<Collectible> held = new List<Collectible>();
		
		public List<Collectible> wInLevel = new List<Collectible>();
		public List<Collectible> rInLevel = new List<Collectible>();
		public List<Collectible> gInLevel = new List<Collectible>();
		public List<Collectible> bInLevel = new List<Collectible>();
		public List<Collectible> heldInLevel = new List<Collectible>();
	}
	
	public class RoomStats{

		public List<Collectible> consumedCollectibles = new List<Collectible>();
		public List<Comic> comics = new List<Comic>();
		public Comic secretComic;
		
		public bool[] thumbsFound = {false, false, false, false, false, false, false};
		public int thumbNumber = 0;
		public bool secretFound = false;
		public bool comicComplete = false;
		/*
		public int redColsUsed;
		public int greenColsUsed;
		public int blueColsUsed;
		public int afterImagesUsed;
		public float timeTaken;
		
		public int redColsIn;
		public int greenColsIn;
		public int blueColsIn;
		public int whiteColsIn;
		
		public int redColsFound;
		public int greenColsFound;
		public int blueColsFound;
		public int whiteColsFound;*/
	
	}
	

#endregion	
	
#region Initialisation et Setup	(Start)
	// Use this for initialization
	void Awake () {
		
		if (statCols == null){
			statCols = collectibles;
		}
		else{
			collectibles = statCols;
		}
		if (roomStats == null){
			roomStats = new RoomStats[13]
			{	new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), 
				new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), 
				new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), 
				new RoomStats(),
			};
		}
		
		
		manager = this;
		
		UpdateRoomStats();
		
	}
	
	void Start(){
		
		
		
		_GUIState = GUIStateEnum.OnStart;
		
		CheckStartPos();
		
		CalculeCollectiblesInLevel();
		
		if(_TimeTrialModeActivated){
			SetupTTT();
			DisplayTimes2Beat();
		}
		
		//Initialise pour la premiere la security pour le FirstCP
		_FirstLevelCPDone = false;
		
		DontDestroyOnLoad(manager);
		if (!hud.mainBox){
			Debug.LogWarning("There are some missing textures....");
		}
		
		rN = collectibles.r.Count;
		gN = collectibles.g.Count;
		bN = collectibles.b.Count;
		wN = collectibles.w.Count;
		
		colX[0] = Screen.width; 		//the red one
		colY[0] = hud.mainBox.height + 90f;
		colX[1] = Screen.width;			//the blue one
		colY[1] = hud.mainBox.height + 180f;
		colX[2] = Screen.width;			//the green one
		colY[2] = hud.mainBox.height + 135f;
		colX[3] = Screen.width;			//the white one
		colY[3] = hud.mainBox.height + 45f;
		colX[4] = Screen.width; 		//the comics
		colY[4] = hud.mainBox.height;	
		
		
		
		for (int i = 0; i < showingCol.Length; i++){
			showingCol[i] = true;
		}
		
		float mainAnchor = Screen.width - hud.mainBox.width;
		float dummy = Screen.width;
		List<float> tempTrack = new List<float>();
		float initSpeed = 6f;
		float increment = 1.5f;
		
		while (dummy >= mainAnchor){		//move my dummy toward the main anchor
			dummy -= initSpeed;
			tempTrack.Add(dummy);
			if (dummy <= 0){
				break;
			}
		}
		
		do{
			initSpeed -= increment;
			dummy -= initSpeed;
			tempTrack.Add(dummy);
		}
		while (initSpeed > 0 || dummy < mainAnchor);
		
		track = tempTrack.ToArray();
		track[track.Length - 1] = mainAnchor;
						//HACK Getting a HUD_skin from the Resources folder
		skin = Resources.Load("Menus/HUD_skin") as GUISkin;
		
		
		barMinY = 28;
		barMaxY = barMinY + hud.energyBar.height;
		barX = 1192;
		barY = barMinY;
		
		
		
		aX = hud.absorbAction.width;
		
		actionTexture = hud.absorbAction;
		shownActionTexture = actionTexture;
		
		rewardsGuy = GameObject.FindWithTag("rewardsGuy");
		if (!rewardsGuy){
			Debug.LogWarning("Hey doofus! There's no rewards guy in this level! Is there even a comic!?");
		}
		else{
			
		}
	}
	
	
	void OnLevelWasLoaded(){
		
		UpdateRoomStats();	
	}
	
	void ResetStats(){
		roomStats = new RoomStats[13]{	new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), 
										new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), 
										new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats()};
		statCols = null;
		collectibles = statCols;
	}
	
	//TODO Faire Fonction qui Save/Load les RoomStats, faire en XML pour pouvoir l'utiliser avec les checkpoint et pesistant
	
	void UpdateRoomStats(){
		
		//Start Du Chu
		_FaderList = FindObjectsOfType(typeof(SpriteFader)) as SpriteFader[];
		
		avatar = GameObject.Find("Avatar").GetComponent<Avatar>();
		curRoom = -1;
		for (int i = 0; i < roomNames.Length; i ++){
			if (Application.loadedLevelName == roomNames[i]){
				curRoom = i;
				
				break;
			}
		}
		if (curRoom == -1 && Application.loadedLevelName != "Menu"){
			Debug.LogWarning("This room is not named properly, or shouldn't be in the build at all.");
		}
		GameObject[] frames = GameObject.FindGameObjectsWithTag("comicFrame");
		GameObject[] thumbs = GameObject.FindGameObjectsWithTag("comicThumb");
		int counter = 0;
		
		if (roomStats[curRoom].comics.Count == 0){
			foreach (GameObject go in frames){
				Comic strip = go.GetComponent<Comic>();
				if (strip.isSecret){
					roomStats[curRoom].secretComic = strip;
				}
				else{
					roomStats[curRoom].comics.Add(strip);
					strip.gameObject.SetActive(false);
				}
			}
		}
		else{
			foreach (GameObject go in thumbs){
				ComicThumb thumb = go.GetComponent<ComicThumb>();
				if (roomStats[curRoom].thumbsFound[counter]){
					thumb.SendMessage("Trigger");
				}
			}
		}
	}
#endregion	
	
#region Update & LateUpdate
	void Update () {
		
			
		//Debug.Log ("Le record est : " + _RecordsList[_Manager.CurRoom]);
		
		if (inComic && animsReady && !checkedComicStats){
			
			roomStats[curRoom].comicComplete = CheckComicStats();
			checkedComicStats = true;
			
			if (roomStats[curRoom].comicComplete){
				// do I have the secret too? play special anim : play normal anim;
				
				if (!roomStats[curRoom].secretFound){
					Instantiate(comicCompleteAnim);
					tk2dAnimatedSprite anim = comicCompleteAnim.GetComponent<tk2dAnimatedSprite>();
					anim.Play();
					TriggerQuestionMark();
				}
				else{
					//SUPER SECRET ANIM TIME!
				}
				
				if (!givenCols){
					givenCols = true;
					rewardsGuy.SendMessage("Trigger");
				}
			}
		}
		else if (!inComic){
			animsReady = false;
			checkedComicStats = false;
		}
		
		if (Input.GetKeyDown(KeyCode.PageUp)){
			if (Application.loadedLevel == 0) return;
			Application.LoadLevel(Application.loadedLevel - 1);
		}
		
		if (Input.GetKeyDown(KeyCode.PageDown)){
			if (Application.loadedLevel == Application.levelCount - 1) return;
			Application.LoadLevel(Application.loadedLevel + 1);
		}
		
		if (Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
		
		if (inComic){
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.P)){
			//	comicTransition.Return();
			}
		}
				
		
		//TODEL KeyTouch M pour pause a Deleter lorsque les tests seront fini
		
		if(Input.GetKeyDown(KeyCode.M)){
			ManagerPause();
		}
		
	
		
										//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
										//<-----------SHOWING COLLECTIBLES!------------>
										//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		/*
		for (int i=0; i < colCouleurs.Length; i++){
			bool identical = UpdateCollectible(colCouleurs[i]);
		}*/
		
		
		
		for (int i = 0; i < colCouleurs.Length; i++){
			if (showingCol[i]){
				colTimers[i] ++;
				if (colTimers[i] > refreshTiming){
					bool identical = UpdateCollectible(colCouleurs[i]);
					if (!identical){
						colTimers[i] = track.Length;
					}
					else{
						//Desactiver Pour Laisse le HUD de collectibles toujours afficher
						//On peut quand meme se servir de "showingCol[i] = false;" pour le faire disparaitre, ex: A la findes Niv
						//
						//showingCol[i] = false;
					}
				}
			}
			
			if (!showingCol[i] && colTimers[i] > 0){
				colTimers[i] --;
			}
			
			if (colTimers[i] >= 0 && colTimers[i] < track.Length){
				colX[i] = track[colTimers[i]];
			}
		}
		
		
										//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
										//<------------UPDATING ENERGY BAR!------------>
										//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		float barDiffY = Avatar.curEnergy * hud.energyBar.height / 100;
		barY = barMaxY - (int) barDiffY;
		if (flashyBarTimer > 0){
			flashyBarTimer --;
		}
		else{
			flashyBar = false;
		}
	
		//Appel du timeTrial
		if(_TimeTrialModeActivated && !TimerOnPause){
			TimeTrialCounter();
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
		case Actions.Release:
			actionTexture = hud.releaseAction;
			break;
			
		default:
			actionMethod = null;
			break;
		}
		
		/*
		Debug.Log("Some state info: " +
			"showingAction = " + showingAction + 
			"shownActionTexture = " + shownActionTexture.name + 
			"actionTexture = " + actionTexture);*/
		
		if (currentAction == Actions.Nothing && aX < hud.absorbAction.width){
			aX -= actionSlideSpeed;
			if (aX < -hud.absorbAction.width){
				aX = Mathf.Abs(aX);
			}
			//Debug.Log("Wiping away the Tears");
		}
		if (!showingAction){
			currentAction = Actions.Nothing;
		}
		
		if (showingAction && aX != 0 && shownActionTexture == actionTexture){
			
			aX -= actionSlideSpeed;
			if (aX > -9 && aX < 9) aX = 0;
			if (aX < -hud.absorbAction.width){
				aX = Mathf.Abs(aX);
			}
		}
		
		if (showingAction && shownActionTexture != actionTexture){
			Debug.Log("Should be working to update the action texture");
			if (aX >= hud.absorbAction.width){
				
				UpdateActionTexture();
				
			}
			else{
				aX -= actionSlideSpeed;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.P) && currentAction > 0 && actionMethod != null && !inComic){
			actionMethod();
		}
		showingAction = false;
		
	}
#endregion
		
#region OnGUI (HUD & Windows)
	void OnGUI(){
		
		float horizRatio = Screen.width / 1280.0f;
		float vertiRatio = Screen.height / 720.0f;
		
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,new Vector3(horizRatio, vertiRatio, 1f));
		
		
			//BackUp de la Matrix Initiale
			Matrix4x4 matrixBackup = GUI.matrix;
			
			Rect bgRect = new Rect(0, 0, 1280, 720);
			
			Rect mainRect = new Rect(1160, -1, hud.mainBox.width, hud.mainBox.height);
			Rect rColRect = new Rect(1160, 320, hud.redCollectible.width, hud.redCollectible.height);
			Rect gColRect = new Rect(1160, colY[1], hud.greenCollectible.width, hud.greenCollectible.height);
			Rect bColRect = new Rect(1160, 365, hud.blueCollectible.width, hud.blueCollectible.height);
			Rect wColRect = new Rect(1160, 275, hud.whiteCollectible.width, hud.whiteCollectible.height);
			Rect comicRect = new Rect(1160, 230, hud.comicCounter.width, hud.comicCounter.height);
		
			Rect actionRect = new Rect(1194, 141, 50, 52);
			Rect timeTrialRect = new Rect(25, 20, hud._TimeTrialBox.width + 100f, hud._TimeTrialBox.height + 10f);
			Rect pauseWindowsRect = new Rect (128, 100, 700, 360);
			Rect endResultRect = new Rect (0, 0, Screen.width, Screen.height);
		
			//NE SEMBLE PAS SERVIR
			Rect energyRect = new Rect(barX, barMinY, hud.energyBar.width, hud.energyBar.height);
			Rect flashyRect = new Rect(barX - 7, barMinY - 8, hud.energyBarFlash.width, hud.energyBarFlash.height);
			Rect tankRect = new Rect(tankX, tankY, 80, 128);
		
		
		#region OnGUI OnStart
		switch(_GUIState){
		case GUIStateEnum.OnStart:
			GUI.skin = hud.skinSansBox;
			
			avatar.CannotControlFor(false, 0);
			
			//Black BackGround
			GUI.DrawTexture(bgRect, hud.blackBG);
			GUI.skin.button.fontSize = 78;
			if(GUI.Button(new Rect(460, 240, 250, 150), "PLAY")){
				_GUIState = GUIStateEnum.Interface;
				avatar.CanControl();
			}
			
			
			break;
			#endregion
			
		#region OnGUI Interface
		case GUIStateEnum.Interface:

			GUI.skin = skin;
			
			GUI.DrawTexture(mainRect, hud.mainBox);
			
			if(_TimeTrialModeActivated){
				GUI.BeginGroup(timeTrialRect);												//TimeTrial Counter
					GUI.skin.textArea.normal.textColor = Color.black;
					GUI.DrawTexture(new Rect(0, 0, hud._TimeTrialBox.width + 100f, hud._TimeTrialBox.height + 10f), hud._TimeTrialBox);
					GUI.TextArea(new Rect(textOffset.x - 20f, textOffset.y, 250, 40), "Time To Beat : " + "'" + DisplayTimes2Beat() +"'");
					GUI.TextArea(new Rect(textOffset.x - 20f, textOffset.y + 30f, 250, 40), "Your Time : " + "'" + TimeString + "'");
				GUI.EndGroup();
			}
			
			GUI.BeginGroup(comicRect);										//comic counter
				GUI.skin.textArea.normal.textColor = Color.black;
				GUI.DrawTexture(new Rect(0, 0, hud.comicCounter.width, hud.comicCounter.height), hud.comicCounter);
				GUI.TextArea(new Rect(textOffset.x, textOffset.y, 80, 40), cN.ToString() + " / " + roomStats[curRoom].comics.Count.ToString());
				
			GUI.EndGroup();
			
			GUI.BeginGroup(wColRect);										//white collectible
				GUI.skin.textArea.normal.textColor = Color.white;
				GUI.DrawTexture(new Rect(0, 0, hud.whiteCollectible.width, hud.whiteCollectible.height), hud.whiteCollectible);
				GUI.TextArea(new Rect(textOffset.x, textOffset.y, 80, 40), wN.ToString());
				
			GUI.EndGroup();
			
			GUI.BeginGroup(rColRect);										//red collectible
				GUI.skin.textArea.normal.textColor = Color.red;
				GUI.DrawTexture(new Rect(0, 0, hud.redCollectible.width, hud.redCollectible.height), hud.redCollectible);
				GUI.TextArea(new Rect(textOffset.x, textOffset.y, 80, 40), rN.ToString());
				
			GUI.EndGroup();

			
			GUI.BeginGroup(bColRect);										//blue collectible
				GUI.skin.textArea.normal.textColor = Color.blue;
				GUI.DrawTexture(new Rect(0, 0, hud.blueCollectible.width, hud.blueCollectible.height), hud.blueCollectible);
				GUI.TextArea(new Rect(textOffset.x, textOffset.y, 80, 40), bN.ToString());
				
			GUI.EndGroup();
			
			
			GUI.BeginGroup(actionRect);										//Action icon
				
				GUI.DrawTexture(new Rect(aX, 0, hud.absorbAction.width, hud.absorbAction.height), actionTexture);
			
			GUI.EndGroup();
			
			if (!avatar.HasOutline){
				
				GUI.BeginGroup(energyRect);
					Rect drawRect = new Rect(0, 0, hud.energyBar.width, hud.energyBar.height);
					GUI.DrawTexture(drawRect, hud.energyBar);
				GUI.EndGroup();
			}
		
			
			
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
				if(GUI.Button(new Rect(pauseWindowsRect.width*0.25f, pauseWindowsRect.height*0.46f, pauseWindowsRect.width*0.5f, pauseWindowsRect.height*0.18f), "RESUME")){
					ManagerPause();
					_GUIState = GUIStateEnum.Interface;
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
			
			if(_TimeTrialModeActivated && DisplayScore){
				if(!DisplayWinWindows){
					//Affichage du Lose result
					GUI.BeginGroup(endResultRect);
						if (GUI.Button(new Rect(390, 360, hud.pauseButton[0].width, hud.pauseButton[0].height), hud.pauseButton[0])){
							
						}
						GUI.skin.textArea.normal.textColor = Color.red;
						GUI.DrawTexture(new Rect(0, 0, hud.endResultWindows.width, hud.endResultWindows.height), hud.endResultWindows);
						GUI.TextArea(new Rect (textOffset.x, textOffset.y, 500, 50), "YOU LOSE, SORRY !");
						GUI.TextArea(new Rect (515, 230, 500, 50), Time2BeatString);
					
								
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
						GUI.TextArea(new Rect (515, 335, 500, 50), TimeString);
						
						GUI.TextArea(new Rect (390, 530, 500, 50), "RETRY");
						if (GUI.Button(new Rect(390, 560, hud.pauseButton[0].width, hud.pauseButton[0].height), hud.pauseButton[0])){
							
							RestartLevel();
							
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
		}
	}
#endregion	
	
#region Collectibles Methods	
	public void AddCollectible(Collectible col){
		if(!_CollAlreadyAdded){
			_CollAlreadyAdded = true;
			switch (col.colColour){
			case Couleur.red:
				collectibles.r.Add(col);
				showingCol[0] = true;
				break;
			case Couleur.green:
				collectibles.g.Add(col);
				showingCol[1] = true;
				break;
			case Couleur.blue:
				collectibles.b.Add(col);
				showingCol[2] = true;
				break;
			case Couleur.white:
				collectibles.w.Add(col);
				showingCol[3] = true;
				break;
			default:
				Debug.LogWarning("Not a real collectible.");
				break;
			}
			StartCoroutine(ResetCanGrabCollectibles(0.01f));
			
		}
	}
	
	public bool UpdateCollectible(Couleur colour){
		switch (colour){
		case Couleur.red:
			if (rN > collectibles.r.Count){
				rN --;
			}
			if (rN < collectibles.r.Count){
				rN ++;
			}
			
			return rN == collectibles.r.Count;
		case Couleur.green:
			if (gN > collectibles.g.Count){
				gN --;
			}
			if (gN < collectibles.g.Count){
				gN ++;
			}
			
			return gN == collectibles.g.Count;
			
		case Couleur.blue:
			
			if (bN > collectibles.b.Count){
				bN --;
			}
			if (bN < collectibles.b.Count){
				bN ++;
			}
			
			return bN == collectibles.b.Count;
			
		case Couleur.white:
			if (wN > collectibles.w.Count){
				wN --;
			}
			if (wN < collectibles.w.Count){
				wN ++;
			}
			
			return wN == collectibles.w.Count;
		default:
			
			
			if (cN > roomStats[curRoom].thumbNumber){
				cN --;
				
			}
			if (cN < roomStats[curRoom].thumbNumber){
				cN++;
			}
			
			return cN == roomStats[curRoom].thumbNumber;
		}
	}
	/// <summary>
	/// Gets the number of a specific type of collectible.
	/// </summary>
	/// <returns>
	/// The number of cols specified
	/// </returns>
	/// <param name='colour'>
	/// Colour.
	/// </param>
	public int GetCollectibles(Couleur colour){
		switch (colour){
		case Couleur.red:
			return collectibles.r.Count;
		case Couleur.green:
			return collectibles.g.Count;
		case Couleur.blue:
			return collectibles.b.Count;
		case Couleur.white:
			return collectibles.w.Count;
		default:
			Debug.LogWarning("Not a real collectible.");
			return 0;
		}
	}
	
	public void DropCollectibles(List<Collectible> list, int no, Vector3 pos){
		
		for (int i = 0; i < no; i ++){
			Collectible inQuestion = list[list.Count - 1];
			
			inQuestion.PutBack(pos + (Vector3)Random.insideUnitCircle * 15);
			list.Remove(inQuestion);
			collectibles.held.Add(inQuestion);
		}
		
	}
	
	public void GrabHeldWhiteCols(){
		foreach(Collectible col in collectibles.held){
			if (!col.colour.White){
				Debug.LogWarning("Turns out this one's not white...");
				continue;
			}
			collectibles.w.Add(col);
		}
		collectibles.held.Clear();
	}
	
	public void RemoveCollectibles(Couleur colour, int value, Vector3 pos){
		switch (colour){
		case Couleur.red:
			JettisonCollectibles(collectibles.r, value, pos);
			showingCol[0] = true;
			return;
		case Couleur.green:
			JettisonCollectibles(collectibles.g, value, pos);
			showingCol[1] = true;
			return;
		case Couleur.blue:
			JettisonCollectibles(collectibles.b, value, pos);
			showingCol[2] = true;
			return;
		case Couleur.white:
			JettisonCollectibles(collectibles.w, value, pos);
			showingCol[3] = true;
			return;
		default:
			Debug.LogWarning("Not a real collectible.");
			return;
		}
	}
	
	public void JettisonCollectibles(List<Collectible> list, int no, Vector3 pos){
		for (int i = 0; i < no; i ++){
			
			Collectible inQuestion = list[list.Count - 1];
			roomStats[Application.loadedLevel].consumedCollectibles.Add(inQuestion);
			inQuestion.Trigger();
			//inQuestion.transform.position = pos;
			list.Remove(inQuestion);
		}
	}
#endregion	
	
#region Comics Stuff
																			//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
																			//<-------------COMICS AND STUFF!-------------->
																			//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	public void OpenComic(int index){
		avatar.movement.SetVelocity(Vector2.zero);
		
		Time.timeScale = 0;
		inComic = true;
		int counter = 0;
		foreach (Comic c in roomStats[curRoom].comics){
			
			if (roomStats[curRoom].thumbsFound[counter]){
				c.gameObject.SetActive(true);
			}
			
			counter ++;
		}
		if (roomStats[curRoom].secretFound){
			roomStats[curRoom].secretComic.gameObject.SetActive(true);
		}
	}
	
	public void CloseComic(){
		Time.timeScale = 1;
		inComic = false;
		
	}
	
	
	public void FindComic(int index){
		//roomStats[curRoom].comics[index].SendMessage("FadeAlpha", 1f);
		
		if (index == roomStats[curRoom].comics.Count){
			
			roomStats[curRoom].secretFound = true;
			
			bool foundEmpty = false;
			bool foundNone = false;
			
			for (int i = 0; i < 3; i ++){
				for (int j = 0; j < 5; j ++){
					if (Avatar.tankStates[i, j] == TankStates.Empty && !foundEmpty){
						Avatar.tankStates[i, j] = TankStates.Full;
						foundEmpty = true;
					}
					if (Avatar.tankStates[i, j] == TankStates.None){
						foundNone = true;
						Avatar.tankStates[i, j] = foundEmpty? TankStates.Empty : TankStates.Full;
						break;
					}
				}
				if (foundEmpty && foundNone) break;
			}
			
			if (!foundEmpty && !foundNone){
				Avatar.curEnergy = 100;
				Debug.LogWarning("You fucked up because there isn't enough room for all these tanks. WTH! I Warned you!");
			}
		}
		else{
			roomStats[curRoom].thumbsFound[index] = true;
			roomStats[curRoom].thumbNumber ++;
			showingCol[4] = true;
		}
		
	}
	
	public bool CheckComicStats(){
		
		int i = 0;
		foreach (Comic comic in roomStats[curRoom].comics){
			if (roomStats[curRoom].thumbsFound[i] && !comic.InMySlot){
				comic.InMySlot = true;
				//TODO Ajouter Anim ou Feedback pour indiquer que l'on a tous les comics
			}
				
			i++;
		}
		
		if (roomStats[curRoom].secretFound){
			roomStats[curRoom].secretComic.InMySlot = true;
		}
		
		foreach (Comic c in roomStats[curRoom].comics){
			if (!c.InMySlot){
				return false;
			}
		}
		roomStats[curRoom].comicComplete = true;
								//turns out the comic is successful!
		//CHU Note: si necessaire activer le ramassage de collectible
		//shavatarComicBlock.SetActive(false);
		Debug.Log("J'ai un comic ou tous les comics ?");
		return true;
		
	}
#endregion	
	
#region Fonctions Diverses
	Avatar.DeathAnim danim;		//avatar's death animation
	public void Death(){
		//Archaic
		/*
		Avatar.tankStates[0, 0] = TankStates.Full;
		Avatar.tankStates[0, 1] = TankStates.Full;
		Avatar.curEnergy = 50;
		Application.LoadLevel(Application.loadedLevel);*/
		
		if(!_NoDeathModeActivated){
			danim = new Avatar.DeathAnim();
			danim.PlayDeath(Reset);
			avatar.SendMessage("FadeAlpha", 0f);
			avatar.movement.SetVelocity(Vector2.zero);
			StartCoroutine(OnDeath(0.15f));
			avatar.CancelOutline();
			avatar.Gone = true;
		}
		else{
			danim = new Avatar.DeathAnim();
			danim.PlayDeath(Reset);
			avatar.SendMessage("FadeAlpha", 0f);
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
		avatar.SetColour(0, 0, 0);
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
#endregion	
	
#region Static Functions	
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<-------------STATIC FUNCTIONS!-------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	
	tk2dSprite spriteInfo;
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
		
		foreach (GameObject rCol in GameObject.FindGameObjectsWithTag("redCollectible")){
			Collectible rColItem = rCol.GetComponent<Collectible>();
			collectibles.rInLevel.Add(rColItem);
		}
		foreach (GameObject wCol in GameObject.FindGameObjectsWithTag("whiteCollectible")){
			Collectible wColItem = wCol.GetComponent<Collectible>();
			collectibles.wInLevel.Add(wColItem);
		}
		foreach (GameObject bCol in GameObject.FindGameObjectsWithTag("blueCollectible")){
			Collectible bColItem = bCol.GetComponent<Collectible>();
			collectibles.bInLevel.Add(bColItem);
		}
		
		//Verifier si ce compteur n'existe pas quelque part deja
		/*
		foreach (GameObject rCol in GameObject.FindGameObjectsWithTag("redCollectible")){
			Collectible rColItem = rCol.GetComponent<Collectible>();
			collectibles.rInLevel.Add(rColItem);
		}*/
		
		Debug.Log("On a : " + collectibles.rInLevel.Count + " rouges, " + collectibles.wInLevel.Count + " blancs et ");
		
	}
	
	void ManagerPause(){
		avatar.Pause();
	
		if(Time.timeScale==1){
			Time.timeScale = 0;
		}
		else{
			Time.timeScale = 1; 
		}
		
		if(!TimerOnPause){
			PauseTimer();
			_GUIState = GUIStateEnum.Pause;
		}
		else{
			UnpauseTimer();
			_GUIState = GUIStateEnum.Interface;
		}
	}
	
#endregion	
		
#region CoRoutine
	IEnumerator OnDeath(float _wait){
		yield return new WaitForSeconds(_wait);
		
		foreach(SpriteFader _SFader in _FaderList){
			_SFader.ResetState();
		}
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
#endregion
	
}

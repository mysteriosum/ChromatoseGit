using UnityEngine;
using System.Collections;
using System.IO;

public enum Actions{
	Back, Absorb, Destroy, Build, Pay, WhitePay, Release, Nothing	
}
public enum GUIStateEnum{
	MainMenu, OnStart, EndResult, EndLevel, Interface, Pause, InComic, Nothing
}
public enum _MenuWindowsEnum{
	MainMenu, FakeSplashScreen, LevelSelectionWindows, CreditWindows, OptionWindows, LoadingScreen, KeyboardSelectionScreen, Stats, TimeTrialScreen, GameModeScreen,None
}
/*public enum _OptionWindowsEnum{
	MainOption, Sound, GameMode, DeleteSavegame, None
}*/

[System.Serializable]
public class HUDManager : MainManager {
	
	public static HUDManager hudManager;
	
	public static GUIStateEnum _GUIState;
	private _MenuWindowsEnum _MenuWindows;
	//private _OptionWindowsEnum _OptionWindows;
	
	public Font chromFont;

	public Texture mainBox, smallBox, timeTrialBox;
	public Texture actionButton, 
					absorbAction, 
					buildAction, 
					destroyAction, 
					payAction, 
					whitePayAction, 
					releaseAction, 
					returnAction;
	
	public Texture redCollectible,
					blueCollectible, 
					whiteCollectible, 
					comicCounter, 
					comicCounter2;
	
	public static Rect _MainMenuBGRect;
	
	public Texture pauseWindows, mainMenuBG, blackBG, whiteBG, optionBG, loadingBG, greyFilterBG, endResultWindows, emptyProgressBar, progressLine, credits, splashScreenBG, ttBg;
	public Texture _AvatarLoadingLoop1, _AvatarLoadingLoop2, _BulleLoadingLoop1, _BulleLoadingLoop2, _LevelReady;
	public Texture whiteColTex, redColTex, blueColText, comicTex, deathCountTex;
	
	public GUISkin _PlayButtonSkin, _GreenlightSkin, skinSansBox, pauseBackButton, mainMenuButtonSkin, _SkinMenuSansBox, _VoidSkin, _TimeTrialButtonSkin;
	public GUISkin _StartButtonSkin, _CreditButtonSkin, _FbookButtonSkin, _TwitterButtonSkin, _BackButtonSkin, _GreenlightButton;
	public GUISkin _QwertySkin1, _QwertySkin2, _AzertySkin1, _AzertySkin2, _EraseSkin, _DoItSkin, _OkButtonSkin, _OpenMenuSkin;
	
	public Texture qwertyKeyboard, azertyKeyboard, leftArrow, rightArrow;	
	public Texture actionTexture, shownActionTexture;
	
	
	public MovieTexture movieLoad, movieLoad2, movieLogo, movieTitle, movieStart, movieCredit, moviePressStart, movieExit, movieSecondLoad;
	
	
	
	
	
	private Actions currentAction = Actions.Nothing;
	
	private bool _CanShowAction = false, 
					actionPressed = false, 
					_GlowComicNb = false, 
					_CanFlash = false, 
					_OnFlash = false, 
					_AfterComic = false, 
					showingAction = false,
					_FirstStart = false,
					_CanShowPressStartAnim = false,
					_CanShowLogo = false,
					_OnErase = false,
					_OnLogo = false;

	private float flashTimer = 0.0f, aX, actionSlideSpeed = 2.0f;
	private Vector2 textOffset = new Vector2 (55f, 8);
	
	public bool canShowPressStartAnim { get { return _CanShowPressStartAnim; } set { _CanShowPressStartAnim = value; } }
		//ACTIONBOX FUNCTIONS
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
	
	public  delegate void ActionDelegate();
	public ActionDelegate actionMethod;
	
	private int _TotalComicThumb = 0;
	
		//VARIABLE ELEMENTS DYNAMICS
	public static bool mainBoxCanDown = false;
	public static bool mainBoxCanUp = false;
	
	private float mainBoxStartY = -230f;
	private float mainBoxPosY = -230f;
	private float mainBoxMaxY = -120f;
	
	private float mainBoxMovingRate = 2.8f;
	
	private Rect[] hudRect = new Rect[4];
	private GameObject[] levelSelectionBut;
	private GameObject backButtton;
	private GameObject[] keyboardButton;
	private GameObject optionBGgo;
	
	private int counterBox = 0;
	
	public static bool[] hudBoxCanAppear = new bool[4];
	
	public static bool[] hudBoxCanDisappear = new bool[4];
	
	private bool _FromGame = false;
	
	private float hudBoxStartX = 1280;
	private float hudBoxMinX = 1150;
	
	private float hudBoxMovingRate = 8f;
	private int _HudFontSize = 19;
	
	
		//VARIABLE LOADING
	private Texture curLoadIcon;

	
	//GETSET ACCESSOR
	public bool afterComic { get { return _AfterComic; } set { _AfterComic = value; } }
	public bool canFlash { get { return _CanFlash; } set { _CanFlash = value; } }
	public bool onFlash { get { return _OnFlash; } set { _OnFlash = value; } }
	public bool firstStart { get { return _FirstStart; } set { _FirstStart = value; } }
	public bool onErase { get { return _OnErase; } set { _OnErase = value; } }
	
	
	public GUIStateEnum guiState { get { return _GUIState; } set { _GUIState = value; } }
	public _MenuWindowsEnum menuWindows { get { return _MenuWindows; } set { _MenuWindows = value; } }
	//public _OptionWindowsEnum optionWindows { get { return _OptionWindows; } set { _OptionWindows = value; } }
	
	
	
	
	//START, SETUP AND INIT
	void Awake(){
		//EMPECHE LE HUDMANAGER DE SE DETRUIRE ENTRE LES NIVEAUX
		//DontDestroyOnLoad(this);
		hudManager = this;
		
		aX = absorbAction.width;// * 1.5f;		
		actionTexture = absorbAction;
		shownActionTexture = actionTexture;
		
		hudRect[0] = new Rect(1286, 115, comicCounter.width + 10, comicCounter.height);
		hudRect[1] = new Rect(1286, 165, whiteCollectible.width+ 10, whiteCollectible.height);
		hudRect[2] = new Rect(1286, 215, redCollectible.width + 10, redCollectible.height);
		hudRect[3] = new Rect(1286, 265, blueCollectible.width + 10, blueCollectible.height);
		
		hudBoxCanAppear[0] = false;
		hudBoxCanAppear[1] = false;
		hudBoxCanAppear[2] = false;
		hudBoxCanAppear[3] = false;
		
		hudBoxCanDisappear[0] = false;
		hudBoxCanDisappear[1] = false;
		hudBoxCanDisappear[2] = false;
		hudBoxCanDisappear[3] = false;
		
		if(Application.loadedLevel == 0){
			levelSelectionBut = GameObject.FindGameObjectsWithTag("levelButton");
			
			foreach(GameObject gos in levelSelectionBut){
				gos.gameObject.SetActive(false);
			}
			
			backButtton = GameObject.FindGameObjectWithTag("backButton");
			backButtton.SetActive(false);
			
			keyboardButton = GameObject.FindGameObjectsWithTag("keyboardButton");
			foreach(GameObject gos2 in keyboardButton){
				gos2.SetActive(false);
			}
			
			optionBGgo = GameObject.FindGameObjectWithTag("optionBG")as GameObject;
			optionBGgo.SetActive(false);
		}
		
			//SETUP LE TOUT, ON LE FAIT A L'EXTERNE CAR J'AIME BIEN POUVOIR LE RAPPELLER, EN CAS DE PROBLEME
		StartCoroutine(Setup(0.1f));
	}
	
	void OnLevelWasLoaded(){
		actionMethod = null;
		movieSecondLoad.Play();
		SetupHud();
		StartCoroutine(Setup(0f));
		if(Application.loadedLevel == 0){
			levelSelectionBut = GameObject.FindGameObjectsWithTag("levelButton");
			
			foreach(GameObject gos in levelSelectionBut){
				gos.gameObject.SetActive(false);
			}
			
			backButtton = GameObject.FindGameObjectWithTag("backButton");
			backButtton.SetActive(false);
			
			keyboardButton = GameObject.FindGameObjectsWithTag("keyboardButton");
			foreach(GameObject gos2 in keyboardButton){
				gos2.SetActive(false);
			}
			
			optionBGgo = GameObject.FindGameObjectWithTag("optionBG")as GameObject;
			optionBGgo.SetActive(false);
		}
	}
	
	void Start () {
		_FirstStart = true;
		_OnLogo = false;
		_MenuWindows = _MenuWindowsEnum.FakeSplashScreen;
	}
	
	void Update(){
		
	}
	
		//FIXED & LATE UPDATE
	void FixedUpdate () {
		
		if(mainBoxCanDown){
			OpenMainBox();
		}
		if(mainBoxCanUp){
			CloseMainBox();
		}
		
		
		if(hudBoxCanAppear[0]){
			OpenHudBox0();
		}
		if(hudBoxCanAppear[1]){
			OpenHudBox1();
		}
		if(hudBoxCanAppear[2]){
			OpenHudBox2();
		}
		if(hudBoxCanAppear[3]){
			OpenHudBox3();
		}
		
		
		if(hudBoxCanDisappear[0]){
			CloseHudBox0();
		}
		if(hudBoxCanDisappear[1]){
			CloseHudBox1();
		}
		if(hudBoxCanDisappear[2]){
			CloseHudBox2();
		}
		if(hudBoxCanDisappear[3]){
			CloseHudBox3();
		}
	}
	void LateUpdate () {
		
	switch (currentAction){
		case Actions.Back:
			actionTexture = returnAction;
			break;
		case Actions.Absorb:
			actionTexture = absorbAction;
			break;
		case Actions.Destroy:
			actionTexture = destroyAction;
			break;
		case Actions.Build:
			actionTexture = buildAction;
			break;
		case Actions.Pay:
			actionTexture = payAction;
			break;
		case Actions.WhitePay:
			actionTexture = whitePayAction;
			break;
		case Actions.Release:
			actionTexture = releaseAction;
			break;
			
		default:
			actionMethod = null;
			break;
		}
				
		if (currentAction == Actions.Nothing && aX < absorbAction.width){
			aX -= actionSlideSpeed;
			if (aX < -absorbAction.width){
				aX = Mathf.Abs(aX);
			}
		}
		if (!showingAction){
			currentAction = Actions.Nothing;
		}
		
		if (showingAction && aX != 0 && shownActionTexture == actionTexture){
			aX -= actionSlideSpeed;
			if (aX > -9 && aX < 9) aX = 0;
			if (aX < -absorbAction.width){
				aX = Mathf.Abs(aX);
			}
		}
		
		if (showingAction && shownActionTexture != actionTexture){
			if (aX >= absorbAction.width){
				UpdateActionTexture();
			}
			else{
				aX -= actionSlideSpeed;
			}
		}
			//Effectue l'action si une action peut etre effectuer et si la touche P est presser
		if (Input.GetKeyDown(KeyCode.P) && currentAction > 0 && actionMethod != null && guiState != GUIStateEnum.OnStart){
			actionMethod();
		}
		
		//showingAction = false;
	}
	
	public void OffAction(){
		showingAction = false;
	}
		
	public void SetupActionBox(){
		aX = absorbAction.width;
		actionTexture = absorbAction;
		shownActionTexture = actionTexture;
	}
	
	public void SetupHud(){
		if(Application.loadedLevel!=0){
				//SET LE HUD POUR LE DEBUT D'UN NIVEAU
			_GUIState = GUIStateEnum.OnStart;
			_MenuWindows = _MenuWindowsEnum.None;
		}
		else{
				//SET LE HUD POUR LE MAIN MENU
			if(_FirstStart){
				_GUIState = GUIStateEnum.MainMenu;
				_MenuWindows = _MenuWindowsEnum.FakeSplashScreen;
				ResetComicCounter();
			}
			else if(_FromGame){
				_GUIState = GUIStateEnum.MainMenu;
				_MenuWindows = _MenuWindowsEnum.LevelSelectionWindows;
				StartCoroutine(ResetFromGame());
				Debug.Log("Retour au mainMenu Screen");
			}
			else{
				_GUIState = GUIStateEnum.MainMenu;
				_CanShowPressStartAnim = false;
				_MenuWindows = _MenuWindowsEnum.MainMenu;
			}
		}
	}
	
	
		//Open HUD MainBox
	void OpenMainBox(){
		if(mainBoxPosY < mainBoxMaxY){
			mainBoxPosY+=mainBoxMovingRate;
		}
		else{
			SetupActionBox();
			_CanHitEscape = true;
			mainBoxCanDown = false;
		}
	}
		
		//Close HUD MainBox
	void CloseMainBox(){
		if(mainBoxPosY > mainBoxStartY){
			mainBoxPosY-=mainBoxMovingRate;
		}
		else{
			mainBoxCanUp = false;
			//_CanHitEscape = true;
			Pause();
		}
	}	
	
		//Open CollBoxx
	void OpenHudBox0(){
		
		if(hudRect[0].x > hudBoxMinX){
			hudRect[0].x-=hudBoxMovingRate;
		}
		else{
			hudBoxCanAppear[0] = false;
			if(hudRect[0].x < hudBoxMinX){
				hudRect[0].x = hudBoxMinX;
			}
		}
	}
	void OpenHudBox1(){
		
		if(hudRect[1].x > hudBoxMinX){
			hudRect[1].x-=hudBoxMovingRate;
		}
		else{
			hudBoxCanAppear[1] = false;
			if(hudRect[1].x < hudBoxMinX){
				hudRect[1].x = hudBoxMinX;
			}
		}
	}
	void OpenHudBox2(){
		
		if(hudRect[2].x > hudBoxMinX){
			hudRect[2].x-=hudBoxMovingRate;
		}
		else{
			hudBoxCanAppear[2] = false;
			if(hudRect[2].x < hudBoxMinX){
				hudRect[2].x = hudBoxMinX;
			}
		}
	}
	void OpenHudBox3(){
		
		if(hudRect[3].x > hudBoxMinX){
			hudRect[3].x-=hudBoxMovingRate;
		}
		else{
			hudBoxCanAppear[3] = false;
			if(hudRect[3].x < hudBoxMinX){
				hudRect[3].x = hudBoxMinX;
			}
		}
	}
	
		//Close CollBoxx
	void CloseHudBox0(){
		
		if(hudRect[0].x < hudBoxStartX){
			hudRect[0].x+=hudBoxMovingRate;
		}
		else{
			hudBoxCanDisappear[0] = false;
		}		
	}
	void CloseHudBox1(){
		
		if(hudRect[1].x < hudBoxStartX){
			hudRect[1].x+=hudBoxMovingRate;
		}
		else{
			hudBoxCanDisappear[1] = false;
		}		
	}
	void CloseHudBox2(){
		
		if(hudRect[2].x < hudBoxStartX){
			hudRect[2].x+=hudBoxMovingRate;
		}
		else{
			hudBoxCanDisappear[2] = false;
		}		
	}
	void CloseHudBox3(){
		
		if(hudRect[3].x < hudBoxStartX){
			hudRect[3].x+=hudBoxMovingRate;
		}
		else{
			hudBoxCanDisappear[3] = false;
		}		
	}
	
		//RESET FUNCTION
	public void ResetComicCounter(){
		_TotalComicThumb = _RoomManager.UpdateTotalComic();
		StatsManager.comicThumbCollected[Application.loadedLevel] = 0;
		StatsManager.manager.ReCalculateStats();
	}
	
		//Start Hud Open Sequence
	public void StartHudOpenSequence(){
		_CanHitEscape = false;
		StartCoroutine(ActiveMainBox(0.25f, true));
		hudBoxCanAppear[3] = true;
		StartCoroutine(ActiveHudBox(0.2f, 2, true));
		StartCoroutine(ActiveHudBox(0.4f, 1, true));
		StartCoroutine(ActiveHudBox(0.6f, 0, true));
	}
	
		//Start Hud Close Sequence
	public void StartHudCloseSequence(){
		_CanHitEscape = false;
		StartCoroutine(ActiveMainBox(0.25f, false));
		hudBoxCanDisappear[0] = true;
		StartCoroutine(ActiveHudBox(0.2f, 1, false));
		StartCoroutine(ActiveHudBox(0.4f, 2, false));
		StartCoroutine(ActiveHudBox(0.6f, 3, false));
	}
	
	public void ResetTitleBool(){
		movieTitle.Stop();
		movieStart.Stop();
		movieCredit.Stop();
		movieExit.Stop();
		moviePressStart.Stop();
		_CanShowPressStartAnim = false;
	}
	
	public void ActiveButton(){
		foreach(GameObject gos in levelSelectionBut){
			gos.SetActive(true);
		}
	}
	public void DesactiveButton(){
		foreach(GameObject gos in levelSelectionBut){
			gos.SetActive(false);
		}
	}
	public void ActiveBackButton(){
		backButtton.SetActive(true);
	}
	public void DesactiveBackButton(){
		backButtton.SetActive(false);
	}
	public void ActiveKeyboardButton(){
		foreach(GameObject gos in keyboardButton){
			gos.SetActive(true);
		}
	}
	public void DesactiveKeyboardButton(){
		foreach(GameObject gos in keyboardButton){
			gos.SetActive(false);
		}
	}
	public void ActiveOptBG(){
		optionBGgo.SetActive(true);
	}
	public void DesactiveOptBG(){
		optionBGgo.SetActive(false);
	}
	
	public void ResetAllMovie(){
		movieTitle.Stop();
		movieStart.Stop();
		movieCredit.Stop();
		movieExit.Stop();
	}
	
	//////////////////////////////////////
	//		    SECTION OnGUI		    //
	//////////////////////////////////////
	void OnGUI(){
					
			//SETUP DE LA MATRICE INITIALE
		float horizRatio = Screen.width / 1280.0f;
		float vertiRatio = Screen.height / 720.0f;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,new Vector3(horizRatio, vertiRatio, 1f));
		
				//BACKUP DE LA MATRICE INITIALE
			Matrix4x4 matrixBackup = GUI.matrix;
			
		switch(_GUIState){
			
				//DRAW L'INTERFACE EN JEU
		case GUIStateEnum.Interface:
			DrawInterfaceHUD();
			return;
			break;
			
			
		case GUIStateEnum.MainMenu:
			
			switch (_MenuWindows){
				
				//DRAW LE FALE SPLASH SCREEN
			case _MenuWindowsEnum.FakeSplashScreen:
				DrawFakeSplashScreen();
				break;
				
				//DRAW LE TITLE SCREEN
			case _MenuWindowsEnum.MainMenu:
				DrawTitleScreen();
				break;
				
				//DRAW LE LEVELSELECTION SCREEN
			case _MenuWindowsEnum.LevelSelectionWindows:
				ActiveButton();
				ActiveBackButton();
				//DrawLevelSelectionScreen();
				break;
				
				//DRAW LES OPTIONS DU MENUS
			case _MenuWindowsEnum.OptionWindows:
				DrawMainMenuOption();				
				break;
	
				//DRAW LE LOADING SCREEN
			case _MenuWindowsEnum.LoadingScreen:
				DrawLoadingScreen();		
				break;

				//DRAW LA FENETRE DES CREDITS
			case _MenuWindowsEnum.CreditWindows:
				//DrawCreditsScreen();
				break;
	
				//DRAW LA FENETRE DE SELECTION DE KEYBOARD
			case _MenuWindowsEnum.KeyboardSelectionScreen:
				DrawKeyboardSelectionScreen();
				break;
				
				//DRAW LA FENETRE DE STATS
			case _MenuWindowsEnum.Stats:
				DrawStatsScreen();
				break;
				
				//DRAW LA FENETRE GAMEMODE
			case _MenuWindowsEnum.GameModeScreen:
				DrawGameModeScreen();
				break;
				
				//DRAW LA FENETRE TIMETRIAL
			case _MenuWindowsEnum.TimeTrialScreen:
				DrawTimeTrialScreen();
				break;
			}
			return;
			break;
			
			//DRAW LA FENETRE START DES NIVEAUX
		case GUIStateEnum.OnStart:
			DrawInGameStart();
			break;
			
			//DRAW LE MENU PAUSE INGAME
		case GUIStateEnum.Pause:
			DrawInterfaceHUD();
			DrawInGamePause();
			break;
			
			//DRAW LE END LEVEL WINDOWS [POURRA SERVIR A UNE OUTRO DE NIVEAU AUSSI]
		case GUIStateEnum.EndLevel:
			
			break;
			
			//DRAW LA FENETRE DE RESULTAT DU TIME TRIAL
		case GUIStateEnum.EndResult:
			/*
			if(_TimeTrialModeActivated && timeTrialClass.DisplayScore){
				if(!timeTrialClass.DisplayWinWindows){
					//Affichage du Lose result
					GUI.BeginGroup(endResultRect);
						if (GUI.Button(new Rect(390, 360, pauseButton[0].width, pauseButton[0].height), pauseButton[0])){
							
						}
						GUI.skin.textArea.normal.textColor = Color.red;
						GUI.DrawTexture(new Rect(0, 0, endResultWindows.width, endResultWindows.height), endResultWindows);
						GUI.TextArea(new Rect (textOffset.x, textOffset.y, 500, 50), "YOU LOSE, SORRY !");
						GUI.TextArea(new Rect (515, 230, 500, 50), timeTrialClass.Time2BeatString);
					
						GUI.Button(new Rect(890, 360, 300, 100), "NEXT LEVEL");
					GUI.EndGroup();
				}
				else{
					//Affichage du win result
					GUI.BeginGroup(endResultRect);
						GUI.skin.textArea.normal.textColor = Color.red;
						GUI.DrawTexture(new Rect(0, 0, endResultWindows.width, endResultWindows.height), endResultWindows);
						GUI.TextArea(new Rect (540, 235, 500, 50), "YOU WIN !");
						GUI.TextArea(new Rect (515, 285, 500, 50), "NEW TIME 2 BEAT");
						GUI.TextArea(new Rect (515, 335, 500, 50), timeTrialClass.TimeString);
						
						GUI.TextArea(new Rect (390, 530, 500, 50), "RETRY");
						if (GUI.Button(new Rect(390, 560, pauseButton[0].width, pauseButton[0].height), pauseButton[0])){
							timeTrialClass.RestartLevel();							
						}
					GUI.EndGroup();					
				}
			}
		
			*/
			break;
			
			//FAITE POUR DRAW UN HUD SPECIAL DANS LES COMICS
		case GUIStateEnum.InComic:
			
			break;
			
			//DRAW NOTHING -- NE DEVRAIT PAS ARRIVER SOUVENT SA
		case GUIStateEnum.Nothing:
			
			break;
		}
	}	
	
		//FUNCTION QUI DRAW LES DIFFERENTES FENETRES D'OPTION
	void DrawMainMenuOption(){
			//BACKGROUND
		//GUI.DrawTexture(new Rect(0, 0, 1280, 720), optionBG);
		
			
			//VOLUME MUSIQUE SLIDER
		GUI.skin = _SkinMenuSansBox;
		StatsManager.musicVolume = GUI.HorizontalSlider(new Rect(450, 150, 350, 70), StatsManager.musicVolume, 0f, 1f);
		
			//MUSIC OK BUTTON
		GUI.skin = _OkButtonSkin;
		if(GUI.Button(new Rect(815, 130, 78, 78), "")){
			MusicManager.soundManager.ReplayCurTrack();
			Debug.Log("music = " + StatsManager.musicVolume);
		}
		
			//VOLUME SFX SLIDER
		GUI.skin = _SkinMenuSansBox;
		StatsManager.sfxVolume = GUI.HorizontalSlider(new Rect(450, 250, 350, 70), StatsManager.sfxVolume, 0f, 1f);
		
			//SFX OK BUTTON
		GUI.skin = _OkButtonSkin;
		if(GUI.Button(new Rect(815, 230, 78, 78), "")){
			MusicManager.soundManager.PlaySFX(19);
		}
		
			//Keyboard Selection Button
		switch(keyboardType){
				//QWERTY
		case _KeyboardTypeEnum.QWERTY:
			GUI.skin = _QwertySkin1;
			if(GUI.Button(new Rect(470, 351, 171, 111), "")){}
			GUI.skin = _AzertySkin2;
			if(GUI.Button(new Rect(645, 340, 195, 125), "")){
				keyboardType = MainManager._KeyboardTypeEnum.AZERTY;
			}			
			break;
				
				//AZERTY
		case _KeyboardTypeEnum.AZERTY:
			GUI.skin = _QwertySkin2;
			if(GUI.Button(new Rect(470, 351, 171, 111), "")){
				keyboardType = MainManager._KeyboardTypeEnum.QWERTY;
			}
			GUI.skin = _AzertySkin1;
			if(GUI.Button(new Rect(645, 340, 195, 125), "")){}	
			break;
		}
		
			//DATA ERASE BUTTON
		if(!_OnErase){
			GUI.skin = _EraseSkin;
			if(GUI.Button(new Rect(540, 500, 197, 142), "")){
				_OnErase = true;
			}
		}
		else{
			GUI.skin = _DoItSkin;
			if(GUI.Button(new Rect(540, 525, 193, 120), "")){
				Debug.Log("Save Game Deleted");
				File.Delete(Application.persistentDataPath + "/" + "Chromasave");
				HUDManager.hudManager.menuWindows = _MenuWindowsEnum.LevelSelectionWindows;	
				Application.Quit();
			}
		}
	}	
	
		//FUNCTION QUI DRAW L'INTERFACE EN JEU
	void DrawInterfaceHUD(){
		
			//INITIALISATION DES RECTANGLEBOX
		Rect bgRect = new Rect(0, 0, 1280, 720);
		Rect mainRect = new Rect(1150, mainBoxPosY, mainBox.width, mainBox.height);  //Default Value PosY = -120
		
		Rect actionRect = new Rect(1183, 22, 60, 52);
		//Rect timeTrialRect = new Rect(25, 20, _TimeTrialBox.width + 100f, _TimeTrialBox.height + 10f);
		Rect endResultRect = new Rect (0, 0, Screen.width, Screen.height);
		
		GUI.skin = _PlayButtonSkin;
		GUI.DrawTexture(mainRect, mainBox);
		/*
		if(_TimeTrialModeActivated){
			GUI.BeginGroup(timeTrialRect);												//TimeTrial Counter
				GUI.skin.textArea.normal.textColor = Color.black;
				GUI.DrawTexture(new Rect(0, 0, _TimeTrialBox.width + 100f, _TimeTrialBox.height + 10f), _TimeTrialBox);
				GUI.TextArea(new Rect(textOffset.x - 20f, textOffset.y, 250, 40), "Time To Beat : " + "'" + timeTrialClass.DisplayTimes2Beat() +"'");
				GUI.TextArea(new Rect(textOffset.x - 20f, textOffset.y + 30f, 250, 40), "Your Time : " + "'" + timeTrialClass.TimeString + "'");
			GUI.EndGroup();
		}*/
		
		GUI.BeginGroup(hudRect[0]);		
													//comic counter	
			_TotalComicThumb = _RoomManager.UpdateTotalComic();
			GUI.skin.textArea.normal.textColor = Color.black;
			if(StatsManager.comicThumbDisplayed >=  _TotalComicThumb && _TotalComicThumb != 0 && !_AfterComic){
				_CanFlash = true;
			}
			if(_CanFlash){
				flashTimer += Time.deltaTime;
				if(flashTimer >= 1){
					flashTimer = 0;
					_OnFlash = !_OnFlash;
				}
			}
			if(!_OnFlash || _AfterComic){
				GUI.DrawTexture(new Rect(0, 0, comicCounter.width, comicCounter.height), comicCounter);
			}
			else{
				GUI.DrawTexture(new Rect(0, 0, comicCounter.width, comicCounter.height), comicCounter2);
			}
			GUI.skin.textArea.fontSize = _HudFontSize;
			GUI.skin.textArea.normal.textColor = Color.black;
			GUI.TextArea(new Rect(textOffset.x, textOffset.y, 90, 45), StatsManager.comicThumbCollected[currentLevel].ToString() + "/" + _TotalComicThumb);
			
			
		GUI.EndGroup();
		
		GUI.BeginGroup(hudRect[1]);										//white collectible
			GUI.skin.textArea.fontSize = _HudFontSize;
			GUI.skin.textArea.normal.textColor = Color.white;
			GUI.DrawTexture(new Rect(0, 0, whiteCollectible.width, whiteCollectible.height), whiteCollectible);
			GUI.TextArea(new Rect(textOffset.x + 10, textOffset.y, 90, 45), StatsManager.whiteCollDisplayed.ToString());// + " / " + _TotalWhiteColl.ToString());
		GUI.EndGroup();
		
		GUI.BeginGroup(hudRect[2]);										//red collectible
			GUI.skin.textArea.fontSize = _HudFontSize;
			GUI.skin.textArea.normal.textColor = Color.red;
			GUI.DrawTexture(new Rect(0, 0, redCollectible.width, redCollectible.height), redCollectible);
			GUI.TextArea(new Rect(textOffset.x + 10, textOffset.y, 90, 45), StatsManager.redCollDisplayed.ToString());// + " / " + _TotalRedColl.ToString());
			
		GUI.EndGroup();

		
		GUI.BeginGroup(hudRect[3]);										//blue collectible
			GUI.skin.textArea.fontSize = _HudFontSize;
			GUI.skin.textArea.normal.textColor = Color.blue;
			GUI.DrawTexture(new Rect(0, 0, blueCollectible.width, blueCollectible.height), blueCollectible);
			GUI.TextArea(new Rect(textOffset.x + 10, textOffset.y, 90, 45), StatsManager.blueCollDisplayed.ToString());// + " / " + _TotalBlueColl.ToString());
			
		GUI.EndGroup();
		
		
		GUI.BeginGroup(actionRect);										//Action icon
			
			GUI.DrawTexture(new Rect(aX, 0, absorbAction.width, absorbAction.height), actionTexture);
		
		GUI.EndGroup();	
		
		GUI.skin = _OpenMenuSkin;
		if(GUI.Button(new Rect(1160, 325, 70, 45), "")){
			StartHudCloseSequence();
		}
		
	}
	
		//DRAW LE MENU PAUSE
	void DrawInGamePause(){
		
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), greyFilterBG);
		GUI.skin = skinSansBox;
		
		//GUIUtility.RotateAroundPivot(-12f, new Vector2(320, 215));
		
			//PAUSE WINDOWS
		GUI.DrawTexture(new Rect(265, 100, 745, 495), pauseWindows);
		
			//TEXTE PAUSE
		GUI.skin.textArea.fontSize = 76;
		//GUI.TextArea(new Rect(pauseWindowsRect.width*0.3f, pauseWindowsRect.height*0.23f, pauseWindowsRect.width*0.8f, pauseWindowsRect.height*0.23f), "ON PAUSE");
	
		
		GUI.skin = mainMenuButtonSkin;
		if(GUI.Button(new Rect(400, 175, 501, 149), "")){
			Pause();
			MusicManager.soundManager.StopSFX(12);
			GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().EmptyingBucket();
			GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().movement.SetVelocity(Vector2.zero);
			LevelSerializer.SaveObjectTreeToFile("Chromasave", GameObject.FindGameObjectWithTag("StatsManager").gameObject);
			//MusicManager.soundManager.StartMenuMusic();
			_FromGame = true;
			OffAction();
			LoadALevel(0);
		}
		
		GUI.skin = pauseBackButton;
		if(GUI.Button(new Rect(450, 325, 372, 159), "")){
			Pause();
			StartHudOpenSequence();
		}					
	}
	
		//DRAW LE INGAME START
	void DrawInGameStart(){
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), blackBG);
			//START BUTTON
		GUI.skin = _SkinMenuSansBox;
		Rect startBox = new Rect(500, 350, 250, 126);
		GUI.DrawTexture(new Rect(300, 175, 489, 374), movieSecondLoad);
		if(!MainManager._CanControl){
			_AvatarScript.movement.SlowToStop();
		}
	}
	
		//DRAW LA FENETRE DU MAIN MENU
	void DrawTitleScreen(){
				
			//START DES ANIM DU TITLESCREEN
		movieTitle.Play();
		movieStart.Play();
		movieCredit.Play();
		movieExit.Play();
		
			//BACKGROUND
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), mainMenuBG);
		
		if(!_CanShowPressStartAnim){
			GUI.DrawTexture(new Rect(310, 200, 690, 306), movieTitle);
		}
		
			//QUITBUTTON
		GUI.skin = null;// _SkinMenuSansBox;
		Rect exitBox = new Rect(550, 600, 150, 100);
		if(GUI.Button(exitBox, "")){
			Application.Quit();
		}
		GUI.DrawTexture(exitBox, movieExit);
		
			//START BUTTON
		GUI.skin = _SkinMenuSansBox;
		Rect startBox = new Rect(500, 450, 250, 126);
		if(GUI.Button(startBox, "")){
			_CanShowPressStartAnim = true;
			MusicManager.soundManager.PlaySFX(51);
		}
		GUI.DrawTexture(startBox, movieStart);
		
		if(_CanShowPressStartAnim){
			GUI.DrawTexture(new Rect(375, 220, 500, 480), moviePressStart);
			if(!moviePressStart.isPlaying){
				_FirstStart = false;
				moviePressStart.Stop();
				moviePressStart.Play();
				StartCoroutine(GoToLvlSelection());
			}
		}
	}
	
		//DRAW LA FENETRE DE SELECTION DU CLAVIER
	void DrawKeyboardSelectionScreen(){
		
		GUI.skin = _SkinMenuSansBox;
		GUI.skin.textArea.fontSize = 70;
		GUI.skin.textArea.normal.textColor = Color.white;
	}
	
		//DRAW LE LOADING SCREEN
	void DrawLoadingScreen(){
		Rect inLoadRect = new Rect(217.5f, 165, 740, 380);
			//BACKGROUND
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), blackBG);
		GUI.DrawTexture(new Rect(300, 175, 489, 374), movieSecondLoad);
				
		//PlayLoadAnim();
		//STAND BY SUR L"ANIM POUR LINSTANT
		//GUI.DrawTexture(new Rect(350, 100, 550, 512), loadingIcon1);
		
	}
	
		//DRAW LE CREDITS SCREEN
	void DrawCreditsScreen(){
		
		
	}
		
		//DRAW LE FAKE SPLASH SCREEN
	void DrawFakeSplashScreen(){
		
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), splashScreenBG);
		
		movieLoad.Play();
		GUI.DrawTexture(new Rect(400, 90, 550, 520), movieLoad);
		StartCoroutine(CanShowLogo());
		
		if(_CanShowLogo){
			movieLogo.Play();
			GUI.DrawTexture(new Rect(400, 90, 550, 520), movieLogo);
			
			if(!_OnLogo){
				_OnLogo = true;
				StartCoroutine(GoToMainMenu(5.1f));
			}
		}
	}
	
		//DRAW LE STATS SCREEN
	void DrawStatsScreen(){
		GUI.skin = _SkinMenuSansBox;
		GUI.skin.textArea.fontSize = 34;
		GUI.skin.textArea.normal.textColor = Color.black;
		
		GUI.DrawTexture(new Rect(536f, 75f, 237f, 115f), whiteColTex);
		GUI.TextArea(new Rect(635f, 130f, 170f, 50f), StatsManager.whiteCollDisplayed.ToString() + " / " + "426");
		GUI.DrawTexture(new Rect(528f, 169f, 240f, 99f), redColTex);
		GUI.TextArea(new Rect(635f, 209f, 170f, 50f), StatsManager.redCollDisplayed.ToString() + " / " + "47");
		GUI.DrawTexture(new Rect(530f, 245f, 224f, 102f), blueColText);
		GUI.TextArea(new Rect(635f, 288, 170f, 50f), StatsManager.blueCollDisplayed.ToString() + " / " + "66");
		GUI.DrawTexture(new Rect(530f, 330f, 236f, 96f), comicTex);
		GUI.TextArea(new Rect(635f, 370f, 170f, 50f), StatsManager.comicThumbDisplayed.ToString() + " / " + "100");
		GUI.DrawTexture(new Rect(528f, 410f, 232f, 105f), deathCountTex);
		GUI.skin.textArea.fontSize = 30;
		GUI.TextArea(new Rect(620f, 465f, 50f, 50f), StatsManager.deathCounter.ToString());
		
		/*
		GUI.skin = _TimeTrialButtonSkin;
		if(GUI.Button(new Rect(528f, 520f, 240f, 100f), "")){
			menuWindows = _MenuWindowsEnum.TimeTrialScreen;
		}*/
	}
	
	void DrawTimeTrialScreen(){
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), ttBg);
		
	}
	
	void DrawGameModeScreen(){
		
	}
	
	IEnumerator Setup(float delai){
		yield return new WaitForSeconds(delai);
		
		if(currentLevel != 0){
			_RoomManager = GetComponent<ChromaRoomManager>();
			_TotalComicThumb = _RoomManager.UpdateTotalComic();
			if(GameObject.FindGameObjectWithTag("avatar")){
				_AvatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
				_AvatarScript.CannotControlFor(false, 0);
			}
		}		
	}
	
	IEnumerator ActiveHudBox(float delai, int boxToActive, bool open){
		yield return new WaitForSeconds(delai);
		if(open){
			hudBoxCanAppear[boxToActive] = true;
		}
		else{
			hudBoxCanDisappear[boxToActive] = true;
		}
	}
	IEnumerator ActiveMainBox(float delai, bool open){
		yield return new WaitForSeconds(delai);
		if(open){
			mainBoxCanDown = true;
		}
		else{
			mainBoxCanUp = true;
		}
	}
	IEnumerator GoToLvlSelection(){
		yield return new WaitForSeconds(0.65f);
		
		if(!StatsManager.keyboardAlreadyChoose){
			_MenuWindows = _MenuWindowsEnum.KeyboardSelectionScreen;
			ActiveKeyboardButton();
		}
		else{
			_MenuWindows = _MenuWindowsEnum.LevelSelectionWindows;
		}
		ResetTitleBool();
	}
	IEnumerator GoToMainMenu(float delai){
		yield return new WaitForSeconds(delai);
		_MenuWindows = _MenuWindowsEnum.MainMenu;
		MusicManager.soundManager.PlaySFX(6);
		StartCoroutine(PlayStartFX());
	}
	IEnumerator CanShowLogo(){
		yield return new WaitForSeconds(1.9f);
		_CanShowLogo = true;
	}
	IEnumerator GoToGame(){
		yield return new WaitForSeconds(0.65f);
		_GUIState = GUIStateEnum.Interface;
		StartHudOpenSequence();		
	}
	IEnumerator PlayStartFX(){
		yield return new WaitForSeconds(4.8f);
		MusicManager.soundManager.PlaySFX(3);
	}
	IEnumerator ResetFromGame(){
		yield return new WaitForSeconds(4.0f);
		_FromGame = false;
	}
}

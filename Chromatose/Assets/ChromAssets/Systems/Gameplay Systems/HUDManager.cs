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
	MainMenu, LevelSelectionWindows, CreditWindows, OptionWindows, LoadingScreen, KeyboardSelectionScreen, None
}
public enum _OptionWindowsEnum{
	MainOption, Sound, GameMode, Stats, DeleteSavegame, None
}

[System.Serializable]
public class HUDManager : MainManager {
	
	public static HUDManager hudManager;
	
	public static GUIStateEnum _GUIState;
	private _MenuWindowsEnum _MenuWindows;
	private _OptionWindowsEnum _OptionWindows;
	
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
	
	public Texture pauseWindows, mainMenuBG, whiteBG, optionBG, loadingBG, greyFilterBG, endResultWindows, emptyProgressBar, progressLine, credits;
	public Texture _AvatarLoadingLoop1, _AvatarLoadingLoop2, _BulleLoadingLoop1, _BulleLoadingLoop2;
	
	public GUISkin _PlayButtonSkin, _GreenlightSkin, skinSansBox, pauseBackButton, _SkinMenuSansBox, _VoidSkin;
	public GUISkin _StartButtonSkin, _CreditButtonSkin, _FbookButtonSkin, _TwitterButtonSkin, _BackButtonSkin, _GreenlightButton;
	
	public Texture qwertyKeyboard, azertyKeyboard, leftArrow, rightArrow;	
	public Texture actionTexture, shownActionTexture;
	
	private Actions currentAction = Actions.Nothing;
	
	private bool _CanShowAction = false, 
					actionPressed = false, 
					_GlowComicNb = false, 
					_CanFlash = false, 
					_OnFlash = false, 
					_AfterComic = false, 
					showingAction = false,
					_FirstStart = false;

	private float flashTimer = 0.0f, aX, actionSlideSpeed = 10.0f;
	private Vector2 textOffset = new Vector2 (55f, 8);
	
	public  delegate void ActionDelegate();
	public ActionDelegate actionMethod;
	
	private int _TotalComicThumb = 0;
	
	
	//MANAGER & VIP OBJECT
	//private Avatar _AvatarScript;	
	
	
	
	//GETSET ACCESSOR
	public bool afterComic { get { return _AfterComic; } set { _AfterComic = value; } }
	public bool canFlash { get { return _CanFlash; } set { _CanFlash = value; } }
	public bool onFlash { get { return _OnFlash; } set { _OnFlash = value; } }
	public bool firstStart { get { return _FirstStart; } set { _FirstStart = value; } }
	
	public GUIStateEnum guiState { get { return _GUIState; } set { _GUIState = value; } }
	public _MenuWindowsEnum menuWindows { get { return _MenuWindows; } set { _MenuWindows = value; } }
	public _OptionWindowsEnum optionWindows { get { return _OptionWindows; } set { _OptionWindows = value; } }
	
	
	//START, SETUP AND INIT
	void Awake(){
		//EMPECHE LE HUDMANAGER DE SE DETRUIRE ENTRE LES NIVEAUX
		//DontDestroyOnLoad(this);
		hudManager = this;
		
	}
	
	void OnLevelWasLoaded(){
		SetupHud();
		StartCoroutine(Setup(0f));
	}
	
	void Start () {
		aX = absorbAction.width * 1.5f;		
		actionTexture = absorbAction;
		shownActionTexture = actionTexture;
		//SETUP LE TOUT, ON LE FAIT A L'EXTERNE CAR J'AIME BIEN POUVOIR LE RAPPELLER, EN CAS DE PROBLEME
		StartCoroutine(Setup(0.1f));
		
	}
	
	
	
	//FIXED & LATE UPDATE
	void FixedUpdate () {
	
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
		if (Input.GetKeyDown(KeyCode.P) && currentAction > 0 && actionMethod != null && !ChromatoseManager.manager.InComic){
			actionMethod();
		}
		showingAction = false;
	}
	
	
	
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
	
	
	
	public void SetupHud(){
		if(Application.loadedLevel!=0){
				//SET LE HUD POUR LE DEBUT D'UN NIVEAU
			_GUIState = GUIStateEnum.OnStart;
			_MenuWindows = _MenuWindowsEnum.None;
			_OptionWindows = _OptionWindowsEnum.None;
		}
		else{
				//SET LE HUD POUR LE MAIN MENU
			_GUIState = GUIStateEnum.MainMenu;
			_MenuWindows = _MenuWindowsEnum.MainMenu;
			_OptionWindows = _OptionWindowsEnum.None;
			ResetComicCounter();
		}
	}
	
	
	
	
	//RESET FUNCTION
	public void ResetComicCounter(){
		_TotalComicThumb = _RoomManager.UpdateTotalComic();
		StatsManager.comicThumbCollected = 0;
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
				
				//DRAW LE TITLE SCREEN
			case _MenuWindowsEnum.MainMenu:
				DrawTitleScreen();
				break;
				
				//DRAW LE LEVELSELECTION SCREEN
			case _MenuWindowsEnum.LevelSelectionWindows:
				DrawLevelSelectionScreen();
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
				DrawCreditsScreen();
				break;
	
				//DRAW LA FENETRE DE SELECTION DE KEYBOARD
			case _MenuWindowsEnum.KeyboardSelectionScreen:
				DrawKeyboardSelectionScreen();
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
		Rect inMenuRect = new Rect(190, 165, 740, 395);
				//BACKGROUND
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), optionBG);

		switch(_OptionWindows){
		
		case _OptionWindowsEnum.MainOption:
			Rect optionRect = new Rect(190, 165, 740, 395);
			GUI.skin = _SkinMenuSansBox;
			GUI.BeginGroup(optionRect);
				if(GUI.Button(new Rect(optionRect.width*0.26f, optionRect.height*0.1f, optionRect.width*0.66f, optionRect.height*0.20f), "- SOUND -")){
					_OptionWindows = _OptionWindowsEnum.Sound;
				}
				if(GUI.Button(new Rect(optionRect.width*-0.03f, optionRect.height*0.34f, optionRect.width*0.66f, optionRect.height*0.20f), "- GAME MODE -")){
					_OptionWindows = _OptionWindowsEnum.GameMode;
				}
				if(GUI.Button(new Rect(125, optionRect.height*0.58f, optionRect.width*0.66f, optionRect.height*0.20f), "- STATS -")){
					_OptionWindows = _OptionWindowsEnum.Stats;
				}
				if(GUI.Button(new Rect(optionRect.width*0.05f, optionRect.height*0.80f, optionRect.width*0.92f, optionRect.height*0.20f), "- DELETE SAVEGAME -")){
					_OptionWindows = _OptionWindowsEnum.DeleteSavegame;
				}
			GUI.EndGroup();
			
				//BACK BUTTON
			GUI.skin.button.fontSize = 48;
			if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
				_MenuWindows = _MenuWindowsEnum.MainMenu;
			}
			
			break;

		case _OptionWindowsEnum.Sound:
			Rect soundRect = new Rect(190, 110, 740, 400);
			
			GUI.BeginGroup(soundRect);
				GUI.skin.button.fontSize = 32;
				GUI.TextArea(new Rect(soundRect.width*0.25f, soundRect.height*0.21f, soundRect.width*0.5f, soundRect.height*0.2f), "- SOUND -");
				
					//Text & Slider
				GUI.skin.textArea.fontSize = 42;
				GUI.TextArea(new Rect(soundRect.width*0.025f, soundRect.height*0.51f, soundRect.width*0.15f, soundRect.height*0.15f), "MUSIC");
				_MusicVolume = GUI.HorizontalSlider(new Rect(soundRect.width*0.23f, soundRect.height*0.51f, soundRect.width*0.55f, soundRect.height*0.18f), _MusicVolume, 100, 0);
			
				GUI.TextArea(new Rect(soundRect.width*0.05f, soundRect.height*0.76f, soundRect.width*0.15f, soundRect.height*0.15f), "SFX");
				_SFXVolume = GUI.HorizontalSlider(new Rect(soundRect.width*0.23f, soundRect.height*0.76f, soundRect.width*0.55f, soundRect.height*0.18f), _SFXVolume, 100, 0);
			
					//Mute Button
				_MusicMute = GUI.Toggle(new Rect(soundRect.width*0.8f, soundRect.height*0.51f, soundRect.width*0.2f, soundRect.height*0.15f), _MusicMute, "MUTE");
				_SFXMute = GUI.Toggle(new Rect(soundRect.width*0.8f, soundRect.height*0.76f, soundRect.width*0.2f, soundRect.height*0.15f), _SFXMute, "MUTE");
			GUI.EndGroup();
				//BACK BUTTON
			GUI.skin.button.fontSize = 48;
			if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
				_OptionWindows = _OptionWindowsEnum.MainOption;
			}
			break;

		case _OptionWindowsEnum.GameMode:
			GUI.BeginGroup(inMenuRect);
				GUI.skin.textArea.fontSize = 60;
				GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.15f, inMenuRect.width*0.6f, inMenuRect.height*0.21f), "- GAMEMODE -");
					//TIME TRIAL MODE & NO DEATH ACTIVATION
				if(!_ExtraModeUnlocked){
					GUI.TextArea(new Rect(inMenuRect.width*0.02f, inMenuRect.height*0.52f, inMenuRect.width*0.9f, inMenuRect.height*0.4f), " Finish the Game to Unlock More Chromatose Extra Mode");
				}
				else{
					GUI.skin.textArea.fontSize = 40;
				
					GUI.TextArea(new Rect(inMenuRect.width*0.1f, inMenuRect.height*0.52f, inMenuRect.width*0.6f, inMenuRect.height*0.25f), "TIME TRIAL CHALLENGE");
					GUI.TextArea(new Rect(inMenuRect.width*0.1f, inMenuRect.height*0.74f, inMenuRect.width*0.6f, inMenuRect.height*0.25f), "NO DEATH CHALLENGE");
					_TimeTrialActive = GUI.Toggle(new Rect(inMenuRect.width*0.7f, inMenuRect.height*0.52f, inMenuRect.width*0.15f, inMenuRect.height*0.2f),_TimeTrialActive ,"Active");
					_NoDeathModeActive = GUI.Toggle(new Rect(inMenuRect.width*0.7f, inMenuRect.height*0.74f, inMenuRect.width*0.15f, inMenuRect.height*0.2f), _NoDeathModeActive, "Active");
					
				}
			GUI.EndGroup();
				//BACK BUTTON
			GUI.skin.button.fontSize = 48;
			if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
				_OptionWindows = _OptionWindowsEnum.MainOption;
			}
			break;

		case _OptionWindowsEnum.Stats:
			GUI.BeginGroup(inMenuRect);
				GUI.skin.textArea.fontSize = 60;
					//STATIC STRING
				GUI.TextArea(new Rect(inMenuRect.width*0.24f, inMenuRect.height*0.01f, inMenuRect.width*0.6f, inMenuRect.height*0.21f), "- STATS -");
				GUI.skin.textArea.fontSize = 30;
				GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.2f, inMenuRect.width*0.3f, inMenuRect.height*0.21f), "DEAD NPC");
				GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.3f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "WHITE COLLECTIBLES");
				GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.4f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "RED COLLECTIBLES");
				GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.5f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "BLUE COLLECTIBLES");
				GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.6f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "COMICS VIEWED");
				GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.7f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "ACHIEVEMENT UNLOCKED");
				GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.8f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "TOTAL DEATH");
				GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.9f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "TOTAL PLAYTIME");
					//STATISTIC STRING
				GUI.TextArea(new Rect(inMenuRect.width*0.675f, inMenuRect.height*0.2f, inMenuRect.width*0.3f, inMenuRect.height*0.21f), ""); 	//AJOUTER LE NB DE NPC TUER
				GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.3f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), StatsManager.whiteCollCollected.ToString());
				GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.4f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), StatsManager.redCollCollected.ToString());
				GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.5f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), StatsManager.blueCollCollected.ToString());
				GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.6f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), StatsManager.comicThumbCollected.ToString());
				GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.7f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "");	//AJOUTER LE NB D'ACHIEVEMENTS DEBLOQUER
				GUI.TextArea(new Rect(inMenuRect.width*0.675f, inMenuRect.height*0.8f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "");	//AJOUTER LE NB DE FOIS QUE LE PLAYER EST MORT
				GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.9f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "");	//AJOUTER LE TEMPS TOTAL JOUER
			GUI.EndGroup();
				//HIGHSCORE BUTTON
			GUI.skin.button.fontSize = 48;
			if(GUI.Button(new Rect(470, 605, 385, 80), "- HIGHSCORES -")){
				
			}
				//BACK BUTTON
			GUI.skin.button.fontSize = 48;
			if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
				_OptionWindows = _OptionWindowsEnum.MainOption;
			}
			break;

		case _OptionWindowsEnum.DeleteSavegame:
			GUI.BeginGroup(inMenuRect);
				GUI.skin.textArea.fontSize = 52;
				GUI.TextArea(new Rect(inMenuRect.width*0.05f, inMenuRect.height*0.14f, inMenuRect.width*0.9f, inMenuRect.height*0.2f), "- DELETE THE ACTUAL -");
				GUI.skin.textArea.fontSize = 46;
				GUI.TextArea(new Rect(inMenuRect.width*0.3f, inMenuRect.height*0.3f, inMenuRect.width*0.55f, inMenuRect.height*0.2f), "- SAVED GAME -");
				GUI.skin.textArea.fontSize = 55;
				GUI.TextArea(new Rect(inMenuRect.width*0.1f, inMenuRect.height*0.51f, inMenuRect.width*0.7f, inMenuRect.height*0.28f), "? ARE YOU SURE ?");
				GUI.skin.button.fontSize = 60;
				if(GUI.Button(new Rect(inMenuRect.width*0.12f, inMenuRect.height*0.73f, 250, 80), "YES")){
					File.Delete(Application.persistentDataPath + "/" + "Chromasave");
					_FirstStart = true;
					_MenuWindows = _MenuWindowsEnum.MainMenu;
					_OptionWindows = _OptionWindowsEnum.MainOption;
					Debug.Log("GAME DELETER");
				}
				if(GUI.Button(new Rect(inMenuRect.width*0.4f, inMenuRect.height*0.73f, 250, 80), "NO")){
					_OptionWindows = _OptionWindowsEnum.MainOption;
				}
			GUI.EndGroup();	
				//BACK BUTTON
			GUI.skin.button.fontSize = 48;
			if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
				_OptionWindows = _OptionWindowsEnum.MainOption;
			}
			break;
		}
	}	
	
		//FUNCTION QUI DRAW L'INTERFACE EN JEU
	void DrawInterfaceHUD(){
		
			//INITIALISATION DES RECTANGLEBOX
		Rect bgRect = new Rect(0, 0, 1280, 720);
		Rect mainRect = new Rect(1150, -120, mainBox.width, mainBox.height);
		Rect comicRect = new Rect(1150, 115, comicCounter.width + 10, comicCounter.height);
		Rect wColRect = new Rect(1150, 165, whiteCollectible.width+ 10, whiteCollectible.height);
		Rect rColRect = new Rect(1150, 215, redCollectible.width + 10, redCollectible.height);
		Rect bColRect = new Rect(1150, 265, blueCollectible.width + 10, blueCollectible.height);
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
		
		GUI.BeginGroup(comicRect);										//comic counter		
			GUI.skin.textArea.normal.textColor = Color.black;
			if(StatsManager.comicThumbCollected >=  _TotalComicThumb && _TotalComicThumb != 0 && !_AfterComic){
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
			GUI.skin.textArea.fontSize = 22;
			GUI.skin.textArea.normal.textColor = Color.black;
			GUI.TextArea(new Rect(textOffset.x, textOffset.y, 100, 50), StatsManager.comicThumbCollected + " / " + _TotalComicThumb);
			
			
		GUI.EndGroup();
		
		GUI.BeginGroup(wColRect);										//white collectible
			GUI.skin.textArea.normal.textColor = Color.white;
			GUI.DrawTexture(new Rect(0, 0, whiteCollectible.width, whiteCollectible.height), whiteCollectible);
			GUI.TextArea(new Rect(textOffset.x + 10, textOffset.y, 100, 50), StatsManager.whiteCollDisplayed.ToString());// + " / " + _TotalWhiteColl.ToString());
			
		GUI.EndGroup();
		
		GUI.BeginGroup(rColRect);										//red collectible
			GUI.skin.textArea.normal.textColor = Color.red;
			GUI.DrawTexture(new Rect(0, 0, redCollectible.width, redCollectible.height), redCollectible);
			GUI.TextArea(new Rect(textOffset.x + 10, textOffset.y, 100, 50), StatsManager.redCollCollected.ToString());// + " / " + _TotalRedColl.ToString());
			
		GUI.EndGroup();

		
		GUI.BeginGroup(bColRect);										//blue collectible
			GUI.skin.textArea.normal.textColor = Color.blue;
			GUI.DrawTexture(new Rect(0, 0, blueCollectible.width, blueCollectible.height), blueCollectible);
			GUI.TextArea(new Rect(textOffset.x + 10, textOffset.y, 100, 50), StatsManager.blueCollCollected.ToString());// + " / " + _TotalBlueColl.ToString());
			
		GUI.EndGroup();
		
		
		GUI.BeginGroup(actionRect);										//Action icon
			
			GUI.DrawTexture(new Rect(aX, 0, absorbAction.width, absorbAction.height), actionTexture);
		
		GUI.EndGroup();			
	}
	
		//DRAW LE MENU PAUSE
	void DrawInGamePause(){
		Rect pauseWindowsRect = new Rect (128, 100, 700, 360);
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), greyFilterBG);
		GUI.skin = skinSansBox;
		
		GUIUtility.RotateAroundPivot(-12f, new Vector2(320, 215));
		GUI.BeginGroup(pauseWindowsRect);
			//PAUSE WINDOWS
			GUI.DrawTexture(new Rect(pauseWindowsRect.width*0.05f, pauseWindowsRect.height*0.05f, pauseWindowsRect.width*0.9f, pauseWindowsRect.height*0.9f), pauseWindows);
			
			//TEXTE PAUSE
			GUI.skin.textArea.fontSize = 76;
			GUI.TextArea(new Rect(pauseWindowsRect.width*0.3f, pauseWindowsRect.height*0.23f, pauseWindowsRect.width*0.8f, pauseWindowsRect.height*0.23f), "ON PAUSE");
		
			GUI.skin = pauseBackButton;
			if(GUI.Button(new Rect(300, 225, 130, 75), "")){
				Pause();
			}	
		
		GUI.EndGroup();
	}
	
		//DRAW LE INGAME START
	void DrawInGameStart(){
		GUI.skin = skinSansBox;
			//Black BackGround
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), whiteBG);
		GUI.skin = _PlayButtonSkin;
		GUI.skin.button.fontSize = 78;
		if(GUI.Button(new Rect(350, 70, 512, 512), "")){
			_GUIState = GUIStateEnum.Interface;
			GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().CanControl();
			MusicManager.soundManager.PlaySFX(19);
			MusicManager.soundManager.CheckLevel();
		}
	}
	
		//DRAW LA FENETRE DU MAIN MENU
	void DrawTitleScreen(){
			//BACKGROUND
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), mainMenuBG);
		if(!_FirstStart){
				//RESUME BUTTON
			GUI.skin.button.fontSize = 76;
			if(GUI.Button(new Rect(195, 455, 400, 150), "RESUME")){
				_MenuWindows = _MenuWindowsEnum.LevelSelectionWindows;
			}
		}
		else{
				//START BUTTON
			GUI.skin = _StartButtonSkin;
			if(GUI.Button(new Rect(195, 307, 400, 300), "")){
				_MenuWindows = _MenuWindowsEnum.KeyboardSelectionScreen;
			}
		}	
		GUI.skin = _SkinMenuSansBox;
		GUI.skin.button.fontSize = 70;
		if(GUI.Button(new Rect(220, 10, 250, 110), "OPTIONS")){
			_MenuWindows = _MenuWindowsEnum.OptionWindows;
		}
		GUI.skin = _CreditButtonSkin;
		GUI.skin.button.fontSize = 76;
		if(GUI.Button(new Rect(664, 307, 400, 300), "")){
			_MenuWindows = _MenuWindowsEnum.CreditWindows;
		}
			//FBOOK & TWITTER
		GUI.skin = _TwitterButtonSkin;			
		if(GUI.Button (new Rect(10, 15, 97, 75), "")){
			Application.OpenURL("https://twitter.com/Chromatosegame");
		}
		GUI.skin = _FbookButtonSkin;
		if(GUI.Button (new Rect(88, 15, 97, 75), "")){
			Application.OpenURL("https://www.facebook.com/FabulamGames?fref=ts");
		}
			//BUY IT ON STEAM
		if(_ADADAD){
			GUI.skin = _GreenlightButton;
			if(GUI.Button(new Rect(920, 20f, 335, 137), "")){
				Application.OpenURL("http://steamcommunity.com/sharedfiles/filedetails/?id=174349688");
			}
		}
			//QUITBUTTON
		GUI.skin = _SkinMenuSansBox;
		GUI.skin.button.fontSize = 45;
		if(GUI.Button(new Rect(900, 160, 400, 60), "EXIT GAME")){
			Application.Quit();
		}
	}
	
		//DRAW LA FENETRE DE SELECTION DE NIVEAU
	void DrawLevelSelectionScreen(){
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), whiteBG);
		GUI.skin = _SkinMenuSansBox;
		GUI.skin.button.fontSize = 40;
		
			//BOUTON SELECTION NIVEAU 1 -- TUTO
		if(StatsManager.levelUnlocked[0] == true){
			GUI.skin = _SkinMenuSansBox;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(250, 50, 300, 50), "TUTO - BLANC 1")){
				_MenuWindows = _MenuWindowsEnum.LoadingScreen;
				LoadALevel(1);
				StatsManager.newLevel = true;
			}
		}
		else{
			GUI.skin = _VoidSkin;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(250, 50, 300, 50), "TUTO - BLANC 1")){}
		}
		
			//BOUTON SELECTION NIVEAU 2 -- MODULE ROUGE 1
		if(StatsManager.levelUnlocked[1] == true){
			GUI.skin = _SkinMenuSansBox;
			GUI.skin.button.fontSize = 40;		
			if(GUI.Button(new Rect(250, 125, 300, 50), "NIV 1 - ROUGE 1")){
				_MenuWindows = _MenuWindowsEnum.LoadingScreen;
				LoadALevel(2);
			}
		}
		else{
			GUI.skin = _VoidSkin;
			GUI.skin.button.fontSize = 40; 
			if(GUI.Button(new Rect(250, 125, 300, 50), "NIV 1 - ROUGE 1")){}
		}
		
			//BOUTON SELECTION NIVEAU 3 -- MODULE BLANC 2
		if(StatsManager.levelUnlocked[2] == true){
			GUI.skin = _SkinMenuSansBox;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(250, 200, 300, 50), "NIV 2 - BLANC 2")){
				_MenuWindows = _MenuWindowsEnum.LoadingScreen;
				LoadALevel(3);
			}
		}
		else{
			GUI.skin = _VoidSkin;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(250, 200, 300, 50), "NIV 2 - BLANC 2")){}
		}
		
			//BOUTON SLECTION NIVEAU 4 -- MODULE ROUGE 2
		if(StatsManager.levelUnlocked[3] == true){
			GUI.skin = _SkinMenuSansBox;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(250, 275, 300, 50), "NIV 3 - ROUGE 2")){
				_MenuWindows = _MenuWindowsEnum.LoadingScreen;
				LoadALevel(4);
			}
		}
		else{
			GUI.skin = _VoidSkin;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(250, 275, 300, 50), "NIV 3 - ROUGE 2")){}
		}
		
			//BOUTON SELECTION NIVEAU 5 -- MODULE BLANC 3
		if(StatsManager.levelUnlocked[4] == true){
			GUI.skin = _SkinMenuSansBox;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(250, 350, 300, 50), "NIV 4 - BLANC 3")){
				_MenuWindows = _MenuWindowsEnum.LoadingScreen;
				LoadALevel(5);
			}
		}
		else{
			GUI.skin = _VoidSkin;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(250, 350, 300, 50), "NIV 4 - BLANC 3")){}
		}
		
			//BOUTON SELECTION NIVEAU 6 -- MODULE ROUGE 3
		if(StatsManager.levelUnlocked[5] == true){
			GUI.skin = _SkinMenuSansBox;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(600, 50, 400, 50), "NIV 5 - ROUGE/BLEU 3")){
				_MenuWindows = _MenuWindowsEnum.LoadingScreen;
				LoadALevel(6);
			}
		}
		else{
			GUI.skin = _VoidSkin;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(600, 50, 400, 50), "NIV 5 - ROUGE/BLEU 3")){}
		}
		
			//BOUTON SELECTION NIVEAU 7 -- MODULE BLANC 4
		if(StatsManager.levelUnlocked[6] == true){
			GUI.skin = _SkinMenuSansBox;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(600, 125, 300, 50), "NIV 6 - BLANC 4")){
				_MenuWindows = _MenuWindowsEnum.LoadingScreen;
				LoadALevel(7);
			}
		}
		else{
			GUI.skin = _VoidSkin;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(600, 125, 300, 50), "NIV 6 - BLANC 4")){}
		}
		
			//BOUTON SELECTION NIVEAU 8 -- MODULE ROUGE 4
		if(StatsManager.levelUnlocked[7] == true){
			GUI.skin = _SkinMenuSansBox;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(600, 200, 300, 50), "NIV 7 - BLEU 4")){
				_MenuWindows = _MenuWindowsEnum.LoadingScreen;
				LoadALevel(8);
			}
		}
		else{
			GUI.skin = _VoidSkin;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(600, 200, 300, 50), "NIV 7 - BLEU 4")){}
		}
		
			//BOUTON SELECTION NIVEAU 9 -- MODULE ROUGE 5
		if(StatsManager.levelUnlocked[8] == true){
			GUI.skin = _SkinMenuSansBox;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(600, 275, 400, 50), "NIV 8 - ROUGE/BLEU 5")){
				_MenuWindows = _MenuWindowsEnum.LoadingScreen;
				LoadALevel(9);
			}
		}
		else{
			GUI.skin = _VoidSkin;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(600, 275, 400, 50), "NIV 8 - ROUGE/BLEU 5")){}
		}
		
			//BOUTON SELECTION NIVEAU 10 -- MODULE ROUGE 6
		if(StatsManager.levelUnlocked[9] == true){
			GUI.skin = _SkinMenuSansBox;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(600, 350, 300, 50), "NIV 9 - ROUGE 6")){
				_MenuWindows = _MenuWindowsEnum.LoadingScreen;
				LoadALevel(10);
			}
		}
		else{
			GUI.skin = _VoidSkin;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(600, 350, 300, 50), "NIV 9 - ROUGE 6")){}
		}
		
			//BOUTON SELECTION NIVEAU 11 -- BOSS FINAL
		if(StatsManager.levelUnlocked[10] == true){
			GUI.skin = _SkinMenuSansBox;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(250, 425, 650, 50), "BOSS FINAL")){
				_MenuWindows = _MenuWindowsEnum.LoadingScreen;
				LoadALevel(11);
			}
		}
		else{
			GUI.skin = _VoidSkin;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(250, 425, 650, 50), "BOSS FINAL")){}
		}
		
			//BOUTON SELECTION NIVEAU GYM -- GYM DU CHU
		if(StatsManager.levelUnlocked[11] == true){
			GUI.skin = _SkinMenuSansBox;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(800, 600, 350, 50), "GYM DU CHU")){
				_MenuWindows = _MenuWindowsEnum.LoadingScreen;
				LoadALevel(12);
			}
		}
		else{
			GUI.skin = _VoidSkin;
			GUI.skin.button.fontSize = 40;
			if(GUI.Button(new Rect(800, 600, 350, 50), "GYM DU CHU")){}
		}

			//BACK BUTTON
		GUI.skin = _SkinMenuSansBox;
		GUI.skin.button.fontSize = 48;
		if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
			_MenuWindows = _MenuWindowsEnum.MainMenu;
		}
	}
	
		//DRAW LA FENETRE DE SELECTION DU CLAVIER
	void DrawKeyboardSelectionScreen(){
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), mainMenuBG);
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), greyFilterBG);
		
		GUI.skin = _SkinMenuSansBox;
		GUI.skin.textArea.fontSize = 60;
		GUI.skin.textArea.fontStyle = FontStyle.Bold;
		GUI.skin.textArea.normal.textColor = Color.black;

		GUI.TextArea(new Rect(300, 90, 750, 70), "CHOOSE YOUR KEYBOARD !");
		
		if(keyboardType == MainManager._KeyboardTypeEnum.QWERTY){
			GUI.DrawTexture(new Rect(198, 150, 884, 361), qwertyKeyboard);
			if(GUI.Button(new Rect(50, 250, 128, 128), leftArrow)){
				keyboardType = MainManager._KeyboardTypeEnum.AZERTY;
			}
			if(GUI.Button(new Rect(1102, 250, 128, 128), rightArrow)){
				keyboardType = MainManager._KeyboardTypeEnum.AZERTY;
			}
		}
		else if(keyboardType == MainManager._KeyboardTypeEnum.AZERTY){
			GUI.DrawTexture(new Rect(198, 150, 884, 361), azertyKeyboard);
			if(GUI.Button(new Rect(50, 250, 128, 128), leftArrow)){
				keyboardType = MainManager._KeyboardTypeEnum.QWERTY;
			}
			if(GUI.Button(new Rect(1102, 250, 128, 128), rightArrow)){
				keyboardType = MainManager._KeyboardTypeEnum.QWERTY;
			}
		}
		if(GUI.Button(new Rect(340, 500, 500, 75), "SELECT THIS ONE")){
			_MenuWindows = _MenuWindowsEnum.LevelSelectionWindows;
		}
	}
	
		//DRAW LE LOADING SCREEN
	void DrawLoadingScreen(){
		Rect inLoadRect = new Rect(217.5f, 165, 740, 380);
			//BACKGROUND
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), loadingBG);
		GUI.BeginGroup(inLoadRect);
		/*
		 * 
		 * A REMPLACER PAR UN ICONE LOADING
		 * 
			//PROGRESS BAR
		if(async != null){
			GUI.DrawTexture(new Rect(150, 278, 6 * async.progress * 100f, inLoadRect.height*0.10f), progressLine);
			GUI.DrawTexture(new Rect(130, 262, 590, inLoadRect.height*0.2f), emptyProgressBar);
		}*/
		GUI.EndGroup();
	}
	
		//DRAW LE CREDITS SCREEN
	void DrawCreditsScreen(){
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), credits);
		GUI.skin = _SkinMenuSansBox;
			//BACK BUTTON
		GUI.skin.button.fontSize = 48;
		if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
			_MenuWindows = _MenuWindowsEnum.MainMenu;
		}
	}
	
	IEnumerator Setup(float delai){
		yield return new WaitForSeconds(delai);
		
		if(currentLevel != 0){
			_RoomManager = GetComponent<ChromaRoomManager>();
			_TotalComicThumb = _RoomManager.UpdateTotalComic();
			_AvatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
			_AvatarScript.CannotControlFor(false, 0);
		}		
	}
}

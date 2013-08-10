using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MainMenu : MonoBehaviour {
	
	public enum _MenuWindowsEnum{
		MainMenu, LevelSelectionWindows, CreditWindows, OptionWindows, LoadingScreen
	}
	public enum _OptionWindowsEnum{
		MainOption, Sound, GameMode, Stats, DeleteSavegame,
	}
	
	public MainMenuButton mainMenuButtton = new MainMenuButton();
	private StatsDisplay _StatsDisplay = new StatsDisplay();
	
	private _MenuWindowsEnum _MenuWindows;
	private _OptionWindowsEnum _OptionWindows;
	
	
	public GUISkin _SkinMenuAvecBox;
	public GUISkin _SkinMenuSansBox;
	public GUISkin _SkinMenuAvecPetitBox;
	
	
	private Color _FontColor;
	private float _FontMultiplier = 1;

	private AsyncOperation _lvlLoad;
	
	private bool[] _SlotSelected = {false, false, false};
	private bool[] _DeleteSelected = {false, false, false};
	
	private float _Alphatiser = 0;
	
	private MainManager _MainManager;
	private bool _FirstStart;
		public bool firstStart{
			get{return _FirstStart;}
			set{_FirstStart = value;}
		}
	
	//VARIABLE SOUND
	private bool _MusicMute = false;
	private bool _SFXMute = false;
	private float _MusicVolume = Mathf.Clamp(80, 0, 100);
	private float _SFXVolume = Mathf.Clamp(80, 0, 100);
	
	//VARIABLE D'INTERFACE DYNAMIC
	private float _RotStartButton = 0;
	private bool _RotUp = false;
	private int _LoadCounter = 0;
	
	//VARIABLE TRIAL (VERSION DEMO)
	private bool _ADADAD = true;
	
	//VARIABLE GAMEMODE
	private bool _ExtraModeUnlocked = false;
	private bool _TimeTrialActive = false;
	private bool _NoDeathModeActive = false;
	
	//VARIABLE LADING SCREEN
	private float _LoadProgress = 0;
	
	
	[System.Serializable]
	public class MainMenuButton{
		
		public Texture mainMenuBG;
		public Texture optionBG;
		public Texture blackBG;
		
		public Texture fbookIcon;
		public Texture twitterIcon;
		
		public Texture emptyProgressBar;
		public Texture progressLine;
		
		public static Rect _MainMenuBGRect;

		
		void Start(){
			_MainMenuBGRect = new Rect(0, 0, Screen.width, Screen.height);
				
		}
	}
	
	[System.Serializable]
	public class StatsDisplay{
		//VARIABLE STATS
		private int _DeadNPCCount = 0;
		private int _whiteCollCollected = 0;
		private int _blueCollCollected = 0;
		private int _redCollCollected = 0;
		private int _comicThumbsCollected = 0;
		
		private int _DeathCounter = 0;
		private float _TotalPlayTime = 0;
		private int _AchievementSucceed = 0;
		
		private int _TotalWhiteCollInLevel = 100;
		private int _TotalBlueCollInLevel = 20;
		private int _TotalRedCollInLevel = 35;
		private int _TotalComicThumbsInLevel = 40;
		private int _TotalAchievementCount = 10;
		
		private string _DeadNPCString;
			public string DeadNPCString{
				get{return _DeadNPCString;}
			}
		private string _whiteCollString;
			public string whiteCollString{
				get{return _whiteCollString;}
			}
		private string _blueCollString;
			public string blueCollString{
				get{return _blueCollString;}
			}
		private string _redCollString;
			public string redCollString{
				get{return _redCollString;}
			}
		private string _AchievementString;
			public string achievementString{
				get{return _AchievementString;}
			}
		private string _DeathCountString;
			public string DeathCountString{
				get{return _DeathCountString;}
			}
		private string _TotalPlaytimeString;
			public string totalPlaytimeString{
				get{return _TotalPlaytimeString;}
			}
		private string _ComicThumbsString;
			public string comicThumbsString{
				get{return _ComicThumbsString;}
			}
		
		public void SetUpStats(){
	
			_DeadNPCString = _DeadNPCCount.ToString();
			
			_whiteCollString = _whiteCollCollected + "/" + _TotalWhiteCollInLevel;
			_blueCollString = _blueCollCollected + "/" + _TotalBlueCollInLevel;
			_redCollString = _redCollCollected + "/" + _TotalRedCollInLevel;
			_AchievementString = _AchievementSucceed + "/" + _TotalAchievementCount;
			_ComicThumbsString = _comicThumbsCollected + "/" + _TotalComicThumbsInLevel;
			
			_DeathCountString = _DeathCounter.ToString();
			
			float min;
			float sec;
			min = Mathf.Floor(_TotalPlayTime/60f);		
			sec = Mathf.RoundToInt(_TotalPlayTime % 60f);
			
			_TotalPlaytimeString = min + "min" + sec + "sec";
			_TotalPlaytimeString = string.Format("{00:00}:{1:00}", min, sec);
		}
	}
	
	
	void Start () {
		
		_StatsDisplay.SetUpStats();
		
		_FontColor = new Color(173, 173, 173);
	
	}
	
	void Update () {
	//Debug.Log(LevelSerializer.SavedGames);
		
		RotateStartButton();
		
		
		if(_MainManager == null){
			_MainManager = GameObject.FindObjectOfType(typeof(MainManager))as MainManager;
		}
	
		_FontMultiplier = (Screen.width/12.8f)/100;
		//_LoadProgress = _lvlLoad.progress;
		//Debug.Log(_LoadProgress);
		
		#region Switch Update (in Case)
		switch(_MenuWindows){
		case _MenuWindowsEnum.MainMenu:
			
			break;
		case _MenuWindowsEnum.LevelSelectionWindows:
			
			break;
		case _MenuWindowsEnum.OptionWindows:
			
			break;
		case _MenuWindowsEnum.CreditWindows:
			
			break;
		case _MenuWindowsEnum.LoadingScreen:
		
			if(_LoadCounter < 200){
				_LoadCounter++;
			}
			else{
				_LoadCounter = 0;
			}
			
			break;
		}	
		#endregion
	}
	
	

	
	
	void RotateStartButton(){
		if(_RotStartButton >= 4f){
			_RotUp = false;
		}
		else if(_RotStartButton < -8f){
			_RotUp = true;
		}
			
		if(!_RotUp){
			_RotStartButton -= 0.075f;
		}
		else{
			_RotStartButton += 0.075f;
		}
	}
	
	void OnGUI(){
		GUI.skin = _SkinMenuSansBox;
		GUI.skin.button.fontSize = 60;
		GUI.skin.textArea.fontSize = 60;
		GUI.skin.toggle.fontSize = 44;
		
		
		Matrix4x4 matrixBackup = GUI.matrix;

		switch (_MenuWindows){
			
		#region Main Windows
		case _MenuWindowsEnum.MainMenu:
			
			//BACKGROUND
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mainMenuButtton.mainMenuBG);
			
			
			if(!_FirstStart){
				//RESUME BUTTON
				GUIUtility.RotateAroundPivot(17.5f + _RotStartButton, new Vector2(Screen.width*0.4f, Screen.height * 0.48f));
				GUI.skin.button.fontSize = Mathf.RoundToInt(76*_FontMultiplier);
				if(GUI.Button(new Rect(Screen.width*0.28f, Screen.height*0.44f, Screen.width*0.22f, 85), "RESUME")){
					_MenuWindows = _MenuWindowsEnum.LevelSelectionWindows;
				}
				GUI.matrix = matrixBackup; 
			}
			else{
				//START BUTTON
				GUIUtility.RotateAroundPivot(17.5f + _RotStartButton, new Vector2(Screen.width*0.4f, Screen.height * 0.48f));
				GUI.skin.button.fontSize = Mathf.RoundToInt(76*_FontMultiplier);
				if(GUI.Button(new Rect(Screen.width*0.28f, Screen.height*0.44f, Screen.width*0.22f, 85), "START")){
					//_MenuWindows = _MenuWindowsEnum.LevelSelectionWindows;
					_MenuWindows = _MenuWindowsEnum.LoadingScreen;
					_lvlLoad = Application.LoadLevelAsync(Application.loadedLevel + 2);
					//StartCoroutine(LoadLevel());
				}
				GUI.matrix = matrixBackup; 
			}	
			
			GUI.skin.button.fontSize = 48;
			GUIUtility.RotateAroundPivot(8f, new Vector2(Screen.width*0.69f, Screen.height*0.40f));
			if(GUI.Button(new Rect(Screen.width*0.69f, Screen.height*0.40f, 190, 60), "OPTIONS")){
				_MenuWindows = _MenuWindowsEnum.OptionWindows;
			}
			GUI.matrix = matrixBackup; 
			
			GUI.skin.button.fontSize = 32;
			GUIUtility.RotateAroundPivot(-8f, new Vector2(Screen.width*0.02f, Screen.height*0.5f));
			if(GUI.Button(new Rect(Screen.width*0.02f, Screen.height*0.5f , 220, 50), "> CREDITS <")){
				
			}
			GUI.matrix = matrixBackup; 
			
			//FBOOK & TWITTER
			GUI.skin = _SkinMenuAvecPetitBox;
		
			if(GUI.Button (new Rect(Screen.width*0.02f, Screen.height*0.02f, 80, 80), mainMenuButtton.fbookIcon)){
				Application.OpenURL("https://www.facebook.com/FabulamGames?fref=ts");
			}
			if(GUI.Button (new Rect(Screen.width*0.1f, Screen.height*0.02f, 80, 80), mainMenuButtton.twitterIcon)){
				Application.OpenURL("https://twitter.com/Chromatosegame");
			}
			
			
			//BUY IT ON STEAM
			if(_ADADAD){
				GUI.skin = _SkinMenuAvecBox;
				if(GUI.Button(new Rect(Screen.width*0.68f, Screen.height*0.025f, Screen.width*0.235f, Screen.height*0.08f), " Buy It On Steam")){
					Application.OpenURL("http://store.steampowered.com/");
				}
			}
			
			//QUITBUTTON
			if(GUI.Button(new Rect(Screen.width*0.68f, Screen.height*0.11f, Screen.width*0.235f, Screen.height*0.08f), "EXIT GAME")){
				Application.Quit();
			}
			
			break;
			#endregion
			
		#region LevelSelection Windows
		case _MenuWindowsEnum.LevelSelectionWindows:
		
			
			
			break;
			#endregion
			
		#region Option Windows
		case _MenuWindowsEnum.OptionWindows:
			
			Rect inMenuRect = new Rect(Screen.width*0.15f, Screen.height*0.23f, Screen.width*0.58f, Screen.height*0.55f);
			
			//BACKGROUND
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mainMenuButtton.optionBG);
			
			switch(_OptionWindows){
				#region MainOption
			case _OptionWindowsEnum.MainOption:
				
				Rect optionRect = new Rect(Screen.width*0.15f, Screen.height*0.23f, Screen.width*0.58f, Screen.height*0.55f);
				
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
				if(GUI.Button(new Rect(Screen.height*0.1f, Screen.height*0.84f, Screen.width*0.235f, Screen.height*0.11f), "- BACK -")){
					_MenuWindows = _MenuWindowsEnum.MainMenu;
				}
				
				break;
				#endregion
				
				#region SoundOption
			case _OptionWindowsEnum.Sound:
				Rect soundRect = new Rect(Screen.width*0.15f, Screen.height*0.15f, Screen.width*0.58f, Screen.height*0.55f);
				
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
				if(GUI.Button(new Rect(Screen.height*0.1f, Screen.height*0.84f, Screen.width*0.235f, Screen.height*0.11f), "- BACK -")){
					_OptionWindows = _OptionWindowsEnum.MainOption;
				}
				
				break;
				#endregion
				
				#region GameModeOption
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
				if(GUI.Button(new Rect(Screen.height*0.1f, Screen.height*0.84f, Screen.width*0.235f, Screen.height*0.11f), "- BACK -")){
					_OptionWindows = _OptionWindowsEnum.MainOption;
				}
				
				break;
				#endregion
				
				#region StatsOption
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
					GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.6f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "COMIC THUMBS");
					GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.7f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "ACHIEVEMENT");
				
					GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.8f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "TOTAL DEATH");
					GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.9f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "TOTAL PLAYTIME");
				
					//STATISTIC STRING
					GUI.TextArea(new Rect(inMenuRect.width*0.675f, inMenuRect.height*0.2f, inMenuRect.width*0.3f, inMenuRect.height*0.21f), _StatsDisplay.DeadNPCString);
					GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.3f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.whiteCollString);
					GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.4f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.redCollString);
					GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.5f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.blueCollString);
					GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.6f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.comicThumbsString);
					GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.7f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.achievementString);
					GUI.TextArea(new Rect(inMenuRect.width*0.675f, inMenuRect.height*0.8f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.DeathCountString);
					GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.9f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.totalPlaytimeString);
				GUI.EndGroup();
				
				//HIGHSCORE BUTTON
				GUI.skin.button.fontSize = 48;
				if(GUI.Button(new Rect(Screen.height*0.65f, Screen.height*0.84f, Screen.width*0.3f, Screen.height*0.11f), "- HIGHSCORES -")){
					
				}
				
				//BACK BUTTON
				GUI.skin.button.fontSize = 48;
				if(GUI.Button(new Rect(Screen.height*0.1f, Screen.height*0.84f, Screen.width*0.235f, Screen.height*0.11f), "- BACK -")){
					_OptionWindows = _OptionWindowsEnum.MainOption;
				}
				
				break;
				#endregion
				
				#region DeleteOption
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
				if(GUI.Button(new Rect(Screen.height*0.1f, Screen.height*0.84f, Screen.width*0.235f, Screen.height*0.11f), "- BACK -")){
					_OptionWindows = _OptionWindowsEnum.MainOption;
				}
				
				break;
				#endregion
			}
			
			break;
			#endregion

		#region Loading Screen
		case _MenuWindowsEnum.LoadingScreen:
			
			Rect inLoadRect = new Rect(Screen.width*0.17f, Screen.height*0.23f, Screen.width*0.58f, Screen.height*0.55f);
			
			//BACKGROUND
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mainMenuButtton.blackBG);
			
			//_LoadProgress = Application.GetStreamProgressForLevel(Application.loadedLevel + 2);
			
			GUI.BeginGroup(inLoadRect);
				GUI.skin.textArea.fontSize = 62;
				GUI.TextArea(new Rect(inLoadRect.width*0.41f, inLoadRect.height*0.25f,inLoadRect.width, inLoadRect.height*0.4f), "Loading");
				
			//TODO Gerer la Progress Bar differement selon WebPlayer/StandAlone
				//PROGRESS BAR
				//GUI.DrawTexture(new Rect(inLoadRect.width*0.25f, inLoadRect.height*0.5f,inLoadRect.width*0.5f, inLoadRect.height*0.2f), mainMenuButtton.emptyProgressBar);
				//GUI.DrawTexture(new Rect(inLoadRect.width*0.25f, inLoadRect.height*0.5f,inLoadRect.width*0.48f * _LoadProgress, inLoadRect.height*0.18f), mainMenuButtton.progressLine);
				
				if(_LoadCounter > 50){GUI.TextArea(new Rect(inLoadRect.width*0.605f, inLoadRect.height*0.25f,inLoadRect.width*0.2f, inLoadRect.height*0.3f), ".");}
				else if(_LoadCounter > 100){GUI.TextArea(new Rect(inLoadRect.width*0.625f, inLoadRect.height*0.25f,inLoadRect.width*0.2f, inLoadRect.height*0.3f), ".");}
				else if(_LoadCounter > 150){GUI.TextArea(new Rect(inLoadRect.width*0.645f, inLoadRect.height*0.25f,inLoadRect.width*0.2f, inLoadRect.height*0.3f), ".");}
			GUI.EndGroup();
			
			break;
			#endregion
			
		#region Credits Windows
		case _MenuWindowsEnum.CreditWindows:
			
			break;
			#endregion
			
			
		}
	}
	
	IEnumerator LoadLevel(){
		
		
		
		yield return _lvlLoad;
	}
}

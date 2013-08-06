using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	public enum _MenuWindowsEnum{
		MainMenu, SaveMenu, LevelSelectionWindows, CreditWindows, OptionWindows
	}
	
	public MainMenuHud mainMenuHud = new MainMenuHud();
	
	public _MenuWindowsEnum _MenuWindows;
	
	[System.Serializable]
	public class MainMenuHud{
		
		public Texture mainMenuBG;
		public Texture startNewGameButton;
		public Texture resumeButton;
		public Texture selectLevelButton;
		public Texture creditsButton;
		public Texture facebookButton;
		public Texture tweeterButton;
		public Texture buyOnSteamButton;
		
		public Rect _MainMenuBGRect;
		public Rect _StartButtonRect;
		public Rect _ResumeButtonRect;
		public Rect _SelectLevelButtonRect;
		public Rect _CreditButtonRect;
		public Rect _FacebookButtonRect;
		public Rect _TweeterButtonRect;
		public Rect _BuyOnSteamButtonRect;
		
		void Start(){
			_MainMenuBGRect = new Rect(0, 0, Screen.width, Screen.height);
			_StartButtonRect = new Rect(0, 0, Screen.width/2, Screen.height/2);
		}
		
	}
	
	
	
	public class LevelSelectionHud{
		
		
	}
	public class OptionMenuHud{
		
		
	}
	public class CreditsHud{
		
		
	}
	
	void Start () {
	
	}
	
	void Update () {
	
		
	}

	
	void OnGUI(){
			
		switch (_MenuWindows){
		case _MenuWindowsEnum.MainMenu:
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mainMenuHud.mainMenuBG);
			if(GUI.Button(new Rect(Screen.width/2.5f - 100f, Screen.height/2 - 100f, mainMenuHud.startNewGameButton.width, mainMenuHud.startNewGameButton.height), mainMenuHud.startNewGameButton, GUIStyle.none)){
				
			}
			break;
		case _MenuWindowsEnum.SaveMenu:
			
			break;
		case _MenuWindowsEnum.LevelSelectionWindows:
		
			break;
		case _MenuWindowsEnum.OptionWindows:
			
			break;
		case _MenuWindowsEnum.CreditWindows:
			
			break;
		}
	}
}

using UnityEngine;
using System.Collections;
using System.IO;

namespace ChromaStats{
	public class UnitySingleton : MonoBehaviour
	{
		//SINGLETON ACCESS
	    static UnitySingleton instance;
	 
		//SINGLETON CONSTRUCTOR
	    public static UnitySingleton Instance {
	        get {
	            if (instance == null) {
	                instance = FindObjectOfType (typeof(UnitySingleton)) as UnitySingleton;
	                if (instance == null) {
	                    GameObject obj = new GameObject ();
	                    obj.hideFlags = HideFlags.HideAndDontSave;
	                    instance = obj.AddComponent(typeof(UnitySingleton))as UnitySingleton;
	                }
	            }
	            return instance;
	        }
	    }
		
		//PUBLIC VARIABLES	
		public GUISkin tempGuiSkin;
		public bool SAVETEST;
		public bool FULLRELEASE = false;
		
		//PRIVATE VARIABLES
		private GameObject mainManager;
		private GameObject statsManager;
		
		
		
		//STATIC STATS VARIABLES
			
		public static int currentLevelUnlocked = 0;
		
		public static int redCollCollected = 0;
		public static int blueCollCollected = 0;
		public static int whiteCollCollected = 0;
		
		public static int totalComicViewed = 0;
		
		public static float playedTime = 0;
		
		
		public static bool doneOneTime = false;
		public static bool versionPirate = false;
		
		
		
		
		void Start(){
			CheckForLoad();	
		}	
		
		void Update(){
			
			if(Input.GetKeyDown(KeyCode.F12)){
				SAVETEST=!SAVETEST;
			}
			
			
		}
		
			//VERIFIE SI UNE SAVEGAME EXISTE, SI OUI, IL LOAD LE CONTENU ET INDIQUE AU MAINMENU
			//D'AFFICHER LE BOUTON "RESUME", SINON IL NE LOAD RIEN ET INDIQUE AU MAINMENU D'AFFICHER
			//LE BOUTON "START".
		void CheckForLoad(){
			
				//Check Si une Sauvegarde existe
						//Debug.Log(File.Exists(Application.persistentDataPath + "/" + "Chromasave"));
			if(!GameObject.FindGameObjectWithTag("MainManager")){
				mainManager = Instantiate(Resources.Load("pre_MainManager"))as GameObject;
								
				if(!File.Exists(Application.persistentDataPath + "/" + "Chromasave")){
					statsManager = Instantiate(Resources.Load("pre_StatsManager"))as GameObject;
					statsManager.transform.parent = mainManager.transform;
					HUDManager hudMan = mainManager.GetComponent<HUDManager>();
					hudMan.firstStart = true;
				}
				else{
					LevelSerializer.LoadObjectTreeFromFile("Chromasave");
					HUDManager hudMan = mainManager.GetComponent<HUDManager>();
					hudMan.firstStart = false;
				}
			}			
		}

		
		
		void OnGUI(){
			GUI.skin = tempGuiSkin;
			GUI.skin.button.fontSize = 30;
			
			if(SAVETEST){
				if(GUI.Button(new Rect(Screen.width*0.1f, Screen.height*0.2f, 120f, 80f), "SAVE")){
					LevelSerializer.SaveObjectTreeToFile("Chromasave", GameObject.FindGameObjectWithTag("StatsManager"));
				}
				if(GUI.Button(new Rect(Screen.width*0.3f, Screen.height*0.2f, 250f, 80f), "DELETE MANAGER")){
					Destroy(mainManager.gameObject);
				}
				if(GUI.Button(new Rect(Screen.width*0.1f, Screen.height*0.33f, 150f, 80f), "LOAD")){
					LevelSerializer.LoadObjectTreeFromFile("Chromasave");
					
				}
				if(GUI.Button(new Rect(Screen.width*0.3f, Screen.height*0.33f, 250f, 80f), "DELETE SAVE")){
					File.Delete(Application.persistentDataPath + "/" + "Chromasave");
					Application.LoadLevel(Application.loadedLevel);
				}
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using System.IO;

public class UnitySingleton : MonoBehaviour
{
    static UnitySingleton instance;
 
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
	
	
	public GUISkin tempGuiSkin;
	public bool SAVETEST;
	
	private GameObject mainManager;
	
	void Start(){
		CheckForLoad();	
	}	
	
	void Update(){
		
		if(Input.GetKeyDown(KeyCode.F12)){
			SAVETEST=!SAVETEST;
		}
		
		//Check Si une Sauvegarde existe
		//Debug.Log(File.Exists(Application.persistentDataPath + "/" + "Chromasave"));
	}
	
	
	void CheckForLoad(){
		MainMenu mainMenu = GameObject.FindObjectOfType(typeof(MainMenu))as MainMenu;
		if(!File.Exists(Application.persistentDataPath + "/" + "Chromasave")){
			mainManager = Instantiate(Resources.Load("pre_MainManager"))as GameObject;
			mainMenu.firstStart = true;
		}
		else{
			LevelSerializer.LoadObjectTreeFromFile("Chromasave");
			mainMenu.firstStart = false;
		}
	}
	
	
	
	
	void OnGUI(){
		GUI.skin = tempGuiSkin;
		GUI.skin.button.fontSize = 30;
		
		if(SAVETEST){
			if(GUI.Button(new Rect(Screen.width*0.1f, Screen.height*0.2f, 120f, 80f), "SAVE")){
				LevelSerializer.SaveObjectTreeToFile("Chromasave", mainManager);
			}
			if(GUI.Button(new Rect(Screen.width*0.3f, Screen.height*0.2f, 250f, 80f), "DELETE MANAGER")){
				Destroy(mainManager.gameObject);
			}
			if(GUI.Button(new Rect(Screen.width*0.1f, Screen.height*0.33f, 150f, 80f), "LOAD")){
				LevelSerializer.LoadObjectTreeFromFile("Chromasave");
				
			}
			if(GUI.Button(new Rect(Screen.width*0.3f, Screen.height*0.33f, 250f, 80f), "DELETE SAVE")){
				File.Delete(Application.persistentDataPath + "/" + "Chromasave");
			}
		}
	}
}

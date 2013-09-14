using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class DevManager : MainManager {
	
	public Texture devMenu;
	public GUISkin devButtonSkin, devSmallButtonSkin;

	private bool devMenuActive = false;
	
	void Start () {
	
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.F12)){
			devMenuActive = !devMenuActive;
		}
	}
	
	
	
	void OnGUI(){	
				
		if(devMenuActive){
			
			GUI.DrawTexture(new Rect(700f, 100f, 400f, 550f), devMenu);
			
				//BOUTON RESTART GAME & RESET SAVE
			GUI.skin = devButtonSkin;
			GUI.skin.button.fontSize = 30;
			if(GUI.Button(new Rect(750f, 190f, 300f, 50f), "RESET ALL SAVE")){
				File.Delete(Application.persistentDataPath + "/" + "Chromasave");
				LevelSerializer.SavedGames.Clear ();
				LevelSerializer.SaveDataToPlayerPrefs (); 
				EditorApplication.isPlaying = false;
			}
			
				//BOUTON SAVE
			GUI.skin = devSmallButtonSkin;
			GUI.skin.button.fontSize = 20;
			if(GUI.Button(new Rect(750f, 250f, 140f, 50f), "SAVE STATS")){
				LevelSerializer.SaveObjectTreeToFile("Chromasave", GameObject.FindGameObjectWithTag("StatsManager").gameObject);
			}
			
				//BOUTON LOAD
			GUI.skin = devSmallButtonSkin;
			GUI.skin.button.fontSize = 20;
			if(GUI.Button(new Rect(910f, 250f, 140f, 50f), "LOAD STATS")){
				LevelSerializer.LoadObjectTreeFromFile("Chromasave");
			}
			
				//BOUTON DELETE MANAGER -- Dont know Why, but... just in case
			GUI.skin = devButtonSkin;
			GUI.skin.button.fontSize = 30;
			if(GUI.Button(new Rect(750, 350, 300f, 50f), "DELETE MANAGER")){
				Destroy(this.transform);
			}
		
				//BOUTON SAVE ROOM
			GUI.skin = devSmallButtonSkin;
			GUI.skin.button.fontSize = 20;
			if(GUI.Button(new Rect(750, 410, 140f, 50f), "SAVE ROOM")){
				StatsManager.savedRoomInt = StatsManager.currentRoomInt;
				SaveRoom();
				LevelSerializer.SaveObjectTreeToFile("Chromasave", GameObject.FindGameObjectWithTag("StatsManager").gameObject);
				StatsManager.saveExist = true;
			}
			
				//BOUTON LOAD ROOM
			GUI.skin = devSmallButtonSkin;
			GUI.skin.button.fontSize = 20;
			
			if(LevelSerializer.CanResume){
				if(GUI.Button(new Rect(910f, 410f, 140f, 50f), "LOAD ROOM")){
					OptiManager.manager.OptimizeZone(StatsManager.savedRoomInt);
		         	LoadRoom();
		         	Time.timeScale = 1;
	        	} 
			}
			
			/*
			foreach(var sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName]) { 
	       		if(GUI.Button(new Rect(910f, 410f, 140f, 50f), "LOAD ROOM")){
					OptiManager.manager.OptimizeZone(StatsManager.savedRoomInt);
		         	LevelSerializer.LoadNow(sg.Data);
		         	Time.timeScale = 1;
	        	} 
    		} */
						
				//BOUTON UNLOCK NEXT LEVEL
			GUI.skin = devSmallButtonSkin;
			GUI.skin.button.fontSize = 16;
			if(GUI.Button(new Rect(750f, 570f, 140f, 50f), "UNLOCK NEXT LEVEL")){
				UnlockNextLevel();
			}
			
				//BOUTON LOCK LAST LEVEL
			GUI.skin = devSmallButtonSkin;
			GUI.skin.button.fontSize = 16;
			if(GUI.Button(new Rect(910f, 570f, 140f, 50f), "LOCK LAST LEVEL")){
				LockLastLevel();
			}
		}
	}
}

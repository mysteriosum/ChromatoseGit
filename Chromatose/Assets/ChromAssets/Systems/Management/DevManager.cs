using UnityEngine;
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
	/*
	void UnlockNextLevel(){
		foreach(bool unlckBool in StatsManager.levelUnlocked){
			if()
		}
	}*/
	
	
	void OnGUI(){	
				
		if(devMenuActive){
			
			GUI.DrawTexture(new Rect(700f, 100f, 400f, 550f), devMenu);
			
				//BOUTON RESTART GAME & RESET SAVE
			GUI.skin = devButtonSkin;
			GUI.skin.button.fontSize = 30;
			if(GUI.Button(new Rect(750f, 190f, 300f, 50f), "RESET GAME & SAVE")){
				
			}
			
				//BOUTON SAVE
			GUI.skin = devSmallButtonSkin;
			GUI.skin.button.fontSize = 30;
			if(GUI.Button(new Rect(750f, 250f, 140f, 50f), "SAVE")){
				LevelSerializer.SaveObjectTreeToFile("Chromasave", GameObject.FindGameObjectWithTag("StatsManager"));
			}
			
				//BOUTON LOAD
			GUI.skin = devSmallButtonSkin;
			GUI.skin.button.fontSize = 30;
			if(GUI.Button(new Rect(910f, 250f, 140f, 50f), "LOAD")){
				LevelSerializer.LoadObjectTreeFromFile("Chromasave");
			}
			
				//BOUTON DELETE MANAGER -- Dont know Why, but... just in case
			GUI.skin = devButtonSkin;
			GUI.skin.button.fontSize = 30;
			if(GUI.Button(new Rect(750, 350, 300f, 50f), "DELETE MANAGER")){
				Destroy(this.transform);
			}
		
				//BOUTON DELETE SAVE
			GUI.skin = devButtonSkin;
			GUI.skin.button.fontSize = 30;
			if(GUI.Button(new Rect(750, 410, 300f, 50f), "DELETE SAVE")){
				File.Delete(Application.persistentDataPath + "/" + "Chromasave");
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}
}

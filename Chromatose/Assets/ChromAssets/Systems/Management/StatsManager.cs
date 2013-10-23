using UnityEngine;
using System.Collections;

[System.Serializable]
public class StatsManager : MainManager {
	
	public static StatsManager manager;
	
	public static int currentRoomInt;
	public static int savedRoomInt;
	public static int currentLevelUnlocked = 0;
	public static bool saveExist;
	
	public static bool[] levelUnlocked;
	public static bool[] levelDoned;
	public static int[] redCollCollected;
	public static int[] blueCollCollected;
	public static int[] whiteCollCollected;
	public static int[] comicThumbCollected;
	
	public static int redCollDisplayed = 0;
	public static int blueCollDisplayed = 0;
	public static int whiteCollDisplayed = 0;
	public static int comicThumbDisplayed = 0;
	public static int deathCounter = 0;
	public static int killedNpc = 0;
	public static int totalComicViewed = 0;
	public static float playedTime = 0;
	
	
	public static bool doneOneTime;
	public static bool versionPirate;
	public static bool newManager;
	public static bool newLevel;
	
	public static bool spaceBarActive;
	public static bool alreadyTakeSpace;
	public static float musicVolume = 0.95f;
	public static float sfxVolume = 0.95f;
	public static bool keyboardAlreadyChoose = false;
	
	
	void Awake(){
		if(LevelSerializer.IsDeserializing) return;
		manager = this;		
		DontDestroyOnLoad(this.gameObject);
	}
	
	void OnLevelWasLoaded(){
		ResetLevelColl();
	}
	
	void Start () {
		
		bool needInit = CheckIfInitializeStats();
		
		if(needInit){
			redCollCollected = new int[14]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
			blueCollCollected = new int[14]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
			whiteCollCollected = new int[14]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
			comicThumbCollected = new int[14]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
		
			
		levelUnlocked = new bool[13]{true, false, false, false, false,
										false, false, false, false, false,
										false, false, true};
		
		levelDoned = new bool[13]{false, false, false, false, false,
										false, false, false, false, false,
										false, false, false};		
		}
	}
	
	bool CheckIfInitializeStats(){
		if(redCollCollected != null){
			return false;
		}
		else if(blueCollCollected != null){
			return false;
		}
		else if(whiteCollCollected != null){
			return false;
		}
		else if(comicThumbCollected != null){
			return false;
		}
		else{
			return true;
		}
	}
	
	void ResetLevelColl(){
		int lvl = Application.loadedLevel;
		redCollCollected[lvl] = 0;
		blueCollCollected[lvl] = 0;
		whiteCollCollected[lvl] = 0;
		comicThumbCollected[lvl] = 0;
		ReCalculateStats();
	}	
	public void ReCalculateStats(){
		redCollDisplayed = 0;
		foreach(int rCol in redCollCollected){
			redCollDisplayed += rCol;
		}
		blueCollDisplayed = 0;
		foreach(int bCol in blueCollCollected){
			blueCollDisplayed += bCol;
		}
		whiteCollDisplayed = 0;
		foreach(int wCol in whiteCollCollected){
			whiteCollDisplayed += wCol;
		}
		comicThumbDisplayed = 0;
		foreach(int cThumb in comicThumbCollected){
			comicThumbDisplayed += cThumb;
		}
		
		if(redCollCollected[currentLevel] < 0){
			redCollCollected[currentLevel] = 0;
		}
		if(blueCollCollected[currentLevel] < 0){
			blueCollCollected[currentLevel] = 0;
		}
		if(whiteCollCollected[currentLevel] < 0){
			whiteCollCollected[currentLevel] = 0;
		}
		if(comicThumbDisplayed < 0){
			comicThumbDisplayed = 0;
		}
	}
	
	/*
	public void ReCalculateStats(Color colorColl, int amount, bool comicIs){
		redCollDisplayed = 0;
		foreach(int rCol in redCollCollected){
			redCollDisplayed += rCol;
		}
		blueCollDisplayed = 0;
		foreach(int bCol in blueCollCollected){
			blueCollDisplayed += bCol;
		}
		whiteCollDisplayed = 0;
		foreach(int wCol in whiteCollCollected){
			whiteCollDisplayed += wCol;
		}
		comicThumbDisplayed = 0;
		foreach(int cThumb in comicThumbCollected){
			comicThumbDisplayed += cThumb;
		}
		if(!comicIs){
			if(colorColl == Color.red){
				redCollDisplayed -= amount;
			}
			if(colorColl == Color.blue){
				blueCollDisplayed -= amount;
			}
			if(colorColl == Color.white){
				whiteCollDisplayed -= amount;
			}
		}
		else{
			comicThumbDisplayed -= amount;
		}
	}	*/
}

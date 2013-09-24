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
	public static int redCollCollected = 0;
	public static int blueCollCollected = 0;
	public static int whiteCollCollected;
	public static int comicThumbCollected = 0;
	
	public static int redCollDisplayed = 0;
	public static int blueCollDisplayed = 0;
	public static int whiteCollDisplayed;
	public static int comicThumbDisplayed = 0;
	
	public static int totalComicViewed = 0;
	public static float playedTime = 0;
	
	public static bool doneOneTime;
	public static bool versionPirate;
	public static bool newManager;
	public static bool newLevel;
	
	void Awake(){
		if(LevelSerializer.IsDeserializing) return;
		manager = this;		
		DontDestroyOnLoad(this.gameObject);
	}
	
	void Start () {
		levelUnlocked = new bool[13]{true, false, false, false, false,
										false, false, false, false, false,
										false, false, true};
		levelDoned = new bool[13]{false, false, false, false, false,
										false, false, false, false, false,
										false, false, false};
	}
}

using UnityEngine;
using System.Collections;

[System.Serializable]
public class StatsManager : MainManager {
	
	public static StatsManager manager;
	
	public static int currentLevelUnlocked = 0;
	public static Vector3 lastSpawnPos;
	
	public static bool[] levelUnlocked = new bool[12]{true, false, false, false, false,
														false, false, false, false, false,
														false, true};
	
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
	
	public static bool doneOneTime = false;
	public static bool versionPirate = false;
	public static bool newManager;
	public static bool newLevel;
	
	void Awake(){
		manager = this;		
	}
	
	void Start () {
		this.transform.parent = GameObject.FindGameObjectWithTag("MainManager").transform;
	}
}

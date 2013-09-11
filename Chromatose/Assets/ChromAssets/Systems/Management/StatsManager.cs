using UnityEngine;
using System.Collections;

[System.Serializable]
public class StatsManager : MainManager {
	
	public static StatsManager manager;
	
	public static int currentLevelUnlocked = 0;
	
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
	
	void Awake(){
		manager = this;		
	}
	
	void Start () {
		this.transform.parent = GameObject.FindGameObjectWithTag("MainManager").transform;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(whiteCollCollected);
	}
}

using UnityEngine;
using System.Collections;

[System.Serializable]
public class ChromaRoomManager : MainManager {
	
	public static ChromaRoomManager roomManager;
		
	public int[] comicInModBlanc1;
	public int[] comicInModBlanc2;
	public int[] comicInModBlanc3;
	public int[] comicInModBlanc4;
	
	private bool _CanAddRoom = false;
	
	void OnLevelWasLoaded(){
		currentRoomString = "room00";
		StatsManager.currentRoomInt = 0;
	}
	void Start () {
		roomManager = this;
		
		_CanAddRoom = true;
		currentRoomString = "room00";
		StatsManager.currentRoomInt = 0;
		
	}
	
	void Update () {
	}
	
	public void NextLilRoom(){
		if(_CanAddRoom){
			StatsManager.currentRoomInt++;
			currentRoomString = OptiManager.manager.roomList[StatsManager.currentRoomInt].name;
			_CanAddRoom = false;
			StartCoroutine(DelaiToAddRoom(2.0f));
			OptiManager.manager.OptimizeZone();
		}
	}
	
	int FindComicBySearch(){
		GameObject[] comicTempList = GameObject.FindGameObjectsWithTag("comicThumb");
		int totalTemp = comicTempList.Length;
		return totalTemp;
	}
	
	public int UpdateTotalComic(){
		
		int retTotalComic = 0;
		int curLevel = currentLevel;
		
		if(curLevel != 1 && curLevel != 3 && curLevel != 5 && curLevel != 7){
			
			retTotalComic = FindComicBySearch();
						
		}	
		else{
			switch(curLevel){
			case 1:
				retTotalComic = comicInModBlanc1[StatsManager.currentRoomInt];
				break;
			case 3:
				retTotalComic = comicInModBlanc2[StatsManager.currentRoomInt];
				break;
			case 5:
				retTotalComic = comicInModBlanc3[StatsManager.currentRoomInt];
				break;
			case 7:
				retTotalComic = comicInModBlanc4[StatsManager.currentRoomInt];
				break;
			}
		}
		
		if(retTotalComic == 0){return 0;}
		
		return retTotalComic;
	}	
	
	IEnumerator DelaiToAddRoom(float delai){
		yield return new WaitForSeconds(delai);
		_CanAddRoom = true;
	}
	IEnumerator CheckPlace(){
		yield return new WaitForSeconds(0.01f);
		ChromatoseManager.manager.NewCheckpoint(_Avatar.transform);
		Debug.Log("PlaceTaken");
	}
}

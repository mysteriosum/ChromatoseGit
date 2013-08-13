using UnityEngine;
using System.Collections;

[System.Serializable]
public class ChromaRoomManager : MonoBehaviour {
	
	public static ChromaRoomManager roomManager;
	
	public enum _RoomTypeEnum{
		WhiteRoom, BlueOrRedRoom, FinalBoss
	}

	public _RoomTypeEnum _RoomType;
		
	public int[] comicInLevel;

	private int _CurRoom = 0;
		public int curRoom{
			get{return _CurRoom;}
		}

	
	// Use this for initialization
	void Start () {
		_CurRoom = 0;
		
	}
	
	void Update () {
		Debug.Log(_CurRoom);
	}
	
	public void NextLilRoom(){
		_CurRoom++;
	}
	
	int FindComicBySearch(){
		
		GameObject[] comicTempList = GameObject.FindGameObjectsWithTag("comicThumb");
		int totalTemp = comicTempList.Length;
		return totalTemp;
	}
	
	public int UpdateTotalComic(){
		
		int retTotalComic = 0;
		
		if(_RoomType != _RoomTypeEnum.WhiteRoom){
			
			retTotalComic = FindComicBySearch();
						
		}	
		else{
			
			retTotalComic = comicInLevel[_CurRoom];
			
		}
		
		return retTotalComic;
	}	
}

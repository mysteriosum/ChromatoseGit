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
	
	private bool _CanAddRoom;
	private int _CurRoom = 0;
		public int curRoom{
			get{return _CurRoom;}
		}

	
	// Use this for initialization
	void Start () {
		_CanAddRoom = true;
		_CurRoom = 0;
		
	}
	
	void Update () {
	}
	
	public void NextLilRoom(){
		if(_CanAddRoom){
			_CurRoom++;
			_CanAddRoom = false;
			StartCoroutine(DelaiToAddRoom(5.0f));
		}
		
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
		
		if(retTotalComic == 0){return 0;}
		
		return retTotalComic;
	}	
	
	IEnumerator DelaiToAddRoom(float delai){
		yield return new WaitForSeconds(delai);
		_CanAddRoom = true;
	}
	
}

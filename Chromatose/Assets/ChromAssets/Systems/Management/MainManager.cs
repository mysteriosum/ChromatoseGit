using UnityEngine;
using System.Collections;

[SerializeAll]
public class MainManager : MonoBehaviour {
	
	
	public enum _WhiteRoomEnum{
		Tuto, ModuleBlanc_2, ModuleBlanc_3, ModuleBlanc_4, None
	}
	public enum _RoomTypeEnum{
		Menu, WhiteRoom, RedRoom, BlueRoom, RedAndBlueRoom, FinalBoss, None
	}
	
	private _WhiteRoomEnum _WhiteRoom;
	public _WhiteRoomEnum whiteRoom { get { return _WhiteRoom; } }
	
	private _RoomTypeEnum _RoomType;
	public _RoomTypeEnum roomType { get { return _RoomType; } }
		
	private ChromatoseManager _Manager;
	private GameObject _Avatar;
	private Avatar _AvatarScript;
	private ChromatoseCamera _Cam;
	
	
	
	//PRIVATE VARIABLES -- INGAME USE
	private static int currentLevel = 0;
	
	
	//VARIABLE STATIC STATS		
	public static int currentLevelUnlocked = 0;
	
	public static int redCollCollected = 0;
	public static int blueCollCollected = 0;
	public static int whiteCollCollected = 0;
	
	public static int totalComicViewed = 0;
	
	public static float playedTime = 0;
	
	public static bool doneOneTime = false;
	public static bool versionPirate = false;
	
	
	
	//VARIABLE HIGHSCORE
	
	
	
	//ACCESSEUR GET/SET	
		
	void Awake(){
		DontDestroyOnLoad(transform.gameObject);
	}
	
	void Start () {

	}
	
	void Update () {
		
	}
	
	//PUBLIC FUNCTION
	public void CheckWhereIAm(){
		int curLevel = Application.loadedLevel;
		
		switch(curLevel){
		case 0:
			_RoomType = _RoomTypeEnum.Menu;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 0;
			break;
		case 1:
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.Tuto;
			currentLevel = 1;
			break;
		case 2:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 2;
			break;
		case 3:
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.ModuleBlanc_2;
			currentLevel = 3;
			break;
		case 4:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 4;
			break;
		case 5:
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.ModuleBlanc_3;
			currentLevel = 5;
			break;
		case 6:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 6;
			break;
		case 7:
			_RoomType = _RoomTypeEnum.WhiteRoom;
			_WhiteRoom = _WhiteRoomEnum.ModuleBlanc_4;
			currentLevel = 7;
			break;
		case 8:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 8;
			break;
		case 9:
			_RoomType = _RoomTypeEnum.RedRoom;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 9;
			break;
		case 10:
			_RoomType = _RoomTypeEnum.FinalBoss;
			_WhiteRoom = _WhiteRoomEnum.None;
			currentLevel = 10;
			break;
		}
	}
}

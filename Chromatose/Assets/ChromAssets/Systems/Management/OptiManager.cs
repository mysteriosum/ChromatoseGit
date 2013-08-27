using UnityEngine;
using System.Collections;

public class OptiManager : MonoBehaviour {
	
	[SerializeField]
	public GameObject[] roomList;
	
	private int _CurRoom = 0;
		public int curRoom{
			get{return _CurRoom;}
		}
	private int _RoomToDisplay = 0;
		public int roomToDisplay{
			get{return _RoomToDisplay;}
			set{_RoomToDisplay = value;}
		}
	
	private ChromaRoomManager _RoomManager;
	private RoomInstancier _RoomSaver;
	// Use this for initialization
	void Start () {
		_RoomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<ChromaRoomManager>();
		_RoomSaver = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<RoomInstancier>();
		StartCoroutine(DelaiBeforeStartOpti());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OptimizeZone(){
		
		_CurRoom = _RoomManager.curRoom;

		foreach(GameObject go in roomList){
			if(go != null){
				go.SetActive(false);
			}
		}
		roomList[_CurRoom].SetActive(true);
		_RoomSaver.SaveRoom();
	}
	
	public void OptimizeZone(int roomNumber){
		_CurRoom = roomNumber;
		foreach(GameObject go in roomList){
			if(go != null){
				go.SetActive(false);
			}
		}
		roomList[_CurRoom].SetActive(true);
		_RoomSaver.SaveRoom();
	}
	
	IEnumerator DelaiToDesactive(){
		yield return new WaitForSeconds(5.0f);
		foreach(GameObject go in roomList){
			if(go != null){
				go.SetActive(false);
			}
		}
	}
	IEnumerator DelaiBeforeStartOpti(){
		yield return new WaitForSeconds(0.5f);
		OptimizeZone();
	}
	
}

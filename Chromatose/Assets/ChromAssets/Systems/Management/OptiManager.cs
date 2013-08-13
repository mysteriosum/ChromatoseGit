using UnityEngine;
using System.Collections;

public class OptiManager : MonoBehaviour {
	
	
	public GameObject[] roomList;
	
	private int _CurRoom = 0;
	private ChromaRoomManager _RoomManager;
	// Use this for initialization
	void Start () {
		_RoomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<ChromaRoomManager>();
		
		OptimizeZone();
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
	}
}

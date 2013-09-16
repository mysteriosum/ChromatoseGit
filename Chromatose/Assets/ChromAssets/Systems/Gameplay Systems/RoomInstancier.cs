using UnityEngine;
using System.Collections;

public class RoomInstancier : MainManager {
	
	public static RoomInstancier instancier;
	
	public GameObject[] RoomPrefab_LVL_1, RoomPrefab_LVL_2,
						RoomPrefab_LVL_3, RoomPrefab_LVL_4,
						RoomPrefab_LVL_5, RoomPrefab_LVL_6,
						RoomPrefab_LVL_7, RoomPrefab_LVL_8,
						RoomPrefab_LVL_9, RoomPrefab_LVL_10,
						RoomPrefab_BossFinal, RoomPrefab_Gym;
	
	
	private GameObject roomSaved;
	
	private MovingBlob[] movingBlob;
	
	private Vector3 roomPos;
	private Vector3 realPos;
	private Quaternion roomRot;
	private Quaternion realRot;
	
	
	void Awake(){
		instancier = this;
	}
	void Start () {
		
	}
	
	void Update(){}
	
	
		
	public void SaveRoom(){
				
		if(_RoomManager != null){
			roomPos = GameObject.Find(currentRoomString).transform.position;
			roomRot = GameObject.Find(currentRoomString).transform.rotation;
			Debug.Log(currentRoomString + " was Saved");
		}
	}
	
	//POUR DEV OU CALL EXTERNE
	public void SaveRoom(GameObject roomToSave){
		
		
		
		if(_RoomManager != null){
			roomPos = GameObject.Find(currentRoomString).transform.position;
			roomRot = GameObject.Find(currentRoomString).transform.rotation;
			Debug.Log("New Room was Saved");
		}
	}
	
	public void LoadRoom(){
		if(_RoomManager != null){
			GameObject newRoom = null;
		switch(currentLevel){
			case 1:					
				Destroy(GameObject.Find(currentRoomString));
				newRoom = Instantiate(RoomPrefab_LVL_1[StatsManager.currentRoomInt], roomPos, roomRot)as GameObject;
				newRoom.name = currentRoomString;
				SaveRoom(newRoom);
				Debug.Log(currentRoomString + " was Loaded");
				break;
			case 2:					
				Destroy(GameObject.Find(currentRoomString));
				newRoom = Instantiate(RoomPrefab_LVL_2[StatsManager.currentRoomInt], roomPos, roomRot)as GameObject;
				newRoom.name = currentRoomString;
				SaveRoom(newRoom);
				Debug.Log(currentRoomString + " was Loaded");
				break;
			case 3:					
				Destroy(GameObject.Find(currentRoomString));
				newRoom = Instantiate(RoomPrefab_LVL_3[StatsManager.currentRoomInt], roomPos, roomRot)as GameObject;
				newRoom.name = currentRoomString;
				SaveRoom(newRoom);
				Debug.Log(currentRoomString + " was Loaded");
				break;
			case 4:					
				Destroy(GameObject.Find(currentRoomString));
				newRoom = Instantiate(RoomPrefab_LVL_4[StatsManager.currentRoomInt], roomPos, roomRot)as GameObject;
				newRoom.name = currentRoomString;
				SaveRoom(newRoom);
				Debug.Log(currentRoomString + " was Loaded");
				break;
			case 5:					
				Destroy(GameObject.Find(currentRoomString));
				newRoom = Instantiate(RoomPrefab_LVL_5[StatsManager.currentRoomInt], roomPos, roomRot)as GameObject;
				newRoom.name = currentRoomString;
				SaveRoom(newRoom);
				Debug.Log(currentRoomString + " was Loaded");
				break;
			case 6:					
				Destroy(GameObject.Find(currentRoomString));
				newRoom = Instantiate(RoomPrefab_LVL_6[StatsManager.currentRoomInt], roomPos, roomRot)as GameObject;
				newRoom.name = currentRoomString;
				SaveRoom(newRoom);
				Debug.Log(currentRoomString + " was Loaded");
				break;
			case 7:					
				Destroy(GameObject.Find(currentRoomString));
				newRoom = Instantiate(RoomPrefab_LVL_7[StatsManager.currentRoomInt], roomPos, roomRot)as GameObject;
				newRoom.name = currentRoomString;
				SaveRoom(newRoom);
				Debug.Log(currentRoomString + " was Loaded");
				break;
			case 8:					
				Destroy(GameObject.Find(currentRoomString));
				newRoom = Instantiate(RoomPrefab_LVL_8[StatsManager.currentRoomInt], roomPos, roomRot)as GameObject;
				newRoom.name = currentRoomString;
				SaveRoom(newRoom);
				Debug.Log(currentRoomString + " was Loaded");
				break;
			case 9:					
				Destroy(GameObject.Find(currentRoomString));
				newRoom = Instantiate(RoomPrefab_LVL_9[StatsManager.currentRoomInt], roomPos, roomRot)as GameObject;
				newRoom.name = currentRoomString;
				SaveRoom(newRoom);
				Debug.Log(currentRoomString + " was Loaded");
				break;
			case 10:					
				Destroy(GameObject.Find(currentRoomString));
				newRoom = Instantiate(RoomPrefab_LVL_10[StatsManager.currentRoomInt], roomPos, roomRot)as GameObject;
				newRoom.name = currentRoomString;
				SaveRoom(newRoom);
				Debug.Log(currentRoomString + " was Loaded");
				break;
			case 11:
				Destroy(GameObject.Find(currentRoomString));
				newRoom = Instantiate(RoomPrefab_BossFinal[StatsManager.currentRoomInt], roomPos, roomRot)as GameObject;
				newRoom.name = currentRoomString;
				SaveRoom(newRoom);
				Debug.Log(currentRoomString + " was Loaded");
				break;
			case 12:
				Destroy(GameObject.Find(currentRoomString));
				newRoom = Instantiate(RoomPrefab_Gym[StatsManager.currentRoomInt], roomPos, roomRot)as GameObject;
				newRoom.name = currentRoomString;
				SaveRoom(newRoom);
				Debug.Log(currentRoomString + " was Loaded");
				break;
			}
		}
	}
}

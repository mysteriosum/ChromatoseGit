using UnityEngine;
using System.Collections;

public class RoomInstancier : MonoBehaviour {
	
	
	public GameObject[] RoomPrefab;
	
	private OptiManager optiManager;
	
	private GameObject roomSaved;
	
	private MovingBlob[] movingBlob;
	
	private Vector3 roomPos;
	private Vector3 realPos;
	private Quaternion roomRot;
	private Quaternion realRot;
	
	
	// Use this for initialization
	void Start () {
		if(GameObject.FindGameObjectWithTag("OptiManager").GetComponent<OptiManager>() != null){
			optiManager = GameObject.FindGameObjectWithTag("OptiManager").GetComponent<OptiManager>();
		}
	}
	
	void Update(){}
		
	public void SaveRoom(){
		if(optiManager != null){
			roomPos = GameObject.Find("room" + optiManager.curRoom).transform.position;
			roomRot = GameObject.Find("room" + optiManager.curRoom).transform.rotation;
			Debug.Log("room" + optiManager.curRoom + " was Saved");
		}
	}
	
	public void SaveRoom(GameObject roomToSave){
		if(optiManager != null){
			roomPos = GameObject.Find("room" + optiManager.curRoom).transform.position;
			roomRot = GameObject.Find("room" + optiManager.curRoom).transform.rotation;
			Debug.Log("New Room was Saved");
		}
	}
	
	public void LoadRoom(){
		if(optiManager != null){
			Destroy(GameObject.Find("room" + optiManager.curRoom));
			GameObject newRoom = Instantiate(RoomPrefab[optiManager.curRoom], roomPos, roomRot)as GameObject;
			newRoom.name = "room" + optiManager.curRoom.ToString();
			SaveRoom(newRoom);
			Debug.Log("room" + optiManager.curRoom + " was Loaded");
		}
	}
}

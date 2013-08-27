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
		optiManager = GameObject.FindGameObjectWithTag("OptiManager").GetComponent<OptiManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public void SaveRoom(){
		roomPos = GameObject.Find("room" + optiManager.curRoom).transform.position;
		roomRot = GameObject.Find("room" + optiManager.curRoom).transform.rotation;
		Debug.Log("room" + optiManager.curRoom + " was Saved");
	}
	
	public void SaveRoom(GameObject roomToSave){
		roomPos = GameObject.Find("room" + optiManager.curRoom).transform.position;
		roomRot = GameObject.Find("room" + optiManager.curRoom).transform.rotation;
		Debug.Log("New Room was Saved");
	}
	
	public void LoadRoom(){
		Destroy(GameObject.Find("room" + optiManager.curRoom));
		GameObject newRoom = Instantiate(RoomPrefab[optiManager.curRoom], roomPos, roomRot)as GameObject;
		newRoom.name = "room" + optiManager.curRoom.ToString();
		SaveRoom(newRoom);
		Debug.Log("room" + optiManager.curRoom + " was Loaded");
	}
}

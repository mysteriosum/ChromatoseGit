using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;


public class OptiManager : MainManager {
	
	[SerializeField]
	public GameObject[] roomList;

	
	void OnLevelWasLoaded(){
		StartCoroutine(DelaiBeforeStartOpti());
		FindAllRoom();
	}
	void Start () {
		//DontDestroyOnLoad(this);
		StartCoroutine(DelaiBeforeStartOpti());
		FindAllRoom();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void FindAllRoom(){
		roomList = GameObject.FindGameObjectsWithTag("room").OrderBy( go => go.name).ToArray();
		//Debug.Log(roomList[0].name);
	}

	public void OptimizeZone(){

		if(currentLevel == 1 || currentLevel == 3 || currentLevel == 5 || currentLevel == 7){
			foreach(GameObject go in roomList){
				if(go != null){
					go.SetActive(false);
				}
			}
			roomList[currentRoomInt].SetActive(true);
			Invoke("SaveRoom", 0.5f);
		}
		else{
			Debug.LogWarning("TU NE PEUX PAS OPTIMISER CE NIVEAU");
		}		
	}
	
	public void OptimizeZone(int roomNumber){
		int _CurRoom = roomNumber;
		
		if(currentLevel == 1 || currentLevel == 3 || currentLevel == 5 || currentLevel == 7){
			foreach(GameObject go in roomList){
				if(go != null){
					go.SetActive(false);
				}
			}
			roomList[_CurRoom].SetActive(true);
			Invoke("SaveRoom", 0.5f);
		}
		else{
			Debug.LogWarning("TU NE PEUX PAS OPTIMISER CE NIVEAU");
		}

	}
	
	void SaveRoom(){
		RoomInstancier.instancier.SaveRoom();
	}
	
	IEnumerator DelaiBeforeStartOpti(){
		yield return new WaitForSeconds(0.5f);
		OptimizeZone();
	}
	
}

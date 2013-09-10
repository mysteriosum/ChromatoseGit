using UnityEngine;
using System.Collections;

public class SpaceBarActivation : MainManager {
	
	private Avatar _AvatarScript;
	
	
	void OnTriggerEnter(Collider other){
		if(other.tag != "avatar") return;
		if(_AvatarScript != null){
			Debug.Log("IN");
			_AvatarScript.spaceBarActive = true;
		}
	}
}

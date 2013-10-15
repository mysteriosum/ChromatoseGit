using UnityEngine;
using System.Collections;

public class SpaceBarActivation : MainManager {
	
	private Avatar _AvatarScript;
	
	
	void OnTriggerEnter(Collider other){
		if(other.tag != "avatar") return;
		if(_AvatarScript != null){
			StatsManager.spaceBarActive = true;
		}
	}
}

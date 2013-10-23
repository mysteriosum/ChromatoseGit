using UnityEngine;
using System.Collections;

public class SpaceBarActivation : MonoBehaviour {
	
	void OnTriggerEnter(Collider other){
		if(other.tag != "avatar") return;
		StatsManager.spaceBarActive = true;
		StatsManager.alreadyTakeSpace = true;
	}
}

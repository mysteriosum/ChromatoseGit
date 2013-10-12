using UnityEngine;
using System.Collections;

public class LaserDeathTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if(other.tag != "avatar")return;
		if(other.GetComponent<Avatar>().curColor == Color.blue)return;
		
		ChromatoseManager.manager.Death();
	}
	
	void OnTriggerStay(Collider other){
		if(other.tag != "avatar")return;
		if(other.GetComponent<Avatar>().curColor == Color.blue)return;
		
		ChromatoseManager.manager.Death();
	}
}

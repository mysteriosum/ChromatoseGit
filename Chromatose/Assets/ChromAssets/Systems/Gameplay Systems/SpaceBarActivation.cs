using UnityEngine;
using System.Collections;

public class SpaceBarActivation : MonoBehaviour {
	
	private Avatar _AvatarScript;
	
	void Start(){
		_AvatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
	}	
	
	void OnTriggerEnter(Collider other){
		
		if(other.tag != "avatar") return;
		if(_AvatarScript != null){
			Debug.Log("IN");
			_AvatarScript.spaceBarActive = true;
		}
	}
}

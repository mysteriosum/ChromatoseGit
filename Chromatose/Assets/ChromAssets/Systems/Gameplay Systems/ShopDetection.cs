using UnityEngine;
using System.Collections;

public class ShopDetection : MonoBehaviour {

	private bool _OnDetectZone;
	public bool onDetectZone { get { return _OnDetectZone; } }
	
	void OnTriggerEnter(Collider other){
		if(other.tag != "avatar")return;
		
		_OnDetectZone = true;
	}
	
	void OnTriggerStay(Collider other){
		if(other.tag != "avatar")return;
		
		_OnDetectZone = true;
	}
	
	void OnTriggerExit(Collider other){
		if(other.tag != "avatar")return;
		
		_OnDetectZone = false;
	}
}

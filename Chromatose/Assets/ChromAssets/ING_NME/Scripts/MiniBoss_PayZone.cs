using UnityEngine;
using System.Collections;

public class MiniBoss_PayZone : MonoBehaviour {
	
	
	private bool _InPayZone = false;
		public bool inPayZone{
			get{return _InPayZone;}
			set{_InPayZone = value;}
		}

	void OnTriggerEnter(Collider other){
		if(other.tag != "avatar")return;
			_InPayZone = true;
	}
	void OnTriggerStay(Collider other){
		if(other.tag != "avatar")return;
		if(!_InPayZone){
			_InPayZone = true;
		}
	}
	void OnTriggerExit(Collider other){
		if(other.tag != "avatar")return;
			_InPayZone = false;
	}
	
}

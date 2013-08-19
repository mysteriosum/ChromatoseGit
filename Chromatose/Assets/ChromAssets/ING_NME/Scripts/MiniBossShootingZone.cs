using UnityEngine;
using System.Collections;

public class MiniBossShootingZone : MonoBehaviour {
	
	private bool _InShootingZone = false;
		public bool inShootingZone{
			get{return _InShootingZone;}
			set{_InShootingZone = value;}
		}
	private SphereCollider _MyZone;
	
	void Start(){
		_MyZone = GetComponent<SphereCollider>();
	}
	
	void Update(){
		_MyZone.radius = transform.parent.gameObject.GetComponent<MiniBoss>().shootingArea;
	}
		
	void OnTriggerEnter(Collider other){
		if(other.tag != "avatar")return;
		_InShootingZone = true;
	}
	
	void OnTriggerStay(Collider other){
		if(other.tag != "avatar")return;
		if(!_InShootingZone){
			_InShootingZone = true;
		}
	}
	
	void OnTriggerExit(Collider other){
		if(other.tag != "avatar")return;
		_InShootingZone = false;
	}

}

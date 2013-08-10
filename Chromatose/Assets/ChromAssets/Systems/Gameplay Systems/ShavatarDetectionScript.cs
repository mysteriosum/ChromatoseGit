using UnityEngine;
using System.Collections;

public class ShavatarDetectionScript : MonoBehaviour {
	
	private int _DetectionZone = 300;
	public int DetectionZone{
		get{return _DetectionZone;}
		set{_DetectionZone = value;}
		}
	private bool _AvatarDetected;
	public bool AvatarDetected{
		get{return _AvatarDetected;}
		}

#region Init & Update
	void Start () {
		gameObject.GetComponent<SphereCollider>().radius = _DetectionZone;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(gameObject.GetComponent<SphereCollider>().radius !=  _DetectionZone){
			gameObject.GetComponent<SphereCollider>().radius = _DetectionZone;
		}
	}
#endregion
	
#region Triggering
	void OnTriggerEnter(Collider other){
		if(!_AvatarDetected){
			if(other.tag == "avatar"){
				_AvatarDetected = true;
			}
		}
	}
	void OnTriggerStay(Collider other){
		if(!_AvatarDetected){
			if(other.tag == "avatar"){
				_AvatarDetected = true;
			}
		}
	}
	void OnTriggerExit(Collider other){
		if(_AvatarDetected){
			if(other.tag == "avatar"){
				_AvatarDetected = false;
			}
		}
	}
#endregion
}

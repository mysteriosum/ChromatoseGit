using UnityEngine;
using System.Collections;

public class MovingBlob_DetectionZone : MonoBehaviour {
	
	private bool _InZone = false;
		public bool inZone{
			get{return _InZone;}
			set{_InZone = value;}
		}

	void OnTriggerEnter(Collider other){
		if (other.tag != "avatar")return;
		
		_InZone = true;
	}
}

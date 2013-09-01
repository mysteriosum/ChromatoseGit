using UnityEngine;
using System.Collections;

public class ShavatarCollisionDetect : MonoBehaviour {
	
	private Shavatar1 _ShavatarScript;

	// Use this for initialization
	void Start () {
	
		_ShavatarScript = transform.parent.GetComponent<Shavatar1>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag == "collision"){
			_ShavatarScript.OnCollision = true;
		}
	}
	void OnTriggerStay(Collider other){
		if(!_ShavatarScript.OnCollision){
			if(other.tag == "collision"){
				_ShavatarScript.OnCollision = true;
			}
		}
	}
	void OnTriggerExit(Collider other){
		if(other.tag == "collision"){
			_ShavatarScript.OnCollision = false;
		}
	}
}

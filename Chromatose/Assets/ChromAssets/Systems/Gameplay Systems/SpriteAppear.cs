using UnityEngine;
using System.Collections;

public class SpriteAppear : MonoBehaviour {
	Transform avatar;
	
	// Use this for initialization
	void Start () {
		avatar = GameObject.Find("Avatar").transform;
		renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		
		if (other.gameObject.name == avatar.name){
			renderer.enabled = true;
		}
	}
	
	void OnTriggerExit(Collider other){
		if (other.gameObject.name == avatar.name){
			renderer.enabled = false;
		}
	}
}

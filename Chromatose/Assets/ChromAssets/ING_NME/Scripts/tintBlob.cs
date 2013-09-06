using UnityEngine;
using System.Collections;

public class tintBlob : MonoBehaviour {
	
	private ChromatoseManager _Manager;

	// Use this for initialization
	void Start () {
		Setup();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Setup(){
		_Manager = ChromatoseManager.manager;
	}
	
	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag != "avatar")return;
		_Manager.Death();
	}
}

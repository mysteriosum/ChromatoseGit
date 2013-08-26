using UnityEngine;
using System.Collections;

public class MovingBlob_ExtraCollision : MonoBehaviour {
	
	private MovingBlob parentScript;
	
	// Use this for initialization
	void Start () {
		parentScript = transform.parent.GetComponent<MovingBlob>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision other){
		if(parentScript.addExtraCollisionBox){
			if(other.gameObject.tag != "avatar") return;
			
			ChromatoseManager.manager.Death();
		}
	}
}

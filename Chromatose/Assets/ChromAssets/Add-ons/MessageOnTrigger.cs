using UnityEngine;
using System.Collections;

public class MessageOnTrigger : GizmoDad {
	
	public string message;
	public string colliderTag = "avatar";
	public GameObject[] targets;
	// Use this for initialization
	void Start () {
		if (message == ""){
			Debug.LogWarning("I don't have a message, dude");
		}
		Collider collider = GetComponent<Collider>();
		collider.isTrigger = true;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == colliderTag){
			foreach (GameObject go in targets){
				go.SendMessage(message);
			}
			Debug.Log("Dude " + message);
		}
	}
	
	
}

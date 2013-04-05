using UnityEngine;
using System.Collections;

public class EventTrigger : MonoBehaviour {
	public Transform triggerer;
	public Transform triggeree;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (other.transform == triggerer){
			triggeree.SendMessage("Trigger");
		}
	}
}

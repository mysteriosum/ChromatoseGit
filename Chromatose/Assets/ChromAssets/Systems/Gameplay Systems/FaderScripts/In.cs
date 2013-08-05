using UnityEngine;
using System.Collections;

public class In : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider other){
		if (other.name == "Avatar"){
			//Debug.Log("Trigger:");
			transform.parent.SendMessage("In");
		}
	}
	
	void OnTriggerEnter(Collider other){
		Npc npc = other.GetComponent<Npc>();
		if (npc){
			Debug.Log("Yeah let's get in!");
			transform.parent.SendMessage("Enter", npc.gameObject);
		}
	}
	
}

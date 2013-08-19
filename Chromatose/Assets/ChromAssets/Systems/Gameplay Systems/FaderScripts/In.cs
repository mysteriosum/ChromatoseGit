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
		Npc2 npc = other.GetComponent<Npc2>();
		if (npc){
			npc.transform.parent = null;
			transform.parent.SendMessage("Enter", npc.gameObject);
		}
	}
}

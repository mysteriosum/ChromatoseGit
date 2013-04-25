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
}

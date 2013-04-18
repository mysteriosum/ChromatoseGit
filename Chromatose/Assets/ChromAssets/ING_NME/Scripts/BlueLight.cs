using UnityEngine;
using System.Collections;

public class BlueLight : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider other){
		Avatar avatar = other.gameObject.GetComponent<Avatar>();
		if (!avatar) return;
		avatar.TempColour(2, 255);
	}
	
	void OnTriggerExit(Collider other){
		Avatar avatar = other.gameObject.GetComponent<Avatar>();
		if (!avatar) return;
		avatar.EndTemp();
	}
	
	
}

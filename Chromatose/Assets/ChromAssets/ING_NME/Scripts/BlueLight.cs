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
		
		if (avatar.name != "Avatar") return;
		
		avatar.InBlueLight = true;
		avatar.TempColour(2, 255);
	}
	
	void OnTriggerExit(Collider other){
		Avatar avatar = other.gameObject.GetComponent<Avatar>();
		
		if (!avatar) return;
		avatar.EndTemp();
		avatar.InBlueLight = false;
	}
	
	
}

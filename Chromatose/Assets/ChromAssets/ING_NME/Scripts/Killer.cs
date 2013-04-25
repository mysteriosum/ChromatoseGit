using UnityEngine;
using System.Collections;

public class Killer : MonoBehaviour {
	public LayerMask layerMask;
	public GameObject mySpecialObject;
	public GameObject[] thingsToTrigger;
	string specialName;
	// Use this for initialization
	void Start () {
		if (mySpecialObject){
			specialName = mySpecialObject.name;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (mySpecialObject){
			if (other.gameObject.name != specialName && other.gameObject.name != specialName + "(Clone)"){
				return;
			}
		}
		
		if (thingsToTrigger.Length > 0){
			foreach (GameObject thing in thingsToTrigger){
				thing.SendMessage("Trigger");
			}
		}
		Destroy(other.gameObject);
	}
}

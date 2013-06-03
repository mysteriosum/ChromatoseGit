using UnityEngine;
using System.Collections;

public class EnergyLineStart : MonoBehaviour {
	private EnergyLine parentGuy;
	// Use this for initialization
	void Start () {
		parentGuy = transform.parent.GetComponent<EnergyLine>();
		if (!parentGuy){
			Debug.LogWarning("This energy line segment doesn't have the right parent...");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerStay(Collider other){
		if (other.GetComponent<Avatar>()){
			parentGuy.Active = true;
		}
	}
}

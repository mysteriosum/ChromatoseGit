using UnityEngine;
using System.Collections;

public class LaserDeathTrigger : MonoBehaviour {

	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag != "avatar")return;
		Debug.Log("DEATH!!");
		ChromatoseManager.manager.Death();
	}
}

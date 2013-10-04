using UnityEngine;
using System.Collections;

public class bulletKiller : MonoBehaviour {

	void OnCollisionEnter(Collision colInfo){
		if(colInfo.gameObject.tag == "bossBullet"){
			Debug.Log("!!!!!!!!!!!");
			Destroy(colInfo.gameObject);
		}
	}
}

using UnityEngine;
using System.Collections;

public class BossDeathZone : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if(other.tag != "avatar")return;
		
		ChromatoseManager.manager.DeathByBoss();
	}
}

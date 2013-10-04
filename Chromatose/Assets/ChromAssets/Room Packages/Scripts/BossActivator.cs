using UnityEngine;
using System.Collections;

public class BossActivator : MonoBehaviour {
	
	public GameObject _Boss;

	void OnTriggerEnter(Collider other){
		if(other.tag != "avatar")return;
		_Boss.GetComponent<EndBoss_DataBase>().enabled = true;
		_Boss.GetComponent<EndBoss_DataBase>().PlayerInCombatZone = true;
		_Boss.GetComponent<EndBoss_FSM>().enabled = true;
		
		if(StatsManager.redCollCollected == 0){
			ChromatoseManager.manager.AddCollectible(Color.red, 10);
		}
	}
}

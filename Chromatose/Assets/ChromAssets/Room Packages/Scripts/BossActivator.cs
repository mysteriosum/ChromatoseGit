using UnityEngine;
using System.Collections;

public class BossActivator : MonoBehaviour {
	
	public GameObject _Boss;

	void OnTriggerEnter(Collider other){
		if(other.tag != "avatar")return;
		_Boss.GetComponent<EndBoss_DataBase>().enabled = true;
		_Boss.GetComponent<EndBoss_DataBase>().PlayerInCombatZone = true;
		_Boss.GetComponent<EndBoss_FSM>().enabled = true;
		/*
		if(StatsManager.redCollCollected[Application.loadedLevel] == 0){
			ChromatoseManager.manager.AddCollectible(Color.red, 10);
			StatsManager.manager.ReCalculateStats();
		}*/
		
		
			//Save les redColl que le joueur detient dans une variable du Boss pour le Reset
		_Boss.GetComponent<EndBoss_DataBase>().redCollAtStart = StatsManager.redCollDisplayed;
	}
}

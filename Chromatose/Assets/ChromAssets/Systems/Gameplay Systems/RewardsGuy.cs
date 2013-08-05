using UnityEngine;
using System.Collections;

public class RewardsGuy : MonoBehaviour {
	private Collectible[] myCols;
	// Use this for initialization
	void Start () {
		myCols = GetComponentsInChildren<Collectible>();
		
		foreach(Collectible col in myCols){
			col.Dead = true;
			col.Gone = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	//rebleblebleble
	}
	
	void Trigger(){
		foreach (Collectible c in myCols){
			c.Dead = false;
			c.Gone = false;
		}
	}
}

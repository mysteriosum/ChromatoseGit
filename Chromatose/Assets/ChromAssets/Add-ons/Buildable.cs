using UnityEngine;
using System.Collections;

public class Buildable : Destructible {

	// Use this for initialization
	void Start () {
		Setup();
	}
	
	// Update is called once per frame
	void Update () {
		Checks();
	}
	protected override void Checks(){
		if (goingToDestroy){
			Destruct();
		}
	}
}

using UnityEngine;
using System.Collections;

public class Collectible : ColourBeing {
	
	public Couleur colColour = Couleur.white;
	
	
	// Use this for initialization
	void Start () {
		switch (colColour){
		case Couleur.white:
			colour.r = 0;
			colour.g = 0;
			colour.b = 0;
			break;
		case Couleur.red:
			colour.r = 255;
			colour.g = 0;
			colour.b = 0;
			break;
		case Couleur.green:
			colour.r = 0;
			colour.g = 255;
			colour.b = 0;
			break;
		case Couleur.blue:
			colour.r = 0;
			colour.g = 0;
			colour.b = 255;
			break;
		
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.tag == "avatar"){
			if (CheckSameColour(collider.gameObject.GetComponent<Avatar>().colour) || colColour == Couleur.white){
				ChromatoseManager.manager.AddCollectible(colColour);
				Dead = true;
			}
		}
	}
	
	override public void Trigger(){
		
	}
}

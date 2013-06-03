using UnityEngine;
using System.Collections;

public class ColourWell : ColourBeing {
	int colourToAdd;
	public int giveRate = 8;
	private ChromatoseManager manager;
	protected Avatar avatar;
	
	// Use this for initialization
	void Start () {
		if (colour.r > colourConsiderMin){
			colourToAdd = 0;
		}
		else if(colour.g > colourConsiderMin){
			colourToAdd = 1;
		}
		else if(colour.b > colourConsiderMin){
			colourToAdd = 2;
		}
		
		hud = ChromatoseManager.manager.hud;
		avatar = GameObject.Find("Avatar").GetComponent<Avatar>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider collider){
		
		if (collider != avatar.collider) return;
		
		hud.UpdateAction(Actions.Absorb, Trigger);		//this tells the hud that I want to do something. But I'll have to wait in line!
		
		
	}
	
	
	override public void Trigger(){
		Debug.Log("Should be setting colour");
		avatar.SetColour(Mathf.Max(avatar.colour.r + colour.r, 255), Mathf.Max(avatar.colour.g + colour.g, 255),Mathf.Max(avatar.colour.b + colour.b, 255));
		
	}
}

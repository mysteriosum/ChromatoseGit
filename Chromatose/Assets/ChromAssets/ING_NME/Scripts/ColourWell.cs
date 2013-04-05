using UnityEngine;
using System.Collections;

public class ColourWell : ColourBeing {
	int colourToAdd;
	public int giveRate = 8;
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider collider){
		
		ColourBeing other = collider.gameObject.GetComponent<ColourBeing>();
		other.AddColour(colourToAdd, giveRate);
		
	}
	
	
	override public void Trigger(){
		
	}
}

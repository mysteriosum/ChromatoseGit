using UnityEngine;
using System.Collections;

// enums and other such things


abstract public class ColourBeing : MonoBehaviour {		//base class for all living things in Chromatose
	
	public Colour colour;
	[System.SerializableAttribute]
	public class Colour{
		
		public int r; 
		public int g;
		public int b;
		
	}
	
	private bool dead;
	public bool Dead{
		get{
			return dead;
		}
		set{
			dead = value;
			transform.position = dead? new Vector3(transform.position.x, transform.position.y, -200)
									 : new Vector3(transform.position.x, transform.position.y, 0);
		}
	}
	
	protected int colourConsiderMin = 100;
	protected bool tinted;
	protected bool triggered = false;
	
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public Colour GetColour(){
		
		return colour;
	}
	public void SetColour(int newR, int newG, int newB){
		
		colour.r = newR;
		colour.g = newG;
		colour.b = newB;
	}
	
	public bool CheckSameColour(Colour otherColour){					//Are we the same colour? Answer me! >=C
		
		if (colour.r > colourConsiderMin && otherColour.r > 0){
			return true;
		}
		if (colour.g > colourConsiderMin && otherColour.g > 0){
			return true;
		}
		if (colour.b > colourConsiderMin && otherColour.b > 0){
			return true;
		}
		return false;
	}
	
	public void AddColour(int toAdd, int amount){
		//Debug.Log("Adding colour!");
		switch (toAdd){
		case 0:
			colour.r += amount;
			break;
		case 1:
			colour.g += amount;
			break;
		case 2:
			colour.b += amount;
			break;
		default:
			Debug.LogWarning("Trying to add colour but your toAdd value is " + toAdd.ToString());
			break;
		}
		
	}
	
	abstract public void Trigger();
	
	void OnDrawGizmos(){
		colour.r = Mathf.Clamp(colour.r, 0, 255);
		colour.g = Mathf.Clamp(colour.g, 0, 255);
		colour.b = Mathf.Clamp(colour.b, 0, 255);
	}
	
	
}

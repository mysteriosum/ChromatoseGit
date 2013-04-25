using UnityEngine;
using System.Collections;

// enums and other such things


abstract public class ColourBeing : MonoBehaviour {		//base class for all living things in Chromatose
	
	public Colour colour;
	protected Color shownColour;
	protected Colour tempColour = new Colour(-1, -1, -1);
	
	[System.SerializableAttribute]
	public class Colour{
		
		public int r; 
		public int g;
		public int b;
		public bool White{		
			get{ return r == 0 && g == 0 && b == 0; }
		}
		public bool Red{
			get{ return r > 0; } 
		}
		public bool Blue{
			get{ return b > 0; }
		}
		public bool Green{
			get{ return g > 0; }
		}
		public Colour(int R, int G, int B){
			r = R;
			g = G;
			b = B;
			
		}
		public Colour(){
			r = 0;
			g = 0;
			b = 0;
		}
		public override string ToString(){
			return "r = " + r + 
				" | g = " + g + 
				" | b = " + b;
			
		}
	}
	private bool dead;
	public bool Dead{
		get{ return dead; }
		set{ dead = value;
			transform.position = dead? new Vector3(transform.position.x, transform.position.y, -50)
									 : new Vector3(transform.position.x, transform.position.y, 0); }
	}
	
	private bool gone;
	public bool Gone{
		get{
			return gone;
		}
		set{
			gone = value;
			transform.position = gone? new Vector3(transform.position.x, transform.position.y, -200)
									 : new Vector3(transform.position.x, transform.position.y, 0);
		}
	}
	
	protected int colourConsiderMin = 100;
	protected bool tinted;
	protected bool triggered = false;
	protected bool tempered = false;
	protected float tempCounter = 0f;
	protected tk2dSprite spriteInfo;
	
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
	
	
	/// <summary>
	/// Sets a temporary value to a colour
	/// </summary>
	/// <param name='toAdd'>
	/// What colour to add; 0 = r; 1 = g; 2 = b.
	/// </param>
	/// <param name='newValue'>
	/// New colour value.
	/// </param>
	
	public void TempColour(int toAdd, int newValue){
		
		Debug.Log("Start of temp colour! and " + tempColour + " vs " + colour);
		switch (toAdd){
		case 0:
			if (tempColour.r >= 0) break;
			tempColour = new Colour(colour.r, -1, -1);
			colour.r = newValue;
			break;
		case 1:
			if (tempColour.g >= 0) break;
			tempColour = new Colour(-1, colour.g, -1);
			colour.g = newValue;
			break;
		case 2:
			if (tempColour.b >= 0) break;
			tempColour = new Colour(-1, -1, colour.b);
			colour.b = newValue;
			break;
		default:
			Debug.LogWarning("Trying to add colour but your toAdd value is " + toAdd.ToString());
			break;
		}
		Debug.Log("End of tempColour! and " + tempColour + " vs " + colour);
		
	}
	
	public void EndTemp(){
		if (tempColour.r >= 0){
			colour.r = tempColour.r;
		}
		if (tempColour.g >= 0){
			colour.g = tempColour.g;
		}
		if (tempColour.b >= 0){
			colour.b = tempColour.b;
		}
		
		Debug.Log("End that temp! and " + tempColour + " vs " + colour);
		tempColour = new Colour (-1, -1, -1);
	}
	
	/// <summary>
	/// Adds the colour.
	/// </summary>
	/// <param name='toAdd'>
	/// What colour to add; 0 = r; 1 = g; 2 = b.
	/// </param>
	/// <param name='amount'>
	/// Amount.
	/// </param>
	
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

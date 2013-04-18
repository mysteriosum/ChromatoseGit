using UnityEngine;
using System.Collections;

enum ColourEnum{
	
}

public class ChromatoseManager : MonoBehaviour {
	private Avatar avatar;
	public static ChromatoseManager manager; 
	
	private class Collectibles{
		public int w;
		public int r;
		public int g;
		public int b;
	}
	
	private Collectibles collectibles = new Collectibles();
	
	// Use this for initialization
	void Start () {
		manager = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddCollectible(Couleur colour){
		switch (colour){
		case Couleur.red:
			collectibles.r += 1;
			break;
		case Couleur.green:
			collectibles.g += 1;
			break;
		case Couleur.blue:
			collectibles.b += 1;
			break;
		case Couleur.white:
			collectibles.w += 1;
			break;
		default:
			Debug.LogWarning("Not a real collectible.");
			break;
		}
		//Debug.Log(collectibles);
	}
	/// <summary>
	/// Gets the number of a specific type of collectible.
	/// </summary>
	/// <returns>
	/// The number of cols specified
	/// </returns>
	/// <param name='colour'>
	/// Colour.
	/// </param>
	public int GetCollectibles(Couleur colour){
		switch (colour){
		case Couleur.red:
			return collectibles.r;
		case Couleur.green:
			return collectibles.g;
		case Couleur.blue:
			return collectibles.b;
		case Couleur.white:
			return collectibles.w;
		default:
			Debug.LogWarning("Not a real collectible.");
			return 0;
		}
	}
	
	public void RemoveCollectibles(Couleur colour, int value){
		switch (colour){
		case Couleur.red:
			collectibles.r -= value;
			return;
		case Couleur.green:
			collectibles.g -= value;
			return;
		case Couleur.blue:
			collectibles.b -= value;
			return;
		case Couleur.white:
			collectibles.w -= value;
			return;
		default:
			Debug.LogWarning("Not a real collectible.");
			return;
		}
	}
	
	void OnGUI(){
		GUI.TextArea(new Rect(Screen.width - 136, 8, 128, 80), "Collectibles"
											+ "\nR = " + collectibles.r
											+ "\nG = " + collectibles.g 
											+ "\nB = " + collectibles.b
											+ "\nW = " + collectibles.w);
	}
	
}

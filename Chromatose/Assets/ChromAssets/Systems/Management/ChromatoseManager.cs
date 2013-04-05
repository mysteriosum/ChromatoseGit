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
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum ColourEnum{
	
}

public class ChromatoseManager : MonoBehaviour {
	private Avatar avatar;
	private AvatarPointer avatarP;
	public static ChromatoseManager manager; 
	
	
	private class CollectiblesManager{
		public List<Collectible> w = new List<Collectible>();
		public List<Collectible> r = new List<Collectible>();
		public List<Collectible> g = new List<Collectible>();
		public List<Collectible> b = new List<Collectible>();
	}
	private class RoomStats{
		public List<Collectible> consumedCollectibles = new List<Collectible>();
	}
	
	private RoomStats[] roomStats = {new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats()};
	
	private CollectiblesManager collectibles = new CollectiblesManager();
	
	public GameObject oneShotSpritePrefab;
	public tk2dSpriteCollectionData bubbleCollection;
	public tk2dFontData chromatoseFont;
	
												//GETTERS & SETTERS
	public List<Collectible> WhiteCollectibles{
		get{ return collectibles.w; }
	}
	
	
	// Use this for initialization
	void Awake () {
		manager = this;
		Debug.Log("this level = " + Application.loadedLevel);
	}
	
	void OnLevelWasLoaded(){
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.PageUp)){
			if (Application.loadedLevel == 0) return;
			Application.LoadLevel(Application.loadedLevel - 1);
		}
		
		if (Input.GetKeyDown(KeyCode.PageUp)){
			if (Application.loadedLevel == Application.levelCount - 1) return;
			Application.LoadLevel(Application.loadedLevel + 1);
		}
		
		if (Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
	
	}
	
	public void AddCollectible(Collectible col){
		switch (col.colColour){
		case Couleur.red:
			collectibles.r.Add(col);
			break;
		case Couleur.green:
			collectibles.g.Add(col);
			break;
		case Couleur.blue:
			collectibles.b.Add(col);
			break;
		case Couleur.white:
			collectibles.w.Add(col);
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
			return collectibles.r.Count;
		case Couleur.green:
			return collectibles.g.Count;
		case Couleur.blue:
			return collectibles.b.Count;
		case Couleur.white:
			return collectibles.w.Count;
		default:
			Debug.LogWarning("Not a real collectible.");
			return 0;
		}
	}
	
	public void DropCollectibles(List<Collectible> list, int no, Vector3 pos){
		
		for (int i = 0; i < no; i ++){
			Collectible inQuestion = list[list.Count - 1];
			
			inQuestion.PutBack(pos + (Vector3)Random.insideUnitCircle * 15);
			list.Remove(inQuestion);
		}
		
	}
	
	public void RemoveCollectibles(Couleur colour, int value, Vector3 pos){
		switch (colour){
		case Couleur.red:
			JettisonCollectibles(collectibles.r, value, pos);
			return;
		case Couleur.green:
			JettisonCollectibles(collectibles.g, value, pos);
			return;
		case Couleur.blue:
			JettisonCollectibles(collectibles.b, value, pos);
			return;
		case Couleur.white:
			JettisonCollectibles(collectibles.w, value, pos);
			return;
		default:
			Debug.LogWarning("Not a real collectible.");
			return;
		}
	}
	
	public void JettisonCollectibles(List<Collectible> list, int no, Vector3 pos){
		for (int i = 0; i < no; i ++){
			Collectible inQuestion = list[list.Count - 1];
			roomStats[Application.loadedLevel].consumedCollectibles.Add(inQuestion);
			inQuestion.Trigger();
			inQuestion.transform.position = pos;
			list.Remove(inQuestion);
		}
	}
	
	void OnGUI(){
		GUI.TextArea(new Rect(Screen.width - 136, 8, 128, 80), "Collectibles"
											+ "\nR = " + collectibles.r.Count
											+ "\nG = " + collectibles.g.Count 
											+ "\nB = " + collectibles.b.Count
											+ "\nW = " + collectibles.w.Count);
	}
	
	
		
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<-------------STATIC FUNCTIONS!-------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	
	tk2dSprite spriteInfo;
	public GameObject OneShotAnim( string animName, float time, Vector3 callerPosition){
		
		GameObject newGuy = GameObject.Instantiate(oneShotSpritePrefab, callerPosition, Quaternion.identity) as GameObject;
		
		spriteInfo = newGuy.GetComponent<tk2dSprite>();
		
			spriteInfo.SetSprite(spriteInfo.Collection.GetSpriteIdByName(animName));
		GameObject.Destroy(newGuy, time);
		return newGuy;
		
	}
	
}

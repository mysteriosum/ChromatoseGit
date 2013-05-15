using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		public List<Comic> comics = new List<Comic>();
		
		public bool[] thumbsFound = {false, false, false, false, false, false};
		
		public bool secretFound = false;
		public bool comicComplete = false;
	
	
	}
	
	private RoomStats[] roomStats = {new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats()};
	private int curRoom;
	
	private CollectiblesManager collectibles = new CollectiblesManager();
	
	
	public GameObject comicCompleteAnim;
	public GameObject oneShotSpritePrefab;
	public tk2dSpriteCollectionData bubbleCollection;
	public tk2dFontData chromatoseFont;
	
	public Texture backButton;
	
	private GameObject shavatarComicBlock;
	private ComicTransition comicTransition;
												//COMICS AND HOW TO USE THEM
	private bool inComic = false;
	public bool InComic{
		get{ return inComic; }
		
	}
	
	
												//GETTERS & SETTERS
	public List<Collectible> WhiteCollectibles{
		get{ return collectibles.w; }
	}
	
	
	
	
	// Use this for initialization
	void Awake () {
		manager = this;
			//comic frame dealings: TODO PUT IN ONLEVELWASLOADED
		UpdateRoomStats();
	}
	
	void OnLevelWasLoaded(){
		
		UpdateRoomStats();
	}
	
	void UpdateRoomStats(){
		Debug.Log("Current room index is " + Application.loadedLevel);
		avatar = GameObject.Find("Avatar").GetComponent<Avatar>();
		curRoom = Application.loadedLevel;
		GameObject[] frames = GameObject.FindGameObjectsWithTag("comicFrame");
		int counter = 0;
		do{
			roomStats[curRoom].comics.Add(null);
			counter ++;
//			Debug.Log("There's some stuff going on in the roomStats thing");
			if (counter > 50){
				Debug.LogWarning("It's fucked up I know, but like, idk, there's just a lot going on in this level apparently");
				break;
			}
		}
		while(roomStats[curRoom].comics.Count < frames.Length);
		
		foreach (GameObject go in frames){
			Comic strip = go.GetComponent<Comic>();
//			Debug.Log(strip);
			roomStats[curRoom].comics[strip.mySlotIndex] = strip;
			strip.gameObject.SetActive(false);
		}
		shavatarComicBlock = GameObject.Find("pre_shavatarComicBlock");
		if (!shavatarComicBlock){
			Debug.LogWarning("Hey loser! There's no Shavatar Comic Block in this level!");
		}
		
		comicTransition = GameObject.Find("pre_comicLoader").GetComponent<ComicTransition>();
		if (!comicTransition){
			Debug.LogWarning("Hey loser! There's no comic loader in this level!");
		}
	}
	
	// Update is called once per frame
	
	void Update () {
		
		if (inComic){
			if (roomStats[curRoom].comicComplete){
				//com
			}
		}
		
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
		
		if (inComic){
			
			Rect backButtonArea = new Rect(48, Screen.height - 96, backButton.width, backButton.height);
			
			bool backButtonPressed = GUI.Button(backButtonArea, backButton, GUIStyle.none);
			if (backButtonPressed){
				comicTransition.Return();
			}
		}
		else{
			
			GUI.TextArea(new Rect(Screen.width - 136, 8, 128, 80), "Collectibles"
																+ "\nR = " + collectibles.r.Count
																+ "\nG = " + collectibles.g.Count 
																+ "\nB = " + collectibles.b.Count
																+ "\nW = " + collectibles.w.Count);
		}
	}
	
	
	
																			//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
																			//<-------------COMICS AND STUFF!-------------->
																			//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	public void OpenComic(int index){
		avatar.movement.SetVelocity(Vector2.zero);
		
		Time.timeScale = 0;
		inComic = true;
		int counter = 0;
		foreach (Comic c in roomStats[curRoom].comics){
			
			if (roomStats[curRoom].thumbsFound[counter]){
				c.gameObject.SetActive(true);
			}
			
			counter ++;
		}
	}
	
	public void CloseComic(){
		Time.timeScale = 1;
		inComic = false;
		
	}
	
	
	public void FindComic(int index){
		roomStats[curRoom].thumbsFound[index] = true;
	}
	
	public bool CheckComicStats(){
		
		foreach (Comic c in roomStats[curRoom].comics){
			if (!c.InMySlot){
				return false;
			}
		}
								//turns out the comic is successful!
		
		shavatarComicBlock.SetActive(false);
		
		Instantiate(comicCompleteAnim);
		return true;
		
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

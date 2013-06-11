using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum Actions{
	Back,
	Absorb,
	Destroy,
	Build,
	Pay,
	Release,
	
	Nothing,
	
}

public enum TankStates{
	None,
	Empty,
	Full,
	Flashing,
}

public class ChromatoseManager : MonoBehaviour {
	private Avatar avatar;
	private AvatarPointer avatarP;
	public static ChromatoseManager manager; 
	public ChromHUD hud = new ChromHUD();
	private GUISkin skin;
	
										//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
										//<--------------ACTION BUTTON!---------------->
										//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	private bool actionPressed;
	
	private Actions currentAction = Actions.Nothing;
	
	private Texture actionTexture;
	private Texture shownActionTexture;
	
	public delegate void ActionDelegate();
	public ActionDelegate actionMethod;
	
	public void UpdateAction(Actions action, ActionDelegate method){
		if (action <= currentAction || action == Actions.Nothing){
			currentAction = action;
			actionMethod = method;
			showingAction = true;
		}
	}
	
	public void UpdateActionTexture(){
		
		shownActionTexture = actionTexture;
	}
	
	
	private bool showingAction;
	private float actionSlideSpeed = 10f;
	
										//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
										//<--------------DATA TRACKING!---------------->
										//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	
	public class CollectiblesManager{
		public List<Collectible> w = new List<Collectible>();
		public List<Collectible> r = new List<Collectible>();
		public List<Collectible> g = new List<Collectible>();
		public List<Collectible> b = new List<Collectible>();
		public List<Collectible> held = new List<Collectible>();
	}
	
	private class RoomStats{
		public List<Collectible> consumedCollectibles = new List<Collectible>();
		public List<Comic> comics = new List<Comic>();
		public Comic secretComic;
		
		public bool[] thumbsFound = {false, false, false, false, false, false, false};
		public int thumbNumber = 0;
		public bool secretFound = false;
		public bool comicComplete = false;
		
		public int redColsUsed;
		public int greenColsUsed;
		public int blueColsUsed;
		public int afterImagesUsed;
		public float timeTaken;
		
		public int redColsIn;
		public int greenColsIn;
		public int blueColsIn;
		public int whiteColsIn;
		
		public int redColsFound;
		public int greenColsFound;
		public int blueColsFound;
		public int whiteColsFound;
	
	}
	
	
	private string[] roomNames = 
		{
		"Tutorial", "Module1_Scene1", "Module1_Scene2", "Module1_Scene3", "Module1_Scene4", "Module1_Scene5,",
		"Module1_Scene6", "Module1_Scene7", "Module1_Scene8", "Module1_Scene9"};
	private static RoomStats[] roomStats;
	private int curRoom;
	private Transform curCheckpoint;
	
	private CollectiblesManager collectibles = new CollectiblesManager();
	public static CollectiblesManager statCols;
	
	public GameObject comicCompleteAnim;
	public GameObject oneShotSpritePrefab;
	public tk2dSpriteCollectionData bubbleCollection;
	public tk2dFontData chromatoseFont;
	
	private bool checkedComicStats = false;
	
	public bool playedCompleteFlourish = false;
	public bool playedSecretFlourish = false;
	
	public bool givenCols = false;
	public GameObject rewardsGuy;
	
	public bool animsReady = false;
	public bool AnimsReady{
		set { animsReady = value; }
	}
	
	public Texture backButton;
	
	//private GameObject shavatarComicBlock;
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
		
		if (statCols == null){
			statCols = collectibles;
		}
		else{
			collectibles = statCols;
		}
		if (roomStats == null){
			roomStats = new RoomStats[10]
			{	new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), 
				new RoomStats(), new RoomStats(), new RoomStats(), new RoomStats(), 
				new RoomStats(), new RoomStats()
			};
		}
		
		
		manager = this;
		
		UpdateRoomStats();
		
	}
	
	void Start(){
		
		DontDestroyOnLoad(manager);
		if (!hud.mainBox){
			Debug.LogWarning("There are some missing textures....");
		}
		
		rN = collectibles.r.Count;
		gN = collectibles.g.Count;
		bN = collectibles.b.Count;
		wN = collectibles.w.Count;
		
		colX[0] = Screen.width; 		//the red one
		colY[0] = hud.mainBox.height + 90f;
		colX[1] = Screen.width;			//the blue one
		colY[1] = hud.mainBox.height + 135f;
		colX[2] = Screen.width;			//the green one
		colY[2] = hud.mainBox.height + 180f;
		colX[3] = Screen.width;			//the white one
		colY[3] = hud.mainBox.height + 45f;
		colX[4] = Screen.width; 		//the comics
		colY[4] = hud.mainBox.height;	
		
		
		for (int i = 0; i < showingCol.Length; i++){
			showingCol[i] = true;
		}
		
		
		float mainAnchor = Screen.width - hud.mainBox.width;
		float dummy = Screen.width;
		List<float> tempTrack = new List<float>();
		float initSpeed = 6f;
		float increment = 1.5f;
		
		while (dummy >= mainAnchor){		//move my dummy toward the main anchor
			dummy -= initSpeed;
			tempTrack.Add(dummy);
			if (dummy <= 0){
				break;
			}
		}
		
		do{
			initSpeed -= increment;
			dummy -= initSpeed;
			tempTrack.Add(dummy);
		}
		while (initSpeed > 0 || dummy < mainAnchor);
		
		track = tempTrack.ToArray();
		track[track.Length - 1] = mainAnchor;
						//HACK Getting a HUD_skin from the Resources folder
		skin = Resources.Load("Menus/HUD_skin") as GUISkin;
		
		
		barMinY = 28;
		barMaxY = barMinY + hud.energyBar.height;
		barX = 1192;
		barY = barMinY;
		
		
		
		aX = hud.absorbAction.width;
		
		actionTexture = hud.absorbAction;
		shownActionTexture = actionTexture;
		
		rewardsGuy = GameObject.FindWithTag("rewardsGuy");
		if (!rewardsGuy){
			Debug.LogWarning("Hey doofus! There's no rewards guy in this level! Is there even a comic!?");
		}
		else{
			
		}
	}
	
	
	void OnLevelWasLoaded(){
		
		UpdateRoomStats();
	}
	
	void UpdateRoomStats(){
		
		avatar = GameObject.Find("Avatar").GetComponent<Avatar>();
		curRoom = -1;
		for (int i = 0; i < roomNames.Length; i ++){
			if (Application.loadedLevelName == roomNames[i]){
				curRoom = i;
				
				break;
			}
		}
		if (curRoom == -1 && Application.loadedLevelName != "Menu"){
			Debug.LogWarning("This room is not named properly, or shouldn't be in the build at all.");
		}
		GameObject[] frames = GameObject.FindGameObjectsWithTag("comicFrame");
		GameObject[] thumbs = GameObject.FindGameObjectsWithTag("comicThumb");
		int counter = 0;
		
		if (roomStats[curRoom].comics.Count == 0){
			foreach (GameObject go in frames){
				Comic strip = go.GetComponent<Comic>();
				if (strip.isSecret){
					roomStats[curRoom].secretComic = strip;
				}
				else{
					roomStats[curRoom].comics.Add(strip);
					strip.gameObject.SetActive(false);
				}
			}
		}
		else{
			foreach (GameObject go in thumbs){
				ComicThumb thumb = go.GetComponent<ComicThumb>();
				if (roomStats[curRoom].thumbsFound[counter]){
					thumb.SendMessage("Trigger");
				}
			}
		}
		
		comicTransition = GameObject.Find("pre_comicLoader").GetComponent<ComicTransition>();
		if (!comicTransition){
			Debug.LogWarning("Hey loser! There's no comic loader in this level!");
		}
	}
	
	// Update is called once per frame
	void TriggerQuestionMark(){
		
		GameObject question = GameObject.FindWithTag("questionMark");
		if (question)
			question.SendMessage("Trigger");
	}
	void Update () {
		
		if (inComic && animsReady && !checkedComicStats){
			
			roomStats[curRoom].comicComplete = CheckComicStats();
			checkedComicStats = true;
			
			if (roomStats[curRoom].comicComplete){
				//TODO PUT ANIMATION COMPLETE ANIMATION!
				// do I have the secret too? play special anim : play normal anim;
				
				if (!roomStats[curRoom].secretFound){
					Instantiate(comicCompleteAnim);
					tk2dAnimatedSprite anim = comicCompleteAnim.GetComponent<tk2dAnimatedSprite>();
					anim.Play();
					TriggerQuestionMark();
				}
				else{
					//SUPER SECRET ANIM TIME!
				}
				
				if (!givenCols){
					givenCols = true;
					rewardsGuy.SendMessage("Trigger");
				}
			}
		}
		else if (!inComic){
			animsReady = false;
			checkedComicStats = false;
		}
		
		if (Input.GetKeyDown(KeyCode.PageUp)){
			if (Application.loadedLevel == 0) return;
			Application.LoadLevel(Application.loadedLevel - 1);
		}
		
		if (Input.GetKeyDown(KeyCode.PageDown)){
			if (Application.loadedLevel == Application.levelCount - 1) return;
			Application.LoadLevel(Application.loadedLevel + 1);
		}
		
		if (Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
		
		
										//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
										//<-----------SHOWING COLLECTIBLES!------------>
										//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		for (int i = 0; i < colCouleurs.Length; i++){
			if (showingCol[i]){
				colTimers[i] ++;
				if (colTimers[i] > refreshTiming){
					bool identical = UpdateCollectible(colCouleurs[i]);
					if (!identical){
						colTimers[i] = track.Length;
					}
					else{
						showingCol[i] = false;
					}
				}
			}
			
			if (!showingCol[i] && colTimers[i] > 0){
				colTimers[i] --;
			}
			
			if (colTimers[i] >= 0 && colTimers[i] < track.Length){
				colX[i] = track[colTimers[i]];
			}
		}
		
		
										//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
										//<------------UPDATING ENERGY BAR!------------>
										//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		float barDiffY = Avatar.curEnergy * hud.energyBar.height / 100;
		barY = barMaxY - (int) barDiffY;
		if (flashyBarTimer > 0){
			flashyBarTimer --;
		}
		else{
			flashyBar = false;
		}
		
		/*
		if (showingW){
			wTimer ++;
			if (wTimer > refreshTiming){
				bool identical = UpdateCollectible(Couleur.white);
				if (!identical){
					wTimer = track.Length;
				}
				else{
					showingW = false;
				}
			}
		}
		
		if (!showingW && wTimer > 0){
			wTimer --;
		}
		
		if (wTimer >= 0 && wTimer < track.Length){
			wX = track[wTimer];
		}*/
		
		//Mathf.Clamp(wTimer, 0, track.Length - 1);
	}
	
	void LateUpdate(){
		
										//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
										//<--------------ACTION BUTTON!---------------->
										//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
								
		switch (currentAction){
		case Actions.Back:
			actionTexture = hud.returnAction;
			break;
		case Actions.Absorb:
			actionTexture = hud.absorbAction;
			break;
		case Actions.Destroy:
			actionTexture = hud.destroyAction;
			break;
		case Actions.Build:
			actionTexture = hud.buildAction;
			break;
		case Actions.Pay:
			actionTexture = hud.payAction;
			break;
		case Actions.Release:
			actionTexture = hud.releaseAction;
			break;
			
		default:
			actionMethod = null;
			break;
		}
		
		/*
		Debug.Log("Some state info: " +
			"showingAction = " + showingAction + 
			"shownActionTexture = " + shownActionTexture.name + 
			"actionTexture = " + actionTexture);*/
		if (currentAction == Actions.Nothing && aX < hud.absorbAction.width){
			aX -= actionSlideSpeed;
			if (aX < -hud.absorbAction.width){
				aX = Mathf.Abs(aX);
			}
			Debug.Log("Wiping away the Tears");
		}
		if (!showingAction){
			currentAction = Actions.Nothing;
		}
		
		if (showingAction && aX != 0 && shownActionTexture == actionTexture){
			
			aX -= actionSlideSpeed;
			if (aX > -9 && aX < 9) aX = 0;
			if (aX < -hud.absorbAction.width){
				aX = Mathf.Abs(aX);
			}
		}
		
		if (showingAction && shownActionTexture != actionTexture){
			Debug.Log("Should be working to update the action texture");
			if (aX >= hud.absorbAction.width){
				UpdateActionTexture();
			}
			else{
				aX -= actionSlideSpeed;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.P) && currentAction > 0 && actionMethod != null){
			actionMethod();
		}
		showingAction = false;
		
	}
	
	
	
	[System.Serializable]
	public class ChromHUD {
		
		public Texture mainBox;
		public Texture smallBox;
		public Texture[] energyTank;
		public Texture energyBar;
		public Texture actionButton;
		public Texture absorbAction;
		public Texture buildAction;
		public Texture destroyAction;
		public Texture payAction;
		public Texture releaseAction;
		public Texture returnAction;
		public Texture energyBarFlash;
		
		
		public Texture redCollectible;
		public Texture greenCollectible;
		public Texture blueCollectible;
		public Texture whiteCollectible;
		public Texture comicCounter;
		
		public Texture[] pauseButton;
		
		public tk2dFontData chromFont;
		
		private tk2dTextMesh rColMesh;
		private tk2dTextMesh gColMesh;
		private tk2dTextMesh bColMesh;
		
		
		
	}
	
	
	private float[] colX = {0, 0, 0, 0, 0};
	private float[] colY = {0, 0, 0, 0, 0};
	
	private float aX;
	
	private int rN;
	private int gN;
	private int bN;
	private int wN;
	private int cN;
	
	private int barX;
	private int barY;
	private int barMinY;
	private int barMaxY;
	
	private bool flashyBar = false;
	private int flashyBarTimer = 0;
	private int flashyBarTiming = 20;
	
	public void Healed(){
		flashyBar = true;
		flashyBarTimer = flashyBarTiming;
		Debug.Log("HEYR");
	}
	
	private int tankX = 1219;
	private int tankY = 28;
	
	
	private Vector2 textOffset = new Vector2 (55f, 8);
	
	private Couleur[] colCouleurs = {Couleur.red, Couleur.green, Couleur.blue, Couleur.white, Couleur.black};		//the black is for the comics
	
	private bool[] showingCol = {false, false, false, false, false};
	private int[] colTimers = {0, 0, 0, 0, 0};

	
	private int showTiming = 1;
	private float defaultSpeed = 1.2f;
	private int refreshTiming = 75;
	
	private float[] track;
	
	private int rTimer = 0;
	private float rSpeed = 0;
	private int gTimer = 0;
	private float gSpeed = 0;
	private int bTimer = 0;
	private float bSpeed = 0;
	private int wTimer = 0;
	private float wSpeed = 0;
	
	
	void OnGUI(){
		
				//UPDATE HUD
		
				//FOR COMICS
		if (inComic){
			
			Rect backButtonArea = new Rect(48, Screen.height - 96, backButton.width, backButton.height);
			
			bool backButtonPressed = GUI.Button(backButtonArea, backButton, GUIStyle.none);
			if (backButtonPressed){
				comicTransition.Return();
			}
		}
		else{		//TODO MAKE THIS NOT STUPID AND UGLY! (ie delete it and handle this stuff in the hud draw thing guy)
			
			Rect mainRect = new Rect(Screen.width - hud.mainBox.width, 0, hud.mainBox.width, hud.mainBox.height);
			GUI.skin = skin;
			
			Rect rColRect = new Rect(colX[0], colY[0], hud.redCollectible.width, hud.redCollectible.height);
			Rect gColRect = new Rect(colX[1], colY[1], hud.greenCollectible.width, hud.greenCollectible.height);
			Rect bColRect = new Rect(colX[2], colY[2], hud.blueCollectible.width, hud.blueCollectible.height);
			Rect wColRect = new Rect(colX[3], colY[3], hud.whiteCollectible.width, hud.whiteCollectible.height);
			Rect comicRect = new Rect(colX[4], colY[4], hud.comicCounter.width, hud.comicCounter.height);
			Rect actionRect = new Rect(1199, 141, 50, 52);
			Rect energyRect = new Rect(barX, barMinY, hud.energyBar.width, hud.energyBar.height);
			Rect flashyRect = new Rect(barX - 7, barMinY - 8, hud.energyBarFlash.width, hud.energyBarFlash.height);
			Rect tankRect = new Rect(tankX, tankY, 80, 128);
			
			GUI.DrawTexture(mainRect, hud.mainBox);
			
			
			GUI.BeginGroup(comicRect);										//comic counter
				GUI.skin.textArea.normal.textColor = Color.black;
				GUI.DrawTexture(new Rect(0, 0, hud.comicCounter.width, hud.comicCounter.height), hud.comicCounter);
				GUI.TextArea(new Rect(textOffset.x, textOffset.y, 80, 40), cN.ToString() + " / " + roomStats[curRoom].comics.Count.ToString());
				
			GUI.EndGroup();
			
			GUI.BeginGroup(wColRect);										//white collectible
				GUI.skin.textArea.normal.textColor = Color.white;
				GUI.DrawTexture(new Rect(0, 0, hud.whiteCollectible.width, hud.whiteCollectible.height), hud.whiteCollectible);
				GUI.TextArea(new Rect(textOffset.x, textOffset.y, 80, 40), wN.ToString());
				
			GUI.EndGroup();
			
			GUI.BeginGroup(rColRect);										//red collectible
				GUI.skin.textArea.normal.textColor = Color.red;
				GUI.DrawTexture(new Rect(0, 0, hud.redCollectible.width, hud.redCollectible.height), hud.redCollectible);
				GUI.TextArea(new Rect(textOffset.x, textOffset.y, 80, 40), rN.ToString());
				
			GUI.EndGroup();
			
			GUI.BeginGroup(gColRect);										//green collectible
				GUI.skin.textArea.normal.textColor = Color.green;
				GUI.DrawTexture(new Rect(0, 0, hud.greenCollectible.width, hud.greenCollectible.height), hud.greenCollectible);
				GUI.TextArea(new Rect(textOffset.x, textOffset.y, 80, 40), gN.ToString());
				
			GUI.EndGroup();
			
			GUI.BeginGroup(bColRect);										//blue collectible
				GUI.skin.textArea.normal.textColor = Color.blue;
				GUI.DrawTexture(new Rect(0, 0, hud.blueCollectible.width, hud.blueCollectible.height), hud.blueCollectible);
				GUI.TextArea(new Rect(textOffset.x, textOffset.y, 80, 40), wN.ToString());
				
			GUI.EndGroup();
			
			
			GUI.BeginGroup(actionRect);										//Action icon
				
				GUI.DrawTexture(new Rect(aX, 0, hud.absorbAction.width, hud.absorbAction.height), actionTexture);
			
			GUI.EndGroup();
			
			if (!avatar.HasOutline){
				
				GUI.BeginGroup(energyRect);
					Rect drawRect = new Rect(0, 0, hud.energyBar.width, hud.energyBar.height);
				
					GUI.DrawTexture(drawRect, hud.energyBar);
				GUI.EndGroup();
			}
			/*
			if (flashyBar){
				GUI.BeginGroup(flashyRect);
					GUI.DrawTexture(new Rect(0, 0, hud.energyBarFlash.width, hud.energyBarFlash.height), hud.energyBarFlash);
				GUI.EndGroup();
			}
			
			GUI.BeginGroup(tankRect);
				for (int i = 0; i < 3; i ++){
					for (int j = 0; j < 5; j ++){
						if (Avatar.tankStates[i, j] == TankStates.None) continue;
						int index = Avatar.tankStates[i, j] == TankStates.Empty? 0 : 1;
						GUI.DrawTexture(new Rect(i * hud.energyTank[0].width, j * hud.energyTank[0].height, hud.energyTank[0].width, hud.energyTank[0].height), hud.energyTank[index]);
					}	
				
				}
			GUI.EndGroup();
			*/
			
			//GUI.Box(new Rect(Screen.width - 128, Screen.height / 2, 96, 32), actionString);
		}
	}
	
	
	public void AddCollectible(Collectible col){
		switch (col.colColour){
		case Couleur.red:
			collectibles.r.Add(col);
			showingCol[0] = true;
			break;
		case Couleur.green:
			collectibles.g.Add(col);
			showingCol[1] = true;
			break;
		case Couleur.blue:
			collectibles.b.Add(col);
			showingCol[2] = true;
			break;
		case Couleur.white:
			collectibles.w.Add(col);
			showingCol[3] = true;
			break;
		default:
			Debug.LogWarning("Not a real collectible.");
			break;
		}
		//Debug.Log(collectibles);
	}
	
	public bool UpdateCollectible(Couleur colour){
		switch (colour){
		case Couleur.red:
			if (rN > collectibles.r.Count){
				rN --;
			}
			if (rN < collectibles.r.Count){
				rN ++;
			}
			
			return rN == collectibles.r.Count;
		case Couleur.green:
			if (gN > collectibles.g.Count){
				gN --;
			}
			if (gN < collectibles.g.Count){
				gN ++;
			}
			
			return gN == collectibles.g.Count;
		case Couleur.blue:
			if (bN > collectibles.b.Count){
				bN --;
			}
			if (bN < collectibles.b.Count){
				bN ++;
			}
			
			return bN == collectibles.b.Count;
		case Couleur.white:
			if (wN > collectibles.w.Count){
				wN --;
			}
			if (wN < collectibles.w.Count){
				wN ++;
			}
			
			return wN == collectibles.w.Count;
		default:
			
			if (cN > roomStats[curRoom].thumbNumber){
				cN --;
				
			}
			if (cN < roomStats[curRoom].thumbNumber){
				cN ++;
			}
			
			return cN == roomStats[curRoom].thumbNumber;
		}
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
			collectibles.held.Add(inQuestion);
		}
		
	}
	
	public void GrabHeldWhiteCols(){
		foreach(Collectible col in collectibles.held){
			if (!col.colour.White){
				Debug.LogWarning("Turns out this one's not white...");
				continue;
			}
			collectibles.w.Add(col);
		}
		collectibles.held.Clear();
	}
	
	public void RemoveCollectibles(Couleur colour, int value, Vector3 pos){
		switch (colour){
		case Couleur.red:
			JettisonCollectibles(collectibles.r, value, pos);
			showingCol[0] = true;
			return;
		case Couleur.green:
			JettisonCollectibles(collectibles.g, value, pos);
			showingCol[1] = true;
			return;
		case Couleur.blue:
			JettisonCollectibles(collectibles.b, value, pos);
			showingCol[2] = true;
			return;
		case Couleur.white:
			JettisonCollectibles(collectibles.w, value, pos);
			showingCol[3] = true;
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
			//inQuestion.transform.position = pos;
			list.Remove(inQuestion);
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
		if (roomStats[curRoom].secretFound){
			roomStats[curRoom].secretComic.gameObject.SetActive(true);
		}
	}
	
	public void CloseComic(){
		Time.timeScale = 1;
		inComic = false;
		
	}
	
	
	public void FindComic(int index){
		//roomStats[curRoom].comics[index].SendMessage("FadeAlpha", 1f);
		
		if (index == roomStats[curRoom].comics.Count){
			
			roomStats[curRoom].secretFound = true;
			//TODO PLAY SUPER ANIMATION OF GETTING A NEW HEALTH CONTAINER
			
			bool foundEmpty = false;
			bool foundNone = false;
			
			for (int i = 0; i < 3; i ++){
				for (int j = 0; j < 5; j ++){
					if (Avatar.tankStates[i, j] == TankStates.Empty && !foundEmpty){
						Avatar.tankStates[i, j] = TankStates.Full;
						foundEmpty = true;
					}
					if (Avatar.tankStates[i, j] == TankStates.None){
						foundNone = true;
						Avatar.tankStates[i, j] = foundEmpty? TankStates.Empty : TankStates.Full;
						break;
					}
				}
				if (foundEmpty && foundNone) break;
			}
			
			if (!foundEmpty && !foundNone){
				Avatar.curEnergy = 100;
				Debug.LogWarning("You fucked up because there isn't enough room for all these tanks. WTH! I Warned you!");
			}
		}
		else{
			roomStats[curRoom].thumbsFound[index] = true;
			roomStats[curRoom].thumbNumber ++;
			showingCol[4] = true;
		}
		
	}
	
	public bool CheckComicStats(){
		
		int i = 0;
		foreach (Comic comic in roomStats[curRoom].comics){
			if (roomStats[curRoom].thumbsFound[i] && !comic.InMySlot){
				comic.InMySlot = true;
				//TODO PLAY ANIMATION FOR COMIC COMPLETE!
			}
				
			i++;
		}
		
		if (roomStats[curRoom].secretFound){
			roomStats[curRoom].secretComic.InMySlot = true;
		}
		
		foreach (Comic c in roomStats[curRoom].comics){
			if (!c.InMySlot){
				return false;
			}
		}
		roomStats[curRoom].comicComplete = true;
								//turns out the comic is successful!
		//TODO PUT COLLECTIBLE ACTIVATION HERE
		//shavatarComicBlock.SetActive(false);
		
		return true;
		
	}
	Avatar.DeathAnim danim;		//avatar's death animation
	public void Death(){
		//Archaic
		/*
		Avatar.tankStates[0, 0] = TankStates.Full;
		Avatar.tankStates[0, 1] = TankStates.Full;
		Avatar.curEnergy = 50;
		Application.LoadLevel(Application.loadedLevel);*/
		danim = new Avatar.DeathAnim();
		danim.PlayDeath(Reset);
		avatar.SendMessage("FadeAlpha", 0f);
		avatar.movement.SetVelocity(Vector2.zero);
		avatar.Gone = true;
		//avatar.renderer.enabled = false;
	}
	
	public void Reset(tk2dAnimatedSprite anim, int index){
		Destroy(danim.go);
		//avatar.renderer.enabled = true;
		avatar.Gone = false;
		avatar.transform.position = curCheckpoint.transform.position;
		avatar.transform.rotation = Quaternion.identity;
		avatar.movement.SetVelocity(Vector2.zero);
		avatar.SetColour(0, 0, 0);
	}
	
	public void NewCheckpoint(Transform cp){
		curCheckpoint = cp;
		GameObject[] cps = GameObject.FindGameObjectsWithTag("checkpoint");
		foreach (GameObject check in cps){
			Checkpoint script = check.GetComponent<Checkpoint>();
			if (check.transform == cp){
				script.Active = true;
			}
			else{
				script.Active = false;
			}
		}
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414


#region Variables et Data
//VARIABLES ENUM



[SerializeAll]
public class ChromatoseManager : MainManager {
	
	
	//Variables Assignation Public
	public tk2dSpriteCollectionData bubbleCollection;
	
	
	//Variables Instance Enum
	private GUIStateEnum _GUIState;
	
	//Avatar Data
	private Avatar avatar;
	private AvatarPointer avatarP;
	private Vector3 _AvatarStartingPos;
	
	//Class & Manager
	public static ChromatoseManager manager; 
	//private ChromaRoomManager _RoomManager;
	//private RoomInstancier _RoomSaver;
	//private HUDManager _HudManager;
	
	private tk2dSprite spriteInfo;
	private GUISkin skin;
	private string _GameName = "CheckpointSave";
	
	//Variables Bool
	private bool _OnPause = false;
	private bool checkedComicStats = false;
	public bool playedCompleteFlourish = false;
	public bool playedSecretFlourish = false;
	public bool givenCols = false;
	public bool animsReady = false;
		public bool AnimsReady{
			set { animsReady = value; }
		}
	private bool inComic = false;
		public bool InComic{
			get{ return inComic; }
			
		}
	public bool _TimeTrialModeActivated = false;
		public bool TimeTrialMode{
			get{return _TimeTrialModeActivated;}
		}
	
	public bool _NoDeathModeActivated = false;
		public bool NoDeathMode{
			get{return _NoDeathModeActivated;}
		}	

	
	
	//Variables Collectible
	private bool _CollAlreadyAdded = false;
		public bool CollAlreadyAdded{
			get{return _CollAlreadyAdded;}
			set{_CollAlreadyAdded = value;}
		}

	
	private int _WhiteCollected = 0;
		public int wCollected{
			get{return _WhiteCollected;}
		}
	private int _RedCollected = 0;
		public int rCollected{
			get{return _RedCollected;}
		}
	private int _BlueCollected = 0;
		public int bCollected{
			get{return _BlueCollected;}
		}
	private int _ComicThumbCollected = 0;
		public int comicCollected{
			get{return _ComicThumbCollected;}
		}
	
	private int _TotalWhiteColl = 0;
	private int _TotalRedColl = 0;
	private int _TotalBlueColl = 0;
	private int _TotalComicThumb = 0;
	
	
	//Variables CheckPoint
	private bool _NewLevel = false;
	private bool _FirstLevelCPDone = false;
		public bool FirstLevelCPDone {
			get{return _FirstLevelCPDone;}
			set{_FirstLevelCPDone = value;}
		}
	private Transform curCheckpoint;
	
	private bool _LevelSaveExist = false; public bool levelSaveExist { get { return _LevelSaveExist; } set { _LevelSaveExist = value; } }
	
	//Variables Listes
	private SpriteFader[] _FaderList;
	
	private int showTiming = 1;
	private float defaultSpeed = 1.2f;
	private int refreshTiming = 75;
	
	
	
#endregion
	

	
#region Initialisation et Setup	(Start)

	void Awake () {
		Setup();
		manager = this;
	}
	void OnLevelWasLoaded(){
		_LevelSaveExist = false;
	}
	
	void Start(){
		Setup ();
	}
	public void Setup(){
			//Initialise pour la premiere la security pour le FirstCP
		_FirstLevelCPDone = false;
		
		if(Application.loadedLevel != 0){
			
			avatar = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
			CreateFirstCheckpoint();
		}
	}


#endregion	
	
#region Update & LateUpdate
	void Update () {
	}

#endregion
		
#region Collectibles Methods	
	public void AddCollectible(Color col){
		
		if(!_CollAlreadyAdded){
			_CollAlreadyAdded = true;
			StartCoroutine(ResetCanGrabCollectibles(0.05f));
			
			if(col == Color.white){
				StatsManager.whiteCollCollected[Application.loadedLevel]++;
				StatsManager.whiteCollDisplayed++;
			}
			else if(col == Color.red){
				StatsManager.redCollCollected[Application.loadedLevel]++;
				StatsManager.redCollDisplayed++;
			}
			else if(col == Color.blue){
				StatsManager.blueCollCollected[Application.loadedLevel]++;
				StatsManager.blueCollDisplayed++;
			}
			else{
				Debug.LogWarning("Not a real collectible.");
			}
			StatsManager.manager.ReCalculateStats();
		}
	}
	public void AddCollectible(Color col, int amount){
		
		if(!_CollAlreadyAdded){
			_CollAlreadyAdded = true;
			StartCoroutine(ResetCanGrabCollectibles(0.05f));
			
			if(col == Color.white){
				StatsManager.whiteCollCollected[Application.loadedLevel] += amount;
				StatsManager.whiteCollDisplayed += amount;
			}
			else if(col == Color.red){
				StatsManager.redCollCollected[Application.loadedLevel] += amount;
				StatsManager.redCollDisplayed += amount;
			}
			else if(col == Color.blue){
				StatsManager.blueCollCollected[Application.loadedLevel] += amount;
				StatsManager.blueCollDisplayed += amount;
			}
			else{
				Debug.LogWarning("Not a real collectible.");
			}
			StatsManager.manager.ReCalculateStats();
		}
	}
	
	public int GetCollectibles(Color color){
		
		if(color == Color.white){
			return StatsManager.whiteCollDisplayed;
		}
		else if(color == Color.red){
			return StatsManager.redCollDisplayed;
		}
		else if(color == Color.blue){
			return StatsManager.blueCollDisplayed;
		}
		else{
			Debug.Log("CANT CHECK THIS COLOR?");
			return 0;
		}
	}
	
	public void RemoveCollectibles(Color color, int amount, Vector3 pos){
		
		
		if(color == Color.white){
			StatsManager.whiteCollDisplayed -= amount;
			BlowWhiteColl(amount, pos);			
		}
		else if(color == Color.red){
			StatsManager.redCollDisplayed -= amount;
			ShootRedCollOnMini(amount, pos);
			Debug.Log("Remove "+amount);
		}
		else if(color == Color.blue){
			StatsManager.blueCollDisplayed -= amount;
			BlowBlueColl(amount, pos);
		}
		else{
			Debug.Log("Cant delete this collectable, check color");
		}
	}
	public void RemoveCollectibles(Color color, int amount, Vector3 pos, EndBoss_DataBase bossData){
		
		if(color == Color.red){
			StatsManager.redCollDisplayed -= amount;
			bossData.nbWhiteColRecu += amount;
			ShootRedCollOnMini(amount, pos);
			Debug.Log("Remove " + amount + ", le boss est rendu a " + bossData.nbWhiteColRecu + " de redColl Recu");
		}
		else{
			Debug.Log("Cant delete this collectable, check color");
		}
	}
#endregion	
	
#region Comics Stuff
																			//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
																			//<-------------COMICS AND STUFF!-------------->
																			//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	public void AddComicThumb(){
		_ComicThumbCollected++;
	}

#endregion	
	
#region Fonctions Diverses
	Avatar.DeathAnim danim;		//avatar's death animation
	public void Death(){
		
		if(!_LevelSaveExist){
			
			avatar = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
			if(!_NoDeathModeActivated){
				MusicManager.soundManager.PlaySFX(6);
				danim = new Avatar.DeathAnim();
				danim.PlayDeath(Reset);
				//avatar.SendMessage("FadeAlpha", 0f);
				GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().movement.SetVelocity(Vector2.zero);
				StartCoroutine(OnDeath(0.15f));
				StartCoroutine(RestartRoom());
				ResetColl();
				ResetComicCounter();
				avatar.EmptyingBucket();
				avatar.CancelOutline();
				avatar.Gone = true;
				
				switch(_AvatarScript.avaTypeAccess){
				case _AvatarTypeEnum.avatar:
					_AvatarScript.avaTypeAccess = _AvatarTypeEnum.shavatar;
					break;
				case _AvatarTypeEnum.shavatar:
					_AvatarScript.avaTypeAccess = _AvatarTypeEnum.avatar;
					break;
				}
			}
			else{
				MusicManager.soundManager.PlaySFX(6);
				danim = new Avatar.DeathAnim();
				danim.PlayDeath(Reset);
				//avatar.SendMessage("FadeAlpha", 0f);
				avatar.movement.SetVelocity(Vector2.zero);
				StartCoroutine(OnDeath(0.15f));
				avatar.CancelOutline();
				avatar.Gone = true;
				ResetColl();
				ResetComicCounter();
				avatar.EmptyingBucket();
				switch(_AvatarScript.avaTypeAccess){
				case _AvatarTypeEnum.avatar:
					_AvatarScript.avaTypeAccess = _AvatarTypeEnum.shavatar;
					break;
				case _AvatarTypeEnum.shavatar:
					_AvatarScript.avaTypeAccess = _AvatarTypeEnum.avatar;
					break;
				}
				//Application.LoadLevel(2);
			}
			
			//avatar.renderer.enabled = false;
		}
		else{
			MusicManager.soundManager.PlaySFX(6);
			danim = new Avatar.DeathAnim();
			danim.PlayDeath(LoadCheckpoint);
			avatar.movement.SetVelocity(Vector2.zero);
		}
	}
	
	public void LoadCheckpoint(tk2dAnimatedSprite anim, int index){
		Destroy(danim.go);
		LevelSerializer.Resume();
	}
	
	
	public void DeathByBoss(){
		Debug.Log("Mort par le Boss");
			//Play le SFX de la Mort
		MusicManager.soundManager.PlaySFX(6);
		
			//Play l'animd e la Mort et Reset le BossLevel a la fin
		danim = new Avatar.DeathAnim();
		danim.PlayDeath(ResetBossLevel);
		avatar.Gone = true;
	}
	
	public void ResetBossLevel(tk2dAnimatedSprite anim, int index){
		Debug.Log("Reset le BossLevel");
		
			//Find the Boss
		GameObject boss = GameObject.FindGameObjectWithTag("Boss");
		
			//Destruction de l'anim de la Mort et Reset des variable en rapport
		Destroy(danim.go);
		avatar.Gone = false;		
	
			//Destruction des Collectibles inScene
		GameObject[] redColInBossLvl = GameObject.FindGameObjectsWithTag("collectible");
		foreach (GameObject redC in redColInBossLvl){
			Destroy(redC.gameObject);
		}
		
			//Destruction des bossBullet inScene
		GameObject[] bossBulletInBossLvl = GameObject.FindGameObjectsWithTag("bossBullet");
		foreach (GameObject bossBullet in bossBulletInBossLvl){
			Destroy(bossBullet.gameObject);
		}
		
			//Reset de la StateMachine du Boss, Reset de la position du boss et Reset du Round
		EndBoss_FSM.fsm.PerformTransition(Transition.tBoss_ReturnIdle);
		if(boss.transform.position != boss.GetComponent<EndBoss_DataBase>().placeForBoss[0].position){
			boss.transform.position = boss.GetComponent<EndBoss_DataBase>().placeForBoss[0].position;
				//Securite d'Angle et Rotation
			boss.transform.rotation = Quaternion.identity;
		}
		boss.GetComponent<EndBoss_DataBase>().round = 0;
		
			//Remise des redColl a l'Avatar
		StatsManager.redCollDisplayed = boss.GetComponent<EndBoss_DataBase>().redCollAtStart;
		
			//Repositionment de l'avatar, Reset de Velocity, Reset de AfterImage, Reset de couleur & Set le CannotControl
		avatar.movement.SetVelocity(Vector2.zero);
		avatar.transform.position = boss.GetComponent<EndBoss_DataBase>().restartSpot.position;
		avatar.transform.rotation = Quaternion.identity;
		avatar.CancelOutline();
		avatar.EmptyingBucket();
		avatar.CannotControlFor(false, 0f);
		
			//Change l'apparence de l'avatar en Shavatar ou Vice-Versa
		switch(_AvatarScript.avaTypeAccess){
		case _AvatarTypeEnum.avatar:
			_AvatarScript.avaTypeAccess = _AvatarTypeEnum.shavatar;
			break;
		case _AvatarTypeEnum.shavatar:
			_AvatarScript.avaTypeAccess = _AvatarTypeEnum.avatar;
			break;
		}
		/*
			//Mise-en Place de la fenetre "Start" et attente pour l'Input du Joueur
		HUDManager.hudManager.ResetTitleBool();
		HUDManager.hudManager.guiState = GUIStateEnum.OnStart;
		*/
	}
	
	public void Reset(tk2dAnimatedSprite anim, int index){
		avatar = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		Destroy(danim.go);
		avatar.Gone = false;
		avatar.transform.position = curCheckpoint.transform.position;
		avatar.transform.rotation = Quaternion.identity;
		avatar.movement.SetVelocity(Vector2.zero);

	}
	
	void TriggerQuestionMark(){
		
		GameObject question = GameObject.FindWithTag("questionMark");
		if (question)
			question.SendMessage("Trigger");
	}
#endregion	
	
#region Fonctions CheckPoint
	
	public void CreateFirstCheckpoint(){
		GameObject fChkp = Instantiate(Resources.Load("pre_checkpoint"), avatar.transform.position, Quaternion.identity)as GameObject;
		fChkp.GetComponent<BoxCollider>().enabled = false;
		fChkp.GetComponent<MeshRenderer>().enabled = false;
		//fChkp.renderer.enabled = false;
		StartCoroutine(NewFirstCheckPoint(fChkp.transform));
	}
	
	IEnumerator NewFirstCheckPoint(Transform cp){
		yield return new WaitForSeconds(0.1f);
		curCheckpoint = cp;
		Debug.Log("CheckSaved");
		foreach(SpriteFader _SFader in _FaderList){
			_SFader.SaveState();
		}
	}
	
	public void NewCheckpoint(Transform cp){
		curCheckpoint = cp;
		MusicManager.soundManager.PlaySFX(2);
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
		
		foreach(SpriteFader _SFader in _FaderList){
			_SFader.SaveState();
			//Debug.Log("SaveState in CP");
		}
	}
	
	public void CheckPointHere(Transform cp){
		curCheckpoint = cp;
	}
	
	public void SaveRoom(){
		LevelSerializer.SaveGame(_GameName);
		Debug.Log("Save");
	}
	public void LoadRoom(){
		foreach(var sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName]) { 
         	LevelSerializer.LoadNow(sg.Data);
			Time.timeScale = 1;
    	} 
		Debug.Log("Load");
	}
	
#endregion	

	
#region A Classer
		//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
		//<-------------FONCTION DU CHU!--------------->
		//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>	
	
	
	public void CheckNewfaderList(){
		_FaderList = FindObjectsOfType(typeof(SpriteFader)) as SpriteFader[];
		//OptiManager.manager.OptimizeZone();
	}
	
	void CheckStartPos(){
		if(_Avatar){
			_AvatarStartingPos = _Avatar.transform.position;
		}
	}
	
	void CalculeCollectiblesInLevel(){

		foreach (GameObject col in GameObject.FindGameObjectsWithTag("collectible")){
			if(col.GetComponent<Collectible2>().colorCollectible == Collectible2._ColorCollectible.White){
				_TotalWhiteColl++;
			}
			else if(col.GetComponent<Collectible2>().colorCollectible == Collectible2._ColorCollectible.Red){
				_TotalRedColl++;
			}
			else if(col.GetComponent<Collectible2>().colorCollectible == Collectible2._ColorCollectible.Blue){
				_TotalBlueColl++;
			}
			else{
				Debug.Log ("A Collectible doesn't have a Color or a Script");
			}
		}	
	}
	
	void BlowWhiteColl(int amount, Vector3 pos){
		
		MusicManager.soundManager.PlaySFX(8);
		for(int i = 0; i < amount; i++){
			Vector2 randomVelocity = Random.insideUnitCircle.normalized * Random.Range(40, 65);
			Vector3 randomPos = _Avatar.transform.position + (Vector3)randomVelocity;
			randomPos.z = -5;
			GameObject wCol = Instantiate(Resources.Load("pre_Collectible"), randomPos, Quaternion.identity)as GameObject;
			wCol.GetComponent<Collectible2>().effect = true;
			wCol.GetComponent<Collectible2>().colorCollectible = Collectible2._ColorCollectible.White;
			StartCoroutine(DelaiToBlowColl(0.5f, wCol));
		}
	}
	
	void BlowBlueColl(int amount, Vector3 pos){
		for(int i = 0; i < amount; i++){
			Vector2 randomVelocity = Random.insideUnitCircle.normalized * Random.Range(40, 65);
			Vector3 randomPos = _Avatar.transform.position + (Vector3)randomVelocity;
			GameObject bCol = Instantiate(Resources.Load("pre_Collectible"), randomPos, Quaternion.identity)as GameObject;
			bCol.GetComponent<Collectible2>().effect = true;
			bCol.GetComponent<Collectible2>().colorCollectible = Collectible2._ColorCollectible.Blue;
			MusicManager.soundManager.PlaySFX(12);
			StartCoroutine(DelaiToBlowColl(0.5f, bCol));
		}
	}
	void ShootRedCollOnMini(int amount, Vector3 pos){
		Debug.Log("Shoot");
		for(int i = 0; i < amount; i++){
			StartCoroutine(ShootRed(i*0.5f, pos));
			
		}
	}
	
	public void ResetComicCounter(){
		//_TotalComicThumb = _RoomManager.UpdateTotalComic();
		//_ComicThumbCollected = 0;
	}
	public void ResetColl(){
		_RedCollected = 0;
		_WhiteCollected = 0;
		_BlueCollected = 0;
	}
	public void ResetPos(){
		avatar.transform.position = _AvatarStartingPos;
	}
	public void SwitchGUIToBlank(){
		_GUIState = GUIStateEnum.Nothing;
	}
	public void SwitchGUIToWait(){
		_GUIState = GUIStateEnum.OnStart;
	}

	
#endregion	
		
#region CoRoutine
	IEnumerator ShootRed(float delai, Vector3 pos){
		yield return new WaitForSeconds(delai);
		GameObject rCol = Instantiate(Resources.Load("pre_Collectible"), avatar.transform.position, Quaternion.identity)as GameObject;
		Vector3 newPos = new Vector3(pos.x + Random.Range(-50, 50), pos.y + Random.Range(-50, 50), 0);
		rCol.GetComponent<Collectible2>().redCollectorPos = newPos;
		rCol.GetComponent<Collectible2>().effect = true;
		rCol.GetComponent<Collectible2>().colorCollectible = Collectible2._ColorCollectible.Red;
	}
	IEnumerator OnDeath(float _wait){
		yield return new WaitForSeconds(_wait);
		
		foreach(SpriteFader _SFader in _FaderList){
			_SFader.ResetState();
		}
		
		//LoadRoom();
	}	
	IEnumerator ResetCanGrabCollectibles(float _wait){
		yield return new WaitForSeconds(_wait);
		//Debug.Log("collectiblesResetStart");
		_CollAlreadyAdded = false;
	}
	IEnumerator DelaiToAddComic(float _delai){
		yield return new WaitForSeconds(_delai);
		//cN ++;
	}
	IEnumerator DelaiToBlowColl(float delai, GameObject col){
		yield return new WaitForSeconds(delai);
		col.GetComponent<Collectible2>().PayEffect();
	}
	IEnumerator RestartRoom(){
		yield return new WaitForSeconds(0.25f);
		if(_RoomInstancier){
			_RoomInstancier.LoadRoom();
		}
	}
#endregion
	
}

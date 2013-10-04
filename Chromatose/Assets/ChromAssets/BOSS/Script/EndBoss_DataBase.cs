using UnityEngine;
using System.Collections;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

public class EndBoss_DataBase : MonoBehaviour {

	
#region DATABASE VARIABLES
	
	//Setup Variables
	public float fireRate = 5; 
	public int nbRoundMax = 3;
	
	public static float bulletSpeed = 100;
	public static float lifeDistance = 1500;
	
	
	//Setup Position
	public Transform[] shooters;
	public Transform[] placeForBoss;
	public Transform restartSpot;
	
	//GameObject Container
	public GameObject bossBullet;
	
	
	public bool activeTint = false;
	
	//Data Variables
	private Avatar _AvatarScript;
	private GameObject _Boss;
	private EndBoss_FSM _StateMachine;
	private ChromatoseCamera _Chromera;
	private Transform _AvatarT;
	
	private float _Knockback = 50f;
	private float _FadeRate = 0.05f;
	
	private int _NbWhiteColRecu = 0; public int nbWhiteColRecu { get { return _NbWhiteColRecu; } set { _NbWhiteColRecu = value; } }
	private int requiredPayment = 10;
	
	private tk2dAnimatedSprite[] _MyFlames;
	private tk2dAnimatedSprite _MainAnim;

	//private List<tk2dAnimatedSprite> _DyingFlames = new List<tk2dAnimatedSprite>();
	//private List<float> _DyingAlphas = new List<float>();
	
	private string flameName = "";		//default: flameName = "flame";
	private int flameNumber;			//default: flameNumber = 11;	
	
	private bool _NoFlameGame = false;
	private bool _DoubleWave = false;
	private bool _PlayerInCombatZone = false;
	private bool _PlayerInPayZone = false;
	private bool _DiesOnImpact = true;
	private bool _IsBlackFlame = true;
	private bool _BeingExtinguished = false;
	private bool _AlreadyShooten = false;
	private bool _ReadyToBlow = false;
	private bool _CanForward = false;
	

	private float _BossJourneyLength;
	private float _JourneyStartTime;
	
	
	//DATA Variables
	private int _RedCollAtStart = 0; public int redCollAtStart { get { return _RedCollAtStart; } set { _RedCollAtStart = value; } }
	
	
	//Security Variables
	private int _Round = 0; public int round { get { return _Round; } set { _Round = value; } }
	
	//Accesseur GetSet
	public bool PlayerInCombatZone{
		get{return _PlayerInCombatZone;}
		set{_PlayerInCombatZone = value;}
	}
	
#endregion
	
#region For Initialisation
	void Start () {
		_MainAnim = GetComponent<tk2dAnimatedSprite>();
		_AvatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		_Boss = this.gameObject;
		_StateMachine = this.gameObject.GetComponent<EndBoss_FSM>();
		
		if(!_MainAnim.IsPlaying("bossIdle")){
			StartMainAnim();
		}
		
		_BossJourneyLength = Vector3.Distance(placeForBoss[0].position, placeForBoss[1].position);
		
	}
#endregion
		
#region UPDATE
	void Update () {
	
	}
#endregion
	
#region Public Function
	public bool CheckRound(){
		if(_Round >= nbRoundMax){return true;}
		else{return false;}
	}
	public bool CheckRedColRecu(){
		if(_NbWhiteColRecu >= requiredPayment && _ReadyToBlow){
			return true;
		}
		else{return false;}
	}
	public bool CheckCanForward(){
		if(_CanForward){
			//StartCoroutine(ResetCanForward());
			return true;
		}
		else{
			return false;
		}
	}
	
		//Animation Public Play
	public void PlayAnim(string animName){
		if(!_MainAnim.IsPlaying(animName)){
			_MainAnim.Play(animName);
		}
	}
	
				//FIRE-SHOOT FUNCTION
		//Single Shoot
	public void SingleShoot(int shooterIndex){
		GameObject newBullet = Instantiate(Resources.Load("pre_BossBullet"), shooters[shooterIndex].position, Quaternion.identity)as GameObject;
	}	
	
	public void SingleShoot(int shooterIndex, bool random){
		GameObject newBullet = Instantiate(Resources.Load("pre_BossBullet"), shooters[shooterIndex].position, Quaternion.identity)as GameObject;
		newBullet.GetComponent<BossBullet>().shootType = shootTypeEnum.atAvaPos;
	}
		//Tire Regulier
	public void FullLineShoot(){
		for(int i = 0; i < shooters.Length; i++){
			SingleShoot(i);
		}
	}
	public void VShapeShoot(){
		SingleShoot(2);
		StartCoroutine(DelayedSingleShoot(1, 0.75f));
		StartCoroutine(DelayedSingleShoot(3, 0.75f));
		StartCoroutine(DelayedSingleShoot(0, 1.5f));
		StartCoroutine(DelayedSingleShoot(4, 1.5f));
	}
	public void WShapeShoot(){
		StartCoroutine(DelayedSingleShoot(0, 0.75f));
		SingleShoot(1);
		StartCoroutine(DelayedSingleShoot(2, 0.75f));
		SingleShoot(3);
		StartCoroutine(DelayedSingleShoot(4, 0.75f));
	}
	public void ThreeWaveShoot(){
		
		
	}
	public void RandomShoot(bool doubleShot){
		int randomShooter = Random.Range(0, 5);
		int randomShooter2 = Random.Range(0, 5);
		while (randomShooter2 == randomShooter){
			randomShooter2 = Random.Range(0, 5);
		}
		float randomDelai = Random.Range(0f, 3f);
		StartCoroutine(DelayedSingleShoot(randomShooter, randomDelai, true));
		if(doubleShot)StartCoroutine(DelayedSingleShoot(randomShooter2, randomDelai, true));
	}
	
		//Public Translate
	public void Forward(){
		if(_CanForward){
			float distCovered = (Time.time - _JourneyStartTime) * 100; //10 = speed a changer au besoin
			float fracJourney = distCovered / _BossJourneyLength;
			gameObject.transform.position = Vector3.Lerp(placeForBoss[0].position, placeForBoss[1].position, fracJourney);
			if(Vector3.Distance(gameObject.transform.position, placeForBoss[1].position) < 10){
				ResetJourneyStartTime();
				_CanForward = false;
			}
		}
		else{
			float distCovered = (Time.time - _JourneyStartTime) * 100; //1 = speed a changer au besoin
			float fracJourney = distCovered / _BossJourneyLength;
			gameObject.transform.position = Vector3.Lerp(placeForBoss[1].position, placeForBoss[0].position, fracJourney);
			if(Vector3.Distance(placeForBoss[0].position, gameObject.transform.position) < 10){
				NextRound();
			}
		}
	}
	
	public void Die(){
		StartCoroutine(DelayedDie());
	}
	
#endregion

#region Private Function
		//Main Trigger Assignation Function [PayZONE]
	void OnTriggerEnter(Collider other){
		if(other.tag != "avatar")return;
		_PlayerInPayZone = true;
		
		
	}
	void OnTriggerStay(Collider other){
		if(other.tag != "avatar")return;
		if(!_PlayerInPayZone){
			_PlayerInPayZone = true;
		}
		if(!_AvatarScript.wantFightBoss){
			HUDManager.hudManager.UpdateAction(Actions.Release, Payment);
			_AvatarScript.requieredPayment = requiredPayment.ToString();
			_AvatarScript.wantFightBoss = true;
		}
	}
	void OnTriggerExit(Collider other){
		if(other.tag != "avatar")return;

	}
	
	
		//Animation Function
	void StartMainAnim(){
		_MainAnim.Play("bossIdle");
	}
	
	
	void Payment(){
		
		if(StatsManager.redCollDisplayed >= requiredPayment){
			if(!_AlreadyShooten){
				ChromatoseManager.manager.RemoveCollectibles(Color.red, requiredPayment, this.transform.position, this);
				_AlreadyShooten = true;
				StartCoroutine(ResetShooten(2.5f));
				StartCoroutine(DelaiBeforeBlow());
			}
		}
		
		print("On Paie le Boss");
	}
	
	void ResetJourneyStartTime(){
		_JourneyStartTime = Time.time;
	}
	
	void NextRound(){
		EndBoss_FSM.fsm.PerformTransition(Transition.tBoss_ReturnIdle);
		_Round++;
		_NbWhiteColRecu = 0;
		print("BossTransition - NEXT ROUND - ReturnToIdle");
	}
	
	
	
#endregion

#region COROUTINE
		//Delayed Single Shoot
	IEnumerator DelayedSingleShoot(int shooterIndex, float delai){
		yield return new WaitForSeconds(delai);
			//SHOOT!!
		SingleShoot(shooterIndex);
	}
	IEnumerator DelayedSingleShoot(int shooterIndex, float delai, bool random){
		yield return new WaitForSeconds(delai);
			//SHOOT!!
		SingleShoot(shooterIndex, random);
	}
	IEnumerator ResetShooten(float delai){
		yield return new WaitForSeconds(delai);
		_AlreadyShooten = false;
	}
	IEnumerator DelaiBeforeBlow (){
		yield return new WaitForSeconds(2.5f);
		_ReadyToBlow = true;
		StartCoroutine(CanResetBlow());
	}
	IEnumerator CanResetBlow(){
		yield return new WaitForSeconds(5.5f);
		_CanForward = true;
		_JourneyStartTime = Time.time;
		_ReadyToBlow = false;
	}
	IEnumerator DelaiToForward(){
		yield return new WaitForSeconds(5.0f);
		_CanForward = true;
	}
	IEnumerator ResetCanForward(){
		yield return new WaitForSeconds(1.0f);
		_CanForward = false;
	}
	
	IEnumerator DelayedDie(){
		yield return new WaitForSeconds(2.0f);
		Destroy(this.gameObject);
	}
	
	
#endregion

	
	
	
}

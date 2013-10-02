using UnityEngine;
using System.Collections;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

public class EndBoss_DataBase : MonoBehaviour {

	
#region DATABASE VARIABLES
	
	//Setupping Variables
	public int nbRoundMax = 3;
	
	
	public Transform[] _FirstWave;
	public Transform[] _SecondWave;
	public Transform[] _ThirdWave;
	
	//Data Variables
	private ChromatoseManager _Chromanager;
	private Avatar _AvatarScript;
	private ChromatoseCamera _Chromera;
	private Transform _AvatarT;
	
	private float _Knockback = 50f;
	private float _FadeRate = 0.05f;
	
	private int _NbRedColRecu = 0;
	private int _Round = 0;
	
	private tk2dAnimatedSprite _MainAnim; public tk2dAnimatedSprite mainAnim { get { return _MainAnim; } set { _MainAnim = value; } }
	private tk2dAnimatedSprite[] _MyFlames;

	//private List<tk2dAnimatedSprite> _DyingFlames = new List<tk2dAnimatedSprite>();
	//private List<float> _DyingAlphas = new List<float>();
	
	private string flameName = "";		//default: flameName = "flame";
	private int flameNumber;			//default: flameNumber = 11;	
	
	private bool _NoFlameGame = false;
	private bool _DoubleWave = false;
	private bool _PlayerInCombatZone = false;
	private bool _DiesOnImpact = true;
	private bool _IsBlackFlame = true;
	private bool _BeingExtinguished = false;
	
	private bool _CanStart = false; public bool canStart { get { return _CanStart; } set { _CanStart = value; } }

	
	//Speed Variables
	private float _FlameSpeed = 1;
	
	
	//Accesseur GetSet
	public bool PlayerInCombatZone{
		get{return _PlayerInCombatZone;}
		set{_PlayerInCombatZone = value;}
	}
	
#endregion
	
#region For Initialisation
	void Start () {
			//Call le Setup
		StartCoroutine(Setup(0.01f));
			//Si l'animatin du Boss ne joue pas, Start l'Anim
		if(!_MainAnim.IsPlaying("bossIdle")){
			StartMainAnimation();
		}
	}
#endregion
		
#region UPDATE
	void Update () {
	
	}
#endregion
	
#region Public Function
		//Renvoi un Int representant le Round
	public bool CheckRoundNb(){
		if(_Round >= nbRoundMax){return true;}
		else{return false;}
	}
#endregion

#region Private Function
		//Demarre l'Animation du Boss
	void StartMainAnimation(){
		_MainAnim.Play();
	}
#endregion

#region COROUTINE
		//Delayed Setup
	IEnumerator Setup(float delai){
		yield return new WaitForSeconds(delai);
		_MainAnim = GetComponent<tk2dAnimatedSprite>();
	}
#endregion

	
	
	
}

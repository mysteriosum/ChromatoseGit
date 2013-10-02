using UnityEngine;
using System.Collections;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

public class EndBoss_DataBase : MonoBehaviour {

	
#region DATABASE VARIABLES
	//Data Variables
	private ChromatoseManager _Chromanager;
	private Avatar _AvatarScript;
	private ChromatoseCamera _Chromera;
	private Transform _AvatarT;
	
	private float _Knockback = 50f;
	private float _FadeRate = 0.05f;
	
	private int _NbWhiteColRecu = 0;
	
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
	

	
	/*

	*/
	
	
	
	
	
	//Speed Variables
	private float _FlameSpeed = 1;
	
	
	//Security Variables
	private int _Round = 0;
	
	//Accesseur GetSet
	public bool PlayerInCombatZone{
		get{return _PlayerInCombatZone;}
		set{_PlayerInCombatZone = value;}
	}
	
#endregion
	
#region For Initialisation
	void Start () {
	
	}
#endregion
		
#region UPDATE
	void Update () {
	
	}
#endregion
	
#region Public Function

#endregion

#region Private Function

#endregion

#region COROUTINE

#endregion

	
	
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteFader : MonoBehaviour {
	
	
	//commm
	public GameObject[] spritesIn;
	public GameObject[] spritesOut;
	
	public bool unchanging = false;
	
	
	
	private List<GameObject> _spritesIn = new List<GameObject>();
	private List<GameObject> _spritesOut = new List<GameObject>();
	
	private float fadeRate = 0.1f;
	
	private float inAlpha;
	private float outAlpha;
	
	private float heldInAlpha;
	private float heldOutAlpha;
	
	private float _TpheldInAlpha;
	private float _TpheldOutAlpha;
	
	private Camera _Chromera;
	private Color _InitialBGColor;
	
	private bool change;
	private bool setup = false;
	private bool _ImBlacky = false;

	
	void OnLevelWasLoaded(){
		StartCoroutine(Setup ());	
	}
	
	
	void Start () {
		
		StartCoroutine(Setup ());
	}
		
	IEnumerator Setup(){
		yield return new WaitForSeconds(0.2f);
				
		inAlpha = -fadeRate;
		outAlpha = 1 + fadeRate;
		foreach (GameObject sprite in spritesIn){
			if(sprite != null){
				sprite.BroadcastMessage("FadeAlpha", inAlpha, SendMessageOptions.DontRequireReceiver);
				_spritesIn.Add(sprite);
			}
		}
		foreach (GameObject sprite in spritesOut){
			if(sprite != null){
				_spritesOut.Add(sprite);
			}
		}
		
		_Chromera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		_InitialBGColor = _Chromera.backgroundColor;
		
		setup = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(!setup)return;
		if (!change) return;
		foreach (GameObject go in _spritesIn){
			if(go != null){
				go.BroadcastMessage("FadeAlpha", inAlpha, SendMessageOptions.DontRequireReceiver);
			}
		}
		foreach (GameObject go in _spritesOut){
			if(go != null){
				go.BroadcastMessage("FadeAlpha", outAlpha, SendMessageOptions.DontRequireReceiver);
			}
		}
		
		change = false;
	}
	
	void OnTriggerStay(Collider other){
		if (other.tag == "avatar"){
			//Debug.Log("Trigger:");
			
		}
	}
	
	void Out(){
		outAlpha = Mathf.Min(outAlpha + fadeRate, 1 + fadeRate);
		inAlpha = Mathf.Max(inAlpha - fadeRate, -fadeRate);
		
		change = true;
		
		if(_ImBlacky){
			_ImBlacky = false;
			StartCoroutine(ReturnWhite());
		}
	}
	
	void In(){
		
		outAlpha = Mathf.Max(outAlpha - fadeRate, -fadeRate);
		inAlpha = Mathf.Min(inAlpha + fadeRate, 1 + fadeRate);
		
		if(!_ImBlacky){
			_ImBlacky = true;
			StartCoroutine(GoBlack());
		}
		
		change = true;
	}
	
	void Enter(GameObject go){
		if (unchanging) return;
		_spritesIn.Add(go);
		_spritesOut.Remove(go);
	}
	
	void Exit(GameObject go){
		if (unchanging) return;
		_spritesOut.Add(go);
		_spritesIn.Remove(go);
	}
	
	void LoadState(){
		if (inAlpha == heldInAlpha && outAlpha == heldOutAlpha){
			return;
		}
		inAlpha = heldInAlpha;
		outAlpha = heldOutAlpha;
		
		change = true;
		//Debug.Log("Yepp, I Load It");
	}
	void LoadStateForTP(){
		if (inAlpha == _TpheldInAlpha && outAlpha == _TpheldOutAlpha){
			return;
		}
		inAlpha = _TpheldInAlpha;
		outAlpha = _TpheldOutAlpha;
		
		change = true;
		//Debug.Log("Yepp, I Load It For TP");
	}
	
	
	
	
	public void SaveState(){
		heldInAlpha = inAlpha;
		heldOutAlpha = outAlpha;
		//Debug.Log("Saving The Alpha");
	}
	public void SaveStateForTP(){
		_TpheldInAlpha = inAlpha;
		_TpheldOutAlpha = outAlpha;
		//Debug.Log("Saving The AlphaForTP");
	}
	public void ResetState(){
		LoadState();
	}
	public void ResetStateForTP(){
		LoadStateForTP();
	}
	
	
	
	
	
	IEnumerator GoBlack(){
		yield return new WaitForSeconds(0.05f);
		_Chromera.backgroundColor = Color.black;
	}
	IEnumerator ReturnWhite(){
		yield return new WaitForSeconds(0.05f);
		_Chromera.backgroundColor = _InitialBGColor;
	}
}

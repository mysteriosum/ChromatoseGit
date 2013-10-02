using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteFader2 : MonoBehaviour {
	
	public tk2dStaticSpriteBatcher[] spritesIn;
	public tk2dStaticSpriteBatcher[] spritesOut;
	
	public bool unchanging = false;
	
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
	private bool _ImBlacky = false;

	
	void OnLevelWasLoaded(){
		_Chromera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		_InitialBGColor = _Chromera.backgroundColor;
		
		foreach (tk2dStaticSpriteBatcher sprite in spritesIn){
			
		}
		foreach (tk2dStaticSpriteBatcher sprite in spritesOut){
			
		}	
	}
	
	
	void Start () {
		
		_Chromera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		_InitialBGColor = _Chromera.backgroundColor;
		
		foreach (tk2dStaticSpriteBatcher sprite in spritesIn){
			
		}
		foreach (tk2dStaticSpriteBatcher sprite in spritesOut){
			
		}	
	}


	void LateUpdate () {
		if (!change) return;
		foreach (tk2dStaticSpriteBatcher go in spritesIn){
			if(go != null){
				//go.BroadcastMessage("FadeAlpha", inAlpha, SendMessageOptions.DontRequireReceiver);
			}
		}
		foreach (tk2dStaticSpriteBatcher go in spritesOut){
			if(go != null){
				//go.BroadcastMessage("FadeAlpha", outAlpha, SendMessageOptions.DontRequireReceiver);
			}
		}
		
		change = false;
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

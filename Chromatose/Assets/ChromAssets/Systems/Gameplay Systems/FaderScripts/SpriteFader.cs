using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteFader : MonoBehaviour {
	
	public GameObject[] spritesIn;
	public GameObject[] spritesOut;
	
	public bool unchanging = false;
	
	
	private List<GameObject> _spritesIn = new List<GameObject>();
	private List<GameObject> _spritesOut = new List<GameObject>();
	
	private bool willPlayOut = false;
	private bool willPlayIn = false;
	
	private float fadeRate = 0.1f;
	
	private float inAlpha;
	private float outAlpha;
	
	private float heldInAlpha;
	private float heldOutAlpha;
	
	private bool change;
	// Use this for initialization
	void Start () {
		inAlpha = -fadeRate;
		outAlpha = 1 + fadeRate;
		
		
		foreach (GameObject sprite in spritesIn){
			sprite.BroadcastMessage("FadeAlpha", inAlpha, SendMessageOptions.DontRequireReceiver);
			_spritesIn.Add(sprite);
		}
		foreach (GameObject sprite in spritesOut){
			_spritesOut.Add(sprite);
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (!change) return;
		foreach (GameObject go in _spritesIn){
			
			go.BroadcastMessage("FadeAlpha", inAlpha, SendMessageOptions.DontRequireReceiver);
		}
		foreach (GameObject go in _spritesOut){
			go.BroadcastMessage("FadeAlpha", outAlpha, SendMessageOptions.DontRequireReceiver);
			
		}
		
		change = false;
	}
	
	void OnTriggerStay(Collider other){
		if (other.name == "Avatar"){
			//Debug.Log("Trigger:");
			
		}
	}
	
	void Out(){/*
		if (inAlpha >= 1){
			foreach (GameObject go in spritesIn){
				go.BroadcastMessage("StopAndResetFrame", SendMessageOptions.DontRequireReceiver);
			}
			Debug.Log("Stopping in");
		}*/
		outAlpha = Mathf.Min(outAlpha + fadeRate, 1 + fadeRate);
		inAlpha = Mathf.Max(inAlpha - fadeRate, -fadeRate);
		
		change = true;
		/*
		if (outAlpha >= 1 && willPlayOut){
			willPlayOut = false;
			foreach (GameObject go in spritesOut){
				go.BroadcastMessage("Play", SendMessageOptions.DontRequireReceiver);
			}
			Debug.Log("Playing out");
		}
		else if (outAlpha < 1){
			willPlayOut = true;
		}*/
	}
	
	void In(){
		/*if (outAlpha >= 1){
			foreach (GameObject go in spritesOut){
				go.BroadcastMessage("StopAndResetFrame", SendMessageOptions.DontRequireReceiver);
			}
			Debug.Log("Stopping out");
		}*/
		
		
		outAlpha = Mathf.Max(outAlpha - fadeRate, -fadeRate);
		inAlpha = Mathf.Min(inAlpha + fadeRate, 1 + fadeRate);
		/*
		if (inAlpha >= 1 && willPlayIn){
			willPlayIn = false;
			foreach (GameObject go in spritesIn){
				go.BroadcastMessage("Play", SendMessageOptions.DontRequireReceiver);
			}
			Debug.Log("Playing In");
		}
		else if (inAlpha < 1){
			willPlayIn = true;
		}*/
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
	
	void SaveState(){
		heldInAlpha = inAlpha;
		heldOutAlpha = outAlpha;
	}
	void LoadState(){
		if (inAlpha == heldInAlpha && outAlpha == heldOutAlpha){
			return;
		}
		inAlpha = heldInAlpha;
		outAlpha = heldOutAlpha;
		
		change = true;
	}
}

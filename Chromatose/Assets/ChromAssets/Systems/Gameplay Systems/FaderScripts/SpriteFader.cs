using UnityEngine;
using System.Collections;

public class SpriteFader : MonoBehaviour {
	
	public tk2dSprite[] spritesIn;
	public tk2dSprite[] spritesOut;
	
	float fadeRate = 0.1f;
	
	float inAlpha;
	float outAlpha;
	bool change;
	// Use this for initialization
	void Start () {
		inAlpha = -fadeRate;
		outAlpha = 1 + fadeRate;
		
		foreach (tk2dSprite sprite in spritesIn){
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, inAlpha);
			sprite.animation.Stop ();
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (!change) return;
		foreach (tk2dSprite sprite in spritesIn){
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, inAlpha);
		}
		foreach (tk2dSprite sprite in spritesOut){
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, outAlpha);
			
		}
		
		change = false;
	}
	
	void OnTriggerStay(Collider other){
		if (other.name == "Avatar"){
			//Debug.Log("Trigger:");
			
		}
	}
	
	void Out(){
		outAlpha = Mathf.Min(outAlpha + fadeRate, 1 + fadeRate);
		inAlpha = Mathf.Max(inAlpha - fadeRate, -fadeRate);
		
		change = true;
	}
	
	void In(){
		
		outAlpha = Mathf.Max(outAlpha - fadeRate, -fadeRate);
		inAlpha = Mathf.Min(inAlpha + fadeRate, 1 + fadeRate);
		
		change = true;
	}
}

using UnityEngine;
using System.Collections;

public class ParticleComic : MonoBehaviour {
	
	private tk2dAnimatedSprite _mainAnim;
	
	// Use this for initialization
	void Start () {
		_mainAnim = GetComponent<tk2dAnimatedSprite>();
		_mainAnim.Play();
	}
	
	// Update is called once per frame
	void Update () {
		_mainAnim.animationCompleteDelegate = DestroyThis;
	}
	
	void DestroyThis(tk2dAnimatedSprite sprite, int clipId){
		Destroy (this.gameObject);
	}
}

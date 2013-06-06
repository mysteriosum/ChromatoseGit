using UnityEngine;
using System.Collections;

public class ComicThumb : MonoBehaviour {
	
	public int myIndex;
	private tk2dAnimatedSprite anim;
	
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<tk2dAnimatedSprite>();
		anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag != "avatar") return;
		anim.Play();
		anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
		anim.animationCompleteDelegate = Trigger;
		//	TODO : put comic thumb get animation here
	}
	
	void OnTriggerExit(Collider other){
		if (other.tag != "avatar") return;
		anim.Play();
		anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
		anim.animationCompleteDelegate = Trigger;
		//	TODO : put comic thumb bounce animation here
	}
	
	void Trigger(tk2dAnimatedSprite spranim, int index){
		
		ChromatoseManager.manager.FindComic(myIndex);
		
		
		transform.Translate(Vector3.forward * -3000);
	}
	
}

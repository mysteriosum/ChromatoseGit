using UnityEngine;
using System.Collections;

public class InfoButton : MonoBehaviour {
	
	public string openName;
	public string closeName;
	private tk2dAnimatedSprite anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<tk2dAnimatedSprite>();
		if (!anim){
			Debug.LogWarning("Info button has no animation component!");
			gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag != "avatar") return;
		
		if (anim.IsPlaying(closeName)){
			anim.Play(openName, anim.CurrentClip.frames.Length * anim.CurrentClip.fps - anim.ClipTimeSeconds);
			anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
			return;
		}
		
		anim.Play(openName);
		anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
		return;
	
	}
	
	
	void OnTriggerExit(Collider other){
		if (other.tag != "avatar") return;
		
		if (anim.IsPlaying(openName)){
			anim.Play(closeName, anim.CurrentClip.frames.Length * anim.CurrentClip.fps - anim.ClipTimeSeconds);
			anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
			return;
		}
		
		anim.Play(closeName);
		anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
		return;
	}
}

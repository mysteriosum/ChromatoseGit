using UnityEngine;
using System.Collections;

public class ComicThumb : MonoBehaviour {
	
	public int myIndex;
	private tk2dAnimatedSprite anim;
	private tk2dAnimatedSprite myCircle;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<tk2dAnimatedSprite>();
		anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
		myCircle = (Instantiate(Resources.Load("animref_ing2")) as GameObject).GetComponent<tk2dAnimatedSprite>();
		myCircle.Play("comicThumb_pickedUp");
		myCircle.Stop();
		myCircle.transform.position = transform.position;
		myCircle.renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag != "avatar") return;
		myCircle.Play("comicThumb_pickedUp");
		myCircle.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
		myCircle.animationCompleteDelegate = Trigger;
		myCircle.renderer.enabled = true;
		//	TODO : put comic thumb get animation here
	}
	
	void OnTriggerExit(Collider other){
		if (other.tag != "avatar") return;
		myCircle.renderer.enabled = false;
		myCircle.StopAndResetFrame();
		//	TODO : put comic thumb bounce animation here
	}
	
	void Trigger(tk2dAnimatedSprite spranim, int index){
		
		ChromatoseManager.manager.FindComic(myIndex);
		
		myCircle.renderer.enabled = false;
		transform.Translate(Vector3.forward * -3000);
	}
	
}

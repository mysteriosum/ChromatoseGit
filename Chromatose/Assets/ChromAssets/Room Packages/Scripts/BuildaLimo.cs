using UnityEngine;
using System.Collections;

public class BuildaLimo : Buildable {
	private GameObject limo;
	// Use this for initialization
	void Start () {
		Setup();
	}
	
	// Update is called once per frame
	void Update () {
		Checks();
	}
	
	protected override void Setup(){
		limo = GameObject.Find("LimoZeen");
		poof = Instantiate(poof) as GameObject;
		poof.SetActive(false);
		anim = poof.GetComponent<tk2dAnimatedSprite>();
		anim.animationCompleteDelegate = SendLimo;	
	}
	
	protected override void Destruct(){
		goingToDestroy = false;
		
		poof.SetActive(true);
		poof.renderer.enabled = false;
		poof.transform.position = transform.position;
		poof.transform.rotation = transform.rotation;
		
		anim.Play();
		anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
	}
	
	private void SendLimo(tk2dAnimatedSprite sprite, int index){
		limo.SendMessage("Trigger");
		gameObject.RemoveComponent(this.GetType());
	}
}

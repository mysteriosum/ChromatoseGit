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
	
	}
	
	protected override void Setup(){
		limo = GameObject.Find("LimoZeen");
		poof = Instantiate(poof) as GameObject;
		poof.SetActive(false);
		anim = poof.GetComponent<tk2dAnimatedSprite>();
		anim.animationEventDelegate = NpcFuckOff;
		anim.animationCompleteDelegate = SendLimo;	
	}
	
	protected override void Destroy(){
		
		poof.SetActive(true);
		poof.renderer.enabled = false;
		poof.transform.position = transform.position;
		poof.transform.rotation = transform.rotation;
		
		anim.Play();
		anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
	}
	
	private void NpcFuckOff(tk2dAnimatedSprite sprite, tk2dSpriteAnimationClip clip, tk2dSpriteAnimationFrame frame, int frameNum){
		myNPCs[0].SendMessage("FuckOff");
	}
	
	private void SendLimo(tk2dAnimatedSprite sprite, int index){
		limo.SendMessage("Trigger");
		gameObject.RemoveComponent(this.GetType());
	}
}

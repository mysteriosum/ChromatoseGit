using UnityEngine;
using System.Collections;

public class FireBooth : TollBooth {
	public string newAnimName = "New Clip";
	// Use this for initialization
	void Start () {
		Setup();
	}
	
	// Update is called once per frame
	void Update () {
		if (triggered){
			
		}
		Check(Couleur.red);
	}
	
	void Animate(){
		anim.Play(anim.GetClipByName(newAnimName), 0);
		anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
		Debug.Log("YEAAANIMATE");
	}
}

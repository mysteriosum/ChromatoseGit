using UnityEngine;
using System.Collections;

public class FadeOutOnTrigger : MonoBehaviour {
	
	private tk2dSprite spriteInfo;
	private bool fadingOut = false;
	private float alpha = 1;
	// Use this for initialization
	void Start () {
		spriteInfo = GetComponent<tk2dSprite>();
		spriteInfo.SendMessage("FadeAlpha", alpha);
	}
	
	// Update is called once per frame
	void Update () {
		if (fadingOut && alpha > 0){
			alpha -= 0.01f;
			spriteInfo.SendMessage("FadeAlpha", alpha);
		}
	}
	
	void Trigger(){
		fadingOut = true;
	}
}

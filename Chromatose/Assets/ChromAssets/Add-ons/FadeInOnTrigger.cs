using UnityEngine;
using System.Collections;

public class FadeInOnTrigger : MonoBehaviour {
	private tk2dSprite spriteInfo;
	private bool fadingIn = false;
	private float alpha = 0;
	// Use this for initialization
	void Start () {
		spriteInfo = GetComponent<tk2dSprite>();
		spriteInfo.SendMessage("FadeAlpha", alpha);
	}
	
	// Update is called once per frame
	void Update () {
		if (fadingIn && alpha < 1f){
			alpha += 0.01f;
			spriteInfo.SendMessage("FadeAlpha", alpha);
		}
	}
	
	void Trigger(){
		fadingIn = true;
	}
}

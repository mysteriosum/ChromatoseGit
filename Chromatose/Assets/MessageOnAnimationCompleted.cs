using UnityEngine;
using System.Collections;

public class MessageOnAnimationCompleted : MonoBehaviour {
	public GameObject[] targets;
	public string message = "Disable";
	tk2dAnimatedSprite anim;
	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<tk2dAnimatedSprite>();
		anim.animationCompleteDelegate = Kill;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Kill(tk2dAnimatedSprite sprite, int ClipId){
		foreach (GameObject target in targets){
			target.SendMessage(message);
		}
	}
}

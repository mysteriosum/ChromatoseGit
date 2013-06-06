using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnergyLine : MonoBehaviour {
	
	private tk2dAnimatedSprite[] chilluns;
	private List<tk2dAnimatedSprite> registered = new List<tk2dAnimatedSprite>();
	private int minDist = 30;
	private Transform avatarT;
	public int hpProvided = 50;
	
	private bool finished = false;
	
	private bool active = false;
	public bool Active{
		get { return active; }
		set { active = value; }
	}
	
	// Use this for initialization
	void Start () {
		chilluns = GetComponentsInChildren<tk2dAnimatedSprite>(true);
		avatarT = GameObject.FindWithTag("avatar").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (!active || finished) return;
		bool foundOne = false;
		bool foundAll = false;
		foreach(tk2dAnimatedSprite anim in chilluns){
			
			if (Vector3.Distance(anim.transform.position, avatarT.position) < minDist){
				foundOne = true;
				
				if (registered.Contains(anim)) continue;		//Check each one to see if the Avatar's still on track...
				
				registered.Add(anim);							//... but if you find one that hasn't been added to the list, add it!
				//anim.Play(registered.Count > 1? registered[0].ClipTimeSeconds : 0);
				anim.Play();
				if (registered.Count == chilluns.Length)
					foundAll = true;
				break;
			}
		}
		
		if (!foundOne){
			active = false;
			registered.Clear();
			BroadcastMessage("StopAndResetFrame");
			Debug.Log("Didn't find, reseting");
		}
		
		if (foundAll){
			registered[registered.Count - 1].animationCompleteDelegate = EndAndProvideComfort;
		}
	}
	
	public void EndAndProvideComfort(tk2dAnimatedSprite sprite, int index){
		Avatar.curEnergy += hpProvided;
		ChromatoseManager.manager.Healed();
		active = false;
		foreach (tk2dAnimatedSprite anim in chilluns){
			anim.Play("energyLines_complete");
			anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
			anim.gameObject.AddComponent("FadeOutOnTrigger");
			anim.animationCompleteDelegate = FadeOut;
			anim.collider.enabled = false;
		}
		
	}
	
	void FadeOut(tk2dAnimatedSprite sprite, int index){
		sprite.gameObject.SendMessage("Trigger");
		Debug.Log("Should prolly trigger");
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnergyLine : MonoBehaviour {
	
	private tk2dAnimatedSprite[] chilluns;
	private List<tk2dAnimatedSprite> registered = new List<tk2dAnimatedSprite>();
	private int minDist = 30;
	private Transform avatarT;
	public int hpProvided = 50;
	
	
	
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
		if (!active) return;
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
		}
		
		if (foundAll){
			Avatar.curEnergy += hpProvided;
			Destroy(gameObject);
		}
	}
}

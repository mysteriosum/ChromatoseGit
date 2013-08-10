using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiTracks : MonoBehaviour {
	public AudioClip[] tracks;
	private List<AudioSource> mySources = new List<AudioSource>();
	
	// Use this for initialization
	void Start () {
		foreach(AudioClip track in tracks){
			AudioSource temp = gameObject.AddComponent<AudioSource>();
			temp.clip = track;
			mySources.Add(temp);
			temp.loop = true;
			temp.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			if(mySources[0].volume ==1){
				mySources[0].volume = 0;
			}
			else mySources[0].volume = 1;
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)){
			if(mySources[1].volume ==1){
				mySources[1].volume = 0;
			}
			else mySources[1].volume = 1;
		}		
		if(Input.GetKeyDown(KeyCode.Alpha3)){
			if(mySources[2].volume ==1){
				mySources[2].volume = 0;
			}
			else mySources[2].volume = 1;
		}		
	}
}

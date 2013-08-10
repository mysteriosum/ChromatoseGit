using UnityEngine;
using System.Collections;

public class MusicMixer : MonoBehaviour {
	public AudioClip[] musiques;
	public float crossfadeDelay = 1.0f;
	public float increment = 0.05f;
	public float fadeIntensity = 0.02f;
	private int compteur = 0;
	private int compteurNext = 1;
	private AudioSource monAudioSource1= new AudioSource();
	private AudioSource monAudioSource2= new AudioSource();
	private int mode = 0;
	private int maxMode = 1;
	private bool crossfade = false;
	private float fadeValue =0.0f;
	// Use this for initialization
	void Start () {
		monAudioSource1 =  this.gameObject.AddComponent<AudioSource>();
		monAudioSource2 =  this.gameObject.AddComponent<AudioSource>();
		monAudioSource2.volume = 0;
		monAudioSource2.playOnAwake = false;
		monAudioSource1.clip = musiques[compteur];
		monAudioSource1.Play();
		monAudioSource2.clip = musiques[compteurNext];
		monAudioSource2.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		if (crossfade){
			CrossFade();
		}
		if(Input.GetKeyDown(KeyCode.KeypadEnter)){
			crossfade = true;	
			//compteurNext = compteur+1;
			monAudioSource2.Play();
		}
		
		if(Input.GetMouseButtonDown(0)){
			switch(mode){
				case 0:
				monAudioSource1.pitch -= increment;
				break;
				/*case 1:
					monAudioSource.volume = 
				break;*/
			}
			
		}
		if(Input.GetMouseButtonDown(1)){
			switch(mode){
				case 0:	
					monAudioSource1.pitch +=  increment;
				break;
				case 1:
				
				break;
			}
		
			
		}	
		if(Input.GetMouseButtonDown(2)){
			switch(mode){
				case 0:
				monAudioSource1.pitch = 1.0f;
				break;
				case 1:
				
				break;
			}

		}	
		
		if(Input.GetKey(KeyCode.Tab)){
			mode++;
			if(mode>maxMode){
				mode = 0;
			}
			
		}
		
	}
	
	void OnGUI(){
		GUI.Box(new Rect(0,0,25,25),mode.ToString());
	}
	
	void CrossFade(){
		//Debug.Log("Devrait crossfader" + fadeValue);
		monAudioSource1.volume = 1.0f -fadeValue;
		monAudioSource2.volume = fadeValue;
		fadeValue+=fadeIntensity;
		if(fadeValue>= 1.0f){
			monAudioSource1.Stop();
			crossfade = false;
			
		}
	}
}

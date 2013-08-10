using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MusicEvents : MonoBehaviour {
	
	public MusicTimeEvent[] events;
	
	private Queue<MusicTimeEvent> myEvents = new Queue<MusicTimeEvent>(); // We use a Queue, because it's FIFO
	
	private float nextEvent = 0f;
	private bool on = false;
	private float timeWentGo = -1f; //-1 = not initialised
	//private int compteur = 0;
	
	private MusicTimeEvent.action monAction;
	private Stack<AudioSource> mySources = new Stack<AudioSource>(); // FILO
	
	void Start(){
		if(events.Length>0){
			nextEvent = events[0].timeOfEvent;
			for(int i = 0;i < events.Length ; i++){
				myEvents.Enqueue (events[i]);
				
			}
		}
		

	}
	
	void Update(){
		if(on){
			Debug.Log("Je suis a On");
			if(Time.time - timeWentGo >= nextEvent){
				//Perform Action
				Debug.Log("The time is now!");
				MusicTimeEvent temp = myEvents.Dequeue();
				monAction = temp.actionAPerformer;
				
				if(myEvents.Count>0) {
					nextEvent = myEvents.Peek().timeOfEvent;
					
					
				} else on = false;
				
				Debug.Log("Next event in : " + nextEvent.ToString());
				switch(monAction){
					case MusicTimeEvent.action.addLayer:
						Debug.Log("let there be drums!");
						AudioSource tempAudio = gameObject.AddComponent<AudioSource>();
						mySources.Push(tempAudio);
						mySources.Peek().clip = temp.newClip;
						mySources.Peek().volume = temp.newLayerVolume;
						mySources.Peek().loop = true;
						mySources.Peek().Play();
					
					break;
					case MusicTimeEvent.action.removeLayer:
						Debug.Log("Devrait stopper!");
						Destroy(mySources.Pop());				;
					break;
					case MusicTimeEvent.action.startNewMusic:
						Debug.Log("Devrait partir!");
						AudioSource tempAudio2 = gameObject.AddComponent<AudioSource>();
						mySources.Push(tempAudio2);
					
						mySources.Peek().clip = temp.newClip;
						mySources.Peek().volume = temp.newLayerVolume;
						mySources.Peek().loop = true;
						mySources.Peek().Play();
					break;
				}
			}
		}
		
		if(Input.GetKeyDown(KeyCode.Insert)){
			on = true;
		}
	}
	
	/*public enum mode{
		crossfade, startLoop
	}
	
	
	
	/*public void TriggerEvent(mode myMode){
		switch(myMode){
			case mode.crossfade:
			break;
			
			case mode.startLoop:
			break;
		}

	}*/
	
	public void SetOn(){
		this.on = true;
		timeWentGo = Time.time;
	}
	//temp
	

}

using UnityEngine;
using System.Collections;

[System.Serializable]
public class MusicTimeEvent {
	
	public enum action{
		startNewMusic, addLayer, removeLayer
	}
	
	public float timeOfEvent;
	public action actionAPerformer;
	public float crossFadeTime = 1f;
	public float newLayerVolume = 1f;
	public AudioClip newClip;
	
/*	private bool go = false;
	private MusicEvents myME;
	private float timeWentGo = -1f; //-1 = not initialised
	
	
	public void activate(){
		timeWentGo = Time.time;
	}
	
	public void SetME(MusicEvents myMe){
		this.myME = myMe;
	}
	
	public bool ActivateAndCanRemove(){
			if(Time.time - timeWentGo >= timeOfEvent){
				switch(actionAPerformer){
					case action.addLayer:
						myME.TriggerEvent(MusicEvents.mode.startLoop);
						
					break;
					
					case action.startNewMusic:
						myME.TriggerEvent(MusicEvents.mode.crossfade);
						break;
				}
			return true;
			}
		else return false;
	}*/
		
}

using UnityEngine;
using System.Collections;

public class InputManager : MainManager {
	
	
	
	private Avatar avatarScript;
	
	void Start(){
		if(Application.loadedLevel != 0){
			avatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		}
	}
	
	

	void FixedUpdate () {
		
			//PAUSE -- NE DOIT PAS ETRE ACTIF DANS LES MENU
		if(Input.GetKeyDown(KeyCode.Escape)){
			Pause();
		}	
	
		
		
			//MOUVEMENT DE L'AVATAR
		if (_CanControl){
				//INPUT DU FORWARD
			if(Input.GetKey(KeyCode.O)){
				getForward = true;
			}
				//INPUT DU PREMIER FORWARD SEULEMENT -- JE L'UTILISE POUR LE SON DE L'ACCEL
			if(Input.GetKeyDown(KeyCode.O)){
				MusicManager.soundManager.PlaySFX(0, 0.6f);
			}
				//INPUT DU LEFT
			if(Input.GetKey(KeyCode.Q)){
				getLeft = true;
			}
				//INPUT DU RIGHT
			if(Input.GetKey(KeyCode.W)){
				getRight = true;
			}			
		}		
	}
}

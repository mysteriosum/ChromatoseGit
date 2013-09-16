using UnityEngine;
using System.Collections;

public class InputManager : MainManager {
	
	
	
	private Avatar avatarScript;
	
	void Start(){
		SetController();
	}
	
	

	void FixedUpdate () {
		if(avatarScript==null){
			SetController();
		}
		
			//ON S'ASSURE QU"ON EST PAS DANS LE MAINMENU
		if(Application.loadedLevel!=0){
				//PAUSE -- NE DOIT PAS ETRE ACTIF DANS LES MENU
			if(Input.GetKeyDown(KeyCode.Escape)){
				//Pause();
				HUDManager.hudManager.StartHudCloseSequence();
			}	
		
				//MOUVEMENT DE L'AVATAR
			if (_CanControl){
					//INPUT DU FORWARD){
				avatarScript.getforward = Input.GetKey(KeyCode.O);
				
					//INPUT DU PREMIER FORWARD SEULEMENT -- JE L'UTILISE POUR LE SON DE L'ACCEL
				if(Input.GetKeyDown(KeyCode.O)){
					MusicManager.soundManager.PlaySFX(0, 0.6f);
				}
				
					//SI LE KEYBOARD SETTING EST SUR QWERTY
				if(keyboardType == MainManager._KeyboardTypeEnum.QWERTY){
						//INPUT DU LEFT
					avatarScript.getleft = Input.GetKey(KeyCode.Q);
		
						//INPUT DU RIGHT
					avatarScript.getright = Input.GetKey(KeyCode.W);
				}
					//SI LE KEYBOARD SETTING EST SUR AZERTY
				else{
						//INPUT DU LEFT
					avatarScript.getleft = Input.GetKey(KeyCode.A);
		
						//INPUT DU RIGHT
					avatarScript.getright = Input.GetKey(KeyCode.Z);
				}
			}
		}
		
		
			//SPACE BAR
		if(Input.GetKeyDown(KeyCode.Space)){
			if(HUDManager._GUIState == GUIStateEnum.OnStart){
				HUDManager._GUIState = GUIStateEnum.Interface;
				GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().CanControl();
				MusicManager.soundManager.PlaySFX(19);
				MusicManager.soundManager.CheckLevel();
			}
		}		
	}
	
	void SetController(){
		//yield return new WaitForSeconds(0.1f);
		if(Application.loadedLevel != 0){
			avatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		}
	}
}

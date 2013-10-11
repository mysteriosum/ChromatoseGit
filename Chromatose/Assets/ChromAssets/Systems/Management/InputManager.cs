using UnityEngine;
using System.Collections;

public class InputManager : MainManager {
	
	
	
	private Avatar avatarScript;
	
	void Start(){
		SetController();
	}
	
	

	void Update () {
		if(avatarScript==null){
			SetController();
		}
		
			//ON S'ASSURE QU"ON EST PAS DANS LE MAINMENU
		if(currentLevel!=0){
				//PAUSE -- NE DOIT PAS ETRE ACTIF DANS LES MENU
			if(Input.GetKey(KeyCode.Escape)){
				//Pause();
				HUDManager.hudManager.StartHudCloseSequence();
			}
			
			if(_CanControl){
		
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
		}
		
		
			//SPACE BAR
		if(Input.GetKeyDown(KeyCode.Space)){
			Debug.Log("SpaceBar Pressed");
			MusicManager.soundManager.PlaySFX(52);
			
		}
		
			//P pour les menu et Start
		if(Input.GetKeyDown(KeyCode.P)){
			if(HUDManager._GUIState == GUIStateEnum.OnStart){
				HUDManager._GUIState = GUIStateEnum.Interface;
				GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().CanControl();
				MusicManager.soundManager.PlaySFX(19);
				MusicManager.soundManager.CheckLevel();
				HUDManager.hudManager.StartHudOpenSequence();
				Debug.Log("OutStart");
			}
		}
		
		
			// Cheat Key
		if(_CheatActivated){
			if(currentLevel == 0)return;
				//Color
			if(Input.GetKeyDown(KeyCode.Keypad7)){
				GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().EmptyingBucket();
			}
			if(Input.GetKeyDown(KeyCode.Keypad8)){
				GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().FillBucket(Color.red);
			}
			if(Input.GetKeyDown(KeyCode.Keypad9)){
				GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().FillBucket(Color.blue);
			}
			
				//Add Coll
			if(Input.GetKeyDown(KeyCode.Keypad4)){
				ChromatoseManager.manager.AddCollectible(Color.white);
			}
			if(Input.GetKeyDown(KeyCode.Keypad5)){
				ChromatoseManager.manager.AddCollectible(Color.red);
			}
			if(Input.GetKeyDown(KeyCode.Keypad6)){
				ChromatoseManager.manager.AddCollectible(Color.blue);
			}
			
				//Remove Coll
			if(Input.GetKeyDown(KeyCode.Keypad1)){
				ChromatoseManager.manager.RemoveCollectibles(Color.white, 1);
			}
			if(Input.GetKeyDown(KeyCode.Keypad2)){
				ChromatoseManager.manager.RemoveCollectibles(Color.red, 1);
			}
			if(Input.GetKeyDown(KeyCode.Keypad3)){
				ChromatoseManager.manager.RemoveCollectibles(Color.blue, 1);
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

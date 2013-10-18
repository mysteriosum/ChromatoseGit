using UnityEngine;
using System.Collections;

public class ChuHudButton : MonoBehaviour {

	public enum buttonTypeEnum{
		Back, Credit, Options, Statistics, GameMode
	}
	
	public buttonTypeEnum buttonType;
	
	private Camera menuCam;
	private tk2dSprite mainSprite;
	

	void Start () {
		menuCam = Camera.mainCamera;
		mainSprite = GetComponent<tk2dSprite>();
		StartCoroutine(LateSetup());
	}
	
	// Update is called once per frame
	void Update () {
		
		if(menuCam == null){
			menuCam = Camera.mainCamera;
		}
		
		if (Input.GetMouseButtonDown(0)){
            Ray ray = menuCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (collider.Raycast(ray, out hitInfo, 1.0e8f))
            {
				if (!Physics.Raycast(ray, hitInfo.distance - 0.01f)){
					switch(buttonType){
					case buttonTypeEnum.Back:
						MusicManager.soundManager.PlaySFX(42);
						if(HUDManager.hudManager.menuWindows == _MenuWindowsEnum.LevelSelectionWindows){
							HUDManager.hudManager.menuWindows = _MenuWindowsEnum.MainMenu;
							HUDManager.hudManager.DesactiveBackButton();
						}
						else if(HUDManager.hudManager.menuWindows == _MenuWindowsEnum.CreditWindows){
							HUDManager.hudManager.menuWindows = _MenuWindowsEnum.LevelSelectionWindows;
						}
						else if(HUDManager.hudManager.menuWindows == _MenuWindowsEnum.OptionWindows){
							HUDManager.hudManager.menuWindows = _MenuWindowsEnum.LevelSelectionWindows;
							HUDManager.hudManager.DesactiveOptBG();
							HUDManager.hudManager.onErase = false;
						}
						else if(HUDManager.hudManager.menuWindows == _MenuWindowsEnum.Stats){
							HUDManager.hudManager.menuWindows = _MenuWindowsEnum.LevelSelectionWindows;
						}
						break;
					case buttonTypeEnum.Credit:
						MusicManager.soundManager.PlaySFX(38);
						HUDManager.hudManager.menuWindows = _MenuWindowsEnum.CreditWindows;
						HUDManager.hudManager.DesactiveOptBG();
						break;
					case buttonTypeEnum.Options:
						MusicManager.soundManager.PlaySFX(38);
						HUDManager.hudManager.menuWindows = _MenuWindowsEnum.OptionWindows;
						HUDManager.hudManager.ActiveOptBG();
						break;
					case buttonTypeEnum.Statistics:
						MusicManager.soundManager.PlaySFX(38);
						StatsManager.manager.ReCalculateStats();
						HUDManager.hudManager.menuWindows = _MenuWindowsEnum.Stats;
						HUDManager.hudManager.DesactiveOptBG();
						break;
					case buttonTypeEnum.GameMode:
						MusicManager.soundManager.PlaySFX(38);
						break;
					}
					HUDManager.hudManager.DesactiveButton();
					mainSprite.SetSprite(0);
				}
            }
        }
	}
	
	void OnMouseEnter(){
		int rndFXOver = Random.Range(0,3);
		switch(rndFXOver){
		case 0:
			MusicManager.soundManager.PlaySFX(35);
			break;
		case 1:
			MusicManager.soundManager.PlaySFX(36);
			break;
		case 2:
			MusicManager.soundManager.PlaySFX(37);
			break;
		}
	}
	
	void OnMouseOver(){
		mainSprite.SetSprite(1);
	}
	void OnMouseExit(){
		mainSprite.SetSprite(0);
	}
	
	IEnumerator LateSetup(){
		yield return new WaitForSeconds(0.1f);
		
	}
}

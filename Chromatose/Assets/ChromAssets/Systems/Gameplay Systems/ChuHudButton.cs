using UnityEngine;
using System.Collections;

public class ChuHudButton : MonoBehaviour {

	public enum buttonTypeEnum{
		Back, Credit, Options, Statistics
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
		
		
		if (Input.GetMouseButtonDown(0)){
            Ray ray = menuCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (collider.Raycast(ray, out hitInfo, 1.0e8f))
            {
				if (!Physics.Raycast(ray, hitInfo.distance - 0.01f)){
					switch(buttonType){
					case buttonTypeEnum.Back:
						
						if(HUDManager.hudManager.menuWindows == _MenuWindowsEnum.LevelSelectionWindows){
							HUDManager.hudManager.menuWindows = _MenuWindowsEnum.MainMenu;
							HUDManager.hudManager.DesactiveBackButton();
						}
						else if(HUDManager.hudManager.menuWindows == _MenuWindowsEnum.CreditWindows){
							HUDManager.hudManager.menuWindows = _MenuWindowsEnum.LevelSelectionWindows;
						}
						else if(HUDManager.hudManager.menuWindows == _MenuWindowsEnum.OptionWindows){
							HUDManager.hudManager.menuWindows = _MenuWindowsEnum.LevelSelectionWindows;
						}
						else if(HUDManager.hudManager.menuWindows == _MenuWindowsEnum.Stats){
							HUDManager.hudManager.menuWindows = _MenuWindowsEnum.LevelSelectionWindows;
						}
						break;
					case buttonTypeEnum.Credit:
						HUDManager.hudManager.menuWindows = _MenuWindowsEnum.CreditWindows;
						break;
					case buttonTypeEnum.Options:
						HUDManager.hudManager.menuWindows = _MenuWindowsEnum.OptionWindows;
						break;
					case buttonTypeEnum.Statistics:
						HUDManager.hudManager.menuWindows = _MenuWindowsEnum.Stats;
						break;
					}
					HUDManager.hudManager.DesactiveButton();
				}
            }
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

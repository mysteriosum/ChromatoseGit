using UnityEngine;
using System.Collections;

public class ChuKeyboardButton : MonoBehaviour {

	public enum keyboardImageEnum{
		qwerty, azerty
	}	
	
	public keyboardImageEnum keyboardImage;
	
	private tk2dSprite mainSprite;
	private Camera menuCam;
	
	private Vector3 scaledVec;
	private Vector3 defaultVec;
	
	void Start () {
		
		mainSprite = GetComponent<tk2dSprite>();
		menuCam = Camera.mainCamera;
		
		scaledVec = new Vector3(1.2f, 1.2f, 1.2f);
		defaultVec = new Vector3(1, 1, 1);
			
		switch(keyboardImage){
		case keyboardImageEnum.qwerty:
			mainSprite.SetSprite(0);
			break;
		case keyboardImageEnum.azerty:
			mainSprite.SetSprite(1);
			break;
		}
	}

	void Update () {
		
		if (Input.GetMouseButtonDown(0)){
            Ray ray = menuCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (collider.Raycast(ray, out hitInfo, 1.0e8f))
            {
				if (!Physics.Raycast(ray, hitInfo.distance - 0.01f)){
					switch(keyboardImage){
					case keyboardImageEnum.qwerty:
						MainManager._MainManager.keyboardType = MainManager._KeyboardTypeEnum.QWERTY;
						break;
					case keyboardImageEnum.azerty:
						MainManager._MainManager.keyboardType = MainManager._KeyboardTypeEnum.AZERTY;
						break;
					}
					HUDManager.hudManager.DesactiveKeyboardButton();
					HUDManager.hudManager.menuWindows = _MenuWindowsEnum.LevelSelectionWindows;
				}
            }
        }
	}
	
	void OnMouseOver(){
		transform.localScale = scaledVec;
	}
	void OnMouseExit(){
		transform.localScale = defaultVec;
	}
}

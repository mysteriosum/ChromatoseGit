using UnityEngine;
using System.Collections;

public class ChuButton : MonoBehaviour {
	
	public int levelIndex;
	
	private tk2dSprite mainSprite;
	private Camera menuCam;
	
	private bool started = false;

	
	void Start () {
		mainSprite = GetComponent<tk2dSprite>();
		menuCam = Camera.mainCamera;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(menuCam == null){
			menuCam = Camera.mainCamera;
		}
		
		if(!started){
			StartCoroutine(CheckUnlockable());
			started = true;
		}
		
		if (Input.GetMouseButtonDown(0)){
            Ray ray = menuCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (collider.Raycast(ray, out hitInfo, 1.0e8f))
            {
				if (!Physics.Raycast(ray, hitInfo.distance - 0.01f)){
					MainManager._MainManager.LoadALevel(levelIndex);
					HUDManager.hudManager.DesactiveKeyboardButton();
					HUDManager.hudManager.DesactiveButton();
					HUDManager.hudManager.DesactiveBackButton();
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
	
	IEnumerator CheckUnlockable(){
		yield return new WaitForSeconds(0.2f);
		if(StatsManager.levelUnlocked[levelIndex - 1] == true){
			mainSprite.SetSprite(0);
		}
		else if(StatsManager.levelDoned[levelIndex - 1] == true){
			mainSprite.SetSprite(2);
		}
		else{
			mainSprite.SetSprite(0);
		}
	}	
}

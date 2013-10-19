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
	
	void OnLevelWasLoaded(){
		StartCoroutine(CheckUnlockable());
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
				if (!Physics.Raycast(ray, hitInfo.distance - 0.01f) && StatsManager.levelUnlocked[levelIndex - 1]){	
					MusicManager.soundManager.PlaySFX(38);
					MainManager._MainManager.LoadALevel(levelIndex);
					HUDManager.hudManager.DesactiveKeyboardButton();
					HUDManager.hudManager.DesactiveButton();
					HUDManager.hudManager.DesactiveBackButton();
				}
            }
        }
	}
	
	void OnMouseEnter(){
		if(StatsManager.levelUnlocked[levelIndex - 1]){
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
	}
	
	void OnMouseOver(){
		if(StatsManager.levelUnlocked[levelIndex - 1] == true){
			mainSprite.SetSprite(1);
		}
	}
	void OnMouseExit(){
		if(StatsManager.levelDoned[levelIndex - 1] == true){
			mainSprite.SetSprite(3);
		}
		else if(StatsManager.levelUnlocked[levelIndex - 1] == true){
			mainSprite.SetSprite(0);
		}
		else{
			mainSprite.SetSprite(2);
		}
	}
	
	public void ExtCheckUnlockable(){
		StartCoroutine(CheckUnlockable());
	}
	
	IEnumerator CheckUnlockable(){
		yield return new WaitForSeconds(0.2f);
		if(StatsManager.levelDoned[levelIndex - 1] == true){
			mainSprite.SetSprite(3);
		}
		else if(StatsManager.levelUnlocked[levelIndex - 1] == true){
			mainSprite.SetSprite(0);
		}
		else{
			mainSprite.SetSprite(2);
		}
	}	
}

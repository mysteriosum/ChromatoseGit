using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
	enum MenuStates{
		Load,
		Logo,
		IntroAnim,
		Title,
		ModuleSelect,
		Options,
		LevelSelect1,
		Stats,
		Comics,
	}
	private MenuStates menuState;
	
	public GameObject startButton;
	public GameObject optionsButton;
	public GameObject moduleButton1;
	public GameObject tutorialButton;
	public GameObject creditsButton;
	public GameObject quitButton;
	public GameObject[] levelSelectButtons = new GameObject[9];
	
	private string levelButtonName = "btn_level";
	private string tutorialButtonName = "btn_tutorial";
	
	void Start () {
		menuState = MenuStates.LevelSelect1;
	}
	
	// Update is called once per frame
	void Update () {
		switch (menuState){
		case MenuStates.LevelSelect1:
			bool chosen = false;
			int i = 0;
			foreach(GameObject button in levelSelectButtons){
				if (button.collider.bounds.Contains(Input.mousePosition) && !chosen){
					button.SendMessage("SetSprite", levelButtonName + (i + 1).ToString() + "_2");
					Select(button);
					chosen = true;
					if (Input.GetMouseButtonDown(0)){
						if ((i + 1) <= 2)
							Application.LoadLevel("Module1_Scene" + (i + 1).ToString());
					}
				}
				else{
					Deselect(button);
					button.SendMessage("SetSprite", levelButtonName + (i + 1).ToString() + "_1");
				}
				i ++;
			}
			
			if (tutorialButton.collider.bounds.Contains(Input.mousePosition)){
				tutorialButton.SendMessage("SetSprite", tutorialButtonName + "_1");
				if (Input.GetMouseButtonDown(0)){
					Application.LoadLevel("Tutorial");
				}
			}
			else{
				tutorialButton.SendMessage("SetSprite", tutorialButtonName + "_1");
			}
			break;
		}
	}
	
	void Select(GameObject button){
		button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y, -2);
	}
	void Deselect(GameObject button){
		button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y, 0);
	}
}

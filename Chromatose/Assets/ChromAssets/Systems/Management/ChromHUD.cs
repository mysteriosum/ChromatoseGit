using UnityEngine;
using System.Collections;

public enum Actions{
	Back,
	Absorb,
	Destroy,
	Build,
	Pay,
	Release,
	
	Nothing,
	
}
[System.Serializable]
public class ChromHUD {
	
	public Texture mainBox;
	public Texture energyTank;
	public Texture powerBar;
	public Texture actionButton;
	public Texture absorbAction;
	public Texture destroyAction;
	public Texture payAction;
	public Texture releaseAction;
	public Texture returnAction;
	
	public Texture redCollectible;
	public Texture greenCollectible;
	public Texture blueCollectible;
	public Texture whiteCollectible;
	
	public Texture pauseButton;
	
	public delegate void ActionDelegate();
	public ActionDelegate actionMethod;
	
	private string actionString = "";
	public string ActionString{
		get { return actionString; }
	}
	
	private Actions currentAction;
	public Actions CurrentAction{
		get { return currentAction; }
		set { currentAction = value; }
	}
	
	
	private bool actionPressed;
	public bool ActionPressed{
		get { return actionPressed; }
		set { actionPressed = value; }
	}
	
	
	
	public void UpdateHUD(){
		
		switch (currentAction){
		case Actions.Back:
			actionString = "Back";
			break;
		case Actions.Absorb:
			actionString = "Absorb";
			break;
		case Actions.Destroy:
			actionString = "Destroy";
			break;
		case Actions.Build:
			actionString = "Build";
			break;
		case Actions.Pay:
			actionString = "Pay";
			break;
		case Actions.Release:
			actionString = "Release";
			break;
		default:
			actionString = "";
			break;
		}
		
		if (Input.GetKeyDown(KeyCode.Space) && currentAction > 0 && actionMethod != null){
			actionMethod();
		}
		currentAction = Actions.Nothing;
		actionMethod = null;
	}
	
	public void Draw(){
		/*Debug.Log("action string is " + actionString);
		GUI.Box(new Rect(Screen.width - 128, Screen.height / 2, 96, 32), actionString);*/
	}
	
	public void UpdateAction(Actions action, ActionDelegate method){
		
		if (action < currentAction || action == Actions.Nothing){
			currentAction = action;
			actionMethod = method;
		}
		
	}

}
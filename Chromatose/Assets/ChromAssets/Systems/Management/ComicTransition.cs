using UnityEngine;
using System.Collections;

public class ComicTransition : MonoBehaviour {
	private bool popped;
	private bool returning = false;
	
	private float counter = 0f;
	
	public Texture blackBox;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (popped) return;
		
		if (other.name == "Avatar"){
			popped = true;
			Debug.Log("Popped!");
		}
	}
	
	void OnTriggerExit(Collider other){
		
		if (other.name == "Avatar" && counter <= 0){
			popped = false;
		}
	}
	
	void OnGUI(){
		if (!popped) {
			return;
		}
		if (ChromatoseManager.manager.InComic){			//YES I KNOW this is bad programming. I'M A N00B OK?! ;_;
			
			
			if (returning){
				if (counter >= 1){
					ChromatoseManager.manager.CloseComic();
				}
				goto fadeOut;
			}
			else if (counter > 0){
				goto fadeIn;
			}
			else{
				ChromatoseManager.manager.AnimsReady = true;
			}
		}
		else{
			//Debug.Log("not in comic");
			if (!returning && counter < 1){
				goto fadeOut;
			}
			if (returning){
				if (counter <= 0){
					returning = false;
					popped = false;
				}
				goto fadeIn;
			}
			else if (counter >= 1){
				GameObject.Find("Avatar").transform.position = transform.position;
				ChromatoseManager.manager.OpenComic(Application.loadedLevel);
			}
		}
		
		return;
		
	fadeIn:
		
		counter -= 0.01f;
		GUI.color = new Color(0, 0, 0, counter);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBox);
		return;
			
			
	fadeOut:
		counter += 0.01f;
		GUI.color = new Color(0, 0, 0, counter);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBox);
		return;
	}
	
	void NoPop(){
		popped = false;
	}
	
	public void Return(){
		returning = true;
	}
}

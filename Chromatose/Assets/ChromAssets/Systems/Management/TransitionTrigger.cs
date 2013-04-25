using UnityEngine;
using System.Collections;

public class TransitionTrigger : MonoBehaviour {
	bool popped;
	float counter = 0f;
	public Texture blackBox;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (other.name == "Avatar")
			popped = true;
	}
	
	void OnGUI(){
		if (!popped) return;
		
		counter += 0.01f;
		
		GUI.color = new Color(0, 0, 0, counter);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBox);
		if (counter >= 1){
			Application.LoadLevel(Application.loadedLevel + 1);
		}
	}
}

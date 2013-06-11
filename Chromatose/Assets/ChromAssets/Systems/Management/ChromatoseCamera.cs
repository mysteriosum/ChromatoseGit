using UnityEngine;
using System.Collections;

public class ChromatoseCamera : MonoBehaviour {
	private Transform t;
	
	public Transform avatar;
	
	private tk2dCamera cam2d;
	private int width;
	private int height;
	private ChromatoseManager manager;
	
	// Use this for initialization
	void Start () {
		if (Application.loadedLevelName == "Menu") return;
		manager = ChromatoseManager.manager;
		if (!avatar){
			avatar = GameObject.FindWithTag("avatar").transform;
		}
		t = GetComponent<Transform>();
		cam2d = GetComponent<tk2dCamera>();
		width = (int) cam2d.camera.GetScreenWidth();
		height = (int) cam2d.camera.GetScreenHeight();

		
		//width = cam2d.nativeResolutionWidth;
		//height = cam2d.nativeResolutionHeight;
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.loadedLevelName == "Menu") return;
		
		if (manager.InComic){
			t.position = new Vector3(0, 0, t.position.z);
		}
		else{
			t.position = new Vector3(avatar.position.x - width/2, avatar.position.y - height/2, t.position.z);
		}
	}
}

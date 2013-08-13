using UnityEngine;
using System.Collections;

public class ChromatoseCamera : MonoBehaviour {
	private Transform t;
	
	public Transform avatar;
	
	private tk2dCamera cam2d;
	private int width;
	private int height;
	private ChromatoseManager manager;
	
	
	public GameObject _HUDHelper_Comics = null;
	private Vector3 _HelperPos;
	
	private float _ScreenHeight = 0;
	private float _ScreenWidth = 0;
	
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
		
		
		if(Screen.width == 848){
			_ScreenHeight = 310;
			_ScreenWidth = 785;
		}
		else{
			_ScreenHeight = 465;
			_ScreenWidth = 1185f;
		}
		
		_HelperPos = new Vector3(transform.position.x + _ScreenWidth, transform.position.y + _ScreenHeight, 0);
		
		_HUDHelper_Comics.transform.position = _HelperPos;
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

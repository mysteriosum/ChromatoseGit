using UnityEngine;
using System.Collections;

public class ChromatoseCamera : MainManager {
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
	
	private float _BdOffset;
	
	// Use this for initialization
	void Start () {
		if(LevelSerializer.IsDeserializing) return;
		
		Setup();
		
		
		if(Screen.width == 800){
			_ScreenHeight = 470;
			_ScreenWidth = 680;
		}
		else{
			_ScreenHeight = 585;
			_ScreenWidth = 1175f;
		}
		
		_HelperPos = new Vector3(transform.position.x + _ScreenWidth, transform.position.y + _ScreenHeight, 0);
		
		_HUDHelper_Comics.transform.position = _HelperPos;
		//width = cam2d.nativeResolutionWidth;
		//height = cam2d.nativeResolutionHeight;
	}
	
	void Update () {
	
		if (Application.loadedLevelName == "MainMenu" || currentLevel == 0) {
			//GetComponent<AudioListener>().enabled = true;
			transform.position = new Vector3(0, 0, -25);
			return;
		}
		//else{GetComponent<AudioListener>().enabled = false;}
		
		if (avatar == null && currentLevel != 0){Setup ();}
		
		if (ChromatoseManager.manager.InComic){
			t.position = new Vector3(0, 0 + _BdOffset, t.position.z);
			return;
		}
		else{
			t.position = new Vector3(avatar.position.x - width/2, avatar.position.y - height/2 + _BdOffset, t.position.z);
		}
	}
	
	void Setup(){
		if (Application.loadedLevelName == "MainMenu") return;
		manager = ChromatoseManager.manager;
		if (!avatar && currentLevel != 0){
			avatar = GameObject.FindGameObjectWithTag("avatar").transform;
		}
		t = GetComponent<Transform>();
		cam2d = GetComponent<tk2dCamera>();
		width = (int) cam2d.camera.GetScreenWidth();
		height = (int) cam2d.camera.GetScreenHeight();
	}
	
	public void SwitchCamType(){
		if(_BdOffset != 0){
			_BdOffset = 0;
		}
		else{
			_BdOffset = 100;
		}
	}
}

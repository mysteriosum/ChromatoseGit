using UnityEngine;
using System.Collections;

public class ChromatoseCamera : MonoBehaviour {
	Transform t;
	
	public Transform avatar;
	
	tk2dCamera cam2d;
	int width;
	int height;
	// Use this for initialization
	void Start () {
		if (!avatar){
			avatar = GameObject.Find("Avatar").transform;
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
		t.position = new Vector3(avatar.position.x - width/2, avatar.position.y - height/2, t.position.z);
	}
}

using UnityEngine;
using System.Collections;

public class ColoredAvatarScript : MonoBehaviour {
	
	private GameObject _Avatar;
	private Avatar _AvatarScript;
	
	//private float _CoolDownTime = 8.0f;

	// Use this for initialization
	void Start () {
		_Avatar = GameObject.FindGameObjectWithTag("avatar");
		_AvatarScript = _Avatar.GetComponent<Avatar>();
	}
	
	// Update is called once per frame
	void Update () {
		
		_Avatar = GameObject.FindGameObjectWithTag("avatar");
		_AvatarScript = _Avatar.GetComponent<Avatar>();
	
		transform.position = _Avatar.transform.position;
		transform.rotation = _Avatar.transform.rotation;
	}
}

using UnityEngine;
using System.Collections;

public class eyeColorChange : MonoBehaviour {
	
	public tk2dSpriteCollectionData avaEye;
	public tk2dSpriteCollectionData shavaEye;
	public Avatar avaScript;
	
	private tk2dSprite spriteInfo;
	
	// Use this for initialization
	void Start () {
		spriteInfo = GetComponent<tk2dSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		avaScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		
		switch(avaScript.avaTypeAccess){
		case _AvatarTypeEnum.avatar:
			spriteInfo.Collection = avaEye;
			break;
		case _AvatarTypeEnum.shavatar:
			spriteInfo.Collection = shavaEye;
			break;
		}
	}
}

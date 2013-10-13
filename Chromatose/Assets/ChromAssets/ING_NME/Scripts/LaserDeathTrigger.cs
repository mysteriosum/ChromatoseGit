using UnityEngine;
using System.Collections;

public class LaserDeathTrigger : MonoBehaviour {
	
	public float delay = 1;
	
	private float _FullColor = 255;
	private float _NoColor = 0;
	private float _Timer = 0;
	private tk2dAnimatedSprite _MainAnim;
	
	void Start(){
		_MainAnim = GetComponent<tk2dAnimatedSprite>();
	}
	
	
	void OnTriggerEnter(Collider other){
		if(other.tag != "avatar")return;
		if(other.GetComponent<Avatar>().curColor == Color.blue)return;
		
		ChromatoseManager.manager.Death();
	}
	
	void OnTriggerStay(Collider other){
		if(other.tag != "avatar")return;
		if(other.GetComponent<Avatar>().curColor == Color.blue)return;
		
		ChromatoseManager.manager.Death();
	}
}

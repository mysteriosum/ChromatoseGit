using UnityEngine;
using System.Collections;

public class Comic : MonoBehaviour {

	private bool inMySlot;
	public bool InMySlot{
		get{ return inMySlot;}
	}
	
	private bool menuOpen = false;
	
	private GameObject go;
	private Transform t;
	private Renderer r;
	private tk2dSprite spr;
	private BoxCollider bc;
	
	void Start (){
		
		go = gameObject;
		t = go.GetComponent<Transform>();
		r = renderer;
		spr = GetComponent<tk2dSprite>();
		bc = GetComponent<BoxCollider>();
	}
	
	void Update(){
		if (!menuOpen){
			
			
			
		}
		else{
			
			
		}
	}
	
	public void OpenInMenu(Vector3 pos){
		t.position = pos;
		spr.SetSprite(name);
		menuOpen = true;
	}
}

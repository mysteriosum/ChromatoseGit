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
	private bool beingDragged = false;
	private int currentSlot = -1;
	
	private ChromatoseManager manager;
	private Rect myRect;
	private ComicBackground bg;
	
	private Vector3 startPos;
	private int slideSpeed;
	private Vector3 slideToPos = Vector3.zero;
	private Transform toSwitchWith;
	
	
	public int mySlotIndex = 0;
	
	public static bool comicSelected = false;
	public static bool canDrag = true;
	public static bool comicComplete;
	
	void Start (){
		
		go = gameObject;
		t = go.GetComponent<Transform>();
		r = renderer;
		spr = GetComponent<tk2dSprite>();
		bc = GetComponent<BoxCollider>();
		manager = ChromatoseManager.manager;
		bg = GameObject.Find("pre_comicBG").GetComponent<ComicBackground>();
		
		myRect = new Rect(transform.position.x - bc.size.x / 2, transform.position.y - bc.size.y / 2, bc.size.x, bc.size.y);
	}
	
	void Update(){
		if (Comic.comicComplete){
			return;
		}
		
		myRect = new Rect(transform.position.x - bc.size.x / 2, transform.position.y - bc.size.y / 2, bc.size.x, bc.size.y);
		
		if (manager.CheckComicStats()){
			canDrag = false;
			comicComplete = true;
		}
		
		if (myRect.Contains(Input.mousePosition) && Input.GetMouseButtonDown(0) && !Comic.comicSelected && Comic.canDrag){
			Comic.comicSelected = true;
			beingDragged = true;
			Debug.Log("HERE!");
			startPos = t.position;
			bg.RemoveFromSlot(currentSlot);
		
		}
		
		if (beingDragged){
			t.position = (Vector3)Input.mousePosition;
			if (Input.GetMouseButtonUp(0)){
				beingDragged = false;
				comicSelected = false;
				currentSlot = bg.CheckForSlot();
				Debug.Log("My current slot is " + currentSlot);
				if (currentSlot > -1){
					t.position = (Vector2)bg.PutInSlot(currentSlot, t, out toSwitchWith);
				}
				if (toSwitchWith != null){
					toSwitchWith.SendMessage("SlideToPos", startPos);
					toSwitchWith = null;
					Comic.canDrag = false;
				}
				
			}
			t.position = new Vector3(Mathf.Clamp(t.position.x, 1, Screen.width), Mathf.Clamp(t.position.y, 1, Screen.height), 0);
		
		}
		
		if (slideToPos != Vector3.zero){
			t.position = Vector3.Lerp(t.position, slideToPos, 0.25f);
			if (Vector3.Distance(t.position, slideToPos) < 1){
				t.position = slideToPos;
				slideToPos = Vector3.zero;
				Comic.canDrag = true;
				startPos = t.position;
				currentSlot = bg.CheckForSlot((Vector2)t.position);
				if (currentSlot >= 0)
					bg.PutInSlot(currentSlot, t);
			}
		}
		
		inMySlot = currentSlot == mySlotIndex;			//a check to see if I'm in the right slot
		
		
	}
	
	void SlideToPos(Vector3 newPos){
		slideToPos = newPos;
	}
	
	void NewStartPos(Vector3 newPos){
		startPos = newPos;
	}
	
	public void OpenInMenu(Vector3 pos){
		t.position = pos;
		spr.SetSprite(name);
		menuOpen = true;
	}
}

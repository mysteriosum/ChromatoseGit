using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour{				//THE BUBBLE AND ITS DECLARATION!
	//variables of various types
	public GameObject go;
	public tk2dSprite spriteInfo;
	private Transform t;
	private Renderer r;
	private Transform parent;
	private Vector2 offset = new Vector2(64, 25);
	private float timer;
	
	private string collectibleBubbleName = "redBubble_veryHappy";
	public string CollectibleBubbleName{ get { return collectibleBubbleName; } }
	
	private Vector3 digitOffset = new Vector3(10, 3, -2);
	private int myDigit = 0;
	
	
	private float frameCounter = 0;
	private int indexFrame = 1;
	private bool bubbleGrowed = false;
	private bool bubbleShrinked = false;
	private bool _CanShow = false;
	public bool canShow{
		get{return _CanShow;}
		set{_CanShow = value;}
	}
	
	private string spriteToPlay = "";
	
	
	public int Digit{
		get{ return myDigit; }
		set{ myDigit = value; }
	}
	
	//getters & setters
	public bool Showing {
		get{ return r.enabled; }
		set{ r.enabled = value;
			 if (value == false){
				Digit = 0;
			}
		}
	}
	
	public string SpriteName{
		get{ return spriteInfo.CurrentSprite.name; }
		set{ spriteInfo.SetSprite(value); }
	}
	
	public SpeechBubble(Transform toFollow, tk2dSpriteCollectionData sprcol){  //constructor!
		go = new GameObject(toFollow.name + "Bubble");
		parent = toFollow;
		tk2dSprite.AddComponent(go, sprcol, 0);
		go.AddComponent("DontDestroyScript");
		spriteInfo = go.GetComponent<tk2dSprite>();
		
		r = go.renderer;
		t = go.transform;
		
		go.tag = "speechBubble";
	}
	
	public SpeechBubble(Transform toFollow): this(toFollow, ChromatoseManager.manager.bubbleCollection){
		
	}
	
	
	void Awake(){
		if(LevelSerializer.IsDeserializing) return;
	}
	
	void Start(){
		DontDestroyOnLoad(this);
	}
	
	public void Main(){
		
		if (timer > 0){
			timer -= Time.deltaTime;
		}
		else if (r.enabled){
			r.enabled = false;
			Blend(Color.white);
		}
		t.position = parent.position + (Vector3)offset;
	}
	
	public void ShowBubbleFor(string bubble, float time, int digit){
		
		string myDigitString = digit > 0? digit.ToString() : "";
		SpriteName = bubble + myDigitString;
		timer = time;
		r.enabled = true;
	}
	
	public void ShowBubbleFor(string bubble, float time){
		ShowBubbleFor(bubble, time, 0);
	}
	
	public void Blend(Color color){
		spriteInfo.color = color;
	}
	
	public override string ToString(){
		return SpriteName;
	}
	
	public void Flip(bool left, bool bottom){
		
		offset = new Vector2(left? -offset.x : offset.x, bottom? -offset.y : offset.y);
		
		t.localPosition = (Vector3) offset;
	}
}

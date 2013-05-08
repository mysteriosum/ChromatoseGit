using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Destructible : MonoBehaviour {		//move sprite @ 15 frames or 0.5f secs
	
	protected tk2dAnimatedSprite anim;
	protected tk2dSprite spriteInfo;
	
	public GameObject poof;
	
	
	public int npcsToTrigger = 1;
	public Transform myNode;
	public string specificName = "";
	public int NPCsNeeded{
		get{ return npcsToTrigger - myNPCs.Count;}
		
	}
	
	List<Npc> myNPCs = new List<Npc>();
	
	
	Collider[] children = new Collider[2];
	int curNPCs = 0;
	
	// Use this for initialization
	void Start () {
		Setup();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	protected void Setup(){
		spriteInfo = GetComponent<tk2dSprite>();
		poof = Instantiate(poof) as GameObject;
		poof.SetActive(false);
		anim = poof.GetComponent<tk2dAnimatedSprite>();
		anim.animationEventDelegate = NextImage/*(anim, anim.CurrentClip, anim.CurrentClip.frames[14], 14)*/;
		anim.animationCompleteDelegate = Done;	
	}
	
	protected void Destroy(){
		
		children = GetComponentsInChildren<Collider>(true);
		
		foreach (Collider c in children){
			c.enabled = false;
		}
		poof.SetActive(true);
		poof.transform.position = transform.position;
		poof.transform.rotation = transform.rotation;
		
		anim.Play();
			anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
	}
	
	public bool AddOne(Npc npc){
		if (specificName != ""){
			if (npc.name != specificName)
				return false;
			Debug.Log("Same name");
		}
		myNPCs.Add(npc);
		curNPCs ++;
		
		if (curNPCs >= npcsToTrigger){
			Destroy();
		}
		return true;
	}
	
	protected void DeadAndGone(){
		transform.position = new Vector3(transform.position.x, transform.position.y, -2000);
	}
	
	public bool CheckName(string name){
		if (specificName == "") return true;
		bool identical = specificName == name;
		print("It is " + identical + " that it's the same name");
		return name == specificName;
	}
	
	protected void NextImage(tk2dAnimatedSprite sprite, tk2dSpriteAnimationClip clip, tk2dSpriteAnimationFrame frame, int frameNum){
		Debug.Log("Next");
		string newName = spriteInfo.CurrentSprite.name;
		string newNewName = "";
		int counter = 0;
		foreach (char c in newName){
			if (counter < newName.Length - 1){
				newNewName += c;
				
			}
			else{
				newNewName += "2";
			}
			counter ++;
		}
		Debug.Log("New name is " + newNewName);
		spriteInfo.SetSprite(spriteInfo.GetSpriteIdByName(newNewName));
		//spriteInfo.spriteId ++;
	}
	
	protected void Done(tk2dAnimatedSprite sprite, int index){
		
		gameObject.RemoveComponent(typeof (Destructible));
	}
	
	
}

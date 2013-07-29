using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Destructible : MonoBehaviour {		//move sprite @ 15 frames or 0.5f secs
	
	protected tk2dAnimatedSprite anim;
	protected tk2dSprite spriteInfo;
	protected Avatar avatarScript;
	public GameObject poof;
	
	
	public Transform myNode;
	public string specificName = "";
	
	public Vector2 poofOffset = new Vector2(0, 0);
	public Rect myColRect;
	
	protected bool goingToDestroy = false;
	protected int avatarMinDist = 85;
	protected Transform avatar;
	
	[System.SerializableAttribute]
	public class TargetMessageReceivers{
		public GameObject target;
		public string message;
		public Object objectValue;
		/// <summary>
		/// The float value to give with the message. This will be overridden by any string value.
		/// </summary>
		public float floatValue = -1000f;
		public string stringValue = "";
		
		public void Shoot(){
			if (objectValue != null){
				target.SendMessage(message, objectValue);
			}
			else if(stringValue != ""){
				target.SendMessage(message, stringValue);
			}
			else if(floatValue != -1000f){
				target.SendMessage(message, floatValue);
			}
			else{
				target.SendMessage(message);
			}
			
		}
	}
	
	public TargetMessageReceivers[] messagesOnFinished;
	
	
	
	Collider[] children = new Collider[2];
	
	// Use this for initialization
	void Start () {
		Setup();
		
	}
	
	// Update is called once per frame
	void Update () {
		Checks();
	}
	
	protected virtual void Setup(){
		avatar = GameObject.Find("Avatar").transform;
		avatarScript = avatar.GetComponent<Avatar>();
		spriteInfo = GetComponent<tk2dSprite>();
		poof = Instantiate(poof) as GameObject;
		poof.SetActive(false);
		anim = poof.GetComponent<tk2dAnimatedSprite>();
		anim.animationEventDelegate = NextImage;
		anim.animationCompleteDelegate = Done;	
	}
	
	protected virtual void Checks(){
		float dist = Vector3.Distance(avatar.position, myNode.position);
		if (!avatarScript.colour.Red) return;
		if (collider.bounds.Contains(avatar.position)){
			ChromatoseManager.manager.UpdateAction(Actions.Destroy, Action);
			avatarScript.AtDestructible = true;
		}
	}
	
	protected virtual void Destruct(){
		
		goingToDestroy = false;
		children = GetComponentsInChildren<Collider>(true);
		
		foreach (Collider c in children){
			c.enabled = false;
		}
		poof.SetActive(true);
		poof.transform.position = transform.position - Vector3.forward * 2 + (Vector3) poofOffset;
		poof.transform.rotation = Quaternion.identity;
		
		anim.Play();
		anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
		
		//TODO PUT AN ANIMATION SHOWING LOSS OF COLOUR!
	}
	
	protected virtual void Action(){
		avatarScript.HasDestroyed = true;
		avatarScript.GiveColourTo(transform, avatar);
		avatarScript.SetColour(0, avatarScript.colour.g, avatarScript.colour.b);
		Invoke("Destruct", 0.5f);
	}
	
	
	protected void DeadAndGone(){
		transform.position = new Vector3(transform.position.x, transform.position.y, -2000);
	}
	
	public bool CheckName(string name){
		if (specificName == "") return true;
		return name == specificName;
	}
	
	protected void NextImage(tk2dAnimatedSprite sprite, tk2dSpriteAnimationClip clip, tk2dSpriteAnimationFrame frame, int frameNum){
		
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
		Debug.Log(newNewName);
		spriteInfo.SetSprite(spriteInfo.GetSpriteIdByName(newNewName));
		//spriteInfo.spriteId ++;
	}
	
	protected virtual void Done(tk2dAnimatedSprite sprite, int index){
		foreach (TargetMessageReceivers tar in messagesOnFinished){
			tar.Shoot();
		}
		gameObject.RemoveComponent(this.GetType());
	}
	
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position + (Vector3) poofOffset, 10);
	}
}

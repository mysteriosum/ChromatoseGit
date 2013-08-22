using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DangerBlob : ColourBeing {
	
	private Rigidbody _AvatarRigid;
	public float knockback = 50f;
	public bool diesOnImpact = false;
	public bool isBlackFlame = false;
	public bool _WaitAvatarMove = false;
	private tk2dAnimatedSprite[] myFlames;
	private bool beingExtinguished = false;
	private List<tk2dAnimatedSprite> dyingFlames = new List<tk2dAnimatedSprite>();
	private List<float> dyingAlphas = new List<float>();
	private float fadeRate = 0.05f;
	private Transform avatarT;
	private string flameName = "flame";
	private int flameNumber = 11;
	
	
	[System.SerializableAttribute]
	public class Movement{
		public bool patrol = false;
		public bool rotates = false;
		public bool playAnimOnTurn = false;
		public float speed = 75f;
		public Transform[] targetNodes;
		public GameObject gameObject;
		
		private tk2dAnimatedSprite anim;
		int currentIndex = 0;
		int maxIndex;
		[System.NonSerialized]
		public Transform t;
		
		public void Setup(GameObject gameObject){
			maxIndex = targetNodes.Length;
			this.gameObject = gameObject;
			anim = this.gameObject.GetComponent<tk2dAnimatedSprite>();
		}
		
		public void Move(){
			if (!patrol) return; //If I'm not moving I don't have much to update, do I
			Vector2 traj = ((Vector2)targetNodes[currentIndex].position - (Vector2)t.position);
			traj = traj.magnitude > speed * Time.deltaTime ? traj.normalized * speed * Time.deltaTime : Vector2.zero;		//Adjust the traj!
			
			if (traj == Vector2.zero){
				currentIndex ++;
				
				if (currentIndex >= maxIndex){
					currentIndex = 0;
				}
				
				if (playAnimOnTurn)
					anim.Play();
				
			}
			if (rotates){
				t.Translate(new Vector3(traj.y * -1, traj.x, 0), Space.Self);
			}
			else{
				t.Translate(traj, Space.World);
			}
		}
	}
	
	public DangerBlob.Movement movement = new DangerBlob.Movement();
	
	// Use this for initialization
	void Start () {
		_AvatarRigid = GameObject.FindWithTag("avatar").GetComponent<Rigidbody>();
		movement.Setup(gameObject);
		movement.t = transform;
		avatarT = GameObject.FindWithTag("avatar").transform;
		anim = GetComponent<tk2dAnimatedSprite>();
		if (colour.Red || isBlackFlame){
			myFlames = GetComponentsInChildren<tk2dAnimatedSprite>();
			
			GameObject obj = Resources.Load("animref_nme") as GameObject;
			tk2dSpriteAnimation nmeAnim = obj.GetComponent<tk2dAnimatedSprite>().anim;
			foreach (tk2dAnimatedSprite flanim in myFlames){
				if (flanim == GetComponent<tk2dAnimatedSprite>()) continue;
				int i = 1;//Random.Range(1, 11);
				flanim.anim = nmeAnim;
				flanim.Play(flameName + i.ToString());
				flanim.transform.rotation = Quaternion.identity;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!_WaitAvatarMove){
			if (colour.White)
			movement.Move();
			if (beingExtinguished){
				tk2dAnimatedSprite next = null;
				float shortestDist = 1000;
				foreach (tk2dAnimatedSprite sprite in myFlames){
					float dist = Vector3.Distance(sprite.transform.position, avatarT.position);
					if (dist < shortestDist && !dyingFlames.Contains(sprite)){
						next = sprite;
						shortestDist = dist;
					}
				}
				if (next){
					dyingFlames.Add(next);
					dyingAlphas.Add(1f);
				}
				float highestAlpha = 0;
				for(int i = 0; i < dyingFlames.Count; i ++){
					dyingAlphas[i] -= fadeRate;
					if (dyingAlphas[i] > highestAlpha){
						highestAlpha = dyingAlphas[i];
					}
					dyingFlames[i].SendMessage("FadeAlpha", dyingAlphas[i]);
					
				}
				if (highestAlpha <= 0){
					beingExtinguished = false;
					DeadAndGone();
				}
			}
			
		}
		else{
			//if(_AvatarRigid.velocity.x > 0.5f || _AvatarRigid.velocity.y > 0.5f || _AvatarRigid.velocity.x < -0.5f || _AvatarRigid.velocity.y < -0.5f){
			if(GameObject.FindGameObjectWithTag("avatar").transform.position != GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().initPos){
			
			_WaitAvatarMove = false;
			}
		}
	}

	
	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag != "avatar"){
			return;
		}
		
		/*
		Avatar avatar = other.gameObject.GetComponent<Avatar>();
		bool sameColour = avatar.curColor == Color.red? true : false;
		if (sameColour && diesOnImpact){
			Dead = true;
			if (colour.Red)
				beingExtinguished = true;
			
				Debug.Log("We're the same colour!");
			return;
		}
		if (sameColour) return;
		
		if (anim != null && colour.Blue){
			anim.Play();
		}*/
		
		ChromatoseManager.manager.Death();
		other.gameObject.GetComponent<Avatar>().EmptyingBucket();
		//avatar.Damage();    //remove HP from the avatar, but this isn't implemented yet
	}
	
	
	void DeadAndGone(){
		Gone = true;
		//Debug.Log("Dead and Gone!");
	}

	
	override public void Trigger(){
		
	}
	
	void OnDrawGizmosSelected(){
		
		int maxIndex = movement.targetNodes.Length;
		if (maxIndex <= 1) return;
		int index = 0;
		
		int nextIndex = 1;
		for(int i = 0; i < maxIndex; i++){
			Gizmos.DrawLine(movement.targetNodes[index].position, movement.targetNodes[nextIndex].position);
			
			index = (index + 1) % maxIndex;
			nextIndex = (index + 1) % maxIndex;
		}
	}
	
	void Deactivate(){
		this.collider.enabled = false;
		this.renderer.enabled = false;
		this.movement.patrol = false;
	}
	
	void Activate(){
		this.collider.enabled = true;
		this.renderer.enabled = true;
		if (this.movement.targetNodes[0] != null)
			this.movement.patrol = true;
	}
	
}


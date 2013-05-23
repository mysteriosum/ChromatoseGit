using UnityEngine;
using System.Collections;

public class DangerBlob : ColourBeing {
	public float knockback = 50f;
	public bool diesOnImpact = false;
	
	[System.SerializableAttribute]
	public class Movement{
		public bool patrol = false;
		public bool rotates = false;
		public float speed = 75f;
		public Transform[] targetNodes;
		int currentIndex = 0;
		int maxIndex;
		[System.NonSerialized]
		public Transform t;
		
		public void Setup(){
			maxIndex = targetNodes.Length;
		}
		
		public void Move(){
			bool rotating = false;
			int counter = 0;
		top:
			if (!patrol) return; //If I'm not moving I don't have much to update, do I
			Vector2 traj = ((Vector2)targetNodes[currentIndex].position - (Vector2)t.position);
			traj = traj.magnitude > speed * Time.deltaTime ? traj.normalized * speed * Time.deltaTime : Vector2.zero;		//Adjust the traj!
			
			//Debug.Log(traj);
			Quaternion lookRot = Quaternion.LookRotation(Vector3.forward, traj);
			
			/*if (rotates){
				float x = traj.x + traj.magnitude * (1 - Mathf.Pow(Mathf.Cos(Time.time), 2)
												  / (1 - Mathf.Pow(Mathf.Cos(Time.time), 2)));
				float y = traj.y + traj.magnitude * (2 * Mathf.Cos(Time.time))
							 					  / (1 - Mathf.Pow(Mathf.Cos(Time.time), 2));
				t.rotation = lookRot;
				
				traj = new Vector3(x, y, 0);
				rotating = true;
			}
			else{*/
						//if I'm closer enough, traj = 0 (next node)
				//traj = traj.normalized * speed * Time.deltaTime;
			//}
			
			if (traj == Vector2.zero){
				currentIndex ++;
				if (currentIndex >= maxIndex){
					currentIndex = 0;
				}
				
			}
		move:
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
		movement.Setup();
		movement.t = transform;
		anim = GetComponent<tk2dAnimatedSprite>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (colour.White)
		movement.Move();
		
		
	}

	
	void OnCollisionStay(Collision other){
		if (other.gameObject.tag != "avatar"){
			return;
		}
		
		Avatar avatar = other.gameObject.GetComponent<Avatar>();
		bool sameColour = CheckSameColour(avatar.colour);
		if (sameColour && diesOnImpact){
			//Debug.Log("Bye bye");
			Dead = true;
				//Debug.Log("We're the same colour!");
			
			if (colour.Red){														//THIS EXPLODES THE NPC BARRIER! (RED BARRIER) (NPC FIGHt) (THING, YOU KNOW)
				Npc[] myNPCs = GetComponentsInChildren<Npc>(true);
				//Debug.Log("I've got some NPCS! This many: " + myNPCs.Length);
				foreach (Npc npc in myNPCs){
					npc.fuckOffReference = (Vector2)(npc.transform.position - transform.position);
					npc.FuckOff();
					npc.SendMessage("Stop");
					npc.transform.parent = null;
				}
			}
			
			
			return;
		}
		if (sameColour) return;
		
		if (anim != null && colour.Blue){
			anim.Play();
		}
		/*
		Vector2 back = (Vector2) other.contacts[0].normal * -1;
		avatar.transform.position += (Vector3)back * 12;
		*/
		
		avatar.Jolt(12f);
		if (!avatar.Hurt){
			avatar.Push(knockback);
		}
		avatar.SendMessage("Ouch", gameObject); //we're going to call this later, k? But with argumetns'
		
		
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


using UnityEngine;
using System.Collections;

public class DangerBlob : ColourBeing {
	public float knockback = 50f;
	public bool diesOnImpact = false;
	public bool respawns = false;
	public float respawnTime = 3f;
	public string deathClipName;
	
	[System.SerializableAttribute]
	public class Movement{
		public bool patrol = false;
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
			int counter = 0;
		top:
			if (!patrol) return; //If I'm not moving I don't have much to update, do I
			Vector2 traj = ((Vector2)targetNodes[currentIndex].position - (Vector2)t.position);
			traj = traj.magnitude > speed * Time.deltaTime ? traj.normalized * speed * Time.deltaTime : Vector2.zero;		//Adjust the traj!
			
			if (traj == Vector2.zero){
				currentIndex ++;
				if (currentIndex >= maxIndex){
					currentIndex = 0;
				}
				
			}
		move:
			t.Translate(traj, Space.World);
			
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

	
	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag != "avatar"){
			return;
		}
		
		Avatar avatar = other.gameObject.GetComponent<Avatar>();
		bool sameColour = CheckSameColour(avatar.colour);
		if (sameColour && diesOnImpact){
			Debug.Log("Bye bye");
			Dead = true;
			if (anim){
				anim.Play(anim.GetClipIdByName(deathClipName));
				anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
			}
			
			if (respawns){
				Invoke("Respawn", respawnTime);
			}
			return;
		}
		if (sameColour) return;
		if (avatar.Hurt) return;
		Vector2 back = (Vector2) other.contacts[0].normal * -1;
		avatar.transform.position += (Vector3)back * 12;
		avatar.SendMessage("Ouch");
		Vector2 diff = new Vector2(avatar.t.position.x, avatar.t.position.y) - new Vector2(transform.position.x, transform.position.y);
		avatar.movement.SetVelocity(diff.normalized * knockback);
		
		//avatar.Damage();    //remove HP from the avatar, but this isn't implemented yet
	}
	
	void Respawn(){
		Dead = false;
	}
	
	void DeadAndGone(){
		Gone = true;
		Debug.Log("Dead and Gone!");
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
}


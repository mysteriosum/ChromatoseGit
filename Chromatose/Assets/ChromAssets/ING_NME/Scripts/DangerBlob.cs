using UnityEngine;
using System.Collections;

public class DangerBlob : ColourBeing {
	public float knockback = 1.5f;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag != "avatar"){
			return;
		}
		
		Avatar avatar = other.gameObject.GetComponent<Avatar>();
		
		if (CheckSameColour(avatar.colour)){
			Debug.Log("Bye bye");
			Dead = true;
			return;
		}
		
		Vector2 diff = new Vector2(avatar.t.position.x, avatar.t.position.y) - new Vector2(transform.position.x, transform.position.y);
		avatar.movement.SetVelocity(diff.normalized * knockback);
		
		//avatar.Damage();    //remove HP from the avatar, but this isn't implemented yet
	}

	
	override public void Trigger(){
		
	}
}


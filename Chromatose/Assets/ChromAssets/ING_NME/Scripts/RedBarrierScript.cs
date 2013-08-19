using UnityEngine;
using System.Collections;

public class RedBarrierScript : MonoBehaviour {
	
	public tk2dSpriteAnimation flameAnim;
	
	
	private tk2dAnimatedSprite[] myFlames;
	private Transform avatarT;
	
	private bool _CanDie = false;
	private bool _Gone = false;
	private float _FadingCounter = 1;
	private float _FadeRate = 0.005f;

	
	#region Start
	void Start () {
			
		avatarT = GameObject.FindWithTag("avatar").transform;
		myFlames = GetComponentsInChildren<tk2dAnimatedSprite>();
		
		foreach(tk2dAnimatedSprite flames in myFlames){
			if (flames == GetComponent<tk2dAnimatedSprite>()) continue;
			
			flames.anim = flameAnim;
			flames.Play("flame1");
			flames.transform.rotation = Quaternion.identity;
		}	
	}
	#endregion
	
	
	#region Update
	void FixedUpdate () {
	
		if(_CanDie){
			foreach(tk2dAnimatedSprite sprite in myFlames){
				_FadingCounter -= _FadeRate;
				sprite.color = new Color(0, 0, 0, _FadingCounter);
			}
		}
		
		if(_FadingCounter <= 0 && !_Gone){
			Die();
		}
	}
	#endregion
	
	
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag != "avatar") return;
		
		Avatar avatar = other.gameObject.GetComponent<Avatar>();
		bool sameColour = avatar.curColor == Color.red? true : false;
		
		if(sameColour){
			_CanDie = true;
		}
		else{
			ChromatoseManager.manager.Death();
		}
	}	
	
	void Die(){
		Debug.Log("Die");
		_Gone = true;
		transform.position = new Vector3(transform.position.x, transform.position.y, -1000);
	}
}

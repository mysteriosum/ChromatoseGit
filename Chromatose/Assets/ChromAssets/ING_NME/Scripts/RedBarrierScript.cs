using UnityEngine;
using System.Collections;

public class RedBarrierScript : MonoBehaviour {
	
	public tk2dSpriteAnimation flameAnim;
	
	
	private tk2dAnimatedSprite[] myFlames;
	
	private bool _CanDie = false;
	private bool _Gone = false;
	private float _FadingCounter = 1;
	private float _FadeRate = 0.005f;

	
	#region Start
	void Start () {
		myFlames = GetComponentsInChildren<tk2dAnimatedSprite>();
		
		foreach(tk2dAnimatedSprite flames in myFlames){
			if (flames == GetComponent<tk2dAnimatedSprite>()) continue;
			float animOffset = Random.Range(0f, 1f);
			flames.anim = flameAnim;
			flames.Play("flame1", animOffset);
			flames.transform.rotation = Quaternion.identity;
		}
	}
	
	#endregion
	
	
	#region Update
	void FixedUpdate () {
		
		if(myFlames.Length <= 1){
			Destroy(this.gameObject);
		}
		
		//Debug.Log(myFlames.Length);
		
	/*
		if(_CanDie){
			foreach(tk2dAnimatedSprite sprite in myFlames){
				_FadingCounter -= _FadeRate;
				sprite.color = new Color(0, 0, 0, _FadingCounter);
			}
		}
		
		if(_FadingCounter <= 0 && !_Gone){
			Die();
		}*/
	}
	#endregion
	
	
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag != "avatar") return;
		
		Avatar avatar = other.gameObject.GetComponent<Avatar>();
		bool sameColour = avatar.curColor == Color.red? true : false;
		
		if(sameColour){
			//_CanDie = true;
			PlayDie();
		}
		else{
			ChromatoseManager.manager.Death();
		}
	}	
	
	void PlayDie(){
		foreach(tk2dAnimatedSprite spr in myFlames){
			StartCoroutine(RandomChange(spr));
		}
		//StartCoroutine(DelaiToDie());
	}

	
	void Die(){
		//Debug.Log("Die");
		_Gone = true;
		transform.position = new Vector3(transform.position.x, transform.position.y, -1000);
	}
	
	void Die(tk2dAnimatedSprite sprite, int index){
		//Debug.Log("Die by Delegate");
		_Gone = true;
		transform.position = new Vector3(transform.position.x, transform.position.y, -1000);
	}
	
	void DestroyFlame(tk2dAnimatedSprite sprite, int index){
		if(sprite){
			myFlames = GetComponentsInChildren<tk2dAnimatedSprite>();
			Destroy(sprite.gameObject);			
		}
	}
	
	IEnumerator RandomChange(tk2dAnimatedSprite spr){
		float ramdomDelai = Random.Range(0.25f, 1.5f);
		yield return new WaitForSeconds(ramdomDelai);
		if(spr){
			spr.PlayFromFrame("flameDie", 0);
			spr.animationCompleteDelegate = DestroyFlame;
			MusicManager.soundManager.PlaySFX(18);
		}
	}
	
	IEnumerator DelaiToDie(){
		yield return new WaitForSeconds(3f);
		Die ();
	}
}

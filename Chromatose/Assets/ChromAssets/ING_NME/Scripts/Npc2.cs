using UnityEngine;
using System.Collections;

public class Npc2 : MonoBehaviour {
	
	public enum _TypeNPCEnum{
		Blue, Red
	}
	
	public _TypeNPCEnum typeNpc;
	public AudioClip _FillBucketSound;
	
	private ChromatoseManager _Manager;
	private tk2dAnimatedSprite _MainAnim;
	private Avatar _AvatarScript;
	private Avatar.LoseAllColourParticle losePart;
	private AudioSource _Player;
	
	private Color myColor = Color.red;
	private bool _ColorGone = false;
	private bool setuped = false;

	private string _redBounceString = "rNPC_bounce";
	private string _greyBounceString = "rNPC_bounceGrey";
	private string _blueBounceString = "bNPC_bounce";
	
	void Start () {
		_MainAnim = GetComponent<tk2dAnimatedSprite>();
		_Player = GetComponent<AudioSource>();
		
		switch(typeNpc){
		case _TypeNPCEnum.Red:
			_MainAnim.Play(_redBounceString);			
			myColor = Color.red;
			break;
		case _TypeNPCEnum.Blue:
			_MainAnim.Play(_blueBounceString);
			myColor = Color.blue;
			break;			
		}
		
		Setup();
	}
	
	void Update () {
		if(losePart != null){
			losePart.Fade();
		}
	}
	
	void OnTriggerStay(Collider other){
		if(other.tag != "avatar") return;
		
		if(!_ColorGone){
			HUDManager.hudManager.UpdateAction(Actions.Absorb, Trigger);		//this tells the hud that I want to do something. But I'll have to wait in line!
		}
	}
	
	void Trigger(){
		if(setuped){
		_AvatarScript.FillBucket(myColor);
		_Player.clip = _FillBucketSound;
		_Player.Play();
		_MainAnim.Play("rNPC_redToGrey");
		_MainAnim.animationCompleteDelegate = GreyBounce;	
		_ColorGone = true;
		losePart = new Avatar.LoseAllColourParticle(_AvatarScript.particleCollection, _AvatarScript.partAnimations, this.transform, myColor);
		//StartCoroutine(DelaiBeforeFade(1.0f));
		}
		else{
			Setup();
		}
	}
		
	void Setup(){
		_AvatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		setuped = true;
	}
	
	public void GreyBounce(tk2dAnimatedSprite clip, int index){
		_MainAnim.Play(_greyBounceString);
	}
	
	IEnumerator DelaiBeforeFade(float delai){
		yield return new WaitForSeconds(delai);
		losePart.Fade();
	}
}

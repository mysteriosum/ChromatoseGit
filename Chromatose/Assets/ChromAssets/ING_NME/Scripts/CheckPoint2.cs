using UnityEngine;
using System.Collections;

public class CheckPoint2 : MonoBehaviour {
	
	public Texture blackHoleBG;
	
	public Texture[] bubbles;
	
	private bool _OnBlackHole = false;
	private bool _CanDisplayBubble = false;
	private float _BlackHoleAlpha = 1;
	private Texture choosenTexture;
	
	private bool _AlreadyTake = false;
	
	void Start () {
	}
	
	void Update () {
	
	}
	
		//Choisi un nombre random pour la bubble, de 0, au nombre maximum de la liste de Bubble
	int ChooseRandomBubble(){
		int maxR = bubbles.Length;
		int rndNb = Random.Range(0, maxR);
		return rndNb;
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag != "avatar")return;
			
		if(!_AlreadyTake){
		
				//Choisi une nouvelle bubble a chaque save
			choosenTexture = bubbles[ChooseRandomBubble()];
			
				//Empeche le joueur de bouger
			other.GetComponent<Avatar>().CannotControlFor(false, 0);
			
				//Autorise le dispaly de la Bubble "Checkpoint"
			_CanDisplayBubble = true;
			BlackHoleSequence();
			
			_AlreadyTake = true;
			ChromatoseManager.manager.levelSaveExist = true;
			
			StartCoroutine(SaveCP());
		}
	}
	
	void OnTriggerStay(Collider other){
		if(other.tag != "avatar")return;
				
		/*
		if(MainManager._MainManager._Decel > 0 && _CanDisplayBubble){
			MainManager._MainManager._Decel -= 0.013f;
		}
		else if (_CanDisplayBubble){
			other.GetComponent<Avatar>().movement.SetVelocity(Vector2.zero);
		}*/
		
	}	
	
	void BlackHoleSequence(){
		if(!_OnBlackHole){
			_OnBlackHole = true;
		}
	}	
	
	void OnGUI(){
		if(_OnBlackHole){
			DrawBlackHole();
		}
		if(_CanDisplayBubble){
			DrawBubble();
		}
	}
	
	void DrawBlackHole(){
		GUI.DrawTexture(new Rect(0, 0, 1280, 720), blackHoleBG);
	}
	void DrawBubble(){
		GUI.DrawTexture(new Rect(650, 5, choosenTexture.width - choosenTexture.width * 0.25f, choosenTexture.height - choosenTexture.height * 0.25f), choosenTexture);
	}
	
	
	IEnumerator SaveCP(){
		yield return new WaitForSeconds(2.5f);		
		LevelSerializer.Checkpoint();
		
		StartCoroutine(CanGo());
	
	}	
	IEnumerator CanGo(){
		
		yield return new WaitForSeconds(1.0f);
		_OnBlackHole = false;
		_CanDisplayBubble = false;
		GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().CanControl();
		
	//	MainManager._MainManager._Decel = 1;
	}
}

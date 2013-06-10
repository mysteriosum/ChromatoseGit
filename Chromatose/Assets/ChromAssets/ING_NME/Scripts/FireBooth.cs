using UnityEngine;
using System.Collections;

public class FireBooth : TollBooth {
	public string newAnimName = "New Clip";
	private bool reddening = false;
	private Avatar avatar;
	// Use this for initialization
	void Start () {
		myCouleur = Couleur.red;
		Setup();
		avatar = GameObject.FindWithTag("avatar").GetComponent<Avatar>();
	}
	
	// Update is called once per frame
	void Update () {
		if (triggered){
			
		}
		if (Check(Actions.Release)){
			avatar.WantsToRelease = true;
		}
		
	}
	
	void Animate(){
		anim.Play(anim.GetClipByName(newAnimName), 0);
		anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
		Debug.Log("YEAAANIMATE");
	}
	
	protected override void StartIn(){
		chroManager.RemoveCollectibles(Couleur.red, requiredPayment, transform.position);
	}
	
	void TurnRed(){
		
		reddening = true;
		GetComponent<DangerBlob>().SetColour(255, 0, 0);
		anim.color = Color.red;
	}
}

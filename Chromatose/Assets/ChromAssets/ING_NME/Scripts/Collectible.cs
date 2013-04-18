using UnityEngine;
using System.Collections;

public class Collectible : ColourBeing {
	
	public Couleur colColour = Couleur.white;
	int closeDist = 50;
	Avatar avatar;
	Transform t;
	Transform avatarT;
	// Use this for initialization
	void Start () {
		switch (colColour){
		case Couleur.white:
			colour.r = 0;
			colour.g = 0;
			colour.b = 0;
			break;
		case Couleur.red:
			colour.r = 255;
			colour.g = 0;
			colour.b = 0;
			break;
		case Couleur.green:
			colour.r = 0;
			colour.g = 255;
			colour.b = 0;
			break;
		case Couleur.blue:
			colour.r = 0;
			colour.g = 0;
			colour.b = 255;
			break;
		
		}
		avatar = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		t = transform;
		avatarT = avatar.transform;
	}
	
	// Update is called once per frame
	
	void Update () {
		Vector2 dist = (Vector2)avatarT.position - (Vector2)t.position;
		if (dist.magnitude < closeDist){
			if (CheckSameColour(avatar.colour) || colColour == Couleur.white){
				ChromatoseManager.manager.AddCollectible(colColour);
				Gone = true;
				this.enabled = false;
			}
		}
	}
	/*
	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.tag == "avatar"){
			if (CheckSameColour(collider.gameObject.GetComponent<Avatar>().colour) || colColour == Couleur.white){
				ChromatoseManager.manager.AddCollectible(colColour);
				Gone = true;
			}
		}
	}
	*/
	override public void Trigger(){
		
	}
}

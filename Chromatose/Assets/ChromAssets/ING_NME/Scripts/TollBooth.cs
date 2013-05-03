using UnityEngine;
using System.Collections;

public class TollBooth : MonoBehaviour {
	public int requiredPayment = 1;
	
	BoxCollider myCollider;
	Transform colliderT;
	Transform avatarT;
	ChromatoseManager chroManager;
	Transform collisionChild;
	tk2dAnimatedSprite anim;
	
	bool triggered = false;
	// Use this for initialization
	void Start () {
		chroManager = GameObject.FindObjectOfType(typeof(ChromatoseManager)) as ChromatoseManager;
		avatarT = GameObject.FindGameObjectWithTag("avatar").GetComponent<Transform>();
		//Debug.Log("Did I find avatar? " + avatarT.name);
		BoxCollider[] chilluns = gameObject.GetAllComponentsInChildren<BoxCollider>();
		foreach (BoxCollider c in chilluns){
			if (c.isTrigger){
				myCollider = c;
			}
			
			if (c.gameObject.layer == LayerMask.NameToLayer("collision")){
				collisionChild = c.transform;
			}
		}
		colliderT = myCollider.transform;
		//Debug.Log("Got a collider " + myCollider.name + " and a transform: " + colliderT.name);
		
		anim = GetComponent<tk2dAnimatedSprite>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (triggered) return;
		
		if (chroManager.GetCollectibles(Couleur.blue) >= requiredPayment){
			if (avatarT.position.x > colliderT.position.x - myCollider.size.x/2 && 
				avatarT.position.x < colliderT.position.x + myCollider.size.x/2 && 
				avatarT.position.y > colliderT.position.y - myCollider.size.y/2 && 
				avatarT.position.y < colliderT.position.y + myCollider.size.y/2
			){
				chroManager.RemoveCollectibles(Couleur.blue, requiredPayment, avatarT.position);
				triggered = true;
				collisionChild.gameObject.SetActive(false);
				if (anim)
					anim.Play();
					anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
			}
		}
	}
	
	
}

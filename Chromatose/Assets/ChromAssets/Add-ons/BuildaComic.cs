using UnityEngine;
using System.Collections;

public class BuildaComic : Buildable {
	private GameObject myThumb;
	// Use this for initialization
	void Start () {
		Setup();
		myThumb = VectorFunctions.FindClosestOfTag(transform.position, "comicThumb", 1000).gameObject;
		myThumb.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	protected override void Done(tk2dAnimatedSprite sprite, int index){
		foreach (TargetMessageReceivers tar in messagesOnFinished){
			tar.Shoot();
		}
		myThumb.SetActive(true);
							//TODO : SET A FUNCTION IN THE THUMB WHICH WILL MAKE IT APPEAR OVER TIME, SO THE PLAYER NECESSARILY KNOWS IT'S THERE BEFORE HE PICKS IT UP
		gameObject.RemoveComponent(this.GetType());
	}
}

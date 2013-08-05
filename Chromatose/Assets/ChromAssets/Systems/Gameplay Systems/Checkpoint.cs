using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	public string onSpriteName;
	public string offSpriteName;
	
	private bool active = false;
	public bool Active{
		get { return active; }
		set { active = value; 
				if (active){
					spriteInfo.SetSprite(onSpriteName);
				}
				else{
					spriteInfo.SetSprite(offSpriteName);
				}
		}
	}
	
	private tk2dSprite spriteInfo;
	// Use this for initialization
	
	
	void Start () {
		transform.position = new Vector3(transform.position.x, transform.position.y, 2);
		
		if (onSpriteName == "" || offSpriteName == ""){
			Debug.LogWarning("Hey doofus, there's no name for the on/off of the checkpoint! Destroying!");
			Destroy(this.gameObject);
		}
		
		spriteInfo = GetComponent<tk2dSprite>();
		collider.isTrigger = true;
		spriteInfo.SetSprite(offSpriteName);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag != "avatar") return;

		ChromatoseManager.manager.NewCheckpoint(transform);
		
	}
	public void CallOnStart(GameObject newCP){
		ChromatoseManager.manager.NewFirstCheckPoint(newCP.transform);
	}
}

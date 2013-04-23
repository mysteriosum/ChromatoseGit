using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {
	
	protected tk2dAnimatedSprite anim;
	public int npcsToTrigger = 1;
	public Transform myNode;
	
	Collider[] children = new Collider[2];
	int curNPCs = 0;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<tk2dAnimatedSprite>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Destroy(){
		
		children = GetComponentsInChildren<Collider>(true);
		
		foreach (Collider c in children){
			c.enabled = false;
		}
		
		anim.Play();
		
		gameObject.RemoveComponent(typeof (Destructible));
	}
	
	public void AddOne(){
		curNPCs ++;
		if (curNPCs >= npcsToTrigger){
			Destroy();
		}
	}
}

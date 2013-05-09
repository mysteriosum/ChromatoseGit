using UnityEngine;
using System.Collections;

public class ComicThumb : MonoBehaviour {
	
	public int index;
	
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag != "avatar") return;
		
		ChromatoseManager.manager.FindComic(index);
		
		//	TODO : put comic thumb get animation here
		
		transform.Translate(Vector3.forward * -3000);
	}
	
}

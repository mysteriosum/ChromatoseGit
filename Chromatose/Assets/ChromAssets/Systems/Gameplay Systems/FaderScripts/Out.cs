using UnityEngine;
using System.Collections;

public class Out : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	void OnTriggerStay(Collider other){
		if (other.name == "Avatar"){
			//Debug.Log("Trigger:");
			transform.parent.SendMessage("Out");
		}
		else{
			
		}
		/*
		tk2dSprite otherSprite = other.GetComponent<tk2dSprite>();
		if (otherSprite){
			foreach (tk2dSprite s in parentScript.spritesIn){
				if (s == otherSprite)
					s.color = new Color(s.color.r, s.color.g, s.color.b, 1f);
			}
		}
		*/
	}
	
	
	void OnTriggerEnter(Collider other){
		Npc npc = other.GetComponent<Npc>();
		if (npc){
			npc.transform.parent = null;
			transform.parent.SendMessage("Exit", npc.gameObject);
		}
	}
}

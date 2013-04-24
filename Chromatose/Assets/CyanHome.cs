using UnityEngine;
using System.Collections;

public class CyanHome : MonoBehaviour {
	
	public string npcName = "";
	Transform myNPC;
	Transform t;
	BoxCollider boxCol;
	bool triggered = false;
	// Use this for initialization
	void Start () {
		if (npcName == ""){
			Debug.LogWarning("This CyanHome has no npcName attached!");
			gameObject.SetActive(false);
		}
		myNPC = GameObject.Find(npcName).GetComponent<Transform>();
		if (!myNPC){
			Debug.LogWarning("This CyanHome has a name but that NPC doesn't exist!");
			gameObject.SetActive(false);
		}
		t = transform;
		boxCol = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((myNPC.position - t.position).magnitude < boxCol.size.x && !triggered){
			triggered = true;
			Debug.Log("Found him!");
			Npc npcScript = myNPC.GetComponent<Npc>();
			npcScript.StopAndDisable();
			npcScript.AddGreenUntilMax();
			npcScript.SetNewTarget(t);
		}
		
	}
}

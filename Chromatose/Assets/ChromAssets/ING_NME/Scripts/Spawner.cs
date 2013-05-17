using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	public GameObject original;
	float timer = 0;
	int currentState = 0;
	int stateAmount;
	[System.SerializableAttribute]
	public class States{
		
		public float rateOfFire = 2f;
		public bool shooting = true;
		
		public States(float rateOfFire, bool shooting){
			this.rateOfFire = rateOfFire;
			this.shooting = shooting;
		}
	}
	
	public States[] states = {new States(2f, true)};
	
	// Use this for initialization
	void Start () {
		if (original){
			original.SendMessage("Deactivate");
		}
		stateAmount = states.Length;
	}
	
	// Update is called once per frame
	void Update () {
		if (!states[currentState].shooting) return;
		timer += Time.deltaTime;
		if (timer >= states[currentState].rateOfFire){
			GameObject newGuy = Instantiate(original as Object, transform.position, transform.rotation) as GameObject;
			newGuy.SendMessage("Activate");
			newGuy.transform.parent = transform;
			timer = 0;
		}
	}
	
	void Disable(){
		states[currentState].shooting = false;
	}
	
	void NextState(){
		currentState = Mathf.Min(stateAmount -1, currentState + 1);
	}
}

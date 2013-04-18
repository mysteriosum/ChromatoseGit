using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public GameObject original;
	public float rateOfFire = 2f;
	float timer = 0;
	
	// Use this for initialization
	void Start () {
		if (original){
			original.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= rateOfFire){
			GameObject newGuy = Instantiate(original as Object, transform.position, transform.rotation) as GameObject;
			newGuy.SetActive(true);
			timer = 0;
		}
	}
}

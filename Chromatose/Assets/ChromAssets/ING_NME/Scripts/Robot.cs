using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour {
	
	public bool rotates = false;
	public float turnRate = 1f;
	public bool alternate = false;
	int angle = 180;
	/*[System.SerializableAttribute]
	public class DirectionClass{
		public bool up = true;
		public bool down = true;
		public bool left = false;
		public bool right = false;
		
	}*/
	
	//public DirectionClass directions;
	
	string[] sprites = new string[2] {"robot1", "robot2"};
	int curSprite = 0;
	int directionBin;
	float timer = 0;
	float delay;
	Transform myLaser;
	Transform t;
	tk2dSprite spriteInfo;

	// Use this for initialization
	void Start () {
		t = GetComponent<Transform>();
		spriteInfo = t.parent.GetComponent<tk2dSprite>();
		
		
		if (t.rotation.eulerAngles.z < -85 && t.rotation.eulerAngles.z > -95){
			curSprite = 1;
		
		}
		myLaser = GetComponentInChildren<Transform>();
		if (alternate){
			delay = turnRate;
		}
		/*
		if (directions.up) directionBin ++;		//TODO : but probably for module 2, fix this so that it will accomodate more than 2 directions!
		if (directions.down) directionBin += 2;
		if (directions.left) directionBin += 4;
		if (directions.right) directionBin += 8;
		*/
	}
	
	// Update is called once per frame
	void Update () {
		if (!rotates) return;
		
		if (delay > 0){
			delay -= Time.deltaTime;
			return;
		}
		timer += Time.deltaTime;
		if (timer >= turnRate){
			curSprite = 1 - curSprite;
			spriteInfo.SetSprite(sprites[curSprite]);
			if (curSprite == 0){
				transform.position = new Vector3(transform.position.x, transform.position.y, -2);
			}
			else{
				transform.position = new Vector3(transform.position.x, transform.position.y, 2);
			}
			timer = 0;
			transform.Rotate(new Vector3(0, 0, angle));
		}
	}
	
	void Disable(){
		myLaser.gameObject.SetActive(false);
	}
}

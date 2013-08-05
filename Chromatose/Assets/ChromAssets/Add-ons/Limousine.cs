using UnityEngine;
using System.Collections;

public class Limousine : MonoBehaviour {
	
	public float accel = 2f;
	public float maxSpeed = 120f;
	public float stopAfter = 3f;
	public float pauseFor = 2f;
	public Transform myNode;
	
	Transform t;
	float timer;
	bool moving;
	bool accelerating = true;
	bool decelerating;
	bool waiting;
	Vector2 curVelocity;
	Vector2 direction;
	bool stoppedOnce;
	
	
	// Use this for initialization
	void Start () {
		if (!myNode){
			Debug.LogWarning("Hey! This limo needs a node! What do I pay you for?!");
			Destroy(this.gameObject);
		}
		direction = (Vector2)(myNode.position - transform.position);
		direction.Normalize();
		t = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!moving) return;
		
		if (waiting || !stoppedOnce)
			timer += Time.deltaTime;
		
		if (accelerating)
			curVelocity += direction * accel * Time.deltaTime;
		
		if (decelerating)
			curVelocity -= 2 * direction * accel * Time.deltaTime;
		
		
		if (curVelocity.magnitude >= maxSpeed)
				accelerating = false;
		
		if (stoppedOnce && !waiting && curVelocity.magnitude <= accel * 2 * Time.deltaTime && decelerating){
			curVelocity = Vector2.zero;
			decelerating = false;
			waiting = true;
			timer = 0f;
			StartTakingCollectibles();
		}
		
		if (timer >= stopAfter && !stoppedOnce){
			accelerating = false;
			stoppedOnce = true;
			decelerating = true;
		}
		
		if (waiting && timer >= pauseFor){
			waiting = false;
			accelerating = true;
		}
		
		t.Translate(curVelocity, Space.World);
	}
	
	void Trigger(){
		Debug.Log("RAWR");
		moving = true;
	}
	
	void StartTakingCollectibles(){
		Debug.Log("Steal steal steal");
	}
	
}

using UnityEngine;
using System.Collections;

public class BlueEyeGuy : MonoBehaviour {
	
	public float controlLockTime = 1.3f;
	public float lookDist = 100f;
	public float throwForce = 1.5f;
	public float throwTime = 2.5f;
	
	private bool looking = true;
	private Avatar avatar;
	private Transform t;
	
	// Use this for initialization
	void Start () {
		t = GetComponent<Transform>();
		avatar = (Avatar) FindObjectOfType(typeof (Avatar));
		Debug.Log(avatar);
	}
	
	// Update is called once per frame
	void Update () {
		//TODO make this guy rotate better
		if (looking){
			t.Rotate(new Vector3(0,0,1));
			Vector2 dist = avatar.t.position - t.position;
			if (dist.magnitude <= lookDist){
				bool line = Physics.Linecast(t.position, t.position + t.right * lookDist);
				if (line){
					looking = false;
					avatar.CannotControlFor(controlLockTime);
					avatar.movement.SetVelocity(dist.normalized * throwForce);
					Invoke("KeepLooking", throwTime);
				}
			}
		}
		
	}
	
	void KeepLooking(){
		looking = true;
	}
	
	void OnDrawGizmos(){
		t = GetComponent<Transform>();
		Gizmos.DrawLine(t.position, t.position + t.right * lookDist);
	}
}

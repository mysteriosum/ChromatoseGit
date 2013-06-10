using UnityEngine;
using System.Collections;

public class GenericTrigger : MonoBehaviour {
	
	private bool triggered = false;
	
	Transform t;
	float initialZ;
	public bool Triggered{
		get{
			return triggered;
		}
		set{
			triggered = value;
			
			t.position = triggered? new Vector3(t.position.x, t.position.y, -100) : new Vector3(t.position.x, t.position.y, initialZ);
			
		}
	}
	
	// Use this for initialization
	void Start () {
		t = gameObject.GetComponent<Transform>();
		initialZ = t.position.z;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Trigger(){
		triggered = triggered? false : true;
	}
	
	
}

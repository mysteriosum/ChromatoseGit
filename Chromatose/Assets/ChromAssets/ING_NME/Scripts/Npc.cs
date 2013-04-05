using UnityEngine;
using System.Collections;

public class Npc : ColourBeing {
	
	Movement movement;
	Transform t;
	Transform target;
	
	public int detectRadius = 100;
	
	// Use this for initialization
	void Start () {
		movement = GetComponent<Movement>();
		t = GetComponent<Transform>();
		target = movement.target;
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		Vector2 diff = new Vector2(target.position.x, target.position.y) - new Vector2(t.position.x, t.position.y);
		
		if (diff.magnitude < detectRadius && CheckSameColour(target.GetComponent<ColourBeing>().colour)){
			float angle = VectorFunctions.PointDirection(diff);
			
			float zGoodFormat = VectorFunctions.Convert360(transform.rotation.eulerAngles.z);
			
			t.Rotate(0, 0, angle - zGoodFormat);
			Vector2 disp = movement.Displace(true);
			
			transform.position += new Vector3(disp.x, disp.y, 0);
		
			}
		
		else{
			movement.SlowToStop();
		}
	}	
	
	override public void Trigger(){
		
	}
}

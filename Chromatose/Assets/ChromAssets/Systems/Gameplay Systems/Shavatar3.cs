using UnityEngine;
using System.Collections;

public class Shavatar3 : MonoBehaviour {

	private float _Magnitude;
	private float _Accel = 5;
	private float maxSpeed = 100;
	private Vector2 _Velocity;
	
	private float angle;
	
	void Start () {
	
	}
	/*
	// Update is called once per frame
	void Update () {
		//Move ();
		if (Vector3.Distance(target.position, t.position) < 50){
			movement.SetNewMoveStats(1f, 0.5f, basicTurnSpeed);
		}
	}
	
	public Vector2 Move(float angle){								//Displacement : Thrust, accel, stuff
			Vector2 displacement = new Vector2(Mathf.Cos(angle) * _Accel, Mathf.Sin(angle) * _Accel);
			_Velocity += displacement;	
			//Debug.Log(thruster.velocity);
			if (_Velocity.magnitude > maxSpeed){
				_Velocity = _Velocity.normalized * maxSpeed;
			}
		return _Velocity * Time.deltaTime;
	}
	
	*/
}

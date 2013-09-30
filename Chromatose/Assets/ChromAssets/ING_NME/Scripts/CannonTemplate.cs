using UnityEngine;
using System.Collections;

public class CannonTemplate : MonoBehaviour {

	
	public int speed = 75;
	public GameObject target;

	
	void Start () {
	}

	void Update () {
		Move();
		
		if(Vector2.Distance((Vector2)target.transform.position, (Vector2)transform.position) < 5f){
			DestroyThis();
		}
	}

	void Move(){
		Vector2 traj = ((Vector2)target.transform.position - (Vector2)transform.position);
		traj = traj.magnitude > speed * Time.deltaTime ? traj.normalized * speed * Time.deltaTime : Vector2.zero;		//Adjust the traj!
		transform.Translate(traj, Space.World);
	}
	
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag != "avatar")return;
		ChromatoseManager.manager.Death();
	}
	
	void Activate(){
		this.collider.enabled = true;
		this.renderer.enabled = true;
		this.enabled = true;
	}
	
	void Deactivate(){
		this.collider.enabled = false;
		this.renderer.enabled = false;
		this.enabled = false;
	}
	
	void DestroyThis(){
		Destroy(this.gameObject);
	}
}

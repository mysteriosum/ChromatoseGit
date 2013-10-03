using UnityEngine;
using System.Collections;

public class MovingBlob : MonoBehaviour {
	
	
public bool waitingAvatarInZone = false;
	public bool addExtraCollisionBox = false;
	
	public bool patrol = false;
	public int patrolSpeed = 75;
	
	public bool rotate = false;
	public float rotateRate = 0.1f;
	public bool inverseRotation = false;
	
	public Transform[] patrolNodes;
	
	
	
	private Transform _AvatarT;
	private Avatar _AvatarScript;
	private ChromatoseManager _Manager;
	private MovingBlob_DetectionZone _DetectionZone;

	private int currentIndex = 0;
	private int maxIndex;
	
	void Start () {
		_DetectionZone = transform.parent.gameObject.GetComponentInChildren<MovingBlob_DetectionZone>();
		SetupBlob();
		SetupPatrol();
	}
	
	// Update is called once per frame
	void Update () {
		if(waitingAvatarInZone){
			if(_DetectionZone && _DetectionZone.inZone){
				Move();
				Rotate();
			}	
		}
		else{
			Move();
			Rotate();
		}
	}
	
	void SetupPatrol(){
		maxIndex = patrolNodes.Length;
		
	}
	
	IEnumerator SetupBlob(){
		yield return new WaitForSeconds(0.1f);
		_AvatarT = GameObject.FindGameObjectWithTag("avatar").transform;
		_AvatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		_Manager = ChromatoseManager.manager;
	}
	
	void Move(){
		if(!patrol)return;
		Vector2 traj = ((Vector2)patrolNodes[currentIndex].position - (Vector2)transform.position);
		traj = traj.magnitude > patrolSpeed * Time.deltaTime ? traj.normalized * patrolSpeed * Time.deltaTime : Vector2.zero;		//Adjust the traj!
		
		if (traj == Vector2.zero){
			currentIndex ++;
			
			if (currentIndex >= maxIndex){
				currentIndex = 0;
			}				
		}
		else{
			transform.Translate(traj, Space.World);
		}
	}
	void Rotate(){
		if(!rotate)return;
		
		if(!inverseRotation){
			transform.RotateAround(Vector3.back, rotateRate/100);
		}
		else{
			transform.RotateAround(Vector3.back, -rotateRate/100);
		}
	}
	
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag != "avatar")return;
		
		_Manager.Death();
	}
}

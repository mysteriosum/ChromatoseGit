using UnityEngine;
using System.Collections;

public class RotatorScript : MonoBehaviour {
	
	public float rotateSpeed = 6f;
	private Transform t;
	// Use this for initialization
	void Start () {
		t = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		t.Rotate(0, 0, 360 * Time.deltaTime / rotateSpeed, Space.World);
	}
}

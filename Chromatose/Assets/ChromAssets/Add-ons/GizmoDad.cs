using UnityEngine;
using System.Collections;

public enum Couleur{
	black = 0,
	white = 1,
	red = 2,
	blue = 3,
	green = 4,
	yellow = 5,
	magenta = 6,
	cyan = 7,
}

public class GizmoDad : MonoBehaviour {
	
	
	public enum Forme{
		cube = 0,
		wireCube = 1,
		sphere = 2,
		wireSphere = 3,
	}
	
	public Couleur myColour = Couleur.black;
	public Forme myShape = Forme.wireSphere;
	public float radius = 50f;
	public Vector3 size = Vector3.one;
	public bool useCollider = false;
	public float alpha = 1f;
	protected Color myColor;
	
	
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	
	void OnDrawGizmos(){
		
		Transform t = GetComponent<Transform>();
		BoxCollider derCollider = GetComponent<BoxCollider>();
		
		
		alpha = Mathf.Clamp01(alpha);
		if (derCollider)
			size = useCollider ? derCollider.size : size;
		
		switch (myColour){
			
		case Couleur.black:
			myColor = new Color(0, 0, 0, alpha);
			break;
			
		case Couleur.white:
			myColor = new Color(255, 255, 255, alpha);
			break;
			
		case Couleur.red:
			myColor = new Color(255, 0, 0, alpha);
			break;
			
		case Couleur.blue:
			myColor = new Color(0, 0, 255, alpha);
			break;
			
		case Couleur.green:
			myColor = new Color(0, 255, 0, alpha);
			break;
			
		case Couleur.yellow:
			myColor = new Color(255, 0, 255, alpha);
			break;
			
		case Couleur.magenta:
			myColor = new Color(255, 255, 0, alpha);
			break;
			
		case Couleur.cyan:
			myColor = new Color(0, 255, 255, alpha);
			break;
		}
		
		Matrix4x4 rotMatrix = Matrix4x4.TRS(t.position, t.rotation, t.lossyScale);
		Gizmos.matrix = rotMatrix;
		Gizmos.color = myColor;
		
		
		switch(myShape){
		case Forme.cube:
			Gizmos.DrawCube(Vector3.zero, size);
			break;
		case Forme.sphere:
			Gizmos.DrawSphere(Vector3.zero, radius);
			break;
		case Forme.wireCube:
			Gizmos.DrawWireCube(Vector3.zero, size);
			break;
		case Forme.wireSphere:
			Gizmos.DrawWireSphere(Vector3.zero, radius);
			break;
			
		}
	}
	/*
	void OnDrawGizmosSelected(){
		
		Transform t = GetComponent<Transform>();
		BoxCollider derCollider = GetComponent<BoxCollider>();
		
		if (t.localScale != Vector3.one && derCollider){
			Vector3 diff = t.localScale - Vector3.one;
			derCollider.size += diff;
			t.localScale = Vector3.one;
		}	
	}*/
}

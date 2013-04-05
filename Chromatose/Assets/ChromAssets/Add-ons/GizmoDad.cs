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
	public float radius = 1;
	public Vector3 size = Vector3.one;
	public bool useCollider = false;
	private Color myColor;
	
	
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnDrawGizmos(){
		Transform t = GetComponent<Transform>();
		BoxCollider collider = GetComponent<BoxCollider>();
		
		if (t.localScale != Vector3.one){
			Vector3 diff = t.localScale - Vector3.one;
			collider.size += diff;
			t.localScale = Vector3.one;
		}
		
		
		size = useCollider ? collider.size : size;
		
		switch (myColour){
			
		case Couleur.black:
			myColor = Color.black;
			break;
			
		case Couleur.white:
			myColor = Color.white;
			break;
			
		case Couleur.red:
			myColor = Color.red;
			break;
			
		case Couleur.blue:
			myColor = Color.blue;
			break;
			
		case Couleur.green:
			myColor = Color.green;
			break;
			
		case Couleur.yellow:
			myColor = Color.yellow;
			break;
			
		case Couleur.magenta:
			myColor = Color.magenta;
			break;
			
		case Couleur.cyan:
			myColor = Color.cyan;
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
}

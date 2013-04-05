using UnityEngine;
using System.Collections;

public class RoomData : MonoBehaviour {
	
	public GameObject collisionMom;
	public GameObject ingredientMom;
	public GameObject artMom;
	public GameObject collectibleMom;
	public GameObject nodeMom;
	
	
	Collectible[] collectibles;
	Transform[] collisions;
	ColourBeing[] ingredients;
	Transform[] artElements;
	Transform[] nodes;
	
	
	// Use this for initialization
	void Start () {
		
		collectibles = collectibleMom.GetComponentsInChildren<Collectible>(true);
		collisions = collisionMom.GetComponentsInChildren<Transform>(true);
		nodes = nodeMom.GetComponentsInChildren<Transform>(true);
		artElements = artMom.GetComponentsInChildren<Transform>(true);
		ingredients = ingredientMom.GetComponentsInChildren<ColourBeing>(true);
		
		foreach(ColourBeing cb in ingredients){
			
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}

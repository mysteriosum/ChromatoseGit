using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class ComicBackground : MonoBehaviour{
	
	public Rect[] slots;
	public Transform[] occupants;
	public Transform dud;
	
	void Start(){
		List<Transform> tList = new List<Transform>();
		for (int i = 0; i < slots.Length; i ++){
			tList.Add(transform);
		}
		
		occupants = tList.ToArray();
	}
	
	void Update(){
		
		
		
	}
	
	
	
	void OnDrawGizmosSelected(){
		foreach (Rect r in slots){
			Vector2 topLeft = new Vector2(r.xMin, r.yMin);
			Vector2 topRight = new Vector2(r.xMax, r.yMin);
			Vector2 bottomRight = new Vector2(r.xMax, r.yMax);
			Vector2 bottomLeft = new Vector2(r.xMin, r.yMax);
			
			Gizmos.DrawLine((Vector2)topLeft, (Vector2)topRight);
			Gizmos.DrawLine((Vector2)topRight, (Vector2)bottomRight);
			Gizmos.DrawLine((Vector2)bottomRight, (Vector2)bottomLeft);
			Gizmos.DrawLine((Vector2)bottomLeft, (Vector2)topLeft);
			
		}
	}
	
	public int CheckForSlot(Vector2 pos){
		int counter = 0;
		int slot = -1;
		foreach (Rect r in slots){
			if (r.Contains(pos)){
				Debug.Log("It's here! in slot " + counter);
				slot = counter;
				break;
			}
			counter ++;
		}
		return slot;
	}
	
	public int CheckForSlot(){
		return CheckForSlot(Input.mousePosition);
	}
	
	public Vector2 PutInSlot(int index, Transform goingIn, out Transform goingOut){
		if (index < 0){
			Debug.Log("The index is too low. This won't give you a slot!");
			goingOut = null;
			return (Vector2)goingIn.position;
		}
		Debug.Log("Index = " + index + " which is compared to the size of occupants: " + occupants.Length);
		Debug.Log("0: Going in = " + goingIn.name + " and occupants[ind] = " + occupants[index].name);
		
		Rect slot = slots[index];
		if (occupants[index] == transform || occupants[index] == goingIn){
			
			goingOut = null;
		}
		else{
			goingOut = occupants[index];
		}
		Debug.Log("1: Going out = " + goingOut + ", going in = " + goingIn + " and occupants[ind] = " + occupants[index]);
		occupants[index] = goingIn;
		Debug.Log("2: Going out = " + goingOut + ", going in = " + goingIn + " and occupants[ind] = " + occupants[index]);
		return new Vector2(slot.x + slot.width/2, slot.y + slot.height / 2);
	}
	
	public Vector2 PutInSlot(int index, Transform goingIn){
		return PutInSlot(index, goingIn, out dud);
	}
	
	public void RemoveFromSlot(int index){
		Debug.Log("Removing from index " + index);
		if (index < 0) return;
		occupants[index] = transform;
	}
	
}

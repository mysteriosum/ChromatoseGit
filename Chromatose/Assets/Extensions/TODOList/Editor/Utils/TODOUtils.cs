using UnityEngine;
using UnityEditor;
using System.Collections;

public class TODOUtils : MonoBehaviour {

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------
	
	//--------------------------------------
	// GET / SET
	//--------------------------------------

	public static bool isLightMode {
		get {
			if (EditorStyles.label.normal.textColor.r == 0f) {
				return true;
			} else {
				return false;
			}
		}
	}
	
	//--------------------------------------
	// EVENTS
	//--------------------------------------
	
	//--------------------------------------
	// PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	// DESTROY
	//--------------------------------------
}

using UnityEngine;
using System.Collections;

public class DontDestroyScript : MonoBehaviour {

	void Awake(){
		DontDestroyOnLoad(this.gameObject);
		
		if(GameObject.Find("Button")){
			Destroy(this.gameObject);
		}
	}
	
	
}

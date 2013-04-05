using UnityEngine;
using System.Collections;

static class VectorFunctions {

	// Use this for initialization
	
	static public float Convert360(float angle){		//convert an angle which is normally 0-360 into one that's 0-180 or -180 - -1
		return angle < 180 ? angle : (angle - 2 * (angle - 180)) * -1;
	}
	
	static public float PointDirection(Vector2 vector){
		 float angle = Mathf.Acos(vector.x / vector.magnitude) * Mathf.Rad2Deg;
			
			if (vector.y < 0){
				angle *= -1;
			}
		return angle;
	}
	
}

using UnityEngine;
using System.Collections;

static class VectorFunctions {

	/// <summary>
	/// Convert the specified angle (0 to 180 or -1 to -181) to a positive one (0 to 359).
	/// </summary>
	/// <param name='angle'>
	/// The angle to convert.
	/// </param>
	static public float Convert360(float angle){		//convert an angle which is normally 0-360 into one that's 0-180 or -180 - -1
		return angle < 180 ? angle : (angle - 2 * (angle - 180)) * -1;
	}
	/// <summary>
	/// Gets the slope of a vector.
	/// </summary>
	/// <returns>
	/// The direction in radians of the vector in question.
	/// </returns>
	/// <param name='vector'>
	/// Vector.
	/// </param>
	static public float PointDirection(Vector2 vector){
		 float angle = Mathf.Acos(vector.x / vector.magnitude) * Mathf.Rad2Deg;
			
			if (vector.y < 0){
				angle *= -1;
			}
		return angle;
	}
	
	static public Vector3 ConvertLookDirection(Vector3 convertee){
		return new Vector3(convertee.z, convertee.y, convertee.x);
	}
	
	static public Transform FindClosestOfTag(Vector3 closestTo, string tag, int maxDistance){
		GameObject[] tagged = GameObject.FindGameObjectsWithTag(tag);
		float closestDist = maxDistance;
		Transform closestHere = null;
		foreach (GameObject n in tagged){
			float tempDist = (closestTo - n.transform.position).magnitude;		//This was cast as (Vector2), but I don't think it's necessary. Maybe it will be eventually?
			
			if (tempDist < closestDist){
				closestDist = tempDist;
				closestHere = n.transform;
			}
		}
		
		return closestHere;
		
	}
	
}

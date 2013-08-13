using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(OptiManager)), CanEditMultipleObjects]
public class OptiManagerEditor : Editor {
	
	private OptiManager _OptiManager;

	private int _RoomToDisplay = 0;
	
	public GameObject[] roomList;
	
	void Awake(){
		_OptiManager = (OptiManager)target;
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();
		
		GUILayout.Space (15);
		
		
		GUILayout.Label("Choose a Room Number to Optimize");
		GUILayout.Space(7);
		_RoomToDisplay = EditorGUILayout.IntField("Room Number Here", _RoomToDisplay);

		GUILayout.Space(7);
		if(GUILayout.Button("Optimize That Room", GUILayout.Height(40))){
			if (EditorApplication.isPlaying || EditorApplication.isPaused){
				_OptiManager.OptimizeZone(_RoomToDisplay);
			}
		}
	}
	
	
}

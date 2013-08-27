using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(RoomInstancier)), CanEditMultipleObjects]
public class RoomInstancierEditor : Editor {
	
	
	private RoomInstancier roomInst;
	
	
	void Awake(){
		roomInst = (RoomInstancier)target;
	}
	
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();
		GUILayout.Space(5f);
		
		if(GUILayout.Button("Save This Room", GUILayout.Height(40))){
			if (EditorApplication.isPlaying || EditorApplication.isPaused){
				roomInst.SaveRoom();
			}
		}
		GUILayout.Space(5f);
		
		if(GUILayout.Button("Load This Room", GUILayout.Height(40))){
			if (EditorApplication.isPlaying || EditorApplication.isPaused){
				roomInst.LoadRoom();
			}
		}
	}
}

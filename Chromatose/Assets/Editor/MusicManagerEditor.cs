using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MusicManager)), CanEditMultipleObjects]
public class MusicManagerEditor : Editor {
	
	private MusicManager _MusicManager;
	
	private bool _SetupDone = false;
	
	public SerializedProperty
		devStateProp;
	
	void OnEnable(){
		devStateProp = serializedObject.FindProperty("_DevState");
		
		
		
		/*
		foreach(AudioClip track in _MusicManager.playlist){
			AudioSource temp = _MusicManager.gameObject.AddComponent<AudioSource>();
			temp.clip = track;
			_MusicManager.musicSources.Add(temp);
			temp.loop = false;
			//temp.Play();
		}*/
	}
	
	void Awake(){
		_MusicManager = (MusicManager)target;
	
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public override void OnInspectorGUI(){
		serializedObject.Update();
		DrawDefaultInspector ();
		
		
		
		
		GUILayout.Space(5);
		
		//_MusicManager.devState = EditorGUILayout.EnumPopup(_MusicManager.devState.ToString(), _MusicManager.devState);
		EditorGUILayout.PropertyField(devStateProp);
		MusicManager._DevStateEnum dS = (MusicManager._DevStateEnum)devStateProp.enumValueIndex;
		
		GUILayout.Space(20);
		
			_MusicManager.volume = EditorGUILayout.IntSlider("Volume", _MusicManager.volume, 0, 100 );
			
		GUILayout.Space(10);
		
			_MusicManager.muteControl = EditorGUILayout.Foldout(_MusicManager.muteControl, "Mute Control Panel");
			if(_MusicManager.muteControl){
				_MusicManager.muteMusic = EditorGUILayout.Toggle("Mute Music", _MusicManager.muteMusic);
				_MusicManager.muteSFX = EditorGUILayout.Toggle("Mute SFX", _MusicManager.muteSFX);
			}
		
		GUILayout.Space(5);
		
			_MusicManager.playOption = EditorGUILayout.Foldout(_MusicManager.playOption, "Play Option");
			if(_MusicManager.playOption){
				_MusicManager.loopPlaylist = EditorGUILayout.Toggle("Loop Playlist", _MusicManager.loopPlaylist);
				_MusicManager.shufflePlaylist = EditorGUILayout.Toggle("Shuffle Playlist", _MusicManager.shufflePlaylist);
			}
		
		GUILayout.Space(5);
		
			_MusicManager.crossfadeOption = EditorGUILayout.Foldout(_MusicManager.crossfadeOption, "Crossfade Option");
			if(_MusicManager.crossfadeOption){
				_MusicManager.crossfadeBetweenTrack = EditorGUILayout.Toggle("Crossfade Between Track", _MusicManager.crossfadeBetweenTrack);
				_MusicManager.fadeIn = EditorGUILayout.Toggle("Allow FadeIn", _MusicManager.fadeIn);
				_MusicManager.crossfadeTime = EditorGUILayout.Slider("Crossfade Time", _MusicManager.crossfadeTime, 1, 10);
			}
		
		GUILayout.Space(15);
			
			EditorGUILayout.BeginVertical();
				
				_MusicManager.trackAddToPlaylist = EditorGUILayout.ObjectField("Select a Music", _MusicManager.trackAddToPlaylist, typeof(AudioClip))as AudioClip;
				GUILayout.Space(5);
				if(GUILayout.Button("Add this Track to Playlist", GUILayout.Height(40))){
					if (_MusicManager.trackAddToPlaylist != null){
						AudioSource temp = _MusicManager.gameObject.AddComponent<AudioSource>();
						temp.clip = _MusicManager.trackAddToPlaylist;
						//temp.clip.name = _MusicManager.trackAddToPlaylist.name;
					//	_MusicManager.musicSources[_MusicManager.musicSources.Length + 1] = temp);
						_MusicManager.musicSources.Add(temp);
						temp.loop = false;
						temp.playOnAwake = false;
						temp.volume = 80;
						_MusicManager.trackAddToPlaylist = null;
						EditorUtility.SetDirty(_MusicManager);
					}
					else{
						Debug.LogWarning("MUSICMANAGER : No Track to Add, Please select a Track one line below the Add Button");
					}
				}
	
		
		GUILayout.Space(10);
		
		_MusicManager.musicMixerOn = EditorGUILayout.Foldout(_MusicManager.musicMixerOn, "MUSIC MIXER");
		if(_MusicManager.musicMixerOn){
			GUILayout.Space(10);
			DisplayCurrentPlaylist();
		}
				
		GUILayout.Space(30);
		
				
			EditorGUILayout.EndHorizontal();
			
			
			
			
			
		
	}	
	
	void DisplayCurrentPlaylist(){
		
		if(_MusicManager.musicSources.Count == 0){return;}
		
		GUILayout.Space(10);
		
		EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("PLAY FULL PLAYLIST")){
				
			}
			if(GUILayout.Button ("STOP PLAYLIST")){
				Debug.Log(_MusicManager.musicSources.Count);
			}
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("DELETE PLAYLIST")){
				_MusicManager.musicSources.Clear();
				_MusicManager.gameObject.RemoveComponent(typeof(AudioSource));
			}
		EditorGUILayout.EndHorizontal();
		
		
		
		for(int i = 0; i < _MusicManager.musicSources.Count; i++){
			
			GUILayout.Space(10);
			GUILayout.Label("TRACK " + i);
			
			GUILayout.Space(10);
			
			EditorGUILayout.BeginHorizontal();
				GUILayout.Label("TRACK NAME (Can Renamed)");
				_MusicManager.musicSources[i].clip.name = GUILayout.TextField(_MusicManager.musicSources[i].clip.name, GUILayout.Width(170));
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("PLAY")){
					_MusicManager.musicSources[i].Play();
				}
				if(GUILayout.Button("PAUSE")){
					
				}
				if(GUILayout.Button("STOP")){
					_MusicManager.musicSources[i].Stop();
				}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
				
				if(GUILayout.Button("UP IN PLAYLIST")){
				
				}
				if(GUILayout.Button("DOWN IN PLAYLIST")){
				
				}
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Space(10);
			
			EditorGUILayout.BeginHorizontal();
				GUILayout.Space(60);
				GUILayout.Label("VOLUME");
				GUILayout.Space(-50);
				GUILayout.Label("TOGGLE OPTION");
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(5);
			
			EditorGUILayout.BeginHorizontal();
				GUILayout.Space(80);
				_MusicManager.musicSources[i].volume = GUILayout.VerticalSlider(_MusicManager.musicSources[i].volume, 1, 0);
				GUILayout.Space(40);
				EditorGUILayout.BeginVertical();
					_MusicManager.musicSources[i].mute = EditorGUILayout.Toggle("MUTE", _MusicManager.musicSources[i].mute);
					_MusicManager.musicSources[i].mute = EditorGUILayout.Toggle("SOLO", _MusicManager.musicSources[i].mute);
					_MusicManager.musicSources[i].loop = EditorGUILayout.Toggle("LOOP", _MusicManager.musicSources[i].loop);
					_MusicManager.musicSources[i].bypassEffects = EditorGUILayout.Toggle("BYPASS EFFECTS", _MusicManager.musicSources[i].bypassEffects);
				EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}		
	}
}

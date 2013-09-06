using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MusicManager : MonoBehaviour{
	public enum _DevStateEnum{
		EditorDevelopment, StandAlone, WebPlayer
	}
	
	
	public static MusicManager soundManager;
	
	
	[HideInInspector]
	public _DevStateEnum _DevState;
	private bool _MuteControl = false;
		public bool muteControl{
			get{return _MuteControl;}
			set{_MuteControl = value;}
		}
	private bool _MuteMusic = false;
		public bool muteMusic{
			get{return _MuteMusic;}
			set{_MuteMusic = value;}
		}
	private bool _MuteSFX = false;
		public bool muteSFX{
			get{return _MuteSFX;}
			set{_MuteSFX = value;}
		}
	private int _Volume = 80;
		public int volume{
			get{return _Volume;}
			set{_Volume = Mathf.Clamp(value, 0, 100);;}
		}
	private bool _PlayOption = false;
		public bool playOption{
			get{return _PlayOption;}
			set{_PlayOption = value;}
		}
	private bool _LoopPlaylist = false;
		public bool loopPlaylist{
			get{return _LoopPlaylist;}
			set{_LoopPlaylist = value;}
		}
	private bool _ShufflePlaylist = false;
		public bool shufflePlaylist{
			get{return _ShufflePlaylist;}
			set{_ShufflePlaylist = value;}
		}
	private bool _CrossfadeOption = false;
		public bool crossfadeOption{
			get{return _CrossfadeOption;}
			set{_CrossfadeOption = value;}
		}
	private float _CrossfadeTime = 80; //Mathf.Clamp(3, 1, 10);
		public float crossfadeTime{
			get{return _CrossfadeTime;}
			set{_CrossfadeTime = Mathf.Clamp(value, 0f, 10f);}
		}
	private bool _CrossfadeBetweenTrack = false;
		public bool crossfadeBetweenTrack{
			get{return _CrossfadeBetweenTrack;}
			set{_CrossfadeBetweenTrack = value;}
		}
	private bool _FadeIn = false;
		public bool fadeIn{
			get{return _FadeIn;}
			set{_FadeIn = value;}
		}
	private bool _GameOption = false;
		public bool gameOption{
			get{return _GameOption;}
			set{_GameOption = value;}
		}
	private bool _PlayOnStart;
		public bool playOnStart{
			get{return _PlayOnStart;}
			set{_PlayOnStart = value;}
		}
	private bool _MusicMixerOn = false;
		public bool musicMixerOn{
			get{return _MusicMixerOn;}
			set{_MusicMixerOn = value;}
		}

	private AudioClip _TrackAddToPlaylist = null;
		public AudioClip trackAddToPlaylist{
			get{return _TrackAddToPlaylist;}
			set{_TrackAddToPlaylist = value;}
		}
	
	private float[] _TrackVolumeList ;
		public float[] trackVolumeList{
			get{return _TrackVolumeList;}
			set{_TrackVolumeList = value;}
		}	
	
	[SerializeField][HideInInspector]
	private List<AudioSource> _MusicSources = new List<AudioSource>();
		public List<AudioSource> musicSources{
			get{return _MusicSources;}
			set{_MusicSources = value;}
		}
	[SerializeField][HideInInspector]
	private List<AudioClip> _MusicToPlay = new List<AudioClip>();
		public List<AudioClip> musicToPlay{
			get{return _MusicToPlay;}
			set{_MusicToPlay = value;}
		}
	
	
	public AudioClip[] _SFXList;
	
	
	private ChromatoseManager _Manager;
	private int _CurTrack = 0;
	
	[System.Serializable]
	public class MusicSources{

		public AudioSource aSource = null;


	}
	
	
	//Var Very Private
	private AudioSource[] _SFXPlayer;
	
	private bool setuped = false;
	
	
	
	
	
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		DontDestroyOnLoad(this);
		
		Setup();
	}
	
	void Setup(){
		soundManager = this;
		
		_Manager = ChromatoseManager.manager;
		GameObject sfxTempObj = GameObject.FindGameObjectWithTag("SFXPlayer");
		_SFXPlayer = sfxTempObj.GetComponents<AudioSource>();
		
		CheckLevel();
		
		setuped = true;
	}
	
	public void CheckLevel(){
		
		int curLevel = Application.loadedLevel;
		
		switch(curLevel){
			//MAIN MENU
		case 0:
			_MusicSources[0].Play();
			break;
			
			//TUTO
		case 1:
			_MusicSources[1].Play();
			break;
			
			//LEVEL
		case 2:
			_MusicSources[2].Play();
			break;
		case 3:
			_MusicSources[3].Play();
			break;
		case 4:
			_MusicSources[4].Play();
			break;
		case 5:
			_MusicSources[5].Play();
			break;
		case 6:
			_MusicSources[6].Play();
			break;
		case 7:
			_MusicSources[7].Play();
			break;
		case 8:
			_MusicSources[8].Play();
			break;
		case 9:
			_MusicSources[9].Play();
			break;
			
			//BOSS FINAL
		case 10:
			_MusicSources[10].Play();
			break;			
		}
	}

	void Update () {
		if(!setuped)Setup();
		
		
		//DEBUG CALL
		Debug.Log(_SFXPlayer.Length);
	
	}
	
	public void PlaySFX(int sfxIndex, float volume){
		
		foreach(AudioSource sfxP in _SFXPlayer){
			if(!sfxP.isPlaying){
				sfxP.PlayOneShot(_SFXList[sfxIndex], volume);
				Debug.Log("Play on Player ");
				return;
			}
		}
	}	
	public void PlaySFX(int sfxIndex){
		PlaySFX(sfxIndex, 1.0f);
	}
	
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MusicManager : MainManager{
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
		}/*
	private int _Volume = 80;
		public int volume{
			get{return _Volume;}
			set{_Volume = Mathf.Clamp(value, 0, 100);;}
		}*/
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
	private bool onCrossfade = false;
	
	
	void OnLevelWasLoaded(){
		//StartCoroutine(CheckLevel());
		CheckLevel();
	}
	
	void Start () {	
		//StartMenuMusic();
		Setup();
	}
	
	void Update(){
		if(!setuped)Setup();
		bool musicOn = CheckMusicOn();
		if(!musicOn){
			CheckLevel();
		}
	}
	
	void Setup(){
		soundManager = this;
		
		_Manager = ChromatoseManager.manager;
		GameObject sfxTempObj = GameObject.FindGameObjectWithTag("SFXPlayer");
		_SFXPlayer = sfxTempObj.GetComponents<AudioSource>();
		
		CheckLevel();
		
		setuped = true;
	}
	
	public void StartMenuMusic(){
		CrossFadeMusic(0, 0.0025f);
	}
	
	public void CheckLevel(){
		//yield return new WaitForSeconds(1.0f);
		int curLevel = Application.loadedLevel;
		
		switch(curLevel){
			//MAIN MENU
		case 0:
			/*
			bool musicOn = CheckMusicOn();
			if(!musicOn){
				StartCoroutine(DelayingTrack(0,0f));
			}
			else{
				CrossFadeMusic(0, 0.0025f);
			}*/
			SwitchTrack(0);
			//CrossFadeMusic(0, 0.0025f);
			break;
			
			//TUTO
		case 1:
			SwitchTrack(1);
			//CrossFadeMusic(1, 0.0025f);
			break;
			
			//LEVEL
		case 2:
			SwitchTrack(2);
			//CrossFadeMusic(2, 0.0025f);
			break;
		case 3:
			SwitchTrack(1);
			//CrossFadeMusic(1, 0.0025f);
			break;
		case 4:
			SwitchTrack(2);
			//CrossFadeMusic(2, 0.0025f);
			break;
		case 5:
			SwitchTrack(1);
			//CrossFadeMusic(1, 0.0025f);
			break;
		case 6:
			SwitchTrack(2);
			//CrossFadeMusic(2, 0.0025f);
			break;
		case 7:
			SwitchTrack(1);
			//CrossFadeMusic(1, 0.0025f);
			break;
		case 8:
			SwitchTrack(3);
			//CrossFadeMusic(3, 0.0025f);
			break;
		case 9:
			SwitchTrack(3);
			//CrossFadeMusic(3, 0.0025f);
			break;
		case 10:
			SwitchTrack(3);
			//CrossFadeMusic(3, 0.0025f);
			break;
		case 11:
			SwitchTrack(2);
			//CrossFadeMusic(2, 0.0025f);
			break;
		case 12:
			SwitchTrack(4);
			//CrossFadeMusic(4, 0.0025f);
			break;
		}
	}
	
	bool CheckMusicOn(){
		foreach(AudioSource mS in _MusicSources){
			if(mS.isPlaying){
				return true;
			}
		}
		return false;
	}
	
	public void StopSFX(int sfxIndex){
		foreach(AudioSource sfxP in _SFXPlayer){
			if(sfxP.clip == _SFXList[sfxIndex]){
				sfxP.loop = false;
				sfxP.Stop();
				return;
			}
		}
	}
	public void PlaySFX(int sfxIndex, bool loop){
		
		foreach(AudioSource sfxP in _SFXPlayer){
			if(!sfxP.isPlaying){
				sfxP.loop = loop;
				sfxP.clip = _SFXList[sfxIndex];
				sfxP.volume = StatsManager.sfxVolume;
				sfxP.Play();
				Debug.Log("Play FX #" + sfxIndex);
				return;
			}
		}
	}
	public void PlaySFX(int sfxIndex, float vol){
		
		foreach(AudioSource sfxP in _SFXPlayer){
			if(!sfxP.isPlaying){
				sfxP.PlayOneShot(_SFXList[sfxIndex], vol);
				//Debug.Log("Play FX #" + sfxIndex);
				return;
			}
		}
	}	
	public void PlaySFX(int sfxIndex){
		PlaySFX(sfxIndex, StatsManager.sfxVolume);
	}
	
	void SwitchTrack(int musicIndex){
		foreach(AudioSource mPlayer in _MusicSources){
			if(mPlayer.isPlaying){
				mPlayer.Stop();
			}
		}
		_MusicSources[musicIndex].volume = StatsManager.musicVolume;
		_MusicSources[musicIndex].Play();
	}
	
	void CrossFadeMusic(int musicIndex, float fadeRate){
		
		foreach(AudioSource mPlayer in _MusicSources){
			if(mPlayer.isPlaying){
				mPlayer.Stop();
			}
		}
		
		int curPlayer = 0;
			
		for(int i = 0; i < _MusicSources.Count; i++){
			if (_MusicSources[i].isPlaying){
				curPlayer = i;
			}
		}
		
		_MusicSources[musicIndex].volume = 0.0f;
		_MusicSources[musicIndex].Play();
			
		onCrossfade = true;
		
		for(float f = 1f; f >= fadeRate; f = f - fadeRate){
			_MusicSources[curPlayer].volume = f;
			_MusicSources[musicIndex].volume += Mathf.Clamp(fadeRate * 2f, 0, StatsManager.musicVolume);
			//Debug.Log("f = " + f);
			if(f <= 0){
				_MusicSources[curPlayer].Stop ();
			}
		}
		
		StartCoroutine(ResetCrossfade());
	}
	
	public void ReplayCurTrack(){
		int curPlayer = 0;
			
		for(int i = 0; i < _MusicSources.Count; i++){
			if (_MusicSources[i].isPlaying){
				curPlayer = i;
			}
		}
		_MusicSources[curPlayer].Stop();
		_MusicSources[curPlayer].volume = StatsManager.musicVolume;
		_MusicSources[curPlayer].Play();
	}
	
	void SkipToNextTrack(){
		
	}
	
	void SkipToThisTrack(int musicIndex){
		
	}
	
	IEnumerator ResetCrossfade(){
		yield return new WaitForSeconds(0.5f);
	}
	IEnumerator DelayingTrack(int trackIndex, float delai){
		yield return new WaitForSeconds(delai);
		_MusicSources[trackIndex].volume = StatsManager.musicVolume;
		_MusicSources[trackIndex].Play();
	}
}


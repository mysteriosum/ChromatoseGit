using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TimeTrialManager : MonoBehaviour {
	/*
	private ChromatoseManager _Manager;
	
	private float _TimerTime = 0f;
	private float _StartTimerTime = 0f;
	private float _SecCounter = 0f;
	private float _MinCounter = 0f;
	private float _FractionCounter = 0f;
	private float _TimeToDisplay = 0;
	private float _FinalLevelTimes = 0;
	
	private bool _DisplayScore = false;
	public bool DisplayScore{
		get{return _DisplayScore;}
		set{_DisplayScore = value;}
	}
	private bool _DisplayWinWindows = false;
	public bool DisplayWinWindows{
		get{return _DisplayWinWindows;}
		set{_DisplayWinWindows = value;}
	}
	
	public bool _TimerOnPause = false; 		//<--- A Remettre private
		public bool TimerOnPause{
			get{return _TimerOnPause;}
		}
	
	private float _TotalPauseTime = 0f;
	private float _PauseTime = 0f;
	private float _StartPauseTime = 0f;
	private float _EndPauseTime = 0f;

	public static string _TimeString = "";
		public string TimeString{
			get{return _TimeString;}
			set{_TimeString = value;}
		}

	public static List<float> _TimesList;	// = new List<float>(10);
		public List<float> TimeList{
			get{return _TimesList;}
		}
	
	public float _Tuto_Time2Beat = 0;
	public float _Lev1_Time2Beat = 0;
	public float _Lev2_Time2Beat = 120;
	public float _Lev3_Time2Beat = 0;
	public float _Lev4_Time2Beat = 0;
	public float _Lev5_Time2Beat = 0;
	public float _Lev6_Time2Beat = 0;
	public float _Lev7_Time2Beat = 0;
	public float _Lev8_Time2Beat = 0;
	public float _Lev9_Time2Beat = 0;
	
	
	public static List<float> _RecordsList;// = new List<float>(10);
		public List<float> RecordsList{
			get{return _RecordsList;}
		}
	
	private float _NEW_Tuto_Time2Beat = 0;
	private float _NEW_Lev1_Time2Beat = 134.25f;
	private float _NEW_Lev2_Time2Beat = 134.77f;
	private float _NEW_Lev3_Time2Beat = 0;
	private float _NEW_Lev4_Time2Beat = 0;
	private float _NEW_Lev5_Time2Beat = 0;
	private float _NEW_Lev6_Time2Beat = 0;
	private float _NEW_Lev7_Time2Beat = 0;
	private float _NEW_Lev8_Time2Beat = 0;
	private float _NEW_Lev9_Time2Beat = 0;
	
	private float _Min2Beat = 0;
	private float _Sec2Beat = 0;
	private float _Fraction2Beat = 000;
	
	
	private static List<float> _NewRecordList;
	public List<float> NewRecordList{
		get{return _NewRecordList;}
	}
	
	private float _NewRecordTimes_Tuto = 0;
	private float _NewRecordTimes_Lev1 = 0;
	private float _NewRecordTimes_Lev2 = 0;
	private float _NewRecordTimes_Lev3 = 0;
	private float _NewRecordTimes_Lev4 = 0;
	private float _NewRecordTimes_Lev5 = 0;
	private float _NewRecordTimes_Lev6 = 0;
	private float _NewRecordTimes_Lev7 = 0;
	private float _NewRecordTimes_Lev8 = 0;
	private float _NewRecordTimes_Lev9 = 0;
	
	
	public static string _Time2BeatString = "";
		public string Time2BeatString{
			get{return _Time2BeatString;}
			set{_Time2BeatString = value;}
		}
	
	
	public void SetupTTT(){
	
	
	if(LevelSerializer.IsDeserializing)return;
		_Manager = ChromatoseManager.manager;
		
		//TODO Finir remplir les tableau contenant les temps a battre	
		if (_TimesList == null){
			_TimesList = new List<float>(10){ 	_Tuto_Time2Beat, _Lev1_Time2Beat, _Lev2_Time2Beat, 
												_Lev3_Time2Beat, _Lev4_Time2Beat, _Lev5_Time2Beat, 
												_Lev6_Time2Beat, _Lev7_Time2Beat, _Lev8_Time2Beat, _Lev9_Time2Beat };
		}	
		if (_RecordsList == null){
			_RecordsList = new List<float>(10){ _NEW_Tuto_Time2Beat, _NEW_Lev1_Time2Beat, _NEW_Lev2_Time2Beat, 
												_NEW_Lev3_Time2Beat, _NEW_Lev4_Time2Beat, _NEW_Lev5_Time2Beat, 
												_NEW_Lev6_Time2Beat, _NEW_Lev7_Time2Beat, _NEW_Lev8_Time2Beat, _NEW_Lev9_Time2Beat };
		}
		if (_NewRecordList == null){
			_NewRecordList = new List<float>(10){ _NewRecordTimes_Tuto, _NewRecordTimes_Lev1, _NewRecordTimes_Lev2,
												_NewRecordTimes_Lev3, _NewRecordTimes_Lev4, _NewRecordTimes_Lev5,
												_NewRecordTimes_Lev6, _NewRecordTimes_Lev7, _NewRecordTimes_Lev8, _NewRecordTimes_Lev9 };
		}
	}
	
	public string DisplayTimes2Beat(){
	
		
		_TimeToDisplay = (_TimesList[_Manager.CurRoom] > _RecordsList[_Manager.CurRoom]? _TimesList[_Manager.CurRoom] : _RecordsList[_Manager.CurRoom]);
		
		_Min2Beat = Mathf.Floor(_TimeToDisplay/60f);		
		_Sec2Beat = Mathf.RoundToInt(_TimeToDisplay % 60f);
		_Fraction2Beat = (_TimeToDisplay * 1000)%1000;
		
		_Time2BeatString = _Min2Beat + ":" + _Sec2Beat + ":" + _Fraction2Beat;
		_Time2BeatString = string.Format("{00:00}:{1:00}:{2:000}",_Min2Beat,_Sec2Beat,_Fraction2Beat);
		
		return _Time2BeatString;
	}
									//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
									//<-------------TIME TRIAL COUNTER------------->
									//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	public void TimeTrialCounter(){
			
		_TimerTime = Time.timeSinceLevelLoad - _TotalPauseTime - _StartTimerTime;		// - starttime

		_MinCounter = Mathf.Floor(_TimerTime/60f);
		_SecCounter = Mathf.RoundToInt(_TimerTime % 60f);
		_FractionCounter = (_TimerTime * 1000)%1000;			//TOFIX Glitch dans le compteur, se reset au retour de la pause au niveau des fraction
		_TimeString = _MinCounter + ":" + _SecCounter + ":" + _FractionCounter;
		
		_TimeString = string.Format("{00:00}:{1:00}:{2:000}",_MinCounter,_SecCounter,_FractionCounter);	
	
	}
	
	public bool CheckForNewRecords(){
		bool newRecord = false;
		//TODO Faire CheckUp pour verifier si le joueur a tous les collectible et comics
		if(_TimerTime < _TimesList[_Manager.CurRoom]){
			_RecordsList[_Manager.CurRoom] = _TimerTime;
			
		
			newRecord = true;
		}	
		return newRecord;
	}
	
	//TODO Faire Fonction Save/Load les records sur un XML externe
	

	
	public bool StopChallenge(){
		
		bool winGame = false;
		_DisplayScore = true;
		_FinalLevelTimes = _TimerTime;
		
		//Stop le Manager et le Jeu
		_Manager.avatar.CannotControlFor(false, 0f);
		Time.timeScale = 0;
		
		//Verifie si le temps du Joueur bats le Temps a battre
		if (_FinalLevelTimes < _TimeToDisplay){
			winGame = true;
			return winGame;
		}
		return winGame;
	}
	
	/// Mets le Compteur a Pause
	public void PauseTimer(){
		_TimerOnPause = true;
		_StartPauseTime = Time.fixedTime;
		Debug.Log("Pause start at : " + _StartPauseTime);
	}
	public void UnpauseTimer(){
		_TimerOnPause = false;
		_EndPauseTime = Time.fixedTime;
		_TotalPauseTime += _EndPauseTime - _StartPauseTime;
		Debug.Log("Pause ends at : " + _EndPauseTime);
		Debug.Log("Total de la Pause : " + _TotalPauseTime);
	}
	public void ResetTimer(){
		_StartTimerTime = Time.fixedTime;
		//TODO remettre les variable pour la endresultWindows a defaut
	}
	
	public void RestartLevel(){
		
		manager.ResetPos();
		ResetTimer();	
		DisplayScore = false;
		_DisplayWinWindows = false;
		Time.timeScale = 1;
		_Manager.avatar.CanControl();
	}*/
}

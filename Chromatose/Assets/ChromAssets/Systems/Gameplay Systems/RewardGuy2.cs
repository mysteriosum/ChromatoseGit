using UnityEngine;
using System.Collections;

public class RewardGuy2 : MonoBehaviour {
	
	
	public GameObject partRewGuy;
	
	private BoxCollider _BoxCollider;
	private AudioSource _SfxPlayer;
	private Vector3 _RewGuyPos;
	
	private bool _CanBe = false;
		public bool canBe{
			get{return _CanBe;}
			set{_CanBe = value;}
		}
	
	
	private Vector3[] partPos = new Vector3[8];
	private Collectible2[] _WhiteColl;
	
	private float _LongDist = 100, _ShortDist = 33;
	private bool _Created = false;
	
	void Start () {
		Setup();
	}
	
	void Update () {
	
	}
	
	void Setup(){
		_BoxCollider = GetComponent<BoxCollider>();
		_SfxPlayer = GetComponent<AudioSource>();
		_RewGuyPos = this.transform.position;
		_WhiteColl = GetComponentsInChildren<Collectible2>();
		
		partPos[0] = new Vector3(_RewGuyPos.x - _LongDist, _RewGuyPos.y + _ShortDist, _RewGuyPos.z - 10);
		partPos[1] = new Vector3(_RewGuyPos.x - _ShortDist, _RewGuyPos.y + _LongDist, _RewGuyPos.z - 10);
		partPos[2] = new Vector3(_RewGuyPos.x + _ShortDist, _RewGuyPos.y + _LongDist, _RewGuyPos.z - 10);
		partPos[3] = new Vector3(_RewGuyPos.x + _LongDist, _RewGuyPos.y + _ShortDist, _RewGuyPos.z - 10);
		
		partPos[4] = new Vector3(_RewGuyPos.x + _LongDist, _RewGuyPos.y - _ShortDist, _RewGuyPos.z - 10);
		partPos[5] = new Vector3(_RewGuyPos.x + _ShortDist, _RewGuyPos.y - _LongDist, _RewGuyPos.z - 10);
		partPos[6] = new Vector3(_RewGuyPos.x - _ShortDist, _RewGuyPos.y - _LongDist, _RewGuyPos.z - 10);
		partPos[7] = new Vector3(_RewGuyPos.x - _LongDist, _RewGuyPos.y - _ShortDist, _RewGuyPos.z - 10);
		
		foreach(Collectible2 wColl in _WhiteColl){
			wColl.gameObject.SetActive(false);
		}		
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag!="avatar")return;
		
		if(_CanBe && !_Created){
			CreateEffect();
			StartCoroutine(PlaySFX());
			StartCoroutine(DelaiToSpawnColl());
			_Created = true;
		}		
	}
	
	void OnTriggerStay(Collider other){
		if(other.tag!="avatar")return;
		
		if(_CanBe && !_Created){
			CreateEffect();
			StartCoroutine(PlaySFX());
			StartCoroutine(DelaiToSpawnColl());
			_Created = true;
		}
	}
	
	void CreateEffect(){
		for(int i = 0; i < 8; i++){
			StartCoroutine(InstanceEffect(i * 0.25f, i));
		}
	}
	
	IEnumerator InstanceEffect(float delai, int index){
		yield return new WaitForSeconds(delai);
		GameObject _VFX = Instantiate(partRewGuy, partPos[index], Quaternion.identity)as GameObject;
		_VFX.name = "VFX"+index;
	}
	IEnumerator PlaySFX(){
		yield return new WaitForSeconds(1.0f);
		_SfxPlayer.Play ();
	}
	IEnumerator DelaiToSpawnColl(){
		yield return new WaitForSeconds(1.0f);
		foreach(Collectible2 wColl in _WhiteColl){
			wColl.gameObject.SetActive(true);
		}	
	}
}

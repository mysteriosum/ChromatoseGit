using UnityEngine;
using System.Collections;

public class FlameBullet : MonoBehaviour {
	
	
	private GameObject _Avatar;				//Avatar
	private Avatar _AvatarScript;			//Script de l'Avatar
	private ChromatoseManager _Manager;		//Pour appeller la Mort
	
	private tk2dAnimatedSprite[] myFlames;
		
	private bool _Setuped = false;
	private bool _DestinationReached = false;
	private bool _CanDie = false;
	
	private float startTime;
	private float journeyTime = 5.0f;
	private float random1;
	private float random2;
	private float _FadeRate = 0.01f;
	private float _FadingCounter = 1;
	
	
	private Vector2 randomVelocity = Vector2.zero;
	private Vector3 randomPos = Vector3.zero;
	

	// Use this for initialization
	void Start () {
		Setup();
	}
	
	// Update is called once per frame
	void Update () {
		if(!_Setuped){
			Setup();
		}
		
		if(!_DestinationReached){
			Vector3 center = (gameObject.transform.position + randomPos) * 0.1f;
	        center -= new Vector3(0, 1, 0);
	        Vector3 riseRelCenter = gameObject.transform.position - center;
	        Vector3 setRelCenter = randomPos - center;
	        float fracComplete = (Time.time - startTime) / journeyTime * Time.deltaTime * 1.7f;
	        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
	        transform.position += center;
		}
		
		if(Vector3.Distance(gameObject.transform.position, randomPos) < 15){
			_DestinationReached = true;
		}
		
		if(_DestinationReached){
			StartCoroutine(DelaiBeforeFade(2.0f));
		}
		
		if(_CanDie){
			_FadingCounter -= _FadeRate;
			
			if(myFlames != null){
				foreach(tk2dAnimatedSprite sprite in myFlames){
					sprite.color = new Color(1, 0, 0, _FadingCounter);
				}
			}
			if(_FadingCounter <= 0 ){
				Die ();
			}
		}
		
	}
	
	void Setup(){
		
		_Setuped = true;
		
		_Avatar = GameObject.FindGameObjectWithTag("avatar");
		_AvatarScript = _Avatar.GetComponent<Avatar>();
		_Manager = ChromatoseManager.manager;
		
		if(GetComponentsInChildren<tk2dAnimatedSprite>() != null){
			myFlames = GetComponentsInChildren<tk2dAnimatedSprite>();
		}
		
		foreach(tk2dAnimatedSprite flm in myFlames){
			flm.Play("flame1");
		}
		
		startTime = Time.time;
		random1 = Random.Range(0.35f, 0.65f);
		random2 = Random.Range(0.5f, 1.5f);
		randomVelocity = Random.insideUnitCircle.normalized * Random.Range(40, 65);
		randomPos = _Avatar.transform.position + (Vector3)randomVelocity;
		
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag != "avatar")return;
		
		if(_AvatarScript.curColor != Color.red){
			_Manager.Death();
		}
		else{
			_CanDie = true;
		}
	}
	
	void Die(){
		Destroy(this.gameObject);
	}
	
	IEnumerator DelaiBeforeFade(float delai){
		yield return new WaitForSeconds(delai);
		_CanDie = true;
	}
}

using UnityEngine;
using System.Collections;

public enum bulletTypeEnum{
	none, flame, tint
}
public enum shootTypeEnum{
	down, atAvaPos, random
}

public class BossBullet : MonoBehaviour {

	
	public bulletTypeEnum bulletType = bulletTypeEnum.flame;
	public shootTypeEnum shootType;
	
	private Vector3 _TargetPos = Vector3.zero; public Vector3 targetPos { get { return _TargetPos; } set { _TargetPos = value; } }
	private float _BulletSpeed = 100; public float bulletSpeed { get { return _BulletSpeed; } set { _BulletSpeed = value; } }
	private float _LifeDistance = 1600; public float lifeDistance { get { return _LifeDistance; } set { _LifeDistance = value; } }
	private float _LifeTime = 10.0f; public float lifeTime { get { return _LifeTime; } set { _LifeTime = value; } }
	
		//Variable Pour Slerp
	private float _JourneyTime = 1F;
    private float _StartTime;
	private float _RealTimer = 0;
	private Vector3 _RandomPos;
	private Vector2 _RandomVelocity;
	
	
	private tk2dAnimatedSprite[] _BulletAnim;
	private GameObject _Boss;
	private EndBoss_DataBase _DataBase;
	
	private Vector3 _StartingPos;
	private Vector3 _AvaPosAtStart;
	
	void Start () {
		if(RandomNumber() <= 66){
			bulletType = bulletTypeEnum.tint;
		}
		_BulletAnim = GetComponentsInChildren<tk2dAnimatedSprite>();
		foreach(tk2dAnimatedSprite animSpr in _BulletAnim){
			float randomDelai = Random.Range(0f, 0.8f);
			switch(bulletType){
			case bulletTypeEnum.flame:
				animSpr.Play("flame1", randomDelai);
				break;
			case bulletTypeEnum.tint:
				animSpr.Play("flame5", randomDelai);
				break;
			}
			
			
				//Randomize the position of the flames
			Vector2 randomVelocity = Random.insideUnitCircle.normalized * Random.Range(5, 25);
			Vector3 randomPos = this.gameObject.transform.position + (Vector3)randomVelocity;
			animSpr.transform.position = randomPos;
		}
		
		_Boss = GameObject.FindGameObjectWithTag("Boss");
		_DataBase = _Boss.GetComponent<EndBoss_DataBase>();
		
			//Save the Start Position of the Bullet
		_StartingPos = this.transform.position;
			//Save the position of avatar at the Start of the Bullet
		_AvaPosAtStart = GameObject.FindGameObjectWithTag("avatar").transform.position;
			
			//Marque le Start dans le Temps
		_StartTime = Time.time;
		
		_RandomVelocity = Random.insideUnitCircle.normalized * Random.Range(40, 65);
		_RandomPos = _AvaPosAtStart + (Vector3)_RandomVelocity;
		
		
		
	}
	
	void Update () {
		
			//Se detruit selon le lifeDistance, setter dans le boss
		
		switch(shootType){
		case shootTypeEnum.down:
			gameObject.transform.Translate(Vector3.down * Time.deltaTime * _BulletSpeed);
			
			if(Vector3.Distance(transform.position, _StartingPos) >= _LifeDistance){
				Destroy(this.gameObject);
			}
			return;
			print ("Down to the Down");
			break;
		case shootTypeEnum.atAvaPos:
			
			Vector3 center = (gameObject.transform.position + _RandomPos) * 0.1F;
	        center -= new Vector3(0, 1, 0);
	        Vector3 riseRelCenter = gameObject.transform.position - center;
	        Vector3 setRelCenter = _RandomPos - center;
	        float fracComplete = (Time.time - _StartTime) / _JourneyTime * Time.deltaTime * 1.7f;
	        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
	        transform.position += center;
			
			_RealTimer += Time.deltaTime;
			if(_RealTimer > _LifeTime){
				Destroy(this.gameObject);
			}
			
			return;
			print("Shoot On Avatar !!");
			break;
		case shootTypeEnum.random:
			return;
			print("Not already Setuped");
			break;
		}
	}
	
	void MakeItTintBullet(){
		bulletType = bulletTypeEnum.tint;
	}
	
	int RandomNumber(){
		int rNb = Random.Range(0, 100);
		return rNb;
	}
	
	void OnTriggerEnter(Collider other){
		switch(bulletType){
		case bulletTypeEnum.flame:
			if(other.tag != "avatar")return;
			if(other.GetComponent<Avatar>().curColor != Color.red){
				ChromatoseManager.manager.Death();
			}
			return;
			break;
		case bulletTypeEnum.tint:
			if(other.tag == "avatar"){
				ChromatoseManager.manager.Death();
			}
			else if(other.tag == "bossWall"){
				Destroy(this.gameObject);		//A Modifier pour lui faire un fadeOut
			}
			break;
		case bulletTypeEnum.none:
			print("You Need Set this bullet");
			break;
		}
	}	
}

using UnityEngine;
using System.Collections;

[System.Serializable]
public class ComicBG : MonoBehaviour {
		
	public GameObject[] comic;
	public float[] yOffset;
	
	public GameObject rewardGuy;
	public GameObject partComic;
	
	
	private int _ComicCollected = 0;
	private bool _ComicDone = false;
	private bool _AvatarInHall = false;
	private bool[] onWait = new bool[]{false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};
	private bool[] alreadySpawn = new bool[]{false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};

	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
		if(_ComicCollected >= comic.Length && _ComicDone){
			if(rewardGuy != null){
				rewardGuy.GetComponent<RewardGuy2>().canBe = true;
				//Debug.Log("Must be Done");
			}
		}		
	}
	
	public void MakeActive(int index){
		//comic[index].SetActive(true);
		onWait[index] = true;
	}
	
	void OnTriggerEnter(Collider other){
		
		if(other.tag == "avatar"){
			for(int i = 0; i < onWait.Length; i++){
				if(onWait[i] == true && alreadySpawn[i] == false){
					StartCoroutine(SpawnPartInBG(i * 2.0f, i));
				}
			}
		
			if(_ComicCollected >= comic.Length && !_ComicDone){
				_ComicDone = true;
				//Debug.Log("Comic = " + _ComicDone);
			}
		}
	}
	
	void OnTriggerStay(Collider other){
		if(other.tag == "avatar"){
			if(_ComicCollected >= comic.Length && !_ComicDone){
				_ComicDone = true;
				//Debug.Log("Comic = " + _ComicDone);
			}
		}
	}
	
	IEnumerator SpawnPartInBG(float delai, int index){
		yield return new WaitForSeconds(delai);
		Vector3 partPos = comic[index].gameObject.transform.position;
		partPos.z = -5;
		partPos.y = partPos.y - (100 + yOffset[index]);
		GameObject partObj1 = Instantiate(partComic, partPos, Quaternion.identity)as GameObject;
		partPos.x = partPos.x - 30;
		GameObject partObj2 = Instantiate(partComic, partPos, Quaternion.identity)as GameObject;
		partPos.x = partPos.x + 60;
		GameObject partObj3 = Instantiate(partComic, partPos, Quaternion.identity)as GameObject;
		StartCoroutine(SpawnComicInBG(1.75f, index));
	}
	IEnumerator SpawnComicInBG(float delai, int index){
		yield return new WaitForSeconds(delai);
		comic[index].SetActive(true);
		alreadySpawn[index] = true;
		_ComicCollected++;
		StatsManager.totalComicViewed++;
		MusicManager.soundManager.PlaySFX(1);
	}
}

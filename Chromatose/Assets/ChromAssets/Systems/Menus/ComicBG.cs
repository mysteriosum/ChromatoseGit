using UnityEngine;
using System.Collections;

[System.Serializable]
public class ComicBG : MonoBehaviour {
		
	public GameObject[] comic;
	
	public GameObject rewardGuy;
	
	private int _ComicCollected = 0;
	private bool _ComicDone = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(_ComicCollected >= comic.Length && _ComicDone){
			if(rewardGuy != null){
				rewardGuy.SetActive(true);
			}
		}		
	}
	
	public void MakeActive(int index){
		comic[index].SetActive(true);
		_ComicCollected++;
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "avatar"){
			if(_ComicCollected >= comic.Length && !_ComicDone){
				_ComicDone = true;
				//Debug.Log("Enter");
			}
		}
	}
	
	void OnTriggerStay(Collider other){
		if(other.tag == "avatar"){
			if(_ComicCollected >= comic.Length && !_ComicDone){
				_ComicDone = true;
				Debug.Log("Enter");
			}
		}
	}
}

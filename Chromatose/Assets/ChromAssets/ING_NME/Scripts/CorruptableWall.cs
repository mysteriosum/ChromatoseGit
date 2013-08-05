using UnityEngine;
using System.Collections;

public class CorruptableWall : MonoBehaviour {
	tk2dAnimatedSprite anim;
	float timing = 8.0f;
	float timer = 0f;
	bool started = false;
	//public GameObject[] myBlobbies;
	
	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<tk2dAnimatedSprite>();
		//anim.ClipFps = 5f;
	}
	
	// Update is called once per frame
	void Update () {
		if (started) timer += Time.deltaTime;
		if (timer >= timing && !anim.IsPlaying(anim.CurrentClip)){
			//anim.ClipFps = 5f;
			anim.Play();
			anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
			anim.animationCompleteDelegate = StopAndDestroy;
			
		}
	}
	
	void Trigger(){
		started = true;
		anim.StopAndResetFrame();
		timer = 0;
	}
	
	void StopAndDestroy(tk2dAnimatedSprite sprite, int clipId){
		/*foreach (GameObject b in myBlobbies){
			Destroy(b);
		}*/
		Destroy(gameObject);
	}
	
}

using UnityEngine;
using System.Collections;

public class TransitionTrigger : MonoBehaviour {
	private bool popped;
	private bool lightening = false;
	private float counter = 0f;
	public Texture blackBox;
	public bool nextLevel = true;
	public Transform localTarget;		//this is for if you just want to teleport the avatar. OVERRIDDEN by 'NextLevel'
	private Avatar _AvatarScript;
	private Transform avatarT;
	private delegate void TriggerMethod();
	private TriggerMethod myTrigger;
	
	// Use this for initialization
	void Start () {
		if (nextLevel){
			myTrigger = NextLevel;
		}
		else if (localTarget){
			myTrigger = ToTarget;
		}
		_AvatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		avatarT = GameObject.FindWithTag("avatar").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (!popped) return;
		
		if (lightening){
			counter -= 0.02f;
		}
		else{
			counter += 0.02f;
		}
		
		if (counter > 1 && !lightening){
			myTrigger();
		}
	}
	
	void OnTriggerEnter(Collider other){
		if (other.name == "Avatar")
			popped = true;
	}
	
	void OnGUI(){
		if (!popped) return;
		
		GUI.color = new Color(0, 0, 0, counter);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBox);
	}

	void ToTarget(){
		avatarT.position = localTarget.position;
		avatarT.rotation = localTarget.rotation;
		avatarT.SendMessage ("SetVelocity", Vector2.zero);
		lightening = true;
		
		_AvatarScript.CallFromFar();
				
		if(_AvatarScript.HasOutline){
			_AvatarScript.CancelOutline();
		}
	}
	void NextLevel(){
		Application.LoadLevel(Application.loadedLevel + 1);
		
		if(_AvatarScript.HasOutline){
			_AvatarScript.CancelOutline();
		}
	}
}

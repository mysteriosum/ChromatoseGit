using UnityEngine;
using System.Collections;

public class Collectible : ColourBeing {
	
	public Couleur colColour = Couleur.white;
	int closeDist = 50;
	Avatar avatar;
	Transform t;
	Transform avatarT;
	private bool dropped = false;
	private Vector2 velocity;
	private Transform collector;
	private float homeTiming = 0.7f;
	private float homeTimer = 0f;
	private bool justPutBack = false;
	private int clearDist = 150;
	private bool isShadow = false;
	
	private string idleAnim;
	private string takeAnim;
	private string loseAnim;
	
	
	// Use this for initialization
	void Start () {
		spriteInfo = GetComponent<tk2dSprite>();
		switch (colColour){
		case Couleur.white:
			colour.r = 0;
			colour.g = 0;
			colour.b = 0;
			idleAnim = "wColl_idle";
			takeAnim = "wColl_pickedUp";
			break;
		case Couleur.red:
			colour.r = 255;
			colour.g = 0;
			colour.b = 0;
			idleAnim = "rColl_idle";
			takeAnim = "rColl_pickedUp";
			break;
		case Couleur.green:
			colour.r = 0;
			colour.g = 255;
			colour.b = 0;
			idleAnim = "gColl_idle";
			takeAnim = "gColl_pickedUp";
			break;
		case Couleur.blue:
			colour.r = 0;
			colour.g = 0;
			colour.b = 255;
			idleAnim = "bColl_idle";
			takeAnim = "bColl_pickedUp";
			break;
		
		}
		avatar = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		t = transform;
		avatarT = avatar.transform;
		velocity = Random.insideUnitCircle.normalized * Random.Range(90, 135);
		
		anim = gameObject.GetComponent<tk2dAnimatedSprite>();
	}
	
	// Update is called once per frame
	
	void Update () {
		if (Gone) return;
		if (!dropped){
			Vector3 dist = avatarT.position - t.position;
			if (dist.magnitude < closeDist && !justPutBack){
				if (CheckSameColour(avatar.colour) || colColour == Couleur.white){
					ChromatoseManager.manager.AddCollectible(this);
					Dead = true;
					anim.Play(takeAnim);
					anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
					anim.animationCompleteDelegate = GoneForever;
					t.parent = null;
					
				}
				if (isShadow){
					avatar.CancelOutline();
				}
			}
			else if (dist.magnitude > clearDist && justPutBack){
				justPutBack = false;
				Dead = false;
				Debug.Log("Shouldn't be dead no mo");
			}
		}
		else{
			if (colour.White)
				goto white;
			if (colour.Blue)
				goto blue;
			if (colour.Red){
				goto red;
			}
		}
		return;		//if I'm not fading I'll skip this next
	white:
		
		t.Translate(velocity * Time.deltaTime);
		Dead = true;
	
		return;
	blue:
		t.SetParent((Transform)null);
		spriteInfo.color = new Color(spriteInfo.color.r, spriteInfo.color.g, spriteInfo.color.b, 1f);
		anim.Play(loseAnim);
		
		homeTimer += Time.deltaTime;	
		if (homeTimer >= 1.5f){
			Gone = true;
			dropped = false;
		}
		
		return;
		
	red:
		velocity = Vector2.Lerp(velocity, Vector2.zero, 0.0075f);
		homeTimer += Time.deltaTime;
		Vector2 distToTarget = (Vector2)(collector.position - t.position);
		Vector2 redVector = Vector2.Lerp(distToTarget * 5, Vector2.zero, homeTiming / homeTimer);
		t.Translate((redVector + velocity) * Time.deltaTime);
		if (distToTarget.magnitude < 5){
			Gone = true;
			dropped = false;
		}
		
		return;
		
	}
	
	
	override public void Trigger(){
		int direction = Random.Range(0, 359);
		t.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(0, direction, 0));
		Gone = false;
		dropped = true;
		t.parent = null;
		spriteInfo.SendMessage("FadeAlpha", 1f);
		if (colour.Blue){
			t.position = avatar.t.position + (Vector3) velocity / 4;
			
			collector = VectorFunctions.FindClosestOfTag(t.position, "blueCollector", 10000);
			anim.Play(anim.GetClipByName("bColl_lose"), 0);
		}
		if (colour.White){
			Dead = true;
			anim.Play(anim.GetClipByName("wColl_lose"), 0);
			anim.animationCompleteDelegate = GoneForever;
			anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
			t.position = avatar.t.position;
		}
		if (colour.Red){
			collector = VectorFunctions.FindClosestOfTag(t.position, "redCollector", 10000);
			anim.Play(anim.GetClipByName("wColl_lose"), 0);		//TODO : CHANGE THIS TO RED COLLECTIBLE ANIM, PLEAZE
			t.position = avatar.t.position;
		}
	}
	
	public void PutBack(Vector3 newPos){
		isShadow = true;
		justPutBack = true;
		t.position = (Vector3) newPos;
		Gone = false;
		Dead = true;
	}
	
	public void GoneForever(tk2dAnimatedSprite sprite, int index){
		Dead = true;
		Gone = true;
	}
	
}

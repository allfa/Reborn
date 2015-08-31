using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BallAI : MonoBehaviour {
	public Animator anim;
	public bool isKillable;
	public float speed;
	public Transform left;
	public Transform right;
	public Transform up;
	public Transform down;
	public Transform spr;
	public Vector2 movement;
	public float rot;
	public bool isDead=false;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	public void doAI(){
		//Kill Logic
		if(!anim.GetBool("isGrounded")){
			anim.SetInteger ("Direction", 2);
			return;
		}
		if (!down)
			return;
		switch (down.tag) {
		case "Player":
			down.GetComponent<InputReader> ().Die ();
			return;
		case "Enemy":
			down.GetComponent<EnemyAI> ().Die ();
			return;
		default:
			if (up) {
				if (up.tag == "Player") {
					Debug.Log("Killed by player on UP");
					Die ();
					return;
				}
			}
			break;
		}
		if(left&&right)
		if((left.tag == "Player" && right)||(right.tag == "Player" && left)){
			Debug.Log ("Killed by Player+Oposite side mess");
			Die();
			return;
		}
		if (left)
			if (left.tag == "Player") {
				anim.SetInteger ("Direction", 1);
				return;
			}
		if (right)
			if (right.tag == "Player") {
				anim.SetInteger ("Direction", 3);
				return;
			}
		anim.SetInteger ("Direction", 4);
	}
	void move(){
		//TrimmPos ();
		//teste root motion
		int Direction = anim.GetInteger ("Direction");
		switch (Direction) {
		case 1: 
			rot = -90;
			break;
		case 3:
			rot = 90;
			break;
		case 4:
			rot = 0;
			break;
		default:
			break;
		}
	}
	public void Die(){
		if (!isKillable)
			return;
		if (isDead)
			return;
		isDead = true;
		Debug.Log (name + " Died.");
		anim.SetTrigger ("isDead");
		//anim.SetBool("isDead", true);
	}
	public void LateUpdate(){
		doAI ();
		spr.Rotate(new Vector3(0,0,rot*Time.deltaTime));
	}
	public void postMortem(){
		Destroy (gameObject);
	}
	public void TrimmPos(){
		rot = 0;
		//Vector3 pos = transform.localPosition;
		//pos = new Vector3(Mathf.RoundToInt(pos.x+0.5f), Mathf.RoundToInt(pos.y+0.5f), Mathf.RoundToInt(pos.z+0.5f));
		//transform.localPosition = pos;
	}
}

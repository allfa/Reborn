using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BallAI : MonoBehaviour {
	public Animator anim;
	public float speed;
	public Transform left;
	public Transform right;
	public Transform up;
	public Transform down;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		doAI ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void doAI(){
		//Kill Logic
		if (down) {
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
						Die ();
						return;
					}
				}
				break;
			}
		} else {
			anim.SetInteger ("Direction", 2);
			return;
		}
		if(left){
			if(left.tag == "Player"){
				if (right){
					Die();
					return;
				}
				else{
					anim.SetInteger("Direction", 1);
					return;
				}
			}
		}
		if (right) {
			if (right.tag == "Player") {
				if (left){
					Die ();
					return;
				}
				else{
					anim.SetInteger ("Direction", 3);
					return;
				}
			}
		}
		anim.SetInteger ("Direction", 4);
	}
	void move(){
		//left = right = up = down = null;
		int Direction = anim.GetInteger ("Direction");
		Rigidbody2D body = GetComponent<Rigidbody2D>();
		//body.velocity = new Vector2 (0, 0);
		body.constraints = RigidbodyConstraints2D.FreezeRotation;
		switch(Direction)
		{
		case 0:
			//Debug.Log("moving to 0");
			body.velocity = new Vector2(0,speed);
			//body.constraints = RigidbodyConstraints2D.FreezePositionX|RigidbodyConstraints2D.FreezeRotation;
			break;
		case 1:
			//Debug.Log("moving to 1");
			body.velocity = new Vector2(speed, 0);
			//body.constraints = RigidbodyConstraints2D.FreezePositionY|RigidbodyConstraints2D.FreezeRotation;
			break;
		case 2:
			//Debug.Log("moving to 2");
			body.velocity = new Vector2(0, -speed);
			//body.constraints = RigidbodyConstraints2D.FreezePositionX|RigidbodyConstraints2D.FreezeRotation;
			break;
		case 3:
			//Debug.Log("moving to 3");
			body.velocity = new Vector2(-speed, 0);
			//body.constraints = RigidbodyConstraints2D.FreezePositionY|RigidbodyConstraints2D.FreezeRotation;
			break;
		case 4:
			body.velocity = new Vector2(0, 0);
			//body.constraints = RigidbodyConstraints2D.FreezeAll;
			break;
		default:
			Debug.LogError("Invalid Direction");
			break;
		}
	}
	public void Die(){
		Debug.Log (name + " Died.");
		anim.SetBool("isDead", true);
		//anim.Play ("Die");
	}
	public void postMortem(){
		Destroy (gameObject);
	}
}

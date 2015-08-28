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
	public Vector2 movement;
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
		int Direction = anim.GetInteger ("Direction");
		Rigidbody2D body = GetComponent<Rigidbody2D>();
		body.constraints = RigidbodyConstraints2D.FreezeRotation;
		switch(Direction)
		{
		case 0:
			movement = Vector2.up;
			body.isKinematic = false;
			break;
		case 1:
			movement = Vector2.right;
			body.isKinematic=true;
			break;
		case 2:
			movement = Vector2.down;
			body.isKinematic=false;
			break;
		case 3:
			movement = Vector2.left;
			body.isKinematic=true;
			break;
		case 4:
			movement = Vector2.zero;
			body.isKinematic=false;
			break;
		default:
			Debug.LogError("Invalid Direction");
			break;
		}
	}
	public void Die(){
		Debug.Log (name + " Died.");
		anim.SetBool("isDead", true);
	}
	public void FixedUpdate(){
		Rigidbody2D body = GetComponent<Rigidbody2D> ();
		if ((movement == Vector2.down) && down)
			return;
		body.MovePosition (
			body.position + 
			Vector2.MoveTowards (Vector2.zero, movement, Time.fixedDeltaTime*speed)
			);
	}
	public void postMortem(){
		Destroy (gameObject);
	}
}

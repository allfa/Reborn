using UnityEngine;
using System.Collections;

public class CogumeloAI : MonoBehaviour {
	public int speed;
	// Use this for initialization
	void Start () {
		Animator anim = GetComponent<Animator> ();
		move (anim.GetInteger ("Direction"));
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void changeDirection(int dir){
		Animator anim = GetComponent<Animator> ();
		anim.SetInteger ("Direction", dir);
		move (dir);
	}
	void move (int dir){
		Rigidbody2D body = GetComponent<Rigidbody2D> ();
		switch (dir) {
		case 0:
			body.velocity = new Vector2 (0, speed);
			break;
		case 1:
			body.velocity = new Vector2 (speed, 0);
			break;
		case 2:
			body.velocity = new Vector2 (0, -speed);
			break;
		case 3:
			body.velocity = new Vector2 (-speed, 0);
			break;
		case 4:
			body.velocity = new Vector2 (0, 0);
			break;
		default:
			Debug.LogError ("Invalid Direction");
			break;
		}
	}
	void OnCollisionEnter2D(Collision2D coll){
		//Debug.Log ("Collided");
		switch (coll.gameObject.tag) {
		case "Enemy":
			var eAI = coll.gameObject.GetComponent<Animator> ();
			eAI.SetBool ("isSleep", true);
			eAI.Play ("Sleep");
			Destroy (gameObject);
			changeDirection (2);
			break;
		case "Moveable":
			Physics2D.IgnoreCollision (coll.collider, this.GetComponent<Collider2D> ());
			break;
		case "Player":
			//does nothing
			break;
		default:
			Debug.Log ("Cogumelo does nothing with tag:" + coll.gameObject.tag);
			changeDirection (2);
			break;
		}
	}
}

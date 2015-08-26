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
		if (coll.gameObject.tag == "Enemy") {
			var eAI = coll.gameObject.GetComponent<Animator> ();
			eAI.SetBool ("isSleep", true);
			eAI.Play("Sleep");
			Destroy(gameObject);
		}
		changeDirection (2);
	}
}

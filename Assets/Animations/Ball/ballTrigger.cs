
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ballTrigger : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D coll){
		BallAI ball = transform.parent.GetComponent<BallAI> ();
		if (ball.name.Contains ("Azul") && coll.name.Contains ("Azul")) {
			Map m = FindObjectOfType<Map>();
			m.won=true;
			//FindObjectOfType<config> ().Won ();
		}
		switch (name) {
		case "Left":
			ball.left = coll.transform;
			break;
		case "Right":
			ball.right = coll.transform;
			break;
		case "Up":
			ball.up = coll.transform;
			break;
		case "Down":
			ball.down = coll.transform;
			break;
		default:
			Debug.LogError ("Nao existe um case para " + name + "?");
			break;
		}
	}
	void OnTriggerExit2D(Collider2D coll){
		BallAI ball = transform.parent.GetComponent<BallAI> ();
		//Debug.Log ("Ended Collision.\n Direction: " + name);
		switch (name) {
		case "Left":
			ball.left = null;
			break;
		case "Right":
			ball.right = null;
			break;
		case "Up":
			ball.up = null;
			break;
		case "Down":
			ball.down = null;
			break;
		default:
			Debug.LogError ("Nao existe um case para " + name + "?");
			break;
		}
	}

}


using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ballTrigger : MonoBehaviour {
	public float[] lastTime={0,0,0,0};
	public List<List<List<Transform>>> eventList;
	void Awake(){
		eventList = new List<List<List<Transform>>> ();
		eventList.Add(new List<List<Transform>>());
		eventList.Add(new List<List<Transform>>());
		eventList.Add(new List<List<Transform>>());
		eventList.Add(new List<List<Transform>>());
		foreach (List<List<Transform>> l in eventList) {
			l.Add (new List<Transform> ());
			l.Add (new List<Transform> ());
		}
	}
	void OnTriggerEnter2D(Collider2D coll){
		BallAI ball = transform.parent.GetComponent<BallAI> ();
		if (coll.transform.parent == ball.transform)
			return;
		if (ball.name.Contains ("Azul") && coll.name.Contains ("Azul")) {
			Map m = FindObjectOfType<Map>();
			m.won=true;
			//FindObjectOfType<config> ().Won ();
		}
		
		switch (name) {
		case "Left":
			eventList[0][0].Add(coll.transform);
			//ball.left = coll.transform;
			break;
		case "Right":
			eventList[1][0].Add(coll.transform);
			//ball.right = coll.transform;
			break;
		case "Up":
			eventList[2][0].Add(coll.transform);
			//ball.up = coll.transform;
			break;
		case "Down":
			eventList[3][0].Add(coll.transform);
			//ball.down = coll.transform;
			//ball.anim.SetBool("isGrounded", true);
			break;
		default:
			//Debug.LogError ("Nao existe um case para " + name + "?");
			break;
		}
	}
	void OnTriggerExit2D(Collider2D coll){
		BallAI ball = transform.parent.GetComponent<BallAI> ();
		if (coll.transform.parent == ball.transform)
			return;
		//Debug.Log ("Ended Collision.\n Direction: " + name);
		switch (name) {
		case "Left":
			eventList[0][1].Add(coll.transform);
			//ball.left = coll.transform;
			break;
		case "Right":
			eventList[1][1].Add(coll.transform);
			//ball.right = coll.transform;
			break;
		case "Up":
			eventList[2][1].Add(coll.transform);
			//ball.up = coll.transform;
			break;
		case "Down":
			eventList[3][1].Add(coll.transform);
			//ball.down = coll.transform;
			//ball.anim.SetBool("isGrounded", true);
			break;
		default:
			//Debug.LogError ("Nao existe um case para " + name + "?");
			break;
		}	
	}
	void Update(){
		BallAI ball = transform.parent.GetComponent<BallAI> ();
		if(eventList[0][0].Count>0||eventList[0][1].Count>0){
			ball.left = upRef (eventList [0] [0], eventList [0] [1]);
		}
		if(eventList[1][0].Count>0||eventList[1][1].Count>0){
			ball.right = upRef (eventList [1] [0], eventList [1] [1]);
		}
		if(eventList[2][0].Count>0||eventList[2][1].Count>0){
			ball.up = upRef (eventList [2] [0], eventList [2] [1]);
		}
		if(eventList[3][0].Count>0||eventList[3][1].Count>0){
			ball.down = upRef (eventList [3] [0], eventList [3] [1]);
		}
		ball.up = checkactive (ball.up) ? ball.up : null;
		ball.left = checkactive (ball.left) ? ball.left : null;
		ball.right = checkactive (ball.right) ? ball.right : null;
		ball.down = checkactive (ball.down) ? ball.down : null;
		ball.anim.SetBool ("isGrounded", checkactive(ball.down));
	}

	Transform upRef(List<Transform> i, List<Transform> o){
		if(i.Count>1||o.Count>1)
			Debug.Log ("In List: " + i.Count + "Out List: " + o.Count);
		Transform ret = null;
		foreach( Transform t in i){
			if(o.Contains(t))
				i.Remove(t);
		}
		if (i.Count > 0) {
			ret = i [0];
		}
		i.Clear ();
		o.Clear ();
		return ret;
	}
	public bool checkactive(Transform t){
		if (t)
			return t.gameObject.active;
		else
			return false;
	}
}

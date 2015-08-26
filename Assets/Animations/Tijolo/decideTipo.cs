using UnityEngine;
using System.Collections;

public class decideTipo : MonoBehaviour {
	public bool isBlue;
	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator>();
		anim.SetBool ("isBlue", isBlue);
	}
	
	// Update is called once per frame
	void Update () {
		if (anim.GetBool("isBlue")!=isBlue)
			anim.SetBool ("isBlue", isBlue);
	}
	void OnTrigger2D(Collider2D coll){
		Debug.Log ("Object " + this.name + " was triggered by " + coll.name + ".");
	}
}

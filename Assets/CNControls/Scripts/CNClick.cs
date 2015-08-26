using UnityEngine;
using System.Collections;

public class CNClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseOver(){
		//Debug.Log ("Got In");
		if (Input.GetButtonUp ("Fire1")) {
			Debug.Log("Should Shoot");
			InputReader i = GameObject.FindObjectOfType<InputReader> ();
			i.shoot();
		}
	}
}

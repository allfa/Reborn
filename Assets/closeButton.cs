using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class closeButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Close(){
		Debug.Log ("Closing");
		Application.Quit ();
	}
}

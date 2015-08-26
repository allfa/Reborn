using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MyJoystick : MonoBehaviour {
	Touch initialTouch;
	Transform inner;
	Transform outer;
	// Use this for initialization
	void Start () {
		//Debug.Log ("Got In");
		foreach (Transform t in transform) {
			switch (t.name) {
			case "Inner":
				inner = t;
				break;
			case "Outer":
				outer = t;
				break;
			default:
				Debug.LogError ("There is a invalid object inside the Joystick: " + t.name);
				break;
			}
		}
	}
	
	// Update is called once per frame
	public Vector3 toWorld(Vector3 pos){
		return Camera.main.ScreenToWorldPoint (pos);
	}
	void Update () {
		//if (initialTouch)
			//return;
		List<Touch> tList = new List<Touch> ();
		tList.AddRange (Input.touches);
		if (tList.Count == 0) {
			Touch t = TouchCreator.GetTouchCreator ().Create ();
			if (t.fingerId == 10)
				tList.Add (t);
		}
		foreach (Touch temp in tList) {
			//Debug.Log("ID: "+ temp.fingerId + " phase: "+temp.phase);
			if (temp.fingerId == initialTouch.fingerId) {
				//codigo principal
				switch (temp.phase){
				case TouchPhase.Ended:
					Destroy(gameObject);
					break;
				case TouchPhase.Moved:
					Vector3 pos = toWorld(temp.position);
					Vector3 pdiff = pos-transform.position;
					inner.localPosition = limitVar(pdiff, 1);
	
					break;
				default:
					//Debug.Log("Joystick TouchPhase:" + temp.phase + " ignored.");
					break;
				}
			}
		}
	}
	private Vector3 limitVar(Vector3 val, float limit){
		Vector3 resp = val;
		resp = Vector3.MoveTowards (Vector3.zero, resp, limit);
		return resp;
	}
	public void SetTouch(Touch t){
		initialTouch = t;
		transform.position = toWorld(t.position);
	}
	public Vector2 GetAxys(){
		if (inner)
			return inner.localPosition;
		return new Vector2();
	}
}

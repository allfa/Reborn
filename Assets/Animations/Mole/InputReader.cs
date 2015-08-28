
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEngine.Networking.Match;
using System.Reflection;

public class InputReader : MonoBehaviour {

	// Use this for initialization
	public enum Direction{
		Left=3,
		Right=1,
		Up=0,
		Down=2,
		None=4
	}
	public Transform Joystick;
	public MyJoystick currentJoystick;
	public Direction dir;
	public float vert;
	public float hor;
	public float offset;
	public float speed;
	public GameObject ammo;
	public int ammoCount;
	public Dictionary<int, Touch> tList = new Dictionary<int, Touch>();
	private Animator anim;
	void Start (){
		dir = Direction.None;
		anim = GetComponent<Animator> ();
		anim.SetInteger ("Direction", (int)dir);
	}
	
	// Update is called once per frame
	void move(){
		Rigidbody2D body = GetComponent<Rigidbody2D> ();
		int dir = anim.GetInteger ("Direction");
		body.constraints = RigidbodyConstraints2D.FreezeRotation;
		switch(dir){
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
			body.constraints = RigidbodyConstraints2D.FreezeAll;
			break;
		default:
			Debug.LogError("Invalid Direction");
			break;
		}
		if(anim.GetBool("isShooting"))
			shoot ();
	}
	void doTouch(){
		List<Touch> temp = new List<Touch>();
		temp.AddRange(Input.touches);
		if (temp.Count == 0) {
			Touch tc = TouchCreator.GetTouchCreator ().Create ();
			if (tc.fingerId == 10) {
				temp.Add (tc);
			}
		}

		if(temp.Count<1) return;
		//Debug.Log ("Correct Finger ID Phase:"+ tc.phase);
		
		foreach (Touch t in temp) {
			switch (t.phase) {
			case TouchPhase.Began:
				if(!tList.ContainsKey(t.fingerId))
					tList.Add(t.fingerId, t);
				break;
			case TouchPhase.Ended:
				//Debug.Log ("Began" + tList.ContainsKey(t.fingerId));
				if(tList.ContainsKey(t.fingerId)){
					if((tList[t.fingerId].phase==TouchPhase.Stationary)||(t.phase==TouchPhase.Began)){
						//Debug.Log ("Shooting");
						anim.SetBool("isShooting", true);
						tList.Remove(t.fingerId);
					}
				}
				break;
			case TouchPhase.Moved:
				if(tList.ContainsKey(t.fingerId)){
					if((tList[t.fingerId].phase==TouchPhase.Began||tList[t.fingerId].phase==TouchPhase.Stationary)){
						if(currentJoystick) 
							break;
						CreateJoystick(t);
						tList.Remove(t.fingerId);
					}
				}
				break;
			case TouchPhase.Stationary:
				if(tList.ContainsKey(t.fingerId)){
					if(!(tList[t.fingerId].phase==TouchPhase.Moved))
						tList[t.fingerId]=t;
				}
				break;
			}
		}

	}
	void LateUpdate () {
		doTouch ();
		if (currentJoystick) {
			Vector2 axys = currentJoystick.GetAxys ();
			vert = axys.y;
			hor = axys.x;
		} else {
			vert = Input.GetAxis("Vertical");
			hor = Input.GetAxis ("Horizontal");
		}
		dir = Direction.None;
		if (Mathf.Abs (vert) > Mathf.Abs (hor)) {
			if (vert > offset) {
				dir = Direction.Up;
			}
			if (vert < -offset) {
				dir = Direction.Down;
			}
		} else {
			if (hor > offset) {
				dir = Direction.Right;
			}
			if (hor < -offset) {
				dir = Direction.Left;
			}
		}
		anim.SetInteger ("Direction", (int)dir);

	}
	public void shoot(){
		//Debug.Log ("Triing to shoot");
		anim.SetBool ("isShooting", false);
		if (ammoCount <= 0)
			return;
		ammoCount--;
		Map map = FindObjectOfType<Map> ();
		Transform temp;
		switch ((int)dir){
		case 0:
			temp = map.spawnUnit (ammo, transform.localPosition.x, transform.localPosition.y-2, map.transform);
			temp.GetComponent<CogumeloAI>().changeDirection(0);
			break;
		case 1:
			temp = map.spawnUnit (ammo, transform.localPosition.x+2, transform.localPosition.y, map.transform);
			temp.GetComponent<CogumeloAI>().changeDirection(1);
			break;
		case 2:
			temp = map.spawnUnit (ammo, transform.localPosition.x, transform.localPosition.y+2, map.transform);
			temp.GetComponent<CogumeloAI>().changeDirection(2);
			break;
		case 3:
			temp = map.spawnUnit (ammo, transform.localPosition.x-2, transform.localPosition.y, map.transform);
			temp.GetComponent<CogumeloAI>().changeDirection(3);
			break;
		default:
			Debug.Log("Nao posso atirar na direcao: " + (int)dir);
			break;
		}
	}
	void OnCollisionEnter2D(Collision2D coll){
		//Debug.Log ("Entrou na Colisao");
		switch (coll.transform.tag) {
		case "Projectile": 
			Destroy (coll.gameObject);
			ammoCount += 1;
			break;
		case "Enemy":
			if(!coll.gameObject.GetComponent<EnemyAI>().isSleeping){
				//Lost Game
				Die();
			}
			break;
		case "bg":
			break;
		case "Moveable":
			break;
		default:
			Debug.Log ("Does nothing with tag: " + coll.transform.tag);
			break;
		}
	}
	public void Die(){
		//Lost Game Actions
		anim.Play ("Die");
		postMortem ();
		Destroy (gameObject);
	}
	void postMortem(){
		//Acoes apos a acao de morrer
		Debug.Log ("O Player Morreu");
	}
	void Update(){
		TouchCreator fakeTouch = TouchCreator.GetTouchCreator ();
		fakeTouch.deltaPosition = (Vector2)Input.mousePosition - (Vector2)fakeTouch.position;
		fakeTouch.position = Input.mousePosition;
		fakeTouch.deltaTime = Time.deltaTime;
		if (Input.GetMouseButton (0)) {
			if (fakeTouch.fingerId == 10){
				//Debug.Log ("On Click");
				fakeTouch.phase = 
					 (fakeTouch.deltaPosition.sqrMagnitude > 1f ? 
					 TouchPhase.Moved : TouchPhase.Stationary);
				//fakeTouch.tapCount += 1;
			}
		}
		else if(fakeTouch.phase==TouchPhase.Ended)
			fakeTouch.fingerId=0;

		if (Input.GetMouseButtonDown(0)){
			//Debug.Log ("Click Detected");
			fakeTouch.fingerId = 10;
		    if(fakeTouch.phase==TouchPhase.Began){
				fakeTouch.tapCount +=1;
				return;
			}
			fakeTouch.phase = TouchPhase.Began;
			fakeTouch.tapCount=1;
		}
		if(Input.GetMouseButtonUp(0)){
			//Debug.Log ("Released TouchData:\n" + fakeTouch.Create());
			fakeTouch.phase = TouchPhase.Ended;
		}
	}
	private void CreateJoystick(Touch t){
		if (currentJoystick)
			return;
		Transform tt = Instantiate (Joystick);
		currentJoystick=tt.GetComponent<MyJoystick>();
		currentJoystick.SetTouch (t);
	}
}
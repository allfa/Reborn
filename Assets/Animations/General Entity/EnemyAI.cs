using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : MonoBehaviour {
	public int roll;
	public Animator anim;
	public float speed;
	public bool isUnicorn;
	public bool isSleeping;
	public InputReader target;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		doAI ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void doAI(){
		int rollbase;
		rollbase = checkBlock() ? 302 : 102;
		roll = Mathf.FloorToInt(Random.Range (0, rollbase + 1));
		anim.SetInteger ("Roll", roll);
		if (roll > 21) {
			followPlayer ();
			return;
		}
		if (roll > 102) {
			blockBall ();
			return;
		}
		stepBack ();
	}
	bool checkBlock(){
		//TODO
		return false;
	}
	List<int> getPath(Transform _in, Transform _out){
		return getPath(Mathf.FloorToInt(_in.localPosition.x), Mathf.FloorToInt(_in.localPosition.y),Mathf.FloorToInt(_out.localPosition.x), Mathf.FloorToInt(_out.localPosition.y));
	}
	List<int> getPath(int sourceX, int sourceY, int destX, int destY){
		int[,] gMap = getGradedMap (destX, destY);
		return _getPath(sourceX, sourceY, destX, destY, gMap);
	}
	List<int> _getPath(int sourceX, int sourceY, int destX, int destY, int[,] map){
		//todo - reduzir custo de recursao
		if (sourceX < 0 || sourceY < 0 || destX < 0 || destY < 0) {
			Debug.Log ("Algum Objeto nao foi inicializado? source:"+sourceX+","+sourceY+"\ndest:"+destX+","+destY);
			return null;
		}
		List<int> resp = _getPath (sourceX, sourceY, destX, destY, map, 0, map[sourceX, sourceY]);
		return resp;
	}
	List<int> _getPath(int sourceX, int sourceY, int destX, int destY, int[,] map, int loopLimit, int initialCost){
			List<int> resp = new List<int> ();
		if (loopLimit > 9||(sourceX == destX && sourceY == destY)) {
			if(initialCost<map[sourceX, sourceY]) return null;
			resp.Add (4);
			return resp;
		}
		Dictionary<int,int> options = new Dictionary<int, int> ();
		if(sourceX>0) options.Add (3, map [sourceX - 1, sourceY]);
		if(sourceX<29)options.Add (1, map [sourceX + 1, sourceY]);
		if(sourceY>0) options.Add (0, map [sourceX , sourceY - 1]);
		if(sourceY<19)options.Add (2, map [sourceX , sourceY + 1]);
		var sortedDict = from entry in options orderby entry.Value ascending select entry;
		foreach (KeyValuePair<int,int> entry in sortedDict) {
			switch (entry.Key){
			case 0:
				resp = _getPath (sourceX, sourceY - 1, destX, destY, map, loopLimit+1,initialCost);
				break;
			case 1:
				resp = _getPath (sourceX + 1, sourceY, destX, destY, map, loopLimit+1,initialCost);
				break;
			case 2:
				resp = _getPath (sourceX, sourceY + 1, destX, destY, map, loopLimit+1,initialCost);
				break;
			case 3:
				resp = _getPath (sourceX - 1, sourceY, destX, destY, map, loopLimit+1, initialCost);
				break;
			case 4:
				return null;
			default:
				Debug.LogError("Caso Invalido de Direcao em _getPath");
				return null;
			}
			if(resp == null) return null;
			resp.Add(entry.Key);
			return resp;
		}
		Debug.Log ("Erro na funcao _GetPath chegou a um ponto invalido");
		return null;
	}
	int[,] getGradedMap(Transform target){
		return getGradedMap (Mathf.FloorToInt (target.transform.localPosition.x), Mathf.FloorToInt (target.transform.localPosition.y));
	}
	int[,] getGradedMap(int targetX, int targetY){
		Map map = FindObjectOfType<Map> ();
		int[,] data = map.getCurrentMap ();
		int[,] grade = new int[30,20];
		//string ss = "";
		for( int i = 0;i<30;i++){
			//string s ="";
			for( int j = 0;j<20;j++){
				grade[i,j] = Mathf.Abs(targetX-i)+ Mathf.Abs(targetY-j);
				if(data[i,j]!=0) grade[i,j] = -1;
			}
		}
		return grade;
	}
	void followPlayer(){
		//TODO - ainda esta muito lento
		if (target == null)
			target = FindObjectOfType<InputReader> ();
		int dir = anim.GetInteger ("Direction");
		if(isUnicorn&&(dir == 0||dir == 2)) Debug.LogError("Direcao Invalida para unicorn");
		if (!target) {
			anim.SetInteger ("Direction", 4);
			return;
		}
		if (isUnicorn) {
			float x = transform.localPosition.x;
			float tx = target.transform.localPosition.x;
			dir = x > tx ? 3 : 1;
			if (tx == x) dir = 4;
			anim.SetInteger ("Direction", dir);
			if (dir != 4) anim.SetInteger ("lastStep", dir);
			return;
		}
		List<int> path = getPath (transform, target.transform);
		if (path == null) {
			return;
		}
		dir = path[path.Count-1];
		anim.SetInteger("Direction", dir);
		if(dir!=4) anim.SetInteger("lastStep", dir);
	}
	void stepBack(){
		int dir;
		switch (anim.GetInteger ("lastStep")) {
		case 0:
			dir = 2;
			break;
		case 1:
			dir = 3;
			break;
		case 2:
			dir = 0;
			break;
		case 3:
			dir = 1;
			break;
		default:
			dir = 4;
			//Debug.LogError ("Aconteceu algum erro com lastStep");
			break;
		}
		anim.SetInteger ("Direction", dir);
		//Debug.Log ("Stepping Back");
	}
	void blockBall(){
		//TODO
		int dir = Mathf.FloorToInt (isUnicorn?Random.Range (0, 3):Random.Range (0, 5));
		anim.SetInteger ("Direction", dir);
		if(dir<(isUnicorn?2:4)) anim.SetInteger ("lastStep", dir);
		//Debug.Log ("Blocking Ball");
	}
	void move(){
		isSleeping = false;
		int Direction = anim.GetInteger ("Direction");
		Rigidbody2D body = GetComponent<Rigidbody2D>();
		body.constraints = RigidbodyConstraints2D.FreezeRotation;
		switch(Direction)
		{
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
		//doAI ();
	}
	void sleep(){
		isSleeping = true;
		anim.SetBool ("isSleep", false);
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
	}
	public void Die(){
		anim.Play ("Die");
		Destroy (gameObject);
	}
}

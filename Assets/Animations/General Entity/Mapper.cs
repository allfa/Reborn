using UnityEngine;
using System.Collections;

public class Mapper : MonoBehaviour {
	private GameObject[] objList;
	public int iterator;
	public GameObject current;
	public Map map;
	// Use this for initialization
	void Start () {
		//old probably not necessary
		//iterator = objList.Length-1;
		map = FindObjectOfType<Map> ();
		objList = map.objList;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver(){
		if(Input.GetMouseButtonDown(0)){
			iterator=(iterator+1)%objList.Length;
			if (current) Destroy(current);
			if(objList[iterator]){
				current = Instantiate(objList[iterator]);
				current.transform.parent=transform;
				current.transform.localPosition=(new Vector3(0,0,0));
				Animator tan = current.GetComponent<Animator>();
				if(tan) tan.enabled=false;
				EnemyAI tai = current.GetComponent<EnemyAI>();
				if(tai) tai.enabled=false;
				BoxCollider2D tco = current.GetComponent<BoxCollider2D>();
				if(tco) tco.enabled=false;
				CircleCollider2D tcc = current.GetComponent<CircleCollider2D>();
				if(tcc) tcc.enabled=false;
				BallAI tba = current.GetComponent<BallAI>();
				if(tba) tba.enabled=false;
				CogumeloAI tca = current.GetComponent<CogumeloAI>();
				if(tca) tca.enabled=false;
			}
			Debug.Log ("Clicked at a object in "+transform.position.x+","+transform.position.y);
		}
	}
}
using UnityEngine;
using System.Collections;

public class playerSpawner : MonoBehaviour {

	// Use this for initialization
	public GameObject playerPrefab;
	void Start () {
		Map map = FindObjectOfType<Map> (); 
		map.spawnUnit (playerPrefab, transform.localPosition.x, transform.localPosition.y, map.transform);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

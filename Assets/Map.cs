using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
//using UnityEditor;
using System.Text;
using UnityEngine.UI;

[XmlRoot("Map")]
public class Map : MonoBehaviour {
	[XmlAttribute("MapperVersion")]
	private const int SizeX = 30;
	private const int SizeY = 20;
	public string MapperVersion = "0.1.0";
	[XmlArray("Elements")]
	[XmlArrayItem("Element")]
	public List<Element> Elements = new List<Element>();
	public GameObject[] objList;
	public bool creator;
	public bool won=false;
	public config cfg;
//serializer
	public Transform spawnUnit(GameObject obj, float posX, float posY, Transform parent){
		if(!obj) return null;
		GameObject temp = Instantiate(obj);
		if (parent)
			temp.transform.parent = parent;
		temp.transform.localPosition = new Vector3 (posX, posY,0);
		return temp.transform;
	}
	public int[,] getCurrentMap(){
		int[,] data = new int[30,20];
		foreach (Transform t in transform) {
			for (int index = 0; index<objList.Length; index++) {
				Transform tr = t;
				if (creator)
					tr = t.GetChild (0);
				if (objList [index] == tr)
					data [Mathf.FloorToInt (t.localPosition.x), Mathf.FloorToInt (t.localPosition.y)] = index;
			}
		}
		return data;
	}
	public Map loadXML(TextAsset file){
		XmlDocument doc = new XmlDocument ();
		doc.LoadXml (file.text);
		return loadXML (doc);
	}
	public Map loadXML(string path){
		XmlDocument doc = new XmlDocument ();
		Debug.Log ("Path: " + path);
		doc.Load (path);
		return loadXML (doc);
	}
	public Map loadXML(XmlDocument doc){
		Map m = new Map();
		XmlElement main = doc.DocumentElement;
		m.MapperVersion = main.Attributes.GetNamedItem ("MapperVersion").Value;
		Debug.Log ("Mapper Version: " + m.MapperVersion);
		XmlNode Elements = main.GetElementsByTagName ("Elements") [0];
		foreach (XmlNode node in Elements.ChildNodes) {
			//Debug.Log(node.Name);
			Element e = new Element ();
			e.x = float.Parse (node.Attributes.GetNamedItem ("x").Value);
			e.y = float.Parse (node.Attributes.GetNamedItem ("y").Value);
			e.data = int.Parse(node.Attributes.GetNamedItem ("data").Value);
			m.Elements.Add (e);
		}
		return m;
	}
	public void saveXML(string path){

		XmlDocument doc = new XmlDocument ();
		XmlElement main = doc.CreateElement ("Map");
		XmlAttribute version = doc.CreateAttribute ("MapperVersion");
		version.Value = MapperVersion;
		main.Attributes.Append (version);
		XmlNode _Elements = doc.CreateNode (XmlNodeType.Element, "Elements", "");
		foreach(Element e in this.Elements ){
			XmlElement node = doc.CreateElement("Element");
			XmlAttribute x = doc.CreateAttribute("x");
			x.Value = e.x.ToString();
			XmlAttribute y = doc.CreateAttribute("y");
			y.Value = e.y.ToString();
			XmlAttribute dat = doc.CreateAttribute("data");
			dat.Value = e.data.ToString();
			node.Attributes.Append(x);
			node.Attributes.Append(y);
			node.Attributes.Append(dat);
			_Elements.AppendChild(node);
		}

		main.AppendChild (_Elements);
		doc.AppendChild (main);
		doc.Save(path);
		/*
		//doc.l
		using (var stream = new FileStream(path, FileMode.Create)) {
			using (StreamWriter writer = new StreamWriter(stream))
				writer.Write (data);
		}
		*/
	}
	// Use this for initialization
	void Start () {
		cfg = FindObjectOfType<config>();
		if (!creator) {
			Carregar ();	
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Carregar(){
		Carregar (false);
	}
	public void Carregar(bool dev){
		InputField fileName = FindObjectOfType<InputField> ();
		Map current;
		if (cfg.isCustom)
			current = loadXML (Application.dataPath + "/Resources/" + ((!dev) ? (cfg.currentLevel) : (fileName.text)) + ".xml");
		else {
			//Debug.Log("Entra aqui");
			TextAsset file = (TextAsset) Resources.Load ( cfg.currentLevel );
			current = loadXML (file);
		}
		this.MapperVersion = current.MapperVersion;
		this.Elements = current.Elements;
		//todo
		if (dev){
			foreach(Transform line in transform){
				foreach(Transform t in line){
					foreach(Transform tt in t){
						Destroy(tt.gameObject);
					}
					t.GetComponent<Mapper>().current=null;
				}
			}
			foreach (Element e in Elements){
				foreach (Transform line in transform){
					if (line.localPosition.y == e.y){
						foreach (Transform t in line){
							Mapper obj = t.GetComponent<Mapper> ();
							if (t.localPosition.x == e.x){
								obj.iterator = e.data;
								Transform temp = spawnUnit(objList[obj.iterator], 0,0, t);
								switch(temp.tag){
								case "Enemy":
									temp.GetComponent<EnemyAI>().enabled=false;
									break;
								case "Projectile":
									temp.GetComponent<CogumeloAI>().enabled=false;
									break;
								case "Spawner":
									temp.GetComponent<playerSpawner>().enabled = false;
									break;
								case "Moveable":
									Debug.Log ("Criou um objeto movivel");
									temp.GetComponent<BallAI>().enabled = false;
									break;
								default:
									break;
								}
								Animator anim = temp.GetComponent<Animator>();
								if(anim) 
									anim.enabled=false;
								obj.current=temp.gameObject;
							}
						}
					}
				}							
			}

			return;
		}
		foreach (Transform t in transform) {
			Destroy (t.gameObject);
		}
		foreach(Element e in Elements){
			Transform temp = spawnUnit(objList[e.data], Mathf.FloorToInt(e.x), Mathf.FloorToInt(e.y), transform);
			if(temp.tag == "Spawner") temp.GetComponent<SpriteRenderer>().enabled = false;
			//Debug.Log ("Object data:\nPosition:" + e.x + "," + e.y + "\nData:" + e.data + "\nInObject: LocalPosition: " + temp.localPosition);
		}
	}

	public void Salvar() {
		InputField fileName= FindObjectOfType<InputField> ();
		float x = 0;
		float y = 0;
		//Debug.Log ("Main Object: " + this.name + ".\n");
		foreach (Transform linha in transform)
		{
			y = linha.localPosition.y;
			//Debug.Log("  SubObject: " + linha.name + ".\n  Position: " + linha.localPosition.x + ", " + linha.localPosition.y + ".\n");
			foreach(Transform tr in linha)
			{
				x = tr.localPosition.x;
				Mapper map = tr.GetComponent<Mapper>();
				//Debug.Log("    SubSubObject: " + tr.name + ".\n    Position: " + tr.localPosition.x + ", " + tr.localPosition.y + ".\n");
				if(map.current!=null){
					//Debug.Log("Objeto = " + AssetDatabase.GetAssetPath(map.current) + "\n Posicao:"+ x + "," + y);
					Element e;
					e.x = x;
					e.y = y;
					e.data = map.iterator;
					//map.current.
					Elements.Add(e);
				}
			}
		}
		string p = Application.dataPath;
		string path = p + "/Resources/" + fileName.text + ".xml";
		Debug.Log ("Path: " + path + "\n");
		saveXML (path);
		//return true;
	}
	public struct Element{
		public float x;
		public float y;
		public int data;
	}
	public void doWon(){
		Debug.Log ("You Won");
		won = false;
		cfg.lastCompletedLevel = cfg.currentLevel;
		if (!cfg.isCustom) {
			//descomentar quanto tiver mais fazes
			//cfg.currentLevel = (int.Parse (cfg.currentLevel) + 1).ToString ("0:000");
			Carregar (false);
		}
	}
	void WonMessageWindow(int windowID){
		foreach (Transform t in transform)
			disableAI (t);
		GUI.Label (new Rect (10, 10, 100, 20), "Parabens.\nVoce Venceu!!!!");
		if (GUI.Button (new Rect (60, 30, 50, 20), "OK")) {
			doWon ();
		}
	}
	void OnGUI(){
		if (won)
			GUI.Window (0, new Rect (200, 200, 200, 90), WonMessageWindow, "Parabens");
	}
	void disableAI(Transform t){
		t.gameObject.SetActive (false);
	}
}

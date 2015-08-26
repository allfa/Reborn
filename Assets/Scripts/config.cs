using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;

public class config : MonoBehaviour {
	//variable ad default values
	public string currentLevel = "001";
	public string lastCompletedLevel;
	public string configVersion = "0.0.1";
	public List<int> scoreHistory;
	public int currentScore;
	public bool isCustom;
	public string configPath;
	public bool won=false;
	// Use this for initialization
	void Start () {
		int val = 1;
		//Debug.Log (val.ToString ("D3"));
		configPath = Application.persistentDataPath + "/config.xml";
		loadXML (configPath);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void loadXML(string path){
		XmlDocument doc = new XmlDocument ();
		Debug.Log ("Path: " + path);
		if (!File.Exists (path)) {
			Debug.Log ("Arquivo Config nao existe criando um.");
			saveXML ();
			return;
		}
		doc.Load (path);
		//teste se o arquivo existe
		XmlElement main = doc.DocumentElement;
		if (configVersion != main.Attributes.GetNamedItem ("Version").Value) {
			Debug.LogError ("Versao da Configuracao Incorreta");
			return;
		}
		Debug.Log ("Version: " + configVersion);
		lastCompletedLevel = main.Attributes.GetNamedItem ("lastCompletedMap").Value;
		XmlNode score = main.GetElementsByTagName ("ScoreHistory") [0];
		foreach (XmlNode node in score.ChildNodes) {
			//Debug.Log(node.Name);
			int i = int.Parse (node.Attributes.GetNamedItem ("Value").Value);
			scoreHistory.Add(i);
		}
	}
	public void saveXML(string path){
		
		XmlDocument doc = new XmlDocument ();
		XmlElement main = doc.CreateElement ("Config");
		XmlAttribute version = doc.CreateAttribute ("Version");
		version.Value = configVersion;
		main.Attributes.Append (version);
		XmlAttribute lastMap = doc.CreateAttribute ("lastCompletedMap");
		lastMap.Value = lastCompletedLevel;
		main.Attributes.Append (lastMap);
		XmlNode score = doc.CreateNode (XmlNodeType.Element, "ScoreHistory", "");
		foreach(int i in scoreHistory ){
			XmlElement node = doc.CreateElement("Score");
			XmlAttribute val = doc.CreateAttribute("Value");
			val.Value = i.ToString();
			node.Attributes.Append(val);
			score.AppendChild(node);
		}
		
		main.AppendChild (score);
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
	public void saveXML(){
		saveXML (configPath);
	}
	public void loadXML(){
		loadXML (configPath);
	}


}

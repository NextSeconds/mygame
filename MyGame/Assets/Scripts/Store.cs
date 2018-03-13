using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class Store : MonoBehaviour {
	public string filename = "/ShopInfo.xml";
	public string filepath = string.Empty;
	public string fullpath = string.Empty;
	public List<ShopInfo> All = new List<ShopInfo> ();//全部商品
	public GameObject cellPre;
	public UIGrid grid;
	// Use this for initialization
	void Start () {
		filepath = Application.dataPath + "/Data";
		fullpath = filepath + filename;
		if (!File.Exists (fullpath)) {
			CreateShopXml ();
		} else {
			LoadShopXml ();
		}
		xianshi ();
	}
	// Update is called once per frame
	void Update () {
	
	}
	void CreateShopXml (){
		XmlDocument _doc = new XmlDocument ();
		XmlElement root = _doc.CreateElement ("ShopInfo");
		_doc.AppendChild (root);
		_doc.Save (fullpath);
		Debug.Log ("Create ShopXML OK");
	}
	void LoadShopXml (){
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("ShopInfo");
		foreach (XmlElement it in root) {
				All.Add (new ShopInfo (int.Parse(it.GetAttribute ("ID")), it.GetAttribute ("Icon"), it.GetAttribute ("Name"), int.Parse(it.GetAttribute ("Price")),it.Name,it.GetAttribute("shuxing"),int.Parse(it.GetAttribute("zhanli"))));
		}
	}
	void xianshi (){
		for (int i = 0; i < All.Count; i++) {
			GameObject cell = Instantiate(cellPre) as GameObject;
			cell.transform.parent = grid.transform;	
			cell.transform.localScale = Vector3.one;
			cell.transform.Find ("icon").GetComponent<UISprite> ().spriteName = All [i].Photoname;
			cell.transform.Find ("name").Find ("Label").GetComponent<UILabel> ().text = All [i].Name;
			cell.transform.Find("price").Find("Label").GetComponent<UILabel>().text= All[i].Price.ToString();
			cell.transform.Find ("Buy").Find("ID").GetComponent<UILabel> ().text = All [i].ID.ToString ();
		}
		grid.Reposition ();
	}

}

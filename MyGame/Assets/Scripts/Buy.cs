using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
public class Buy : MonoBehaviour {
	public UILabel ID;
	public string fullpath = string.Empty;
	public List<ShopInfo> Wupin = new List<ShopInfo> ();
	public string filename = "/ShopInfo.xml";
	public GameObject uiroot;//ui root根节点
	// Use this for initialization
	void Start () {
		uiroot = GameObject.Find ("UI Root");
		fullpath = Application.dataPath + "/Data" + filename;
	}
	// Update is called once per frame
	void Update () {
	
	}
	void OnClick(){//读取出商品具体信息 调用bag类中add方法存入文档并显示

		ID = transform.Find ("ID").GetComponent<UILabel> ();
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("ShopInfo");
		foreach (XmlElement it in root) {
			if (it.GetAttribute("ID") == ID.text) {
				Wupin.Add (new ShopInfo (int.Parse(it.GetAttribute ("ID")), it.GetAttribute ("Icon"), it.GetAttribute ("Name"), int.Parse(it.GetAttribute ("Price")),it.Name,it.GetAttribute("shuxing"),int.Parse(it.GetAttribute("zhanli"))));
			}
		}
//		Debug.Log (Wupin.Count);
		uiroot.transform.Find("Hero").GetComponent<Bag>().BagAdd (Wupin [0]);
		Wupin.Clear ();
	}
}

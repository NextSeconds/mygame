using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Collections.Generic;
public class Bag : MonoBehaviour {
	public string filename = string.Empty;
	public string filepath = string.Empty;
	public string fullpath = string.Empty;
	public GameObject WupinPre;
	public UIGrid WupinGrid;
	public List<ShopInfo> wupin = new List<ShopInfo> ();
	public Hero hero;
	private int StorageMaxNum = 20;//背包最大存储物品格数
	// Use this for initialization
	void Awake () {
		string Userpath = Application.dataPath + "/Data//CurrentUser";
		CurrentUser (Userpath);
		filepath = Application.dataPath +"/data/UserMess";
		fullpath = filepath + "/" + filename + ".xml";

		StartCoroutine ("Heroparse",fullpath);
		BagParse ();
		wupinxianshi ();
	}
	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator Heroparse(string path){
		hero.HeroParse (path);
		yield return 0;
	}
	public void CurrentUser(string path){//读出当前登录的用户名
		string text = ReadFileContent (path);
//		string[] alluser = text.Split ('\n');
//		string Currentusername = alluser [alluser.Length - 1];
		filename = text;
	}
	public string ReadFileContent(string fullPath){//读取用户个人信息存储文档

		FileInfo info = new FileInfo (fullPath);
		if (!info.Exists) {
			Debug.Log ("找不到文件");
			return null;
		} else {
			return File.ReadAllText (fullPath);
		}
	}
	public void BagParse(){//将背包文档内物品存入List数组wupin
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("Mess");
		XmlNode items = root.SelectSingleNode ("items");
		foreach (XmlElement it in items) {
			wupin.Add (new ShopInfo (int.Parse(it.GetAttribute ("ID")), it.GetAttribute ("Icon"), it.GetAttribute ("Name"), int.Parse(it.GetAttribute ("Price")),it.Name,it.GetAttribute("shuxing"),int.Parse(it.GetAttribute("zhanli"))));
		}
	}
	void wupinxianshi (){//将背包中物品图片显示出来
		for (int i = 0; i < wupin.Count; i++) {
			GameObject cell = Instantiate(WupinPre) as GameObject;
			cell.transform.parent = WupinGrid.transform;	
			cell.transform.localScale = Vector3.one;
			cell.transform.name = wupin [i].ID.ToString ();
			cell.GetComponent<UISprite> ().spriteName = wupin[i].Photoname;
		}
		WupinGrid.Reposition ();
	}
	public void BagAdd(ShopInfo it){//将物品存入背包文档，并刷新背包UI
		wupin.Add (it);
		if (wupin.Count <= StorageMaxNum) {
			if (hero.Money >= wupin [wupin.Count - 1].Price) {
				XmlDocument _doc = new XmlDocument ();
				_doc.Load (fullpath);
				XmlNode root = _doc.SelectSingleNode ("Mess");
				XmlNode items = root.SelectSingleNode ("items");
//				Debug.Log (wupin.Count);
				XmlElement I = _doc.CreateElement (wupin [wupin.Count - 1].type);
				I.SetAttribute ("ID", (wupin [wupin.Count - 1].ID).ToString ());
				I.SetAttribute ("Icon", wupin [wupin.Count - 1].Photoname);
				I.SetAttribute ("Name", wupin [wupin.Count - 1].Name);
				I.SetAttribute ("Price", (wupin [wupin.Count - 1].Price).ToString ());
				I.SetAttribute ("shuxing", wupin [wupin.Count - 1].Shuxing);
				I.SetAttribute ("zhanli", (wupin [wupin.Count - 1].zhanli).ToString ());
				items.AppendChild (I);
				_doc.Save (fullpath);
				GameObject cell = Instantiate (WupinPre) as GameObject;
				cell.transform.parent = WupinGrid.transform;	
				cell.transform.localScale = Vector3.one;
				cell.name = wupin [wupin.Count - 1].ID.ToString ();
				cell.GetComponent<UISprite> ().spriteName = wupin [wupin.Count - 1].Photoname;
				WupinGrid.Reposition ();
				hero.MoneyChange ('-', wupin [wupin.Count - 1].Price);
			} else {
				Debug.Log ("你的金币不足");
			}
		} else {
			Debug.Log ("您的背包已满");
		}
	}
	public void DelItem(string id){
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("Mess");
		XmlNode items = root.SelectSingleNode ("items");
		XmlNodeList Ilist = items.ChildNodes;
		foreach (XmlNode i in Ilist) {
			if (i.Attributes ["ID"].Value == id) {
//				Debug.Log (i.Attributes ["ID"].Value);
				items.RemoveChild (i);
				_doc.Save (fullpath);
				break;
			}
		}
	}
	public void ADDHeroitem(string id){//将物品信息添入人物装备文档
		StartCoroutine("AddZhaungbei",id);
	}
	IEnumerator AddZhaungbei(string id){
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("Mess");
		XmlNode items = root.SelectSingleNode ("items");
		XmlNode Clothes = root.SelectSingleNode ("Clothes");
		XmlNodeList ZBlist = Clothes.ChildNodes;
		XmlNodeList Ilist = items.ChildNodes;
		foreach (XmlNode i in Ilist) {
			if (i.Attributes ["ID"].Value == id) {
				foreach (XmlNode ZB in ZBlist) {
					if (ZB.Name == i.Name) {//如果已有同类型物品 替换之前的物品
						items.RemoveChild (i);
						Clothes.RemoveChild (ZB);
						if (ZB.Attributes ["ID"] != null) {//如果装备确实存在 将其脱下后 在背包ui中显示
							items.AppendChild (ZB);
							GameObject cell = Instantiate (WupinPre) as GameObject;
							cell.transform.parent = WupinGrid.transform;	
							cell.transform.localScale = Vector3.one;
							cell.name = ZB.Attributes ["ID"].Value;
							cell.GetComponent<UISprite> ().spriteName = ZB.Attributes ["Icon"].Value;
							//此时为换装备，所以应将之前的装备属性去除，并添加新装备的属性
							if (ZB.Name == "weapons") {//如果是武器
								hero.Damage = int.Parse (i.Attributes ["shuxing"].Value);
							} else {
								hero.Defense -= int.Parse (ZB.Attributes ["shuxing"].Value);
								hero.Defense += int.Parse (i.Attributes ["shuxing"].Value);
							}
						} else {
							if (ZB.Name == "weapons") {//如果是武器
								hero.Damage = int.Parse (i.Attributes ["shuxing"].Value);
							} else {
								hero.Defense += int.Parse (i.Attributes ["shuxing"].Value);
							}
						}
						hero.HeroShuxingShow ();
						Clothes.AppendChild (i);
						_doc.Save (fullpath);
						break;
					}
				}
				break;
			}
		}
		yield return 0;
	}
}

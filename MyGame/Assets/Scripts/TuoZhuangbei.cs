using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class TuoZhuangbei : MonoBehaviour {
	private GameObject HeroBackGround;
	private string fullpath = string.Empty;
	private Hero hero;
	public UIGrid WupinGrid;
	public GameObject WupinPre;
	// Use this for initialization
	void Start () {
		hero = GameObject.Find ("Hero").GetComponent<Hero> ();
		HeroBackGround = GameObject.Find ("UI Root").transform.Find ("beibaomianban/HeroBackGround").gameObject;
		string Userpath = Application.dataPath + "/Data//CurrentUser";
		string filepath = Application.dataPath + "/data/UserMess";
		fullpath = filepath + "/" + CurrentUser (Userpath) + ".xml";
	}	
	public string CurrentUser(string path){//读出当前登录的用户名
		return ReadFileContent (path);
	}
	public string ReadFileContent(string Path){//读取用户个人信息存储文档
		FileInfo info = new FileInfo (Path);
		if (!info.Exists) {
			Debug.Log ("找不到文件");
			return null;
		} else {
			return File.ReadAllText (Path);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnClick(){
		if (gameObject.GetComponent<UISprite>().spriteName == "BG_002") {
			Debug.Log ("没有装备可以卸下");
		} else {
			gameObject.GetComponent<UISprite> ().spriteName = "BG_002";
			gameObject.GetComponent<UIButton>().normalSprite = "BG_002";
			StartCoroutine ("TuoZB", transform.name);
			HeroBackGround.transform.Find(gameObject.name + "1").GetComponent<UILabel>().text = "";
		}
	}
	IEnumerator TuoZB(string type){
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("Mess");
		XmlNode items = root.SelectSingleNode ("items");
		XmlNode Clothes = root.SelectSingleNode ("Clothes");
		XmlNodeList ZBlist = Clothes.ChildNodes;
		foreach (XmlNode ZB in ZBlist) {
			if (ZB.Name == type) {//节点名字是相应的类型
				Clothes.RemoveChild (ZB);
				items.AppendChild (ZB);
				GameObject cell = Instantiate (WupinPre) as GameObject;
				cell.transform.parent = WupinGrid.transform;	
				cell.transform.localScale = Vector3.one;
				cell.name = ZB.Attributes ["ID"].Value;
				cell.GetComponent<UISprite> ().spriteName = ZB.Attributes ["Icon"].Value;
				if (ZB.Name == "weapons") {//如果是武器
					hero.Damage -= int.Parse (ZB.Attributes ["shuxing"].Value);
				} else {
					hero.Defense -= int.Parse (ZB.Attributes ["shuxing"].Value);
				}
				XmlNode Tp = _doc.CreateElement (type);
				Clothes.AppendChild (Tp);
				hero.HeroShuxingShow ();
				_doc.Save (fullpath);
				WupinGrid.Reposition ();
				break;
			}
		}
		yield return 0;
	}
}

using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
public class MakeHero : MonoBehaviour {
	public GameObject yangjian;//0
	public GameObject change;//1
	public GameObject daomo;//2
	public GameObject nveba;//3
	public GameObject xiaoyaozi;//4
	public GameObject changdaonv;//5
	private GameObject CurrentHero;
	private string fullpath = string.Empty;
	private int JveSeNum;
	// Use this for initialization
	void Start () {
		string Userpath = Application.dataPath + "/Data//CurrentUser";//当前用户名
		string filepath = Application.dataPath +"/Data/UserMess";
		fullpath = filepath + "/" + CurrentUser (Userpath) + ".xml";
		JveseParse ();
	}
	public string CurrentUser(string path){//读出当前登录的用户名
		return ReadFileContent (path);
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
	void JveseParse(){
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("Mess");
		XmlNode hero = root.SelectSingleNode ("Hero");
		XmlNode JveSe = hero.SelectSingleNode ("JveSe");
		JveSeNum = int.Parse (JveSe.InnerText);
		switch (JveSeNum) {
		case 0:
			yangjian.SetActive (true);
			CurrentHero = yangjian;
			break;
		case 1:
			change.SetActive (true);
			CurrentHero = change;
			break;
		default:
            yangjian.SetActive (true);
			CurrentHero = yangjian;
			Debug.Log ("未知的角色，已使用备用角色杨戬。");
			break;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
	public void a(){
		CurrentHero.GetComponent<HeroCtrl> ().a ();
	}
	public void q(){
		CurrentHero.GetComponent<HeroCtrl> ().q ();
	}
	public void Esc(){
		Application.LoadLevel ("SheQu");
	}
}

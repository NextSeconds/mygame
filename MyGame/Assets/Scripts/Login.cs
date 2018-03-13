using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class Login : MonoBehaviour {
	public UILabel LoginTip;
	public UILabel RegisterTip;
	private string filename = "/Userdata.xml";
	private string fullpath = string.Empty;
	public GameObject WindowLogin;
	public GameObject WindowRegister;
	// Use this for initialization
	void Awake(){
		WindowRegister.SetActive (false);
		WindowLogin.SetActive (true);
	}
    void Start()
    {
        string fillpath = Application.dataPath + "/Data";
        fullpath = fillpath + filename;
        if (!Directory.Exists(fillpath))
        {
            Directory.CreateDirectory(fillpath);
        }
        if (!File.Exists(fullpath))
        {
            StartCoroutine("Create");
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
	IEnumerator Create(){
		XmlDocument _doc = new XmlDocument ();
		XmlElement root = _doc.CreateElement ("data");
		_doc.AppendChild (root);
		_doc.Save (fullpath);
		Debug.Log ("Create XML OK");
		yield return 0;
	}
	public void zhuce(){
		//注册账号
		string Username = GameObject.Find ("LblInput_Login").GetComponent<UILabel> ().text;
		string passwd = GameObject.Find ("Input_PWD").GetComponent<UIInput> ().value;
		RegisterTip.text = zc (Username,passwd);
	}
	string zc(string Username,string passwd){
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("data");
		foreach (XmlElement it in root) {
			if (it.GetAttribute("Username") == Username) {
				return "该用户名已存在";
			}
		}
		XmlElement n = _doc.CreateElement ("I");
		n.SetAttribute ("Username", Username);
		n.SetAttribute ("Password", passwd);
		root.AppendChild (n);
		_doc.Save (fullpath);
		return "注册成功";
	}
	public void quxiao(){
		//返回登录页面(取消)
		GameObject.Find ("LblInput_Login").GetComponent<UILabel> ().text = "请输入账号";
		GameObject.Find ("LblInput_PWD").GetComponent<UILabel> ().text = "请输入密码";
		WindowRegister.SetActive (false);
		WindowLogin.SetActive (true);

	}
	public void login(){
		//确认登录
		string Username = GameObject.Find ("LblInput_Login").GetComponent<UILabel> ().text;
		string passwd = GameObject.Find ("Input_PWD").GetComponent<UIInput> ().value;
		LoginTip.text = dl (Username,passwd);
		if (LoginTip.text == "登录成功") {
			string Userpath = Application.dataPath + "/Data//CurrentUser";
			File.WriteAllText (Userpath, Username);
			Debug.Log ("当前登录用户名已保存");
			StartCoroutine ("jishi",Username);
		}
	}
	IEnumerator jishi(string name){
        string userMess = Application.dataPath + "/Data/UserMess/";
        if (!Directory.Exists(userMess)) 
        {
            Directory.CreateDirectory(userMess);
        }
		string path = Application.dataPath + "/Data/UserMess/" + name + ".xml";

		if (File.Exists (path)) {
			yield return new WaitForSeconds (1f);
			Application.LoadLevel ("Load");
		} else {
			Application.LoadLevel ("ChoosePlayer");
		}
	}
	string dl(string Username,string passwd){
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("data");
		foreach (XmlElement it in root) {
			if (it.GetAttribute ("Username") == Username && it.GetAttribute ("Password") == passwd) {
				return "登录成功";
			}
		}
		return "用户名或密码错误";
	}
	public void Gozhuce(){
		//去注册页面
		GameObject.Find ("LblInput_Login").GetComponent<UILabel> ().text = "请输入账号";
		GameObject.Find ("LblInput_PWD").GetComponent<UILabel> ().text = "请输入密码";
		WindowLogin.SetActive (false);
		WindowRegister.SetActive (true);
	}
}

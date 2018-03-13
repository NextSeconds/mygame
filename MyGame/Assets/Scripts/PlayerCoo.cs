using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
public class PlayerCoo : MonoBehaviour {
	public GameObject yangjian;//0
	public GameObject yangjianUI;
	public GameObject change;//1
	public GameObject changeUI;
	public GameObject daomo;//2
	public GameObject nveba;//3
	public GameObject xiaoyaozi;//4
	public GameObject changdaonv;//5
	public List<GameObject> PlayerShow = new List<GameObject> ();
	public List<string> Name = new List<string> ();
	public int CurrentNum = 0;
	public UILabel nameshow;
	public string CurrentUserName= string.Empty;
	public string fullpath =string.Empty;//当前用户xml文档保存的路径

	void Start () {
		string Userpath = Application.dataPath + "/Data//CurrentUser";//当前用户名
		CurrentUserName = CurrentUser (Userpath);

		string filepath = Application.dataPath +"/Data/UserMess";
		fullpath = filepath + "/" + CurrentUserName + ".xml";
		if (!File.Exists (fullpath)) {
			StartCoroutine ("CreateUserFile", fullpath);
		}
		PlayerShow.Add (yangjian);
		PlayerShow.Add (change);
		PlayerShow.Add (daomo);
		PlayerShow.Add (nveba);
		PlayerShow.Add (xiaoyaozi);
		PlayerShow.Add (changdaonv);
		TxTParse ();
	}
	IEnumerator CreateUserFile(string path){//创建用户角色初始数据文档
		XmlDocument _doc = new XmlDocument ();
		XmlElement root = _doc.CreateElement ("Mess");
		_doc.AppendChild (root);

		//英雄具体资料
		XmlElement Hero = _doc.CreateElement ("Hero");
		XmlElement PlayerName = _doc.CreateElement ("PlayerName");
		XmlElement JveSe = _doc.CreateElement ("JveSe");
		XmlElement Level = _doc.CreateElement ("Level");
		Level.InnerText = "1";
		XmlElement EXP = _doc.CreateElement ("EXP");
		EXP.InnerText ="0";
		XmlElement LevelNum = _doc.CreateElement ("LevelNum");
		LevelNum.InnerText = "1";
		XmlElement HP = _doc.CreateElement ("HP");
		HP.InnerText = "2000";
		XmlElement MP = _doc.CreateElement ("MP");
		MP.InnerText = "1000";
		XmlElement Money = _doc.CreateElement ("Money");
		Money.InnerText = "1000";
		XmlElement Damage = _doc.CreateElement ("Damage");
		Damage.InnerText = "10";
		XmlElement Defense = _doc.CreateElement ("Defense");
		Defense.InnerText = "10";
		XmlElement CurrentHP = _doc.CreateElement ("CurrentHP");
		CurrentHP.InnerText = HP.InnerText;
		XmlElement CurrentMP = _doc.CreateElement ("CurrentMP");
		CurrentMP.InnerText = MP.InnerText;

		root.AppendChild (Hero);
		Hero.AppendChild (PlayerName);
		Hero.AppendChild (Level);
		Hero.AppendChild (EXP);
		Hero.AppendChild (LevelNum);
		Hero.AppendChild (HP);
		Hero.AppendChild (MP);
		Hero.AppendChild (CurrentHP);
		Hero.AppendChild (CurrentMP);
		Hero.AppendChild (Damage);
		Hero.AppendChild (Defense);
		Hero.AppendChild (Money);
		Hero.AppendChild (JveSe);

		//装备具体资料
		XmlElement Clothes = _doc.CreateElement ("Clothes");
		XmlElement clothes = _doc.CreateElement ("clothes");
		XmlElement Hat = _doc.CreateElement ("hat");
		XmlElement Cuirass = _doc.CreateElement ("weapons");
		XmlElement Shoes = _doc.CreateElement ("shoes");

		root.AppendChild (Clothes);
		Clothes.AppendChild (Hat);
		Clothes.AppendChild (Cuirass);
		Clothes.AppendChild (Shoes);
		Clothes.AppendChild (clothes);
		//物品具体资料
		XmlElement items = _doc.CreateElement ("items");

		root.AppendChild (items);

		//保存角色初始文档（创建）
		_doc.Save (path);
		yield return 0;
	}
	string CurrentUser(string path){//读出当前登录的用户名
		string text = ReadFileContent (path);
		return text;
	}
	string ReadFileContent(string FullPath){//读取用户个人信息存储文档

		FileInfo info = new FileInfo (FullPath);
		if (!info.Exists) {
			Debug.Log ("找不到文件");
			return null;
		} else {
			return File.ReadAllText (FullPath);
		}
	}
	void TxTParse(){
		string Full = Application.dataPath + "/Data//suijiname.txt";
		StreamReader sr = null;
		if (!File.Exists (Full)) {
			File.Create (Full);
			Debug.Log ("没有找到随机名字存储文档");
		} else {
			sr = File.OpenText (Full);
		}
		string Content = string.Empty;
		while ((Content = sr.ReadLine ()) != null) {
			Name.Add (Content);
		}
		sr.Close ();
		sr.Dispose ();
	}
	public void SuijiMess(){//随机
		int i = Random.Range (0, Name.Count - 1);
		nameshow.text = Name[i];
	}
	public void ShowMess(){
		switch (CurrentNum) {
		case 0:
			if (!yangjianUI.activeSelf) {
				yangjianUI.SetActive (true);
			} else {
				yangjianUI.SetActive (false);
			}
			break;
		case 1:
			if (!changeUI.activeSelf) {
				changeUI.SetActive (true);
			} else {
				changeUI.SetActive (false);
			}
			break;
		default:
			Debug.Log ("当前角色的详细信息尚未录入");
			break;
		}
	}
	public void NextClick(){
		PlayerShow [CurrentNum].transform.rotation = new Quaternion (0, 180, 0, 0);
		if (CurrentNum != 5) {
			PlayerShow [CurrentNum + 1].SetActive (true);
			PlayerShow [CurrentNum].SetActive (false);
			CurrentNum++;
		} else {
			PlayerShow [0].SetActive (true);
			PlayerShow [CurrentNum].SetActive (false);
			CurrentNum = 0;
		}
	}
	public void BackClick(){
		PlayerShow [CurrentNum].transform.rotation = new Quaternion (0, 180, 0, 0);
		if (CurrentNum != 0) {
			PlayerShow [CurrentNum - 1].SetActive (true);
			PlayerShow [CurrentNum].SetActive (false);
			CurrentNum--;
		} else {
			PlayerShow [5].SetActive (true);
			PlayerShow [CurrentNum].SetActive (false);
			CurrentNum = 5;
		}
	}
	public void Login(){
		CunCuPlayerName ();
		Application.LoadLevel ("Load");
	}
    void CunCuPlayerName()
    {
        XmlDocument _doc = new XmlDocument();
        _doc.Load(fullpath);
        XmlNode root = _doc.SelectSingleNode("Mess");
        XmlNode hero = root.SelectSingleNode("Hero");
        XmlNode PlayerName = hero.SelectSingleNode("PlayerName");
        PlayerName.InnerText = nameshow.text;

        XmlNode JveSe = hero.SelectSingleNode("JveSe");
        PlayerPrefs.SetInt("heroShowIndex", CurrentNum);
        JveSe.InnerText = CurrentNum.ToString();
        _doc.Save(fullpath);
    }

	public Camera WNGUICamera;  
	public Vector3 _WoldPosition;//指针的初始位置  
	// Vector3 _WoldAng;  
	public Vector3 WscreenSpace;  //该对象在屏幕中的坐标位置（点）
	public Vector3 Woffset;  
	public Vector3 WcurScreenSpace;  
	public Vector3 WcurPosition;  
	//工具跟随鼠标拖拽移动  
	IEnumerator fnOnMouseDown()  
	{  
		// 获取目标对象当前的世界坐标系位置，并将其转换为屏幕坐标系的点
		WscreenSpace = WNGUICamera.WorldToScreenPoint(transform.position);  //脚本所挂对象在平面上的位置
		// 设置鼠标的屏幕坐标向量，用上面获得的Pos的z轴数据作为鼠标的z轴数据，使鼠标坐标与目标对象坐标处于同一层面上
		Woffset = transform.position - WNGUICamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, WscreenSpace.z));  //脚本所挂对象于鼠标点击的位置相差的距离

		while (Input.GetMouseButton(0))  
		{  
			//当点击的位置处于屏幕中时
			if (Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width && Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height)  
			{  
				// 用上面获取到的鼠标坐标转换为世界坐标系的点，并用其设置目标对象的当前位置
				WcurScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, WscreenSpace.z);  //鼠标在屏幕上点击的位置
				WcurPosition = WNGUICamera.ScreenToWorldPoint(WcurScreenSpace) + Woffset;  //该对象的位置
				transform.position = new Vector3(WcurPosition.x, WcurPosition.y, 0f);  //该对象相对于鼠标点击位置是固定的 但是会跟随鼠标移动
			}  
			// 等待下一帧对数据进行更新，实现目标对象的位移
			yield return new WaitForFixedUpdate();  
		}  
	}  
	void Update(){
		if(Input.GetMouseButtonDown(0)){
			StartCoroutine ("Hua");
		}

	}
	IEnumerator Hua(){
		while (Input.GetMouseButton (0)) {
			if (Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width && Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height) {
				float x = Input.mousePosition.x;
				yield return 0;
				float Xrota = -(Input.mousePosition.x - x);
				PlayerShow [CurrentNum].transform.Rotate (new Vector3 (0f, Xrota, 0f));
			}
			yield return new WaitForFixedUpdate ();
		}
	}

}

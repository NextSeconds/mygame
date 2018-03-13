using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class Qiandao : MonoBehaviour {//这个脚本用于操作签到面板
	public string timeURL = "http://www.beijing-time.org/time15.asp";//北京时间地址
	public List<string> timelist = new List<string>();
	public int year;
	public int month;
	public int day;
	public int week;
	public int hour;
	public int min;
	public int sec;
	public int Num;//创建对象的个数
	public GameObject prefab;
	public UIGrid grid;
	public UILabel tianshutip;
	public UILabel jinri;
	private string Userpath = string.Empty;
	public string Currentusername = string.Empty;
	public string filePath =string.Empty;
	public string fullPath =string.Empty;//文件的绝对路径
	public string fileinfos =string.Empty;//文件的具体信息
	public string fileName = string.Empty; //签到信息的存储文件
	// Use this for initialization
	void Start()
	{
		StartCoroutine(GetTime1());
		Userpath = Application.dataPath + "/Data//CurrentUser";
		CurrentUser (Userpath);
		filePath = Application.dataPath+"/Data/Sign in";
		fullPath = filePath + "//" + fileName;
		if (!File.Exists (fullPath)) {
			CreateOrWriteJsonFile (fullPath,"");
			Debug.Log ("此用户签到文件创建成功");
		} else {
			Debug.Log ("已读取此用户签到文件");
		}
	}

	public void CurrentUser(string path){//读出当前登录的用户名
		string text = ReadFileContent (path);
		string[] alluser = text.Split ('\n');
		Currentusername = alluser [alluser.Length - 1];
		fileName = Currentusername;
	}
	IEnumerator GetTime1()
	{
		WWW www = new WWW(timeURL);
		while (!www.isDone)
		{
			yield return www;
			Debug.Log("网络时间获取成功" + www.text);
			SplitString(www);
		}
	}

	public void SplitString(WWW www)
	{
		string[] timeData = www.text.Split(';');
		for (int i = 0; i < timeData.Length-1; i++)
		{
			string[] exactTime = timeData[i].Split('=');
			timelist.Add(exactTime [1]);
		}
		year = int.Parse(timelist [1]);
		month = int.Parse (timelist [2]);
		day = int.Parse (timelist [3]);
		week = int.Parse (timelist [4]);
		hour = int.Parse (timelist [5]);
		min = int.Parse (timelist [6]);
		sec = int.Parse (timelist [7]);

		Run (year,month);
		ChuangJian ();
	}
	public void ChuangJian(){//动态创建的在面板上出每个日期
		for (int i = 0; i < Num; i++) {
			GameObject cell = Instantiate (prefab)as GameObject;
			cell.transform.parent = grid.transform;	
			//保证自身大小不变， 
			//localScale：自身缩放， 
			//Vector3.one是指x, y, z轴的缩放比例均为1
			cell.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
			cell.name = System.Convert.ToString(i+1);
			GameObject.Find ("MonthTip").GetComponent<UILabel> ().text = "[b]" + System.Convert.ToString (month);
			cell.transform.Find ("Label").GetComponent<UILabel> ().text = "[000000][b]" + System.Convert.ToString (i + 1);
			if (i < (day-1)) {//对在今天之前的日期 进行如下操作
				cell.transform.Find ("buke").GetComponent<UISprite> ().enabled = true;
				cell.transform.Find ("biaozhi").GetComponent<UISprite> ().enabled = false;
				cell.GetComponent<UIButton> ().enabled = false;
				cell.GetComponent<BoxCollider> ().enabled = false;
			} else {
				cell.transform.Find ("buke").GetComponent<UISprite> ().enabled = false;
				cell.transform.Find ("biaozhi").GetComponent<UISprite> ().enabled = false;
				cell.GetComponent<UIButton> ().enabled = true;
			}
		}
		change (timelist[1],timelist[2],timelist[3]);
		grid.Reposition ();

	}
	public void Run(int y,int m){//在创建时调用，确定该月应该创建的日期预设体个数
		if (((y % 4 == 0) && (y % 100 != 0)) || (y % 400 == 0)) {
			switch (m) {
			case 1:
				Num = 31;
				break;
			case 2:
				Num = 29;
				break;
			case 3:
				Num = 31;
				break;
			case 4:
				Num = 30;
				break;
			case 5:
				Num = 31;
				break;
			case 6:
				Num = 30;
				break;
			case 7:
				Num = 31;
				break;
			case 8:
				Num = 31;
				break;
			case 9:
				Num = 30;
				break;
			case 10:
				Num = 31;
				break;
			case 11:
				Num = 30;
				break;
			case 12:
				Num = 31;
				break;
			default:
				Debug.Log ("月份获取错误");
				break;
			}
		} else {
			switch (m) {
			case 1:
				Num = 31;
				break;
			case 2:
				Num = 28;
				break;
			case 3:
				Num = 31;
				break;
			case 4:
				Num = 30;
				break;
			case 5:
				Num = 31;
				break;
			case 6:
				Num = 30;
				break;
			case 7:
				Num = 31;
				break;
			case 8:
				Num = 31;
				break;
			case 9:
				Num = 30;
				break;
			case 10:
				Num = 31;
				break;
			case 11:
				Num = 30;
				break;
			case 12:
				Num = 31;
				break;
			default:
				Debug.Log ("月份获取错误");
				break;
			}
		}
	}


	public void CreateOrWriteJsonFile( string fullPath ,string fileContent){//当没有签到信息存储文档时 创建这个文档 
		//绝对路径
		File.WriteAllText (fullPath,fileContent);
	}
	public void add(string year,string month,string day){
		string fileContent = year + "." + month + "." + day;
		File.AppendAllText (fullPath, fileContent + "\n");//将签到的日期添入文件中
	}
	public void change(string year,string month,string day){//读出已签信息，并改变UI显示 传递的参数信息为今天的日期
		string text = ReadFileContent (fullPath);
		string[] textinfo = text.Split ('\n');

		for (int i = 0; i < textinfo.Length; i++) {
			string[] date = textinfo [i].Split ('.');
			if ((year == date [0]) && (month == date [1])) {
				GameObject.Find(date [2]).transform.Find ("biaozhi").GetComponent<UISprite> ().enabled = true;
				QD ();
				if (day == date [2]) {
					GameObject.Find(date [2]).GetComponent<UIButton> ().enabled = false;
					GameObject.Find(date [2]).GetComponent<BoxCollider> ().enabled = false;
					jinri.text = "今日已签";
				}
			}
		}
	}
	public string ReadFileContent(string fullPath){//读取签到信息存储文档

		FileInfo info = new FileInfo (fullPath );
		if (!info.Exists) {
			Debug.Log ("找不到文件");
			return null;
		} else {
			return File.ReadAllText (fullPath);
		}
	}
	public int Qday = 0;//签到天数统计

	public void QD(){
		Qday++;
		tianshutip.text = "[b]"+System.Convert.ToString (Qday);
	}

}
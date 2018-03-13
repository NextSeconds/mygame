using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Dianji : MonoBehaviour {
	public string timeURL = "http://www.beijing-time.org/time15.asp";
	public List<string> time = new List<string> ();
	public int day;
	public int djday;//这个脚本是挂到日期上 未了在点击相应的日期时触发点击回调函数
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnClick(){
		StartCoroutine (GetTime1());

	}
	IEnumerator GetTime1()
	{
		WWW www = new WWW(timeURL);
		while (!www.isDone)
		{
			yield return www;
//			Debug.Log("网络时间获取成功：" + www.text);
			SplitString(www);
		}
	}

	public void SplitString(WWW www)
	{
		string[] timeData = www.text.Split (';');
		for (int i = 0; i < timeData.Length - 1; i++) {
			string[] exactTime = timeData [i].Split ('=');
			time.Add (exactTime [1]);
		}
		day = int.Parse (time[3]);
		djday = int.Parse(gameObject.name);
		if (day == djday) {
			gameObject.GetComponent<UIButton> ().enabled = false;
			gameObject.GetComponent<BoxCollider> ().enabled = false;
			Debug.Log ("签到成功");
			GameObject.Find("jinri").GetComponent<UILabel>().text = "今日已签";
			gameObject.transform.Find ("biaozhi").GetComponent<UISprite> ().enabled = true;
			GameObject.Find ("SignInToControl").GetComponent<Qiandao> ().QD();
			GameObject.Find ("SignInToControl").GetComponent<Qiandao> ().add (time[1], time[2], time[3]);
		} else {
			if (day > djday) {
				gameObject.transform.Find ("buke").GetComponent<UISprite> ().enabled = true;
				gameObject.GetComponent<UIButton> ().enabled = false;
				gameObject.GetComponent<BoxCollider> ().enabled = false;
			}
			Debug.Log ("签到失败");
		}
	}
}
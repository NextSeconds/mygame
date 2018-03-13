using UnityEngine;
using System.Collections;
using System.Xml;

public class Hero : MonoBehaviour {
	public int Money;
	public float HP;
	public float MP;
	public float CurrentHP;
	public float CurrentMP;
	public int Damage = 0;//攻击力
	public int Defense = 0;//防御力
	public int Combat;
	public string PlayerName = string.Empty;
	public UILabel MoneyXianshi;
	public UILabel PlayerNameShow;
	public GameObject heroBackground;
	public UILabel Tip;
	public string fullpath =string.Empty;//将会在背包创建完成后初始化
	// Use this for initialization
	void Start () {
		Tip = GameObject.Find ("UI Root").transform.Find("FuBen/Tip").gameObject.GetComponent<UILabel> ();
	}
	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator ShowZBui(XmlNodeList ZBlist){//刚开始的时候 去显示装备栏中的装备 并存入对应id于1
		for (int i = 0; i < ZBlist.Count; i++) {
			if (ZBlist [i].Attributes ["ID"] != null) {
				heroBackground.transform.Find (ZBlist [i].Name).gameObject.GetComponent<UISprite> ().spriteName = ZBlist [i].Attributes ["Icon"].Value;
				heroBackground.transform.Find (ZBlist [i].Name).gameObject.GetComponent<UIButton>().normalSprite = ZBlist [i].Attributes ["Icon"].Value;
				heroBackground.transform.Find (ZBlist [i].Name + "1").gameObject.GetComponent<UILabel> ().text = ZBlist [i].Attributes ["ID"].Value;
				if (ZBlist [i].Name == "weapons") {
					Damage = int.Parse (ZBlist [i].Attributes ["shuxing"].Value);
					heroBackground.transform.Find ("Damage/Damage").gameObject.GetComponent<UILabel> ().text = ZBlist [i].Attributes ["shuxing"].Value;
				} else {
					Defense += int.Parse (ZBlist [i].Attributes ["shuxing"].Value);
					heroBackground.transform.Find ("Defense/Defense").gameObject.GetComponent<UILabel> ().text = Defense.ToString();
				}
				yield return 0;
			}
		}
	}
	public void HeroParse(string path){
		heroBackground = GameObject.Find ("UI Root").transform.Find ("beibaomianban/HeroBackGround").gameObject;
		fullpath = path;
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("Mess");
		XmlNode hero = root.SelectSingleNode ("Hero");
		XmlNode money = hero.SelectSingleNode ("Money");
		XmlNode Clothes = root.SelectSingleNode ("Clothes");
		XmlNodeList ZBlist = Clothes.ChildNodes;
		StartCoroutine ("ShowZBui", ZBlist);
		XmlNode playername = hero.SelectSingleNode ("PlayerName");
		PlayerName = playername.InnerText;
		PlayerNameShow.text = PlayerName;
		Money = int.Parse (money.InnerText);
		UpdateMoney ();
		XmlNode HPN = hero.SelectSingleNode ("HP");
		heroBackground.transform.Find ("HP/HP").gameObject.GetComponent<UILabel> ().text = HPN.InnerText;
		HP = float.Parse (HPN.InnerText);
		XmlNode MPN = hero.SelectSingleNode ("MP");
		heroBackground.transform.Find ("MP/MP").gameObject.GetComponent<UILabel> ().text = MPN.InnerText;
		MP = float.Parse (MPN.InnerText);
		XmlNode CHP = hero.SelectSingleNode ("CurrentHP");
		CurrentHP = float.Parse (CHP.InnerText);
		XmlNode CMP = hero.SelectSingleNode ("CurrentMP");
		CurrentMP = float.Parse (CMP.InnerText);
		XueLiangShow ();
	}
	public void XueLiangShow(){
		GameObject.Find("UI Root").transform.Find("UIWindow_Header/HeaderBackground/HP").GetComponent<UIProgressBar> ().value = CurrentHP / HP;
		GameObject.Find("UI Root").transform.Find("UIWindow_Header/HeaderBackground/MP").GetComponent<UIProgressBar> ().value = CurrentMP / MP;
	}
	public void HeroShuxingShow(){
		heroBackground.transform.Find ("Damage/Damage").gameObject.GetComponent<UILabel> ().text = Damage.ToString();
		heroBackground.transform.Find ("Defense/Defense").gameObject.GetComponent<UILabel> ().text = Defense.ToString ();
	}
	void UpdateMoney(){//显示更新后的金钱数额
		MoneyXianshi.text = Money.ToString ();
	}
	public void MoneyChange(char change,int mon){//当金钱数额发生改变时调用
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("Mess");
		XmlNode hero = root.SelectSingleNode ("Hero");
		XmlNode money = hero.SelectSingleNode ("Money");
		switch (change) {
		case '+':
			Money += mon;
			UpdateMoney ();
			money.InnerText = Money.ToString ();
			_doc.Save (fullpath);
			break;
		case '-':
			Money -= mon;
			UpdateMoney ();
			money.InnerText = Money.ToString ();
			_doc.Save (fullpath);
			break;
		default:
			Debug.Log ("你输入的金钱刷新方式错误");
			break;
		}
	}
	public void Putong(){
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("Mess");
		XmlNode hero = root.SelectSingleNode ("Hero");
		XmlNode LevelN = hero.SelectSingleNode ("LevelNum");
		if (int.Parse (LevelN.InnerText) > 0) {
			LevelN.InnerText = "1";
			_doc.Save (fullpath);
			Application.LoadLevel ("fuben");
		} else {
			StartCoroutine ("TipShow");
		}
	}
	public void Jingying(){
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("Mess");
		XmlNode hero = root.SelectSingleNode ("Hero");
		XmlNode LevelN = hero.SelectSingleNode ("LevelNum");
		if (int.Parse (LevelN.InnerText) > 1) {
			LevelN.InnerText = "2";
			_doc.Save (fullpath);
			Application.LoadLevel ("fuben");
		} else {
			StartCoroutine ("TipShow");
		}
	}
	public void Boss(){
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("Mess");
		XmlNode hero = root.SelectSingleNode ("Hero");
		XmlNode LevelN = hero.SelectSingleNode ("LevelNum");
		if (int.Parse (LevelN.InnerText) > 2) {
			LevelN.InnerText = "3";
			_doc.Save (fullpath);
			Application.LoadLevel ("fuben");
		} else {
			StartCoroutine ("TipShow");
		}
	}
	IEnumerator TipShow(){
		Tip.text = "请解锁之前的关卡";
		yield return new WaitForSeconds (2f);
		Tip.text = "";
	}
}

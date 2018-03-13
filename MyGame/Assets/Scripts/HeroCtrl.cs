using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class HeroCtrl : MonoBehaviour {
	public Animation ani;
	public EasyJoystick myjoy;
	public CharacterController ccr;
	private Vector3 de;
	public bool isAttack = false;
	public static bool isdie;
	private Camera maincam;
	private Camera MapCamera;
	private HUDText hudtext;
	public GameObject monster1;
	public GameObject monster2;
	public GameObject Boss;
	public int LevelNum;
	public UILabel Tip;
	public UILabel C;
	public UILabel Z;
	// Use this for initialization
	// Update is called once per frame
	void Start () {
        HeroCtrl.isdie = false;
		maincam = GameObject.Find ("Main Camera1").GetComponent<Camera> ();
		MapCamera = GameObject.Find ("MapCamera").GetComponent<Camera> ();
		hudtext = GameObject.Find ("heroHUD").GetComponent<HUDText> ();
		MoneyXianshi = GameObject.Find ("jinbi").GetComponent<UILabel> ();
		PlayerNameShow = GameObject.Find ("UI Root").transform.Find("UIWindow_Header/HeaderBackground/PlayerName").gameObject.GetComponent<UILabel> ();
		HeroParse ();
		switch (LevelNum) {
		case 1:
			monster1.SetActive (true);
			break;
		case 2:
			monster1.SetActive (true);
			monster2.SetActive (true);
			break;
		case 3:
			monster1.SetActive (true);
			monster2.SetActive (true);
			Boss.SetActive (true);
			break;
		case 4:
			monster1.SetActive (true);
			monster2.SetActive (true);
			Boss.SetActive (true);
			break;
		default:
			Debug.Log ("未知的关卡");
			break;
		}
		StartCoroutine ("T");
	}
	void Update () {
		if (!isAttack&&!isdie) {
			Ctrl ();
		}
		CameraFollow ();
		MapCameraFollow ();

	}
	IEnumerator T(){
		Tip = GameObject.Find ("Tip").GetComponent<UILabel> ();
		C = GameObject.Find ("C").GetComponent<UILabel> ();
		Z =GameObject.Find ("Z").GetComponent<UILabel> ();
		if (LevelNum == 4) {
			Z.text = "3";
		} else {
			Z.text = LevelNum.ToString ();
		}
		yield return new WaitForSeconds (5f);
		Tip.text = "";
	}
	void MapCameraFollow(){
		MapCamera.transform.position = transform.position + new Vector3 (0, 25, 0);
//		MapCamera.transform.LookAt (transform.position);
	}
	void CameraFollow(){
		maincam.transform.position = transform.position + new Vector3 (-7, 9, 0);
		maincam.transform.LookAt (transform);
	}
	void Ctrl(){
		float y = -myjoy.JoystickTouch.x;
		float x = myjoy.JoystickTouch.y;
		transform.LookAt (transform.position + new Vector3 (x, 0, y));
		if (x != 0 || y != 0) {
			de = new Vector3 (x, 0, y);
//			transform.Translate (Vector3.forward * 4f * Time.deltaTime, Space.Self);
			de.y -= 9.8f;
			ccr.Move (de * 4 * Time.deltaTime);
			ani.CrossFade ("Run");
		} else {
			ani.CrossFade ("Idle");
		}
	}
	public void q(){
		if (!isAttack&&!isdie) {
			StartCoroutine ("Attack2");
		}
	}
	public void w(){
	
	}
	public void e(){

	}
	public void a(){
		if (!isAttack&&!isdie) {
			StartCoroutine ("Attack1");
		}
	}
	IEnumerator Attack1(){
		isAttack = true;
		ani.CrossFade ("Attack1");
		while(ani.isPlaying){
			yield return 0;
		}
		isAttack = false;
	}
	IEnumerator Attack2(){
		isAttack = true;
		int a = Damage;
		Damage = 500;
		ani.CrossFade ("Attack2");
		while(ani.isPlaying){
			yield return 0;
		}
		Damage = a;
		isAttack = false;
	}
	public int Money;
	public float HP;
	public float MP;
	public float CurrentHP;
	public float CurrentMP;
	public int Damage = 0;//攻击力
	public int Defense = 0;//防御力
	public int Combat;
	public string PlayerName = string.Empty;
	private UILabel MoneyXianshi;
	private UILabel PlayerNameShow;
	public string fullpath =string.Empty;
	// Use this for initialization
	void Awake () {
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

	public void HeroParse(){
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("Mess");
		XmlNode hero = root.SelectSingleNode ("Hero");
		XmlNode money = hero.SelectSingleNode ("Money");
		XmlNode playername = hero.SelectSingleNode ("PlayerName");
		PlayerName = playername.InnerText;
		PlayerNameShow.text = PlayerName;
		Money = int.Parse (money.InnerText);
		UpdateMoney ();
		XmlNode HPN = hero.SelectSingleNode ("HP");
		HP = float.Parse (HPN.InnerText);
		XmlNode MPN = hero.SelectSingleNode ("MP");
		MP = float.Parse (MPN.InnerText);
		XmlNode CHP = hero.SelectSingleNode ("CurrentHP");
		CurrentHP = float.Parse (CHP.InnerText);
		XmlNode CMP = hero.SelectSingleNode ("CurrentMP");
		CurrentMP = float.Parse (CMP.InnerText);
		XmlNode Leveln = hero.SelectSingleNode ("LevelNum");
		LevelNum = int.Parse (Leveln.InnerText);
		XueLiangShow ();
		XmlNode gongjili = hero.SelectSingleNode ("Damage");
		Damage = int.Parse(gongjili.InnerText);
		XmlNode fangyuli = hero.SelectSingleNode ("Defense");
		Defense = int.Parse (fangyuli.InnerText);
	}
	public void XueLiangShow(){
		GameObject.Find("UI Root").transform.Find("UIWindow_Header/HeaderBackground/HP").GetComponent<UIProgressBar> ().value = CurrentHP / HP;
		GameObject.Find("UI Root").transform.Find("UIWindow_Header/HeaderBackground/MP").GetComponent<UIProgressBar> ().value = CurrentMP / MP;
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
	void OnTriggerEnter(Collider it){
		if (it.tag.Equals ("MonsterWeapon")&& !isdie) {
			CurrentHP -= 100;
			if (CurrentHP > 0) {
				GameObject.Find("UI Root").transform.Find("UIWindow_Header/HeaderBackground/HP").GetComponent<UIProgressBar> ().value = CurrentHP / HP;
				hudtext.Add ("-100",Color.red,1f);
			} else {
				isdie = true;
				GameObject.Find("UI Root").transform.Find("UIWindow_Header/HeaderBackground/HP").GetComponent<UIProgressBar> ().value = 0;
				ani.CrossFade ("Death");
				StartCoroutine ("Death");
			}
		}
	}
	IEnumerator Death(){
		while (ani.isPlaying) {
			yield return 0;
		}
		Tip.text = "您已死亡，正在返回主页面，请等待。";
		yield return new WaitForSeconds (1f);
		Application.LoadLevel ("SheQu");
	}

}

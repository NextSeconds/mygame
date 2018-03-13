using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Xml;

public class MonsterAIUI : MonoBehaviour {
	public float HP; 
	public float CurrentHP = 2000;
	public float Dam;
	public Slider MonsterSlider;
	public Transform xuetiao;
	public GameObject Hero;
	public Vector3 chushipos;
//	private float movespeed = 5f;
	public Animation ani;
	public CharacterController ccr;
	public UILabel Tip;
	public Camera maincam;
	public HUDText hudtext;
	private UILabel C;
	private UILabel Z;
	private UILabel jinbi;
	private string fullpath = string.Empty;
	// Use this for initialization
	void Start () {
		Hero = GameObject.FindGameObjectWithTag ("Hero");
		Tip = GameObject.Find ("Tip").GetComponent<UILabel> ();
		HP = 2000;
		Dam = 100;
		chushipos = transform.position;
		StartCoroutine ("Move");
		C =GameObject.Find ("C").GetComponent<UILabel> ();
		Z =GameObject.Find ("Z").GetComponent<UILabel> ();
		jinbi = GameObject.Find ("jinbi").GetComponent<UILabel> ();

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
	IEnumerator Move(){//当无人时 战立 有人时追击 被甩时回到初始位置
		while (true) {
			if (Vector3.Distance (transform.position, Hero.transform.position) <= 5f && !HeroCtrl.isdie) {
				yield return StartCoroutine ("Follow");
			}
			if (Vector3.Distance (transform.position, chushipos) > 1f) {
				Vector3 direction = chushipos - transform.position;
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion
					.LookRotation (direction), 10 * Time.deltaTime);
				direction = direction.normalized;
				direction.y -= 9.8f;
				ccr.Move (direction * 4 * Time.deltaTime);
				ani.CrossFade ("Walk");
			} else {
				ani.CrossFade ("Idle");
			}
			yield return new WaitForEndOfFrame ();
		}
	}
	IEnumerator Follow(){
		while (true) {
			if (Vector3.Distance (transform.position, Hero.transform.position) > 5f || HeroCtrl.isdie) {
				break;
			}
			if (Vector3.Distance (transform.position, Hero.transform.position) <= 2f && !HeroCtrl.isdie) {
				yield return StartCoroutine ("Attack");
			}
			Vector3 direction = Hero.transform.position - transform.position;
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion
				.LookRotation (direction), 10 * Time.deltaTime);
			direction = direction.normalized;
			direction.y -= 9.8f;
			ccr.Move (direction * 4 * Time.deltaTime);
			ani.CrossFade ("Run");
			yield return new WaitForEndOfFrame ();
		}
		yield return 0;
	}
	IEnumerator Attack(){
		while (true) {
			if (Vector3.Distance (transform.position, Hero.transform.position) > 2f || HeroCtrl.isdie) {
				break;
			}
			ani.CrossFade ("Attack1");
			yield return new WaitForEndOfFrame ();
		}
	}
	// Update is called once per frame
	public void Shoushang(int x){
        if (CurrentHP <= 0) 
        {
            return;
        }
		CurrentHP -= x;
		if (CurrentHP > 0) {
			MonsterSlider.value = CurrentHP/HP;
			hudtext.Add ("-" + Hero.GetComponent<HeroCtrl>().Damage.ToString(),Color.red,1f);
		} else {
			MonsterSlider.value = 0;
			ani.CrossFade ("Death");
			StopAllCoroutines ();
			StartCoroutine ("Death");
		}
	}
	IEnumerator Death(){
		while (ani.isPlaying) {
			yield return 0;
		}
		Tip.text = "杀死怪物获得40金币";
		yield return new WaitForSeconds (1f);
		Tip.text = "";
		C.text = (int.Parse (C.text) + 1).ToString ();
		jinbi.text = (int.Parse (jinbi.text) + 40).ToString ();
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("Mess");
		XmlNode hero = root.SelectSingleNode ("Hero");
		XmlNode money = hero.SelectSingleNode ("Money");
		money.InnerText = (int.Parse (money.InnerText) + 40).ToString();
		if (C.text == Z.text) {
			XmlNode LevelNum = hero.SelectSingleNode ("LevelNum");
			LevelNum.InnerText = (int.Parse(LevelNum.InnerText) + 1).ToString();
			StartCoroutine ("TiaoZhuan");
		} else {
			this.gameObject.SetActive (false);
		}
		_doc.Save (fullpath);
	}
	IEnumerator TiaoZhuan(){
		yield return 0;
		Tip.text = "结算奖励中请等待";
		yield return new WaitForSeconds (2f);
		Application.LoadLevel ("SheQu");
	}
	void Update () {
		xuetiao.LookAt (maincam.transform);
	}
	void OnTriggerEnter(Collider it){
		Debug.Log (it.tag);
		if (it.tag.Equals ("HeroWeapon") && Hero.GetComponent<HeroCtrl>().isAttack) {
			Shoushang (Hero.GetComponent<HeroCtrl>().Damage);
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Xml;
public class BagOnClick : MonoBehaviour {//用于背包内物品点击时传递id 以构建详细信息面板
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnClick(){
		GameObject UIroot = GameObject.Find ("UI Root");
		GameObject Show = UIroot.transform.Find ("beibaomianban/wupin/SHOW").gameObject;
		Show.transform.Find("ID").GetComponent<UILabel>().text = transform.name;
		string fullpath = Application.dataPath + "/Data/ShopInfo.xml";
		XmlDocument _doc = new XmlDocument ();
		_doc.Load (fullpath);
		XmlNode root = _doc.SelectSingleNode ("ShopInfo");
		foreach (XmlElement it in root) {
			if (it.GetAttribute ("ID") == transform.name) {
				Show.transform.Find ("Photo").GetComponent<UISprite> ().spriteName = it.GetAttribute ("Icon");
				Show.transform.Find ("Name").GetComponent<UILabel> ().text = it.GetAttribute ("Name");
				Show.transform.Find ("jiage").GetComponent<UILabel> ().text = it.GetAttribute("Price").ToString();
				switch (it.Name) {//C#中支持string类型
				case "clothes":
					Show.transform.Find ("Type").GetComponent<UILabel> ().text = "衣服";
					Show.transform.Find ("gongjili (1)").GetComponent<UILabel> ().text = "防御力";
					break;
				case "weapons":
					Show.transform.Find ("Type").GetComponent<UILabel> ().text = "武器";
					Show.transform.Find ("gongjili (1)").GetComponent<UILabel> ().text = "攻击力";
					break;
				case "hat":
					Show.transform.Find ("Type").GetComponent<UILabel> ().text = "帽子";
					Show.transform.Find ("gongjili (1)").GetComponent<UILabel> ().text = "防御力";
					break;
				case "shoes":
					Show.transform.Find ("Type").GetComponent<UILabel> ().text = "鞋子";
					Show.transform.Find ("gongjili (1)").GetComponent<UILabel> ().text = "防御力";
					break;
				case "medicine":
					Show.transform.Find ("Type").GetComponent<UILabel> ().text = "消耗品";
					if (int.Parse(it.GetAttribute ("ID")) < 2000) {
						Show.transform.Find ("gongjili (1)").GetComponent<UILabel> ().text = "回复HP";
					}else{
						Show.transform.Find ("gongjili (1)").GetComponent<UILabel> ().text = "回复MP";
					}
					break;
				default:
					Debug.Log ("错误");
					break;
				}
				Show.SetActive (true);
				break;
			}
		}
	}
}

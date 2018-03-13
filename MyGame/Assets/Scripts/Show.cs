using UnityEngine;
using System.Collections;

public class Show : MonoBehaviour {
	private GameObject uiroot;//ui root根节点
	// Use this for initialization
	void Start () {
		uiroot = GameObject.Find ("UI Root");
	}
	public void Esc(){
		gameObject.SetActive (false);
	}
	public void Sale(){// *这个是出售 可以直接获取id 就可进行背包内的删除操作*
		//在文档中删除道具信息
		uiroot.transform.Find("Hero").GetComponent<Bag>().DelItem (transform.Find("ID").GetComponent<UILabel>().text);
		//在背包ui中删除图标
		Destroy (uiroot.transform.Find ("beibaomianban/wupin/Grid/" + transform.Find ("ID").GetComponent<UILabel> ().text).gameObject);
		//卖出物品的收益获得
		uiroot.transform.Find ("Hero").GetComponent<Hero> ().MoneyChange ('+', int.Parse (gameObject.transform.Find ("jiage").GetComponent<UILabel> ().text) / 5);
		StartCoroutine ("wait");
	}
	IEnumerator wait(){
		yield return 0;
		//将详细信息面板关闭
		gameObject.SetActive (false);
		//将背包uigird重新排列
		uiroot.transform.Find ("beibaomianban/wupin/Grid").gameObject.GetComponent<UIGrid> ().Reposition ();
	}
	public void Use(){
		//获取当前操作的装备栏
		string zb = string.Empty;
		switch (gameObject.transform.Find ("Type").GetComponent<UILabel> ().text) {
		case "武器":
			zb = "weapons";
			break;
		case "衣服":
			zb = "clothes";
			break;
		case "帽子":
			zb = "hat";
			break;
		case "鞋子":
			zb = "shoes";
			break;
		case "消耗品":
			zb = "";
			break;
		default:
			Debug.Log("这个物品不能使用");
			break;
		}
		if (zb != "") {
			GameObject typephoto = uiroot.transform.Find ("beibaomianban/HeroBackGround/" + zb).gameObject;
			//1、删除背包文档中 2、在文档装备位置添加 3、在ui背包中删除（排列） 4、在ui装备栏中添加 5、每装备一次修改一次人物的属性
			//在背包ui中删除图标
			Destroy (uiroot.transform.Find ("beibaomianban/wupin/Grid/" + transform.Find ("ID").GetComponent<UILabel> ().text).gameObject);
			//将装备栏图标显示
			typephoto.GetComponent<UISprite> ().spriteName = transform.Find ("Photo").GetComponent<UISprite> ().spriteName;
			typephoto.GetComponent<UIButton>().normalSprite = transform.Find ("Photo").GetComponent<UISprite> ().spriteName;
			//装备栏中id存入相应的1
			GameObject IDlabel = uiroot.transform.Find ("beibaomianban/HeroBackGround/" + zb + "1").gameObject;
			IDlabel.GetComponent<UILabel> ().text = transform.Find ("ID").GetComponent<UILabel> ().text;

			uiroot.transform.Find ("Hero").GetComponent<Bag> ().ADDHeroitem (transform.Find ("ID").GetComponent<UILabel> ().text);
			StartCoroutine ("wait");
		}
	}
}
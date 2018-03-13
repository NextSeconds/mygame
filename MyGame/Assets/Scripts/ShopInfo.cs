using UnityEngine;
using System.Collections;

public class ShopInfo{
	public int ID;
	public string Photoname;
	public string Name;
	public int Price;
	public string type;//物品类型
	public string Shuxing;//物品加成属性
	public int zhanli;//物品装备后的战斗力
	public ShopInfo(){
	}
	public ShopInfo(int id,string pn,string n,int p,string tp,string sx,int zl){
		ID = id;
		Photoname = pn;
		Name = n;
		Price = p;
		type = tp;
		Shuxing = sx;
		zhanli = zl;
	}
}

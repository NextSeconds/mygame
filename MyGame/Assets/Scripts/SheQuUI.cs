using UnityEngine;
using System.Collections;

public class SheQuUI : MonoBehaviour {
    public GameObject yangjian;
    public GameObject change;

	public GameObject qiandaomianban;
	public GameObject shangchengmianban;
	public GameObject Bagmianban;
	public GameObject Fubenmianban;
	// Use this for initialization
    void Start()
    {
        if (PlayerPrefs.GetInt("heroShowIndex") == 0)
        {
            yangjian.SetActive(true);
        }
        else
        {
            change.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
	public void QianDaoClick(){
		qiandaomianban.SetActive (true);
	}
	public void QianDaoGuanBi(){
		qiandaomianban.SetActive (false);
	}
	public void ShangchengClick(){
		shangchengmianban.SetActive (true);
	}
	public void ShangchengGuanBi(){
		shangchengmianban.SetActive (false);
	}
	public void BagClick(){
		Bagmianban.SetActive (true);
	}
	public void BagGuanBi(){
		Bagmianban.SetActive (false);
	}
	public void FubenClick(){
		Fubenmianban.SetActive (true);
	}
	public void FubenGuanbi(){
		Fubenmianban.SetActive (false);
	}
}

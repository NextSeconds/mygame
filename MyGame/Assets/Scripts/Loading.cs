using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {
	public UIProgressBar bar;
	private AsyncOperation async;
	public UILabel label;
	// Use this for initialization
	void Start () {
		StartCoroutine ("Asy");
	}
	IEnumerator Asy(){
		async = Application.LoadLevelAsync ("SheQu");
		yield return async;
	}
	// Update is called once per frame
	void Update () {
		bar.value = async.progress * 1.0f * 0.9f;
		label.text = System.Convert.ToInt32(bar.value * 100) + "%";

//		Debug.Log (label.text);
	}
}

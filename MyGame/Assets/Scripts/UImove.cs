using UnityEngine;
using System.Collections;

public class UImove : MonoBehaviour {//挂在UI物体下可以拖动该UI物体
	public Camera WNGUICamera;  
	public Vector3 _WoldPosition;//指针的初始位置  
	// Vector3 _WoldAng;  
	public Vector3 WscreenSpace;  //该对象在屏幕中的坐标位置（点）
	public Vector3 Woffset;  
	public Vector3 WcurScreenSpace;  
	public Vector3 WcurPosition;  
//	void Update(){
//		if(Input.GetMouseButtonDown(0)){
//			StartCoroutine ("fnOnMouseDown");
//		}
//	}
	void OnPress(){//当按住该控件的时候会调用
		StartCoroutine ("fnOnMouseDown");
	}
	//工具跟随鼠标拖拽移动  
	IEnumerator fnOnMouseDown()  
	{  
		// 获取目标对象当前的世界坐标系位置，并将其转换为屏幕坐标系的点
		WscreenSpace = WNGUICamera.WorldToScreenPoint(transform.position);  
		// 设置鼠标的屏幕坐标向量，用上面获得的Pos的z轴数据作为鼠标的z轴数据，使鼠标坐标与目标对象坐标处于同一层面上
		Woffset = transform.position - WNGUICamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, WscreenSpace.z));  

		while (Input.GetMouseButton(0))  
		{  
//			DrawLineScript.isMoveLine = true;  
			if (Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width && Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height)  
			{  
				// 用上面获取到的鼠标坐标转换为世界坐标系的点，并用其设置目标对象的当前位置
				WcurScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, WscreenSpace.z);  
				WcurPosition = WNGUICamera.ScreenToWorldPoint(WcurScreenSpace) + Woffset;  
				transform.position = new Vector3(WcurPosition.x, WcurPosition.y, 0f);  
			}  
			// 等待下一帧对数据进行更新，实现目标对象的位移
			yield return new WaitForFixedUpdate();  
		}  
	}  
}

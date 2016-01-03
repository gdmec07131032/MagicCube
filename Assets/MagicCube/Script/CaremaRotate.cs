
/// <summary>
/// *** wangyel1 ***
/// 魔方整体的转动用摄像机的转动来模拟，这里用了一个中心体带动相机转动
/// </summary>

using UnityEngine;
using System.Collections;

public class CaremaRotate : MonoBehaviour {

	public Transform cameraTarget;
	public float horizontalSpeed = 2.0f; //水平滑动速度//
	public float verticalSpeed = 2.0f; //竖直滑动速度//
	public float scrollSpeed = 2.0f; //滚轮滚动速度//
	public float lerpSpeed = 5.0f; //插值速度//
	private float h = 0;
	private float v = 0;
	private float caremaDistance;
	
	void Start(){ //获取初始角度//
		Vector3 localRotation = transform.eulerAngles;
		v = localRotation.x;
		h = localRotation.y;	
	}
	
	void Update() {
		if(Input.GetMouseButton(1)){
			h += horizontalSpeed * Input.GetAxis("Mouse X");
			v -= verticalSpeed * Input.GetAxis("Mouse Y");
			v = Mathf.Clamp (v,-50,50);
		}
		Quaternion rotationTo = Quaternion.Euler(v, h, 0);
		transform.rotation = Quaternion.Lerp(transform.rotation,rotationTo,Time.deltaTime * lerpSpeed);
		
		caremaDistance += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed ;
		caremaDistance = Mathf.Clamp(caremaDistance,-4,1);
		cameraTarget.localPosition = Vector3.Lerp(cameraTarget.localPosition,new Vector3(0,0,caremaDistance - 3),Time.deltaTime * lerpSpeed);
	}
}

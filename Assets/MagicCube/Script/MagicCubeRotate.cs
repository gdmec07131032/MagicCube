
/// <summary>
/// ***  wangyel1  ***
/// 魔方旋转使用向量的点乘和叉乘来获取和控制。
/// 首先在鼠标点击魔方时，获取点击点的信息，包括点击的点和此处的法线；
/// 之后在鼠标拖拽后获取鼠标运行的方向，它叉乘法线得到垂直于二者的一条法线；
/// 之后用得到的法线点乘六个原始轴，取值为45度以内的轴为转动轴，则能得到所要转动的层和转动方向；
/// 最后这一层开始转动，之后进入下一轮
/// </summary>

using UnityEngine;
using System.Collections;

public class MagicCubeRotate : MonoBehaviour {
	
	public GameObject MagicCube;
	public float rotateSpeed = 2f;
	
	private GameObject cubeObject; //被点中的cube//
	private Vector3 normal; //点击点的法线//
	private Vector3 mouseStart; //鼠标点击的初始点//
	private Vector3 mouseRun; //鼠标拖动时的点//
	private Vector3 mouseCross; //叉乘结果//
	private Vector3 vecZhou; //魔方某层转动时的转动轴//
	private bool isRotate = false; //某层是否可转动//
	private bool isRun = false; //是否可拖动鼠标//
	private float t = 0; //转动时的递增值的临时变量//
	private Vector3[] vecPoint; //六个原始轴//
	private GameObject[] cubes; //保存所有标签为Cube物体的父物体//
	
	void Start () {
		vecPoint = new Vector3[]{Vector3.right,Vector3.up,Vector3.forward,Vector3.left,Vector3.down,Vector3.back};
		cubes = GameObject.FindGameObjectsWithTag("Cube");
		isRun = true;
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0) && isRun){ //按下鼠标记录相关内容//
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitStart;
			if(Physics.Raycast(ray.origin,ray.direction,out hitStart)){
				cubeObject = hitStart.transform.gameObject;
				normal = hitStart.normal;
				mouseStart = hitStart.point;
			}
		}
		
		if(Input.GetMouseButton(0) && cubeObject != null){ //拖动鼠标，记录拖动方向，叉乘得到法线方向//
			Ray rayRun = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitRun;
			if(Physics.Raycast(rayRun.origin,rayRun.direction,out hitRun)){
				mouseRun = hitRun.point - mouseStart;
				if(mouseRun.sqrMagnitude > 0.2f){
					mouseCross = Vector3.Cross(normal, mouseRun).normalized;
					RotatePoint(mouseCross);
					cubeObject = null;
				}
			}		
		}
		
		if(isRotate){ //转动相应的层//
			t += Time.deltaTime * rotateSpeed;
			Vector3 rotate = vecZhou * Mathf.Clamp01(t) * 90;
			transform.eulerAngles = rotate; 	
			if(t >= 1f){
				t = 0;
				foreach(GameObject cube in cubes){
					cube.transform.parent = MagicCube.transform;
					cube.transform.localScale = Vector3.one;
				}
				transform.rotation = Quaternion.identity;
				isRotate = false;
				isRun = true;
			}
		}
	}
	
	void RotatePoint(Vector3 cross){ //遍历六个轴找出要转动的轴//
		for(int i = 0; i< 6; i++){
			Vector3 vec = vecPoint[i];
			float dot = Vector3.Dot(vec,cross);
			if(dot > 0.72f && dot <= 1){
				vecZhou = vec;
				for(int j=0; j<3; j++){
					if(Mathf.Abs(vec[j]) == 1){
						FindFather(j);
					}
				}
			}
		}
	}
	
	void FindFather(int point){ //找出转动轴后将要转动的层添加到这个物体上，用这个物体的转动带动这一层//
		float cubeObjecePoint =cubeObject.transform.position[point];
		foreach(GameObject cube in cubes){
			float cubePoint = cube.transform.position[point];
			if(Mathf.Abs(cubePoint - cubeObjecePoint) < 0.1f){
				cube.transform.parent = gameObject.transform;
			}
		}
		isRun = false;
		isRotate = true;
	}	
}
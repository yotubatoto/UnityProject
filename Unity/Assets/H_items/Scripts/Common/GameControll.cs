using UnityEngine;
using System.Collections;

public class GameControll : MonoBehaviour 
{

	// EnemyServeの参照ができるようにする
	EnemyServe eneS;
	//
	GameObject uiCRef;
	bool onceFlag;
	private float waitTime = 0;
	// Use this for initialization
	void Start ()
	{
		onceFlag = false;
		eneS = GetComponent<EnemyServe>();
		uiCRef = GameObject.Find("Text");
		int layer1 = LayerMask.NameToLayer("Player");
		int layer2 = LayerMask.NameToLayer("PBullet");
		Physics.IgnoreLayerCollision(layer1, layer2);
		Physics.IgnoreLayerCollision(layer2, layer2);
		//Result.resultScore = 600;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Debug.Log(TitleController.FRAMERATE);
		// 仮でリザルト画面に遷移させている
		if (waitTime > 60+8)
		{
			Application.LoadLevel("Result");
		}
		//
		waitTime += Time.deltaTime;
		if (waitTime > 8 && onceFlag == false)
		{
			onceFlag = true;
			eneS.EnemyIni();
		}
		/*if (onceFlag == false)
		{
			onceFlag = true;
			UICountDownController ui = uiCRef.GetComponent<UICountDownController>();
			if (ui.Flag() == true)
			{
				eneS.EnemyIni();
			}
		}*/
		/*if (ui.Flag() && onceFlag == false)
		{
			onceFlag = true;
			eneS.EnemyIni();
		}*/
		/*if (ui.EnemyServerFlag && onceFlag == false)
		{
			onceFlag = true;
			eneS.EnemyIni();
		}*/
	}
}

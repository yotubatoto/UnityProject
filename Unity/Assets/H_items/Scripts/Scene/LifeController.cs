using UnityEngine;
using System.Collections;

public class LifeController : MonoBehaviour
{
	// 変数
	const int lifeMax = 20;
	private GameObject[] chidLife = new GameObject[lifeMax];
	public GameObject player;
	// 関数

	// Use this for initialization
	void Start () 
	{
		player.GetComponent<StateMachineSample.PlayerController>();
		int count = 1;
		for (int i = 0; i < lifeMax; i++)
		{
			chidLife[i] = transform.FindChild("LifeIcon" + count).gameObject;
			chidLife[i].SetActive(true);
			Debug.Log(chidLife[i]);
			count++;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void DesLife()
	{
		// 倒れたら体力回復
		if (player.GetComponent<StateMachineSample.PlayerController>().dest())
		{
			//Debug.Log("jajjjajaja");
			//GameObject.Find("LifeIcon1");
			for (int i = 0; i < lifeMax; i++)
			{
				chidLife[i].SetActive(true);
			}
		}
		// マックスのライフ　ー　現在のプレイヤーのライフ　＝　消す分のライフが出る　
		int nowLife = player.GetComponent<StateMachineSample.PlayerController>().LifeGet();
		int desLife = lifeMax - nowLife;

		for (int i = 0; i < desLife; i++)
		{
			chidLife[i].SetActive(false);
		}
	}
	void OnEnable()
	{
		Debug.Log("ここっっこっこ");
		/*
		if (player.GetComponent<StateMachineSample.PlayerController>().dest())
		{
			int count = 1;
			for (int i = 0; i < lifeMax; i++)
			{
				chidLife[i] = transform.FindChild("LifeIcon" + count).gameObject;
				chidLife[i].SetActive(true);
				Debug.Log(chidLife[i]);
				count++;
			}
		}*/

	}
}

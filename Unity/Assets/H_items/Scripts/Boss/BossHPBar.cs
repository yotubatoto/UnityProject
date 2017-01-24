using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
	private Slider slider;
	private float hp = 50;
	private float damege = 1;
	private GameObject refObj;
	// Use this for initialization
	void Start () 
	{
		slider = GameObject.Find("Slider").GetComponent<Slider>();
		slider.value = hp;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//hp = 0.01f;

		//Debug.Log(slider.value);
	}

	/// <summary>
	/// Bossへのダメージ
	/// </summary>
	/// <param name="player">If set to <c>true</c> player.</param>
	void BossDamege(bool player)
	{
		// trueの場合playerの弾が当たった
		if (player)
		{
			slider.value += damege;
		}
		else 
		{
			slider.value -= damege;
		}
	}

	void BossDie()
	{
		
	}

	/// <summary>
	/// 当たり判定
	/// </summary>
	/// <param name="collision">Collision.</param>
	private void OnCollisionEnter(Collision collision)
	{
		refObj = GameObject.FindWithTag("Enemy");
		StateMachineSample.Enemy a = refObj.GetComponent<StateMachineSample.Enemy>();
		int friend = a.EnemyFriend();
		Debug.Log("シャーーーーーー" + friend);

		if (collision.gameObject.tag == "PBullet")
		{
			BossDamege(true);
			//Debug.Log(a.name);
		}
		if (collision.gameObject.tag == "AIBullet")
		{
			BossDamege(false);
		}
		// 1でプレイヤーの仲間
		if (collision.gameObject.tag == "EBullet" && friend == 1)
		{
			//Debug.Log("ううううううううううう");
			BossDamege(true);
		}
		// 2で敵の仲間
		if (collision.gameObject.tag == "EBullet" && friend == 2)
		{
			//Debug.Log("かきくけ国家化かk");
			BossDamege(false);
		}
	}
}

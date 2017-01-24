using UnityEngine;
using System.Collections;

public class EnemyServe : MonoBehaviour {

	public GameObject enemy1;
	public GameObject enemy2;
	public GameObject enemy3;
	public GameObject boss;
	public Vector3[] pos;
	private int MAXENEMY = 10;
	private float INCREASE_TIME = 30;

	private float increaseTime=0;
	private bool flag = false;

	// Use this for initialization
	void Start () 
	{
		//EnemyIni();
		for (int i = 0; i < MAXENEMY+10; i++)
		{
			pos[i].y = 10;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		increaseTime += Time.deltaTime;
		if (increaseTime > INCREASE_TIME && flag == false)
		{
			flag = true;
			Increase();
		}
	}

	public void EnemyIni()
	{
		GameObject[] enemies = new GameObject[10];
		for (int i = 0; i < MAXENEMY; i++)
		{
			pos[i].y -= 1;
			if(i % 2 == 0 || i == 0 || i == 1){
				enemies[i] = Instantiate(enemy1);
			}
			else if (i %3 == 0)
			{
				enemies[i] = Instantiate(enemy2);
			}
			else
			{
				enemies[i] = Instantiate(enemy3);
			}
			enemies[i].GetComponent<Transform>().position = pos[i];
		}
	}
	/// <summary>
	/// １０秒後敵増加
	/// </summary>
	public void Increase()
	{
		GameObject[] enemies = new GameObject[20];
		for (int i = 0; i < 20; i++)
		{
			pos[i].y -= 1;
			if (i % 2 == 0 || i == 0 || i == 1)
			{
				enemies[i] = Instantiate(enemy1);
			}
			else if (i % 3 == 0)
			{
				enemies[i] = Instantiate(enemy2);
			}
			else
			{
				enemies[i] = Instantiate(enemy3);
			}
			enemies[i].GetComponent<Transform>().position = pos[i];
		}
	}
}

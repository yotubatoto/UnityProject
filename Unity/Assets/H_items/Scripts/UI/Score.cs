using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	private float figure = 0;
	private float AIfigure = 0;
	private float delayTime = 0;
	private bool onceFlag = false;
	private int reS = 0;
	private int reSAI = 0;
	private float ti;
	private GameObject refAIScore = null;
	private GameObject parent = null;
	private float waitTime = 0;
	private GameObject win = null;
	private GameObject loss = null;
	// Use this for initialization
	void Start () 
	{
		GetComponent<Text>().text = ((int)figure).ToString();
		// AI側のスコア
		refAIScore = GameObject.Find("AIScore");
		refAIScore.GetComponent<Text>().text = ((int)AIfigure).ToString();
		parent = transform.parent.parent.gameObject;
		Debug.Log(parent.name);
		//Result r = GetComponent<Result>();
		win = GameObject.Find("Win");
		loss = GameObject.Find("Loss");
		win.SetActive(false);
		loss.SetActive(false);
		ti = 0.0f;
		//gameObject.transform.localPosition += new Vector3(200, -250, 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		ti++;
		if (ti >= 400)
		{
			figure = reS;
			AIfigure = reSAI;
		}
		if (onceFlag != true)
		{
			onceFlag = true;
			reS = Result.Re();
			reSAI = Result.AIRe();
		}
		//Debug.Log(a);
		delayTime += Time.deltaTime;
		if (figure < reS)
		{
			if (delayTime > 1)
			{
				figure += 1;
			}
		}
		if (AIfigure < reSAI)
		{
			if (delayTime > 1)
			{
				AIfigure += 1;
			}
		}
		// 勝った場合
		if (reS > reSAI && figure == reS && AIfigure == reSAI)
		{
			waitTime += Time.deltaTime;
			if (waitTime > 2)
			{
				parent.SetActive(false);
			}
			win.SetActive(true);
		}
		// 負けた場合
		else if (reS < reSAI && figure == reS && AIfigure == reSAI)
		{
			waitTime += Time.deltaTime;
			if (waitTime > 2)
				parent.SetActive(false);
			loss.SetActive(true);
		}
		GetComponent<Text>().text = ((int)figure).ToString();
		refAIScore.GetComponent<Text>().text = ((int)AIfigure).ToString();
	}
}

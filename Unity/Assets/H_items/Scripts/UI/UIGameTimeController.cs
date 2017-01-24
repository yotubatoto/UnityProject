// スタートのUIのコントロール

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIGameTimeController : MonoBehaviour
{
	private float time = 60;
	public GameObject TimeText;
	private float waitTime = 0;
	public bool startGo = false;
	public bool onceFlag = false;
	////////GameObject refobj;
	// Use this for initialization
	void Start()
	{
		waitTime = 0;
		GetComponent<Text>().text = ((int)time).ToString();
	}

	// Update is called once per frame
	void Update()
	{
		//gameStartText.SetActive(false);
		waitTime += Time.deltaTime;
		if (waitTime > 8)
		{
			time -= Time.deltaTime;
			GetComponent<Text>().text = ((int)time).ToString();
		}
	}
}

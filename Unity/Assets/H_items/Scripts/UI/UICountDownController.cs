// スタートのUIのコントロール

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UICountDownController : MonoBehaviour {
	private float time = 4;
	public GameObject TimeText;
	public GameObject gameStartText;
	private float waitTime = 0;
	public bool startGo = false;
	public bool onceFlag = false;
	////////GameObject refobj;
	// Use this for initialization
	void Start () 
	{
		waitTime = 0;
		GetComponent<Text>().text = ((int)time).ToString();

		/////////refobj = GameObject.Find("GameStart");
	}
	
	// Update is called once per frame
	void Update ()
	{
		gameStartText.SetActive(false);
		waitTime += Time.deltaTime;
		if (waitTime > 3)
		{
			time -= Time.deltaTime;
			if (time < 1)
			{
				StartCoroutine("GameStart");
			}

			if (time < 1) time = 1;
			GetComponent<Text>().text = ((int)time).ToString();
		}
	}
	IEnumerator GameStart()
	{
		yield return new WaitForSeconds(2.0f);
		onceFlag = true;
		Destroy(TimeText);
		//if (waitTime > 7)
		//{
		//}
		//UIDestroyer d = GetComponent<UIDestroyer>();
		//d.
		//UIDestroyer d = gameStartText.GetComponent<UIDestroyer>();
		gameStartText.SetActive(true);
		UIDestroyer d = gameStartText.GetComponent<UIDestroyer>();
		d.Dest();

		////////UIDestroyer de = refobj.GetComponent<UIDestroyer>();
		/////////de.Dest();

		//gameObject.GetComponent<UICountDownController>().enabled = false;
	}

	public bool Flag()
	{
		if (onceFlag)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}

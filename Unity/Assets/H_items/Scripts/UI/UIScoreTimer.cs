using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UIScoreTimer : MonoBehaviour 
{
	private int score;
	// Use this for initialization
	void Start ()
	{
		GetComponent<Text>().text = (score).ToString();
	}
	
	// Update is called once per frame
	void Update () 
	{
		score = Result.UIRe();
		GetComponent<Text>().text = (score).ToString();
	}
}

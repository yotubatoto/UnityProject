using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleController : MonoBehaviour 
{
	public Text highScoreLabel;
	public AudioClip se;
	public static int FRAMERATE;
	public void Start ()
	{
		FRAMERATE = Application.targetFrameRate = 60;
		// ハイスコアを表示
		//highScoreLabel.text = "High Score : " + PlayerPrefs.GetInt("HighScore") + "m";
	}

	public void OnStartButtonClicked ()
	{
		GetComponent<AudioSource>().PlayOneShot(se);
		Application.LoadLevel("Main");
	}
}

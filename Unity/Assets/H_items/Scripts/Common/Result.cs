using UnityEngine;
using System.Collections;

public class Result : MonoBehaviour
{
	//public static int resultEnemy;
	// Player
	public static int resultScore = 0; // 仮で３００にしている
	public static float resultEnemyCrushing = 0; // 0
	public static float resultBossCrushing = 0; // ボスへのダメージ
	public static float resultPlusEnemy = 1; // 

	// AI
	public static int AIresultScore = 0; // 仮で３００にしている
	public static float AIresultEnemyCrushing = 0; // 0
	public static float AIresultBossCrushing = 0; // ボスへのダメージ
	public static float AIresultPlusEnemy = 1; // ボスへのダメージ

	 // Use this for initialization
	void Start () 
	{
		//resultEnemy = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log("倒した敵"+resultEnemy);
	}

	public static int Re()
	{
		if (resultPlusEnemy <= 0)
			resultPlusEnemy = 0;
		resultScore += ((int)resultEnemyCrushing + (int)resultBossCrushing) * (int)resultPlusEnemy;
		return resultScore;
	}

	public static int UIRe()
	{
		int result = 0;
		if (resultPlusEnemy <= 0)
			resultPlusEnemy = 0;
		result = ((int)resultEnemyCrushing + (int)resultBossCrushing);
		return result;
	}

	public static int AIRe()
	{
		if (AIresultPlusEnemy <= 0)
			AIresultPlusEnemy = 0;
		AIresultScore += ((int)AIresultEnemyCrushing + (int)AIresultBossCrushing) * (int)AIresultPlusEnemy;
		return AIresultScore;
	}

	public static void Clear()
	{
			// Player
	resultScore = 0; // 仮で３００にしている
	resultEnemyCrushing = 0; // 0
	resultBossCrushing = 0; // ボスへのダメージ
	resultPlusEnemy = 1; // 

	// AI
	AIresultScore = 0; // 仮で３００にしている
	AIresultEnemyCrushing = 0; // 0
	AIresultBossCrushing = 0; // ボスへのダメージ
	AIresultPlusEnemy = 1; // ボスへのダメージ
}
}

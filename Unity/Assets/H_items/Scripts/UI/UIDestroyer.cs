// UI(GameStart)の削除

using UnityEngine;
using System.Collections;

public class UIDestroyer : MonoBehaviour {

	public float delay = 1f;

	void Start()
	{
		//Destroy(gameObject, delay);
	}

	public void Dest()
	{
		Destroy(gameObject, delay);
	}
}

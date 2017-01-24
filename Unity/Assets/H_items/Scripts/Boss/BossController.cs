using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour {
	// Use this for initialization
	public float delay = 3f;
	private bool collisionFlag = false;

	void Start()
	{
	}

	public void Dest()
	{
		Destroy(gameObject);
	}
}


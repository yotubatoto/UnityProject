using UnityEngine;
using System.Collections;

public class BulletDelete : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(Dest());
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	// もし？秒経った場合は容赦なく削除
	IEnumerator Dest()
	{
		yield return new WaitForSeconds(3.0f);
		Destroy(gameObject);
	}
	// もし的か障害物に当たったら削除
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Enemy")
		//if (collision.gameObject.name == "Character") 
		{
			//Debug.Log("当たった");
			Destroy(gameObject);
		}
	}
}

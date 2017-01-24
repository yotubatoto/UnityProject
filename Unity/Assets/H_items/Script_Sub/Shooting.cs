using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {
	
	// 弾のプレファブ
	public GameObject bullet;

	// 弾の発射位置
	public Transform hand_former;

	// 弾の速度
	public float speed = 10;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		// 今のところはスペースキーが押された時
		if (Input.GetKeyDown(KeyCode.RightShift))
		{
			// 弾の複製
			GameObject bullets = Instantiate(bullet);
			Vector3 force;
			force = this.gameObject.transform.forward * speed;
			// Rigidbodyにforceを加えてドーーーン
			bullets.GetComponent<Rigidbody>().AddForce(force);
			// 弾の位置調整
			bullets.transform.position = hand_former.position;
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
namespace StateMachineSample
{
	public class PlayerController : MonoBehaviour
	{
		private CharacterController charaCon;       // キャラクターコンポーネント用の変数
		private Vector3 move = Vector3.zero;    // キャラ移動量.
		private float speed = 7.0f;         // 移動速度
		private float jumpPower = 7.0f;        // 跳躍力.
		private const float GRAVITY = 5.8f;         // 重力
		private float rotationSpeed = 8000.0f;   // プレイヤーの回転速度
		public GameObject bullet;
		public GameObject enemyBullet;
		private int bulletTime = 0;

		private int life = 0;
		private const int MAXLIFE = 20;
		// キャラのリスポーン地点
		private Vector3 RESPAWN = new Vector3(-30, 0, -30);
		// 弾の発射位置
		public Transform hand_former;
		// 
		Animator anime;
		public bool destFlag = false;
		// ライフShare
		private LifeController shareLife;
		private GameObject shareRef;
		// 音
		public AudioClip seShot;

		void Start()
		{
			charaCon = GetComponent<CharacterController>();
			anime = GetComponent<Animator>();
			// UI上のライフを減らす
			shareRef = GameObject.Find("LifePanel");
			shareLife = shareRef.GetComponent<LifeController>();
			life = MAXLIFE;

		}

		void Update()
		{
			float y = move.y;
			//スマホ用
			//move = new Vector3(CrossPlatformInputManager.GetAxisRaw("Horizontal"), 0.0f,
			// CrossPlatformInputManager.GetAxisRaw("Vertical"));
			// pc移動量の取得
			move = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));       // 左右上下のキー入力を取得し、移動量に代入.
			Vector3 playerDir = move;   // 移動方向を取得.
			move *= speed;              // 移動速度を乗算.

			Jumping();
			// プレイヤーの向き変更
			if (playerDir.magnitude > 0.1f)
			{
				anime.SetBool("is_Running", true);
				Quaternion q = Quaternion.LookRotation(playerDir);          // 向きたい方角をQuaternionn型に直す .
																			//transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);   // 向きを q に向けてじわ～っと変化させる.
				transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);   // 向きを q に向けてじわ～っと変化させる.

			}
			else
			{
				anime.SetBool("is_Running", false);
			}
			// 移動処理
			charaCon.Move(move * Time.deltaTime);   // プレイヤー移動.

			Shooting();

		}

		// ジャンプ
		private void Jumping()
		{
			float y = move.y;
			// 重力／ジャンプ処理
			move.y += y;
			if (charaCon.isGrounded)
			{                   // 地面に設置していたら
				if (Input.GetKeyDown(KeyCode.Space))
				{   // ジャンプ処理.
					move.y = jumpPower;
				}
			}
			move.y -= GRAVITY * Time.deltaTime; // 重力を代入.
		}

		// 弾発射
		private void Shooting()
		{
			// 今のところはスペースキーが押された時
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				GetComponent<AudioSource>().PlayOneShot(seShot);
				// 弾の複製
				/*GameObject bullets = Instantiate(bullet);
				Vector3 force;
				force = this.gameObject.transform.forward * speed;
				// Rigidbodyにforceを加えてドーーーン
				bullets.GetComponent<Rigidbody>().AddForce(force);
				// 弾の位置調整
				bullets.transform.position = hand_former.position;
				*/
				//Vector3 a = new Vector3(0, 1, 0);
				//Instantiate(bullet,transform.position+a,transform.rotation);
				GameObject newTarget1 = (GameObject)Instantiate(bullet, hand_former.transform.position, transform.rotation);
				newTarget1.name = bullet.name;
			}
		}

		public void IphoneShooting()
		{
			bulletTime += 1;
			if (bulletTime % 3 != 0)
			{
				GameObject newTarget1 = (GameObject)Instantiate(bullet, hand_former.transform.position, transform.rotation);
				newTarget1.name = bullet.name;

			}
		}

		//void OnControllerColliderHit(ControllerColliderHit hit)
		//{
		/*if (hit.gameObject.CompareTag("EBullet"))
		{
			Debug.Log("サッカーしようぜ");
		}
		if (hit.gameObject.CompareTag("AIBullet"))
		{
			Debug.Log("なんくる");
		}*/
		//if (hit.gameObject.CompareTag("EBullet"))
		//{
		//	Debug.Log("ここ");
		//}	
		//}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.tag == "EBullet")
			{
				if (life != 0)
				{
					--life;
					// UI上のライフを減らす
					shareLife.GetComponent<LifeController>().DesLife();
					Debug.Log(life);
				}
			}
			if (life <= 0)
			{
				life = MAXLIFE;
				// UI上のライフを減らす
				destFlag = true;
				shareLife.GetComponent<LifeController>().DesLife();
				//collision.gameObject.GetComponent<StateMachineSample.Enemy>().ColorWhite();
				//GameObject refObj;
				//refObj = GameObject.FindWithTag("Enemy");
				//refObj.GetComponent<StateMachineSample.Enemy>().ColorWhite();
				charaCon.transform.position = RESPAWN;
			}
			else
			{
				destFlag = false;
			}
		}
		/// <summary>
		/// 仲間解放
		/// </summary>
		public bool dest()
		{
			return destFlag;
		}

		public void setDest()
		{
			destFlag = false;
		}

		/// <summary>
		/// lifeを渡す
		/// </summary>
		public int LifeGet()
		{
			return life;
		}
	}
}
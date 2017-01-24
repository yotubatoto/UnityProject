using UnityEngine;
using System.Collections;

namespace StateMachineSample
{
	public enum EnemyState
	{
		Wander,
		Pursuit,
		Attack,
		Explode,
		BossWander,
		BossPursuit,
		BossAttack,
		BossExplode,
	}

	public class Enemy : StatefulObjectBase<Enemy, EnemyState>
	{
		public Transform turret;
		public Transform muzzle;
		public GameObject bulletPrefab;

		private Transform player;
		private Transform boss;
		private GameObject child;

		private int maxLife = 3;
		private int life;

		private float speed = 5f;
		private float rotationSmooth = 1f;
		private float turretRotationSmooth = 0.8f;
		private float attackInterval = 3f;
		// プレイヤーとの距離が大きければ、徘徊ステートに遷移 プレイヤーとの距離が小さければ、追跡ステートに遷移
		private float pursuitSqrDistance = 100.0f + 16.0f;
		private float attackSqrDistance = 50.0f + 16.0f; // （ここの距離）プレイヤーとの距離が小さいと攻撃ステートに移行
		private float margin = 16.0f;

		private float changeTargetSqrDistance = 13.0f;

		private float MapMaxSize = 28.0f;

		// 今誰の仲間なのか 1なら仲間　２なら敵
		private int destroyFlag = 0;
		// 今ボスアタック中の場合false
		private bool bossStateFlag = false;
		// プレイヤーのオブフェクト
		private GameObject wPlayer;
		//
		public bool playerRefFlag = false;
		private GameObject refObj;
		public int stateR = 0;
		// BossのHPバー
		private GameObject refHPBar;

		private void Start()
		{
			Initialize();
		}

		public void Initialize()
		{
			// 始めにプレイヤーの位置を取得できるようにする
			player = GameObject.FindWithTag("Player").transform;
			boss = GameObject.FindWithTag("Boss").transform;
			child = gameObject.transform.FindChild("up").gameObject;
			wPlayer = GameObject.Find("Player");
			life = maxLife;

			// ステートマシンの初期設定
			stateList.Add(new StateWander(this));
			stateList.Add(new StatePursuit(this));
			stateList.Add(new StateAttack(this));
			stateList.Add(new StateExplode(this));
			stateList.Add(new BossStateWander(this));
			stateList.Add(new BossStatePursuit(this));
			stateList.Add(new BossStateAttack(this));
			stateList.Add(new BossStateExplode(this));



			stateMachine = new StateMachine<Enemy>();

			ChangeState(EnemyState.Wander);
		}

		public void TakeDamage()
		{
			life--;
			if (life <= 0)
			{
				ChangeState(EnemyState.Explode);
			}
		}

		public int stateReturn()
		{
			return stateR;
		}

		/// <sammary>
		///  仲間を解放
		/// </sammary>
		public void WhiteClear()
		{
			Result.resultPlusEnemy = 1;
			refObj = GameObject.Find("Player");
			playerRefFlag = refObj.GetComponent<PlayerController>().dest();

			if (playerRefFlag && destroyFlag != 2)
			{
				//refObj.GetComponent<PlayerController>().setDest();
				//playerRefFlag = false;
				Debug.Log("課かかっかっかか閣下か");
				child.GetComponent<Renderer>().material.color = Color.white;
				//refObj.GetComponent<PlayerController>().setDest();
				ChangeState(EnemyState.Wander);
			}
		}

		/// <summary>
		/// pop位置ランダム
		/// </summary>
		/// <returns>The random position on level.</returns>
		public Vector3 GetRandomPositionOnLevel()
		{
			// マップの限界の目的地
			float levelSize = MapMaxSize;
			return new Vector3(Random.Range(-levelSize, levelSize), 0, Random.Range(-levelSize, levelSize));
		}
		#region States
		/// <summary>
		/// プレイヤー
		/// </summary>
		/// 
		/// <summary>
		/// ステート: 徘徊
		/// </summary>
		private class StateWander : State<Enemy>
		{
			private Vector3 targetPosition;

			public StateWander(Enemy owner) : base(owner) { }

			public override void Enter()
			{
				owner.destroyFlag = 0;
				// 始めの目標地点を設定する
				targetPosition = owner.GetRandomPositionOnLevel();
				owner.stateR = 1;
			}

			public override void Execute()
			{
				// プレイヤーとの距離が小さければ、追跡ステートに遷移
				float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
				if (sqrDistanceToPlayer < owner.pursuitSqrDistance - owner.margin)
				{
					owner.ChangeState(EnemyState.Pursuit);
				}

				// 目標地点との距離が小さければ、次のランダムな目標地点を設定する
				float sqrDistanceToTarget = Vector3.SqrMagnitude(owner.transform.position - targetPosition);
				if (sqrDistanceToTarget < owner.changeTargetSqrDistance)
				{
					targetPosition = owner.GetRandomPositionOnLevel();
				}

				// 目標地点の方向を向く
				Quaternion targetRotation = Quaternion.LookRotation(targetPosition - owner.transform.position);
				owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

				// 前方に進む
				owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);
			}

			public override void Exit() { }
		}

		/// <summary>
		/// ステート: 追跡
		/// </summary>
		private class StatePursuit : State<Enemy>
		{
			public StatePursuit(Enemy owner) : base(owner) { }

			public override void Enter() {
				owner.stateR = 2;
			}

			public override void Execute()
			{
				// プレイヤーとの距離が小さければ、攻撃ステートに遷移
				float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
				if (sqrDistanceToPlayer < owner.attackSqrDistance - owner.margin)
				{
					owner.ChangeState(EnemyState.Attack);
				}

				// プレイヤーとの距離が大きければ、徘徊ステートに遷移
				if (sqrDistanceToPlayer > owner.pursuitSqrDistance + owner.margin)
				{
					owner.ChangeState(EnemyState.Wander);
				}
				// プレイヤーの方向を向く
				Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.transform.position);
				owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

				// 前方に進む
				owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);
			}

			public override void Exit() {}
		}

		/// <summary>
		/// ステート: 攻撃
		/// </summary>
		private class StateAttack : State<Enemy>
		{
			private float lastAttackTime;

			public StateAttack(Enemy owner) : base(owner) { }

			public override void Enter() {
				owner.stateR = 3;
			}

			public override void Execute()
			{
				// プレイヤーとの距離が大きければ、追跡ステートに遷移
				float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
				if (sqrDistanceToPlayer > owner.attackSqrDistance + owner.margin)
				{
					owner.ChangeState(EnemyState.Pursuit);
				}

				// プレイヤーの方向を向く
				Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.transform.position);
				owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);
				// 砲台をプレイヤーの方向に向ける
				// Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.turret.position);
				//owner.turret.rotation = Quaternion.Slerp(owner.turret.rotation, targetRotation, Time.deltaTime * owner.turretRotationSmooth);

				// 一定間隔で弾丸を発射する
				if (Time.time > lastAttackTime + owner.attackInterval)
				{
					GameObject newTarget1 = (GameObject)Instantiate(owner.bulletPrefab, owner.turret.position, owner.turret.rotation);
					newTarget1.name = owner.bulletPrefab.name;
					lastAttackTime = Time.time;
				}
			}

			public override void Exit() { }
		}

		/// <summary>
		/// ステート: 爆発
		/// </summary>
		private class StateExplode : State<Enemy>
		{
			private bool oFlag = false; // 倒した数のカウントを一回だけに限定するため

			public StateExplode(Enemy owner) : base(owner) { }

			public override void Enter()
			{
				// ランダムな吹き飛ぶ力を加える
				//  Vector3 force = Vector3.up * 300f + Random.insideUnitSphere * 100f;
				//owner.GetComponent<Rigidbody>().AddForce(force);

				// ランダムに吹き飛ぶ回転力を加える
				//Vector3 torque = new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f));
				// owner.GetComponent<Rigidbody>().AddTorque(torque);
				// 1秒後に自身を消去する
				owner.stateR = 4;
				if (oFlag == false)
				{
					//Result.resultEnemy += 1;
				}
				oFlag = true;
				// 倒されたらプレイヤーの仲間としてボスに攻撃するためまず飛ぶ
				Vector3 fly = owner.GetRandomPositionOnLevel();
				fly += new Vector3(0, 10, 0);
				owner.transform.position += fly;
				owner.life += owner.maxLife;

				//owner.ChangeState(EnemyState.Fall);
				//Destroy(owner.gameObject);
			}

			private void Over()
			{
				if (Result.resultPlusEnemy < 1)
					Result.resultPlusEnemy = 1;
				if (Result.AIresultPlusEnemy < 1)
					Result.AIresultPlusEnemy = 1;
			}

			public override void Execute()
			{
				owner.transform.position -= new Vector3(0, 0.05f, 0);
				//Debug.Log(owner.transform.position);
				if (owner.transform.position.y <= 0.0f)
				{
					/*owner.transform.position = new Vector3(owner.transform.position.y, 0.0f,
															owner.transform.position.z);*/
					if (owner.destroyFlag == 1)
					{
						// 敵を倒した数Player 足し算
						Result.resultEnemyCrushing += 10.0f;
						owner.child.GetComponent<Renderer>().material.color = Color.blue;
					}
					else if (owner.destroyFlag == 2)
					{
						// 敵を倒した数AI
						Result.AIresultEnemyCrushing += 10.0f;
						// 最終的な仲間の数Player -
						owner.child.GetComponent<Renderer>().material.color = Color.red;
					}
					owner.ChangeState(EnemyState.BossWander);
				}
			}

			public override void Exit()
			{
				// 敵を倒した数*0.5
				//Result.resultEnemyCrushing += 1.0f;
			}
		}

		#endregion


		////////////////////////////////////////////////////////////////////////////////



		/// <summary>
		/// BOSS
		/// </summary>
		/// 
		/// <summary>
		/// ステート: 徘徊
		/// </summary>
		private class BossStateWander : State<Enemy>
		{
			private Vector3 targetPosition;

			public BossStateWander(Enemy owner) : base(owner) { }

			public override void Enter()
			{
				// 始めの目標地点を設定する
				targetPosition = owner.GetRandomPositionOnLevel();
			}

			public override void Execute()
			{
				owner.WhiteClear();
				// プレイヤーとの距離が小さければ、追跡ステートに遷移
				float sqrDistanceToBoss = Vector3.SqrMagnitude(owner.transform.position - owner.boss.position);
				if (sqrDistanceToBoss < owner.pursuitSqrDistance - owner.margin)
				{
					owner.ChangeState(EnemyState.BossPursuit);
				}

				// 目標地点との距離が小さければ、次のランダムな目標地点を設定する
				float sqrDistanceToTarget = Vector3.SqrMagnitude(owner.transform.position - targetPosition);
				if (sqrDistanceToTarget < owner.changeTargetSqrDistance)
				{
					targetPosition = owner.GetRandomPositionOnLevel();
				}

				// 目標地点の方向を向く
				Quaternion targetRotation = Quaternion.LookRotation(targetPosition - owner.transform.position);
				owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

				// 前方に進む
				owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);
			}

			public override void Exit() { }
		}

		/// <summary>
		/// ステート: 追跡
		/// </summary>
		private class BossStatePursuit : State<Enemy>
		{
			public BossStatePursuit(Enemy owner) : base(owner) { }

			public override void Enter() { }

			public override void Execute()
			{
				owner.WhiteClear();

				// プレイヤーとの距離が小さければ、攻撃ステートに遷移
				float sqrDistanceToBoss = Vector3.SqrMagnitude(owner.transform.position - owner.boss.position);
				if (sqrDistanceToBoss < owner.attackSqrDistance - owner.margin)
				{
					owner.ChangeState(EnemyState.BossAttack);
				}

				// プレイヤーとの距離が大きければ、徘徊ステートに遷移
				if (sqrDistanceToBoss > owner.pursuitSqrDistance + owner.margin)
				{
					owner.ChangeState(EnemyState.BossWander);
				}

				// プレイヤーの方向を向く
				Quaternion targetRotation = Quaternion.LookRotation(owner.boss.position - owner.transform.position);
				owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

				// 前方に進む
				owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);
			}

			public override void Exit() { }
		}

		/// <summary>
		/// ステート: 攻撃
		/// </summary>
		private class BossStateAttack : State<Enemy>
		{
			private float lastAttackTime;

			public BossStateAttack(Enemy owner) : base(owner) { }

			public override void Enter() { }

			public override void Execute()
			{
				owner.WhiteClear();

				// プレイヤーとの距離が大きければ、追跡ステートに遷移
				float sqrDistanceToBoss = Vector3.SqrMagnitude(owner.transform.position - owner.boss.position);
				if (sqrDistanceToBoss > owner.attackSqrDistance + owner.margin)
				{
					owner.ChangeState(EnemyState.BossPursuit);
				}
				// プレイヤーの方向を向く
				Quaternion targetRotation = Quaternion.LookRotation(owner.boss.position - owner.transform.position);
				owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);
				// 砲台をプレイヤーの方向に向ける
				//Quaternion targetRotation = Quaternion.LookRotation(owner.boss.position - owner.turret.position);
				//owner.turret.rotation = Quaternion.Slerp(owner.turret.rotation, targetRotation, Time.deltaTime * owner.turretRotationSmooth);

				// 一定間隔で弾丸を発射する
				if (Time.time > lastAttackTime + owner.attackInterval)
				{
					Instantiate(owner.bulletPrefab, owner.turret.position, owner.turret.rotation);
					lastAttackTime = Time.time;
				}
			}

			public override void Exit() { }
		}

		/// <summary>
		/// ステート: 爆発
		/// </summary>
		private class BossStateExplode : State<Enemy>
		{
			private bool oFlag = false; // 倒した数のカウントを一回だけに限定するため

			public BossStateExplode(Enemy owner) : base(owner) { }

			public override void Enter()
			{
				// ランダムな吹き飛ぶ力を加える
				//  Vector3 force = Vector3.up * 300f + Random.insideUnitSphere * 100f;
				//owner.GetComponent<Rigidbody>().AddForce(force);

				// ランダムに吹き飛ぶ回転力を加える
				//Vector3 torque = new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f));
				// owner.GetComponent<Rigidbody>().AddTorque(torque);
				// 1秒後に自身を消去する
				if (oFlag == false)
				{
//					Result.resultEnemy += 1;
				}
				oFlag = true;
				//Destroy(owner.gameObject);
			}

			public override void Execute() { }

			public override void Exit() { }
		}
		// 判定
		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.tag == "PBullet")
			{
				destroyFlag = 1;
			}
			if (collision.gameObject.tag == "AIBullet")
			{
				destroyFlag = 2;
			}
		}

		/// <summary>
		/// 自身が誰の仲間からを返す
		/// </summary>
		/// <returns>The friend.</returns>
		public int EnemyFriend()
		{
			return destroyFlag;
		}

	}
}

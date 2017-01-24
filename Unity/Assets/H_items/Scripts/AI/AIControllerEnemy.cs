using UnityEngine;
using System.Collections;

namespace StateMachineSample
{
	public enum EnemyDown
	{
		Wander,
		Pursuit,
		Attack,
		Explode,
		EnemyAttack
	}

	public class AIControllerEnemy : StatefulObjectBase<AIControllerEnemy, EnemyDown>
	{
		public Transform turret;
		public Transform muzzle;
		public GameObject bulletPrefab;

		private Transform player;
		private Transform boss;
		//private Transform enemy;
		private GameObject enemy;

		private int maxLife = 10;
		private int life;

		// aiの速度
		private float speed = 8f;
		private float rotationSmooth = 1f;
		private float turretRotationSmooth = 0.8f;
		private float attackInterval = 0.5f;
		// プレイヤーとの距離が大きければ、徘徊ステートに遷移 プレイヤーとの距離が小さければ、追跡ステートに遷移
		private float pursuitSqrDistance = 200.0f + 10.0f;
		private float attackSqrDistance = 20.0f + 10.0f; // （ここの距離）プレイヤーとの距離が小さいと攻撃ステートに移行
		private float margin = 10.0f;

		private float changeTargetSqrDistance = 5.0f;

		private float waittime = 0;
		private float MapMaxSize = 28.0f;
		private bool explore = false;
		private GameObject targetObj;
		private void Start()
		{
			Initialize();
		}

		public void Initialize()
		{
			// 始めにプレイヤーの位置を取得できるようにする
			player = GameObject.FindWithTag("Player").transform;
			boss = GameObject.FindWithTag("Boss").transform;
			//enemy = GameObject.FindWithTag("Enemy").transform;
			//enemy = GameObject.FindWithTag("Enemy").transform;
			life = maxLife;

			// ステートマシンの初期設定
			stateList.Add(new StateWander(this));
			stateList.Add(new StatePursuit(this));
			stateList.Add(new StateAttack(this));
			stateList.Add(new StateExplode(this));


			stateMachine = new StateMachine<AIControllerEnemy>();

			ChangeState(EnemyDown.Wander);
		}

		public void TakeDamage()
		{
			life--;
			if (life <= 0)
			{
				ChangeState(EnemyDown.Explode);
			}
		}
		/// <summary>
		/// タグ検索
		/// </summary>
		///   //指定されたタグの中で最も近いものを取得
		GameObject serchTag(GameObject nowObj, string tagName)
		{
			float tmpDis = 0;           //距離用一時変数
			float nearDis = 0;          //最も近いオブジェクトの距離
										//string nearObjName = "";    //オブジェクト名称
			targetObj = null; //オブジェクト

			//タグ指定されたオブジェクトを配列で取得する
			foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
			{
				//自身と取得したオブジェクトの距離を取得
				tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

				//オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
				//一時変数に距離を格納
				if (nearDis == 0 || nearDis > tmpDis)
				{
					nearDis = tmpDis;
					//nearObjName = obs.name;
					targetObj = obs;
				}

			}
			//最も近かったオブジェクトを返す
			//return GameObject.Find(nearObjName);
			return targetObj;
		}

		#region States
		/// <summary>
		/// プレイヤー
		/// </summary>
		/// 
		/// <summary>
		/// ステート: 徘徊
		/// </summary>
		private class StateWander : State<AIControllerEnemy>
		{
			private Vector3 targetPosition;
			private bool onceFlag = false;

			public StateWander(AIControllerEnemy owner) : base(owner) { }

			public override void Enter()
			{
				owner.explore = false;
				// 始めの目標地点を設定する
				targetPosition = GetRandomPositionOnLevel();
			}

			public override void Execute()
			{
				owner.waittime += Time.deltaTime;
				// 10秒後敵を見つける
				if (owner.waittime > 10)
				{
					onceFlag = true;
					if (owner.explore == false)
					{
						owner.explore = true;
						//owner.enemy = GameObject.FindWithTag("Enemy").transform;
						owner.enemy = owner.serchTag(owner.gameObject, "Enemy");
					}
					// プレイヤーとの距離が小さければ、追跡ステートに遷移
					float sqrDistanceToEnemy = Vector3.SqrMagnitude(owner.transform.position
					                                                - owner.enemy.transform.position);
					if (sqrDistanceToEnemy < owner.pursuitSqrDistance - owner.margin)
					{
						owner.ChangeState(EnemyDown.Pursuit);
					}

					// 目標地点との距離が小さければ、次のランダムな目標地点を設定する
					float sqrDistanceToTarget = Vector3.SqrMagnitude(owner.transform.position - targetPosition);
					if (sqrDistanceToTarget < owner.changeTargetSqrDistance)
					{
						targetPosition = GetRandomPositionOnLevel();
					}

					// 目標地点の方向を向く
					Quaternion targetRotation = Quaternion.LookRotation(targetPosition - owner.transform.position);
					owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

					// 前方に進む
					owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);
				}
			}

			public override void Exit() { }

			public Vector3 GetRandomPositionOnLevel()
			{
				// マップの限界の目的地
				float levelSize = owner.MapMaxSize;
				return new Vector3(Random.Range(-levelSize, levelSize), 0, Random.Range(-levelSize, levelSize));
			}
		}

		/// <summary>
		/// ステート: 追跡
		/// </summary>
		private class StatePursuit : State<AIControllerEnemy>
		{
			public StatePursuit(AIControllerEnemy owner) : base(owner) { }

			public override void Enter() { }

			public override void Execute()
			{
				// 敵との距離が小さければ、攻撃ステートに遷移
				float sqrDistanceToEnemy = Vector3.SqrMagnitude(owner.transform.position - owner.enemy.transform.position);
				if (sqrDistanceToEnemy < owner.attackSqrDistance - owner.margin)
				{
					/*
					// 10秒後エネミーとの距離の比較をする
					if (owner.waittime > 10)
					{
						// もし敵との距離がプレイヤーとの距離を比較して敵との距離のほうが近い場合StateEnemyAttackへ遷移
						float sqrDistanceToEnemy = Vector3.SqrMagnitude(owner.transform.position - owner.enemy.position);
						if (sqrDistanceToEnemy < sqrDistanceToPlayer)
						{
							owner.ChangeState(EnemyDown.EnemyAttack);
						}
					}
					*/
					owner.ChangeState(EnemyDown.Attack);
				}

				// プレイヤーとの距離が大きければ、徘徊ステートに遷移
				if (sqrDistanceToEnemy > owner.pursuitSqrDistance + owner.margin)
				{
					owner.ChangeState(EnemyDown.Wander);
				}

				// プレイヤーの方向を向く
				Quaternion targetRotation = Quaternion.LookRotation(owner.enemy.transform.position - owner.transform.position);
				owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

				// 前方に進む
				owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);
			}

			public override void Exit() { }
		}

		/// <summary>
		/// ステート: 攻撃
		/// </summary>
		private class StateAttack : State<AIControllerEnemy>
		{
			private float lastAttackTime;

			public StateAttack(AIControllerEnemy owner) : base(owner) { }

			public override void Enter() { }

			public override void Execute()
			{
				// プレイヤーとの距離が大きければ、追跡ステートに遷移
				float sqrDistanceToEnemy = Vector3.SqrMagnitude(owner.transform.position - owner.enemy.transform.position);
				if (sqrDistanceToEnemy > owner.attackSqrDistance + owner.margin)
				{
					if(owner.enemy.GetComponent<Enemy>().stateReturn() == 4)
					//if (owner.enemy.transform.position.y >= 9)
					{
						//Debug.Log("やほおー");
						owner.explore = false;
						owner.ChangeState(EnemyDown.Wander);
					}
					//owner.ChangeState(EnemyDown.Pursuit);
				}
				// プレイヤーの方向を向く
				Quaternion targetRotation = Quaternion.LookRotation(owner.enemy.transform.position - owner.transform.position);
				owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth + 3);
				// 砲台をプレイヤーの方向に向ける
				//Quaternion targetRotation = Quaternion.LookRotation(owner.enemy.position - owner.turret.position);
				//owner.turret.rotation = Quaternion.Slerp(owner.turret.rotation, targetRotation, Time.deltaTime * owner.turretRotationSmooth);
				// 前方に進む
				//owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);

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
		private class StateExplode : State<AIControllerEnemy>
		{
			private bool oFlag = false; // 倒した数のカウントを一回だけに限定するため

			public StateExplode(AIControllerEnemy owner) : base(owner) { }

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
					//Result.resultEnemy += 1;
				}
				oFlag = true;
				//Destroy(owner.gameObject);
			}

			public override void Execute()
			{
			}

			public override void Exit() { }
		}
		#endregion
	}
}

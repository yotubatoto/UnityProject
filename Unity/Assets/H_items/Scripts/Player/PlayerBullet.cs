using UnityEngine;
using System.Collections;

namespace StateMachineSample
{
	public class PlayerBullet : MonoBehaviour
	{
		public ParticleSystem explosionPrefab;
		public GameObject refboss;
		private float speed = 10f;
		private float force = 5f;
		private void Start()
		{
			Rigidbody rigid = GetComponent<Rigidbody>();
			rigid.useGravity = false;
			GetComponent<Rigidbody>().velocity = transform.forward * speed;
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.tag != "Stage")
			{
				Vector3 contactPoint = collision.contacts[0].point;
				Instantiate(explosionPrefab, contactPoint, Quaternion.identity);
			}

			if (collision.rigidbody != null)
			{
				if (collision.gameObject.tag != "Stage")
				{
					collision.rigidbody.AddForce(transform.forward * force);
				}
			}
			if (collision.gameObject.tag == "Enemy")
			{
				collision.gameObject.GetComponent<Enemy>().TakeDamage();
			}
		/*	if (collision.gameObject.tag == "Boss")
			{
				Debug.Log(Result.resultEnemyCrushing);
				//Result.resultEnemyCrushing += 0.5f;
				Result.resultEnemyCrushing += 10000f;
				BossController boss = refboss.GetComponent<BossController>();
				if (Result.resultEnemyCrushing > 10000)
				{
					boss.Dest();
				}

			}*/
			if (collision.gameObject.tag != "Boss")
			{
				//Result.resultBossCrushing += 1;
			}
			if (collision.gameObject.tag != "Stage")
			{
				Destroy(gameObject);
			}
		}


	}
}

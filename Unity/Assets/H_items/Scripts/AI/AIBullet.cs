using UnityEngine;
using System.Collections;

namespace StateMachineSample
{
	public class AIBullet : MonoBehaviour
	{
		public ParticleSystem explosionPrefab;
		// 弾の速度
		private float speed = 24f;
		private float force = 1f;

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
			if (collision.gameObject.tag != "Stage")
			{
				Destroy(gameObject);
			}
		}
	}
}

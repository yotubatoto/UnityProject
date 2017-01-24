using UnityEngine;
using System.Collections;

namespace StateMachineSample
{
    public class Bullet : MonoBehaviour
    {
        public ParticleSystem explosionPrefab;

        private float speed = 8f;
        private float force = 8f;

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
           /* if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage();
            }*/
			if (collision.gameObject.tag != "Stage")
			{
				Destroy(gameObject);
			}
			if (collision.gameObject.tag != "Boss")
			{
				//Result.resultPlusEnemy += 10.0f;
			}
        }
    }
}

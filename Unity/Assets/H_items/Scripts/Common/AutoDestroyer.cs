using UnityEngine;
using System.Collections;

namespace StateMachineSample
{
    public class AutoDestroyer : MonoBehaviour
    {
        public float delay = 3f;

        void Start()
        {
            Destroy(gameObject, delay);
        }

		void Dest()
		{
			
		}
    }
}

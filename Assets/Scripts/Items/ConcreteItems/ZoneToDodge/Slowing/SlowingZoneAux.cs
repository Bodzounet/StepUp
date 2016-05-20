using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Items
{
    public class SlowingZoneAux : MonoBehaviour
    {
        public float timeToDodge = 1f;

        private float radius;

        void Awake()
        {
            radius = this.GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
        }

        void Start()
        {
            StartCoroutine(Co_EndCountDown());
        }

        private IEnumerator Co_EndCountDown()
        {
            yield return new WaitForSeconds(timeToDodge);

            foreach (var v in Physics2D.OverlapCircleAll(transform.position, radius, 1 << LayerMask.NameToLayer("Player")))
            {
                var slowed = v.GetComponent<Slowed>();
                if (slowed != null)
                {
                    slowed.ResetSpeed();
                }
                else
                {
                    v.gameObject.AddComponent<Slowed>();
                }
            }

            Destroy(this.gameObject);
        }
    }   
}
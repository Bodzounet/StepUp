using UnityEngine;
using System.Collections;
using System.Linq;

namespace Items
{
    public class Missile : BaseItem
    {
        Transform target;

        public float maxRotationAngle = 1f;
        public float missileSpeed = 0.01f;

        public float stunDurationWhenHit = 2f;

        public override void DoAction()
        {
            GameObject go = GameObject.FindGameObjectsWithTag("Player").Where(x => x != User).OrderByDescending(y => y.transform.position.y).FirstOrDefault();

            if (go == null)
            {
                // TODO : throw a useless missile
                return;
            }

            target = go.transform;
            target.rotation.SetLookRotation(target.position - transform.position);
            StartCoroutine("Co_MoveTowardTarget");
        }

        private IEnumerator Co_MoveTowardTarget()
        {
            while (true)
            {
                //transform.position += (target.position - transform.position).normalized * missileSpeed;

                var dir = target.position - transform.position;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);



                yield return new WaitForEndOfFrame();
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer != LayerMask.NameToLayer("Player"))
                return;

            Controller playerController = col.GetComponent<Controller>();
            playerController.Stun(stunDurationWhenHit);

            Destroy(this.gameObject);
        }
    }
}
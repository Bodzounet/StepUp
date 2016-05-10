using UnityEngine;
using System.Collections;
using System.Linq;

namespace Items
{
    public class Hook : BaseItem
    {
        public override void DoAction()
        {
            StartCoroutine("Co_PullTarget");
        }

        private IEnumerator Co_PullTarget()
        {
            var targets = GameObject.FindGameObjectsWithTag("Player").Where(x => x.transform.position.y > transform.position.y);

            float maxDistance = 0;
            GameObject target = null;

            foreach(var v in targets)
            {
                float challengingMaxDistance = Vector3.Distance(v.transform.position, transform.position);
                if (challengingMaxDistance > maxDistance)
                {
                    maxDistance = challengingMaxDistance;
                    target = v;
                }
            }

            if (target == null)
                yield break;

            
        }
    }
}
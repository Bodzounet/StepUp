using UnityEngine;
using System.Collections;

namespace Items
{
    public class Shield : BaseItem
    {
        public float shieldDuration = 5f;

        public override void DoAction()
        {
            transform.parent = User.transform;
            transform.localPosition = Vector3.zero;

            var userController = User.GetComponent<Controller>();

            userController.InvulnerabilityDuration = shieldDuration;
            userController.OnEndBeingInvulnerable += OnInvulnerabilityEnds;
        }

        protected void OnInvulnerabilityEnds()
        {
            User.GetComponent<Controller>().OnEndBeingInvulnerable -= OnInvulnerabilityEnds;
            Destroy(this.gameObject);
        }
    }
}
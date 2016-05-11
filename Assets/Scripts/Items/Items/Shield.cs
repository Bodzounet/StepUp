using UnityEngine;
using System.Collections;

namespace Items
{
    public class Shield : BaseItem
    {
        public float shieldDuration = 5f;

        public override void DoAction()
        {
            var previousShield = User.GetComponentInChildren<Shield>();
            if (previousShield != null)
            {
                Destroy(previousShield.gameObject);
            }

            transform.parent = User.transform;
            transform.localPosition = new Vector3(-0.003f, 0.297f, 0f);
            transform.localScale *= transform.parent.localScale.x;

            var userController = User.GetComponent<Controller>();

            userController.InvulnerabilityDuration = shieldDuration;
            userController.OnEndBeingInvulnerable += OnInvulnerabilityEnds;
        }

        protected void OnInvulnerabilityEnds()
        {
            User.GetComponent<Controller>().OnEndBeingInvulnerable -= OnInvulnerabilityEnds;
            Destroy(this.gameObject);
        }

        protected void OnDestroy()
        {
            try
            {
                User.GetComponent<Controller>().OnEndBeingInvulnerable -= OnInvulnerabilityEnds;
            }
            catch
            {
                // osef, quitting
            }
        }
    }
}
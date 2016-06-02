using UnityEngine;
using System.Collections;

namespace Items
{
    public class MoreJump : BaseItem
    {
        public float duration = 10f;

        Controller _controller;


        public override void DoAction()
        {
            _controller = User.GetComponent<Controller>();
            User.GetComponent<DeathManager>().OnRespawn += CB_OnRespawn;
            StartCoroutine(Co_MoreJump());
        }

        private IEnumerator Co_MoreJump()
        {
            _controller.MaxJumpCharges += 1;
            yield return new WaitForSeconds(duration);
            _controller.MaxJumpCharges -= 1;
            _Clean();
        }

        private void CB_OnRespawn()
        {
            StopAllCoroutines();
            if (_controller.MaxJumpCharges != 2)
            {
                _controller.MaxJumpCharges = 2;
            }
            _Clean();
        }

        private void _Clean()
        {
            User.GetComponent<DeathManager>().OnRespawn -= CB_OnRespawn;
            Destroy(this.gameObject);
        }
    }
}
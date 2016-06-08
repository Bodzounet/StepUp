using UnityEngine;
using System.Collections;

namespace Items
{
    public class IBelieveICanFly : BaseItem
    {
        public float duration = 7;

        private Controller _controller;
        private Rigidbody2D _rgbd;
        private float _gravitiyScale;

        private bool _runs = false;

        public override void DoAction()
        {
            _controller = User.GetComponent<Controller>();
            _rgbd = User.GetComponent<Rigidbody2D>();
            _gravitiyScale = _rgbd.gravityScale;

            User.GetComponent<DeathManager>().OnRespawn += CB_OnRespawn;

            _runs = true;

            StartCoroutine(Co_Fly());
        }

        void Update()
        {
            if (_runs && _controller.Jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.A))
            {
                _rgbd.velocity = new Vector2(_rgbd.velocity.x, _controller.LateralSpeed);
            }
        }

        private IEnumerator Co_Fly()
        {
            _rgbd.gravityScale = 0;
            _controller.MaxJumpCharges = 0;
            _rgbd.velocity = new Vector2(_rgbd.velocity.x, _controller.LateralSpeed);
            yield return new WaitForSeconds(duration);
            _Clean();
        }

        private void CB_OnRespawn()
        {
            StopAllCoroutines();
            _Clean();
        }

        private void _Clean()
        {
            _rgbd.gravityScale = _gravitiyScale;
            _controller.MaxJumpCharges = 2;
            User.GetComponent<DeathManager>().OnRespawn -= CB_OnRespawn;
            Destroy(this.gameObject);
        }
    }
}
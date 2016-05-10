using UnityEngine;
using System.Collections;

namespace Items
{
    public class PlayerOnSlowingArea : MonoBehaviour
    {
        bool _affected = false;

        Controller _controller;

        void Awake()
        {
            _controller = this.GetComponent<Controller>();
            _controller.OnStartBeingInvulnerable += OnInvulnerabilityStarts;
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            StartSlowing(col);
        }

        void OnTriggerStay2D(Collider2D col)
        {
            if (!_affected)
            {
                StartSlowing(col);
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.tag != "SlowingArea")
                return;

            if (_affected)
            {
                EndSlowing();
            }
        }

        private void OnInvulnerabilityStarts()
        {
            if (_affected)
            {
                EndSlowing();
            }
        }

        private void OnInvulnerabilityEnds()
        {

        }

        private void StartSlowing(Collider2D col)
        {
            if (_controller.Invulnerable || col.tag != "SlowingArea" || col.GetComponent<Items.BaseItem>().User == this.gameObject)
                return;

            _affected = true;
            _controller.LateralSpeed /= SlowingArea.decreasingSpeedFactor;
        }

        private void EndSlowing()
        {
            _affected = false;
            _controller.LateralSpeed *= SlowingArea.decreasingSpeedFactor;
        }
    }
}
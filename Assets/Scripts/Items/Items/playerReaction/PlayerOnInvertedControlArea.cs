using UnityEngine;
using System.Collections;

namespace Items
{
    public class PlayerOnInvertedControlArea : MonoBehaviour
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
            if (_controller.Invulnerable || col.tag != "InvertedControlsArea" || col.GetComponent<Items.BaseItem>().User == this.gameObject)
                return;

            _affected = true;
            _controller.LateralSpeed *= -1;
        }

        void OnTriggerStay2D(Collider2D col)
        {
            if (!_affected)
            {
                if (_controller.Invulnerable || col.tag != "InvertedControlsArea" || col.GetComponent<Items.BaseItem>().User == this.gameObject)
                    return;

                _affected = true;
                _controller.LateralSpeed *= -1;
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.tag != "InvertedControlsArea")
                return;

            if (_affected)
            {
                Invoke("EndInvertedCOntrols", 1);
            }
        }

        private void OnInvulnerabilityStarts()
        {
            if (_affected)
            {
                EndInvertedControls();
            }
        }

        private void OnInvulnerabilityEnds()
        {

        }

        private void EndInvertedControls()
        {
            _affected = false;
            _controller.LateralSpeed *= -1;
        }
    }
}
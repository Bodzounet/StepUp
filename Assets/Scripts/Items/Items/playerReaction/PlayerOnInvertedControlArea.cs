using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Items
{
    public class PlayerOnInvertedControlArea : MonoBehaviour
    {

        bool _affected = false;
        Controller _controller;

        int _multizone = 0;

        void Awake()
        {
            _controller = this.GetComponent<Controller>();
            _controller.OnStartBeingInvulnerable += OnInvulnerabilityStarts;
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "InvertedControlsAreaStart")
            {
                _multizone++;
            }
        }

        void OnTriggerStay2D(Collider2D col)
        {
            if (!_affected)
            {
                if (_controller.Invulnerable || col.tag != "InvertedControlsAreaStart" || col.GetComponentInParent<Items.BaseItem>().User == this.gameObject)
                    return;

                SwitchInvertedControls();
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            OnTriggerExit2DLogic(col.gameObject);
        }

        public void OnTriggerExit2DLogic(GameObject go)
        {
            if (go.tag == "InvertedControlsAreaStart")
            {
                _multizone--;
            }

            if (go.tag != "InvertedControlsAreaEnd" || _multizone > 0)
                return;

            if (_affected)
            {
                SwitchInvertedControls();
            }
        }

        private void OnInvulnerabilityStarts()
        {
            if (_affected)
            {
                SwitchInvertedControls();
            }
        }

        private void SwitchInvertedControls()
        {   
            _affected = !_affected;
            _controller.LateralSpeed *= -1;
        }
    }
}
using UnityEngine;
using System.Collections;

namespace Items
{
    public class InvertedControls : MonoBehaviour
    {
        private Controller _controller;

        public float duration = 3f;

        private Coroutine _routine;

        void Awake()
        {
            _controller = this.GetComponent<Controller>();
        }

        void Start()
        {
            _routine = StartCoroutine(Co_MessControls());
        }

        public void ResetTime()
        {
            ResetControls();
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _routine = StartCoroutine(Co_MessControls());
            }
        }

        public void ResetControls()
        {
            if (Mathf.Sign(_controller.LateralSpeed) < 0)
            {
                _controller.LateralSpeed *= -1;
            }
        }

        private IEnumerator Co_MessControls()
        {
            _controller.LateralSpeed *= -1;
            yield return new WaitForSeconds(duration);
            ResetControls();
            Destroy(this);
        }
    }
}
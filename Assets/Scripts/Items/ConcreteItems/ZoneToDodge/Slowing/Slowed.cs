using UnityEngine;
using System.Collections;

namespace Items
{
    public class Slowed : MonoBehaviour
    {
        public float factorMalusLateralSpeed = 2;
        public float factorMalusJumpSpeed = 2;

        public float duration = 5;

        private Controller _affectedController;

        private Coroutine _routine;

        void Awake()
        {
            _affectedController = this.GetComponent<Controller>();
        }

        void Start()
        {
            _routine = StartCoroutine(Co_EndSlowed());
        }

        public void ResetTime()
        {
            ResetSpeed();
            if (_routine != null)
            {
                StopAllCoroutines();
                StartCoroutine(Co_EndSlowed());
            }
        }

        public void ResetSpeed()
        {
            _affectedController.JumpSpeed = _affectedController.BaseJumpSpeed;
            _affectedController.LateralSpeed = _affectedController.BaseLateralSpeed * Mathf.Sign(_affectedController.LateralSpeed);
        }

        private IEnumerator Co_EndSlowed()
        {
            _affectedController.LateralSpeed /= factorMalusLateralSpeed;
            _affectedController.JumpSpeed /= factorMalusJumpSpeed;
            yield return new WaitForSeconds(duration);
            ResetSpeed();
            Destroy(this);
        }
    }
}
using UnityEngine;
using System.Collections;

namespace Items
{
    public class SlowingArea : BaseItem
    {
        [SerializeField]
        private float speedFactorReduction = 0.5f;

        public static float decreasingSpeedFactor;

        private float lifeTime = 10f;

        void Awake()
        {
            decreasingSpeedFactor = speedFactorReduction;
        }

        public override void DoAction()
        {
            Destroy(this.gameObject, lifeTime);
        }
    }
}
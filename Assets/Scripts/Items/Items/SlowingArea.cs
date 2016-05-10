using UnityEngine;
using System.Collections;

namespace Items
{
    public class SlowingArea : BaseItem
    {
        public static float decreasingSpeedFactor;

        private float lifeTime = 5f;

        public override void DoAction()
        {
            Destroy(this.gameObject, lifeTime);
        }
    }
}
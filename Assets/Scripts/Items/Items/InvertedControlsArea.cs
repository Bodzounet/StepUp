using UnityEngine;
using System.Collections;

namespace Items
{
    public class InvertedControlsArea : BaseItem
    {
        public float lifeTime = 10;

        public override void DoAction()
        {
            Destroy(this.gameObject, lifeTime);
        }

        private void OnDestroy()
        {
            transform.GetChild(0).GetComponent<InvertedControlsEndDestroyHelper>().DestroyHelper();
            transform.GetChild(1).GetComponent<InvertedControlsEndDestroyHelper>().DestroyHelper();
        }
    }
}
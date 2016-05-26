using UnityEngine;
using System.Collections;
using System.Linq;

namespace Items
{
    public class HookMeToTarget : BaseItem
    {
        public float beamWidth;

        public override void DoAction()
        {
            this.transform.parent = User.transform;
            this.transform.localPosition = new Vector3(User.transform.FindChild("Center").localPosition.x, User.transform.FindChild("TopLeft").localPosition.y * 1.2f);
        }

        public virtual void EndLife()
        {
            var target = Physics2D.BoxCastAll(transform.position, new Vector2(beamWidth, beamWidth), 0, Vector2.up, Mathf.Infinity, 1 << LayerMask.NameToLayer("Player")).Where(x => x.transform != User.transform);

            if (target.Count() != 0)
            {
                User.transform.position = target.First().transform.position;
            }
        }

        public void CleanItem()
        {
            Destroy(this.gameObject);
        }
    }
}
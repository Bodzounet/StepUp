using UnityEngine;
using System.Collections;
using System.Linq;

namespace Items
{
    public class HookMeToTargetAndExchangePositions : HookMeToTarget
    {
        public override void EndLife()
        {
            float beamWidth = this.GetComponent<SpriteRenderer>().sprite.rect.width / 100;
            var targets = Physics2D.BoxCastAll(transform.position, new Vector2(beamWidth, beamWidth), 0, Vector2.up, Mathf.Infinity, 1 << LayerMask.NameToLayer("Player")).Where(x => x.transform != User.transform);

            if (targets.Count() != 0)
            {
                this.transform.parent = null;
                var target = targets.First();
                var targetIniPos = target.transform.position;
                target.transform.position = new Vector3(targetIniPos.x, User.transform.position.y);
                User.transform.position = new Vector3(User.transform.position.x, targetIniPos.y);

                Instantiate(swapFx, User.transform.FindChild("Center").position, Quaternion.identity);
                Instantiate(swapFx, target.transform.FindChild("Center").position, Quaternion.identity);
            }
        }

        /// <summary>
        /// 'cause the animator doesn't want to call an overriden fct...
        /// </summary>
        public void callEndLife()
        {
            EndLife();
        }
    }
}
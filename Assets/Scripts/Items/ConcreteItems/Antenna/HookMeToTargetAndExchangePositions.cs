using UnityEngine;
using System.Collections;
using System.Linq;

namespace Items
{
    public class HookMeToTargetAndExchangePositions : HookMeToTarget
    {
        public override void EndLife()
        {
            var targets = Physics2D.BoxCastAll(transform.position, new Vector2(beamWidth, beamWidth), 0, Vector2.up, Mathf.Infinity, 1 << LayerMask.NameToLayer("Player")).Where(x => x.transform != User.transform);

            if (targets.Count() != 0)
            {
                var target = targets.First();
                var targetIniPos = target.transform.position;
                target.transform.position = User.transform.position;
                User.transform.position = targetIniPos;  
            }
        }

        /// <summary>
        /// 'cause the animator doesn't want to call an overrident fct...
        /// </summary>
        public void callEndLife()
        {
            EndLife();
        }
    }
}
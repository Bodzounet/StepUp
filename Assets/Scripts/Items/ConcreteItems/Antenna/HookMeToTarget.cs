using UnityEngine;
using System.Collections;
using System.Linq;

namespace Items
{
    public class HookMeToTarget : BaseItem
    {
        public GameObject swapFx;

        public override void DoAction()
        {
            this.transform.parent = User.transform;
            this.transform.localPosition = new Vector3(User.transform.FindChild("Center").localPosition.x, User.transform.FindChild("TopLeft").localPosition.y * 1.2f);
        }

        public virtual void EndLife()
        {
            float beamWidth = this.GetComponent<SpriteRenderer>().sprite.rect.width / 100;

            var target = Physics2D.BoxCastAll(transform.position, new Vector2(beamWidth, beamWidth), 0, Vector2.up, Mathf.Infinity, 1 << LayerMask.NameToLayer("Player")).Where(x => x.transform != User.transform);

            if (target.Count() != 0)
            {
                 var absorbShield = target.First().collider.transform.FindChild("AbsorbChild");
                 if (absorbShield != null)
                 {
                     absorbShield.SendMessage("StealWifi", "Wifi");
                 }
                 else
                 {
                     target = target.Where(y => !y.collider.GetComponent<Controller>().Invulnerable);
                     if (target.Count() != 0)
                     {
                         Instantiate(swapFx, User.transform.FindChild("Center").position, Quaternion.identity);

                         float x = transform.parent.position.x;
                         this.transform.parent = null;
                         User.transform.position = new Vector2(x, target.First().transform.position.y);
                     }
                 }
            }
        }

        public void CleanItem()
        {
            Destroy(this.gameObject);
        }
    }
}
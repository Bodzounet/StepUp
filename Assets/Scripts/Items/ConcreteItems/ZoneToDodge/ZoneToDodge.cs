using UnityEngine;
using System.Collections;
using System.Linq;

namespace Items
{
    public class ZoneToDodge : Items.BaseItem
    {
        public GameObject ZoneAux;

        public override void DoAction()
        {
            foreach (var v in GameObject.FindObjectsOfType<Controller>().Where(x => x.gameObject != User))
            {
                GameObject.Instantiate(ZoneAux, v.transform.FindChild("Center").position, Quaternion.identity);
            }
        }
    }
}
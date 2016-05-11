using UnityEngine;
using System.Collections;

namespace Items
{
    public class AbsorbShield : Shield
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log(col.name);
            //Debug.Log(col.transform.root.tag);

            if (col.tag == "SlowingArea" || col.tag == "InvertedControlsAreaStart")
            {
                if (User.GetComponent<Inventory>().Item == null)
                {
                    User.GetComponent<Inventory>().PickSpecificItem(GameObject.FindObjectOfType<ItemManager>().PickSpecificItem(col.transform.root.name));
                }

                Destroy(col.transform.root.gameObject);

                var UserController = User.GetComponent<Controller>();

                UserController.OnEndBeingInvulnerable -= OnInvulnerabilityEnds;
                UserController.InvulnerabilityDuration = 0.1f;
                Destroy(this.gameObject);
            }
        }
    }
}
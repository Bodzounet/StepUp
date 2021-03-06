﻿using UnityEngine;
using System.Collections;

namespace Items
{
    public class AbsorbShield : Shield
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "SlowingZone" || col.tag == "InvertedControlsZone")
            {
                if (User.GetComponent<Inventory>().Item == null)
                {
                    User.GetComponent<Inventory>().PickSpecificItem(GameObject.FindObjectOfType<ItemManager>().PickSpecificItem(col.tag));
                }

                Destroy(col.transform.root.gameObject);

                var UserController = User.GetComponent<Controller>();

                UserController.OnEndBeingInvulnerable -= OnInvulnerabilityEnds;
                UserController.InvulnerabilityDuration = 0.1f;
                Destroy(this.gameObject);
            }
        }

        public void StealWifi(string wifiName)
        {
            if (User.GetComponent<Inventory>().Item == null)
            {
                User.GetComponent<Inventory>().PickSpecificItem(GameObject.FindObjectOfType<ItemManager>().PickSpecificItem(wifiName));
            }

            var UserController = User.GetComponent<Controller>();

            UserController.OnEndBeingInvulnerable -= OnInvulnerabilityEnds;
            UserController.InvulnerabilityDuration = 0.1f;
            Destroy(this.gameObject);
        }
    }
}
using UnityEngine;
using System.Collections;

namespace Items
{
    public class PickableItem : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                col.GetComponent<Inventory>().PickItem();
                Destroy(this.gameObject);
            }
        }
    }
}
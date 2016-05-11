using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Items
{
    public class SlowingAreaDestroyHelper : MonoBehaviour
    {
        private List<GameObject> _playerInArea = new List<GameObject>();

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                _playerInArea.Add(col.gameObject);
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                _playerInArea.Remove(col.gameObject);
            }
        }

        void OnDestroy()
        {
            foreach (var v in _playerInArea)
            {
                v.GetComponent<PlayerOnSlowingArea>().OnTriggerExit2DLogic(this.gameObject);
            }
        }
    }
}
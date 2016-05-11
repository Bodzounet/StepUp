using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Items
{
    public class InvertedControlsEndDestroyHelper : MonoBehaviour
    {
        private List<GameObject> _playersInArea = new List<GameObject>();

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                _playersInArea.Add(col.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                _playersInArea.Remove(col.gameObject);
            }
        }

        public void DestroyHelper()
        {
            foreach (var v in _playersInArea)
            {
                v.GetComponent<PlayerOnInvertedControlArea>().OnTriggerExit2DLogic(this.gameObject);
            }
        }
    }
}
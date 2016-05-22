using UnityEngine;
using System.Collections;

namespace Items
{
    public class HookHead : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Player" && col.name != "Player1")
            {
                transform.root.GetComponent<HomingRope>().StopAllCoroutines();
                transform.root.GetComponent<HomingRope>().Test();
            }
        }
    }
}
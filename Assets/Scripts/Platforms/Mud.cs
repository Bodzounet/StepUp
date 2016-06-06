using UnityEngine;
using System.Collections;

public class Mud : MonoBehaviour 
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Controller c = col.gameObject.GetComponent<Controller>();
            c.OnMud = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Controller c = col.gameObject.GetComponent<Controller>();
            c.OnMud = false;
        }
    }
}

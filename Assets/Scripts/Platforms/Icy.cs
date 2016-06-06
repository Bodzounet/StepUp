using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Icy : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Controller>().OnIce = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Controller>().OnIce = false;
        }
    }
}

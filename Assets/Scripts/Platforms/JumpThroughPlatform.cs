using UnityEngine;
using System.Collections;

public class JumpThroughPlatform : MonoBehaviour 
{
    private Collider2D _col;

    void Awake()
    {
        _col = this.transform.parent.GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Physics2D.IgnoreCollision(_col, col, true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Physics2D.IgnoreCollision(_col, col, false);
        }
    }
}

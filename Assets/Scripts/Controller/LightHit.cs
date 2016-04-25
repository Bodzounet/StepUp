using UnityEngine;
using System.Collections;

public class LightHit : MonoBehaviour 
{
    Attacks _attacks;

    void Awake()
    {
        _attacks = GetComponentInParent<Attacks>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        _attacks.LightHit(col.gameObject);
    }
}

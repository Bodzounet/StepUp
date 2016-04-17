using UnityEngine;
using System.Collections;

public class LightHit : MonoBehaviour 
{
    GameObject _lightHitCollider; // the child

    Controller _controller;
    Attacks _attacks;

    void Awake()
    {
        _controller = GetComponentInParent<Controller>();
        _controller.OnStun += () => { HittingState = false; };

        _attacks = GetComponentInParent<Attacks>();
    }

    void Start()
    {
        _lightHitCollider = transform.FindChild("LightHitCollider").gameObject;
    }
    
    public bool HittingState
    {
        get
        {
            return _lightHitCollider.activeSelf;
        }
        set
        {
            _lightHitCollider.SetActive(value);
        }
    }

    /// <summary>
    /// animation cannot call setter/getter
    /// </summary>
    /// <param name="active"></param>
    public void SetActiveState(bool active)
    {
        HittingState = active;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        _attacks.LightHit(col.gameObject);
    }
}

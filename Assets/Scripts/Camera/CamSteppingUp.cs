using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamSteppingUp : MonoBehaviour
{
    [SerializeField]
    private Transform _arbiterTransform;

    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine("co_MoveCamera");
        }
    }

    private IEnumerator co_MoveCamera()
    {
        Vector3 finalPos = _arbiterTransform.position + new Vector3(0, 3, 0);
        while (_arbiterTransform.position.y < finalPos.y)
        {
            _arbiterTransform.position = Vector3.Lerp(_arbiterTransform.position, finalPos, 0.1f);
            yield return (new WaitForEndOfFrame());
        }
    }
}

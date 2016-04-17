using UnityEngine;
using System.Collections;

public class CheckDead : MonoBehaviour
{
    public GameObject player;
    public GameObject deadTrigger;

    void Start()
    {
    }

    bool isDead(GameObject go)
    {
        Transform objPos = go.GetComponent<Transform>();
        Transform deadTriggerPos = deadTrigger.GetComponent<Transform>();
        Renderer rendPlayer = go.GetComponent<Renderer>();
        if (objPos.position.y + rendPlayer.bounds.size.y / 2 < deadTriggerPos.position.y)
        {
            //retirer une vie
            return true;
        }
        return false;
    }

    void Update()
    {
        Debug.Log(isDead(player));
    }
}
using UnityEngine;
using System.Collections;

public class CamSteppingUp : MonoBehaviour
{

    public GameObject Player;
    public GameObject stepUpTrigger;
    private Transform playerPos;
    private Transform triggerPos;
    private float time;
    Vector3 velo = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        time = 0.3f;

    }

    void StepUpCam()
    {
        playerPos = Player.GetComponent<Transform>();
        triggerPos = stepUpTrigger.GetComponent<Transform>();
        if (playerPos.position.y > triggerPos.position.y)
        {
            transform.position = Vector3.SmoothDamp(transform.position, transform.position + new Vector3(0, 5, 0), ref velo, time);
        }
    }

    // Update is called once per frame
    void Update()
    {
        StepUpCam();
    }
}

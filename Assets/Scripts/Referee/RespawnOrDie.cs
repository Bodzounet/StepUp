using UnityEngine;
using System.Collections;

public class RespawnOrDie : MonoBehaviour
{
    private Transform playerPos;
    private Transform camPos;

    public void respawn()
    {
        playerPos = this.GetComponent<Transform>();
        camPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        //check nb vies
        playerPos.position = new Vector3(-5.0f, camPos.position.y + 4.0f, 0);
        int layermask = 1 << LayerMask.NameToLayer("Platform");
        for (int i = -5; i <= 5; i++)
        {
            //Debug.DrawRay(playerPos.position, -playerPos.up, Color.yellow, 10.0f);
            if (Physics.Raycast(playerPos.position, -playerPos.up, Mathf.Infinity, layermask))
                return;
            playerPos.position = new Vector3(i, camPos.position.y + 4.0f, 0);
        }
    }

    //public void OnDrawGizmos()
    //{
    //    if (playerPos == null)
    //        playerPos = this.GetComponent<Transform>();
    //    Gizmos.DrawRay(playerPos.position, -playerPos.up);
    //}
}

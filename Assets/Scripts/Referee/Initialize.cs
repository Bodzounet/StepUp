using UnityEngine;
using System.Collections;

public class Initialize : MonoBehaviour 
{
    public GameObject[] players;

	void Awake () 
    {
	    foreach (var v in GameConfig.Instance.Players)
        {
            players[v - 1].SetActive(true);
        }
	}
}

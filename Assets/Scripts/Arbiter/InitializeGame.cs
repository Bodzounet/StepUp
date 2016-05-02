using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitializeGame : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _players;

	void Start ()
    {
	    foreach (int player in GameConfig.Instance.Players)
        {
            _players[player - 1].SetActive(true);
        }
	}
}

using UnityEngine;
using System.Collections;

public class UI_PlayerDisplayer : MonoBehaviour 
{
    public UI_PlayerInfo[] playerInfo;

    void Start()
    {
        ResetWholeUI();
    }

    void ResetWholeUI()
    {
        foreach (var v in playerInfo)
        {
            if (GameConfig.Instance.Players.Contains(v.playerIdToManage))
            {
                v.gameObject.SetActive(true);
                v.ResetPlayerInfo();

            }
            else
            {
                v.gameObject.SetActive(false);
            }
        }
    }
}

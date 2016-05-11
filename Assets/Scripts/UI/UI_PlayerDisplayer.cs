using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_PlayerDisplayer : MonoBehaviour 
{
    public UI_PlayerInfo[] playerInfo;
    public GameObject WinningBanner;

    void Start()
    {
        ResetWholeUI();

        foreach (var v in GameObject.FindObjectsOfType<DeathManager>())
        {
            v.OnWin += Wins;
        }
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

    void Wins(GameObject go)
    {
        WinningBanner.SetActive(true);

        WinningBanner.transform.GetChild(0).GetComponent<Image>().sprite = go.GetComponent<SpriteRenderer>().sprite;
        WinningBanner.transform.GetChild(0).GetComponent<Image>().color = go.GetComponent<SpriteRenderer>().color;
    }
}

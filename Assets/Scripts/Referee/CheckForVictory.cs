using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

public class CheckForVictory : MonoBehaviour 
{
    public DeathManager[] players;

    void Start()
    {
        foreach (var v in players)
        {
            if (v.gameObject.activeSelf)
            {
                v.OnDeath += Check;        
            }
        }
    }

    void Check()
    {
        if (players.Count(x => x.gameObject.activeSelf == true) == 1)
        {
            players.Single(x => x.gameObject.activeSelf == true).Wins();
            Invoke("I_EndGame", 5);
        }
    }

    private void I_EndGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
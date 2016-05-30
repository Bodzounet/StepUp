using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Multiplayer_menu : MonoBehaviour {
    private Dictionary<int, string> _startsBtn;
    private List<int>               _connectedPlayers;

    private void AddController(int id)
    {
        _connectedPlayers.Add(id);
        GameObject.Find("Player" + id + "Status").GetComponent<Text>().text = "Connected";
        GameObject.Find("Player" + id + "Icon").GetComponent<Animator>().SetBool("ready", true);
    }

    private void RemoveController(int id)
    {
        _connectedPlayers.Remove(id);
        GameObject.Find("Player" + id + "Status").GetComponent<Text>().text = "Press Start...";
        GameObject.Find("Player" + id + "Icon").GetComponent<Animator>().SetBool("ready", false);
    }

    void Start ()
    {
        _startsBtn = new Dictionary<int, string>() { { 1, "Start1" }, { 2, "Start2" }, { 3, "Start3" }, { 4, "Start4" } };
        _connectedPlayers = new List<int>() { };
    }

    void Update () {
        foreach (KeyValuePair<int, string> entry in _startsBtn)
            if (Input.GetButtonDown(entry.Value))
            {
                if (!_connectedPlayers.Exists(x => x == entry.Key))
                    AddController(entry.Key);
                else
                    RemoveController(entry.Key);
            }

        //GameObject.Find("PanelMultiplayer/Play").SetActive(_connectedPlayer.Count >= 2);
    }

    public void launchTheGame()
    {
        if (_connectedPlayers.Count < 1)
            return;
        GameConfig.Instance.Players = _connectedPlayers;
        SceneManager.LoadScene("Level");
    }
}

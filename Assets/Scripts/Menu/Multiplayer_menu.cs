using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Multiplayer_menu : MonoBehaviour {
    private Dictionary<int, string> _startsBtn = new Dictionary<int, string>() {{ 1, "Start1"}, { 2, "Start2"}, { 3, "Start3"}, { 4, "Start4"}};
    private List<int>               _connectedPlayer = new List<int>() { };

    private void AddCotroller(int id)
    {
        _connectedPlayer.Add(id);
        GameObject.Find("Player" + id + "Status").GetComponent<Text>().text = "Connected";
    }

    private void RemoveCotroller(int id)
    {
        _connectedPlayer.Remove(id);
        GameObject.Find("Player" + id + "Status").GetComponent<Text>().text = "Press Start...";
    }

    // Update is called once per frame
    void Update () {
        foreach (KeyValuePair<int, string> entry in _startsBtn)
            if (Input.GetButtonDown(entry.Value))
            {
                if (!_connectedPlayer.Exists(x => x == entry.Key))
                    AddCotroller(entry.Key);
                else
                    RemoveCotroller(entry.Key);
            }

        //GameObject.Find("PanelMultiplayer/Play").SetActive(_connectedPlayer.Count >= 2);
    }

    public void launchTheGame()
    {
        if (_connectedPlayer.Count < 2)
            return;
    }
}

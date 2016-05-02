using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Multiplayer_menu : MonoBehaviour {
    private Dictionary<int, string> _startsBtn;
    private List<int>               _connectedPlayer;
    SharedObject                    _sharedObject;

    private void AddController(int id)
    {
        _connectedPlayer.Add(id);
        GameObject.Find("Player" + id + "Status").GetComponent<Text>().text = "Connected";
    }

    private void RemoveController(int id)
    {
        _connectedPlayer.Remove(id);
        GameObject.Find("Player" + id + "Status").GetComponent<Text>().text = "Press Start...";
    }

    void Start ()
    {
        _startsBtn = new Dictionary<int, string>() { { 1, "Start1" }, { 2, "Start2" }, { 3, "Start3" }, { 4, "Start4" } };
        _connectedPlayer = new List<int>() { };
        _sharedObject = GameObject.Find("SharedObject").GetComponent<SharedObject>();
    }

    void Update () {
        foreach (KeyValuePair<int, string> entry in _startsBtn)
            if (Input.GetButtonDown(entry.Value))
            {
                if (!_connectedPlayer.Exists(x => x == entry.Key))
                    AddController(entry.Key);
                else
                    RemoveController(entry.Key);
            }

        //GameObject.Find("PanelMultiplayer/Play").SetActive(_connectedPlayer.Count >= 2);
    }

    public void launchTheGame()
    {
        if (_connectedPlayer.Count < 2)
            return;
    }
}

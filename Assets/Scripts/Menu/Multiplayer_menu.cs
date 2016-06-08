using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Multiplayer_menu : MonoBehaviour {
    private Dictionary<int, JoystickManagerMini>    _startsBtn;
    private List<int>                               _connectedPlayers;

    public GameObject                               PanelHowTo;

    private void AddController(int id)
    {
        SoundManager.PlaySound("WupMenu");
        _connectedPlayers.Add(id);
        GameObject.Find("Player" + id + "Status").GetComponent<Text>().text = "Connected";
        GameObject.Find("Player" + id + "Icon").GetComponent<Animator>().SetBool("ready", true);
    }

    private void RemoveController(int id)
    {
        SoundManager.PlaySound("KoeingMenu");
        _connectedPlayers.Remove(id);
        GameObject.Find("Player" + id + "Status").GetComponent<Text>().text = "Press Start...";
        GameObject.Find("Player" + id + "Icon").GetComponent<Animator>().SetBool("ready", false);
    }

    void Start ()
    {
        _startsBtn = new Dictionary<int, JoystickManagerMini>() { { 1, new JoystickManagerMini(0) }, { 2, new JoystickManagerMini(1) }, { 3, new JoystickManagerMini(2) }, { 4, new JoystickManagerMini(3) } };
        _connectedPlayers = new List<int>() { };
    }

    void Update () {
        foreach (KeyValuePair<int, JoystickManagerMini> entry in _startsBtn)
        {
            entry.Value.Update();
            if (entry.Value.isVibrate)
                entry.Value.SetVibration(.0f, .0f);


            if (entry.Value.GetButtonDown(JoystickManagerMini.e_XBoxControllerButtons.Start))
            {
                if (!_connectedPlayers.Exists(x => x == entry.Key))
                    AddController(entry.Key);
                else
                    RemoveController(entry.Key);

                entry.Value.SetVibration(1.0f, 1.0f);
            }
        }

        //GameObject.Find("PanelMultiplayer/Play").SetActive(_connectedPlayer.Count >= 2);
    }

    public void launchTheGame()
    {
        SoundManager.PlaySound("WupMenu");
        if (_connectedPlayers.Count < 1)
            return;
        GameConfig.Instance.Players = _connectedPlayers;
        
        this.gameObject.SetActive(false);
        PanelHowTo.SetActive(true);
    }
}

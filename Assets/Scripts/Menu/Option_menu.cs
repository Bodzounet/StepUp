using UnityEngine;
using System.Collections;

public class Option_menu : MonoBehaviour
{
    public UnityEngine.UI.Scrollbar    volumeScroll;
    public UnityEngine.UI.Toggle       HowtoToggle;

    // Use this for initialization
    void Start () {
        volumeScroll.value = AudioListener.volume;
        HowtoToggle.isOn = GameConfig.Instance.HowtoEnable;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setVolume(float vol)
    {
        AudioListener.volume = vol;
    }

    public void setEnableHowTo(bool state)
    {
        GameConfig.Instance.HowtoEnable = state;
    }
}

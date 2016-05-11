﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UI_Main : MonoBehaviour
{
    public GameObject PanelMainMenu;
    public GameObject PanelMultiplayer;
    public GameObject PanelOptions;
    public GameObject PanelCredit;

    public GameObject DefaultBtnMainMenu;
    public GameObject DefaultBtnMultiplayer;
    public GameObject DefaultBtnOptions;
    public GameObject DefaultBtnCredit;

    private void SelectButton(GameObject btn)
    {
        //EventSystem.current.SetSelectedGameObject(btn, new BaseEventData(EventSystem.current));
        btn.GetComponent<UnityEngine.UI.Button>().Select();
    }

    public void BtnSiglePlayer()
    {
        //Application.LoadLevel("Intro");
    }

    public void BtnMultiplayer()
    {
        SelectButton(DefaultBtnMultiplayer);
        PanelMainMenu.SetActive(false);
        PanelMultiplayer.SetActive(true);
    }

    public void BtnOptions()
    {
        SelectButton(DefaultBtnOptions);
        PanelMainMenu.SetActive(false);
        PanelOptions.SetActive(true);
    }

    public void BtnCredit()
    {
        SelectButton(DefaultBtnCredit);
        DefaultBtnMultiplayer.GetComponent<UnityEngine.UI.Button>().Select();
        PanelMainMenu.SetActive(false);
        PanelCredit.SetActive(true);
    }

    public void BtnReturn()
    {
        SelectButton(DefaultBtnMainMenu);
        PanelCredit.SetActive(false);
        PanelMultiplayer.SetActive(false);
        PanelOptions.SetActive(false);
        PanelMainMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
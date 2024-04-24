using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public static MenuManager instance;
    public TextMeshProUGUI ConnectionText;
    private string connectionStatus;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
       
    }

    private void Start()
    {
        connectionStatus = "Disconnected";
        RelayManager.Instance.OnConnectionChanged.AddListener(() =>  OnConnectionChanged());
    }
    private void OnConnectionChanged()
    {
        if (!RelayManager.Instance.joinCode.IsNullOrEmpty())
        {
            connectionStatus = "JoinCode is " + RelayManager.Instance.joinCode;
        }
    }

    public void ShowOrHideMenu()
    {
        var isMainMenuVisible = MainMenu.activeSelf;

        MainMenu.SetActive(!isMainMenuVisible);
    }

    private void Update()
    {
        ConnectionText.text = connectionStatus;
    }
}

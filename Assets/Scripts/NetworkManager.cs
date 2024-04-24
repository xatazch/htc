using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

using Unity.Netcode;

public class NetworkConnectManager : MonoBehaviour
{
    //Create a singelton of this class to be used in other classes
    public static NetworkConnectManager instance;
    public TextMeshProUGUI ConnectionText;
    private string connectionStatus;
    private ulong hostClientId;
    public string hostAddress;
    private int connectedClients;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        connectionStatus = "Disconnected";
    }

    public void HostSession()
    {
        NetworkManager.Singleton.StartHost();
        hostClientId = NetworkManager.ServerClientId;
        connectionStatus = $"Hosting Session ({hostAddress})";
    }

    public void JoinSession()
    {
        NetworkManager.Singleton.StartClient();
        hostClientId = NetworkManager.ServerClientId;
        connectionStatus = "Joined Session";
    }

    public void DisconnectSession()
    {
        NetworkManager.Singleton.DisconnectClient(hostClientId);
        connectionStatus = "Disconnected";
    }

    public void ShutdownSession()
    {
        NetworkManager.Singleton.Shutdown();
        connectionStatus = "Disconnected";
    }

    private void Update()
    {
        connectedClients = NetworkManager.Singleton.ConnectedClientsList.Count;
        ConnectionText.text = connectionStatus;
    }

}

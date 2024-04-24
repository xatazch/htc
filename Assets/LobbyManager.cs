using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using Unity.Services.Authentication;
using WebSocketSharp;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;
    private Lobby currentLobby;

    public struct LobbyData
    {
        public string lobbyName;
        public int maxPlayers;
    }

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            LogManager.Instance.AddLog("Player Not Signed in. Signing in user again. ");
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            LogManager.Instance.AddLog("Player ID: "+ AuthenticationService.Instance.PlayerId);
        }
    }

    public async void CreateLobby()
    {
        LobbyData lobbyData = new LobbyData();
        lobbyData.lobbyName = "Test Lobby";
        lobbyData.maxPlayers = 6;

        CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
        lobbyOptions.IsPrivate = false;
        lobbyOptions.Data = new Dictionary<string, DataObject>();

        string joinCode = await RelayManager.Instance.CreateRelayGame(lobbyData.maxPlayers);

        DataObject dataObject = new DataObject(DataObject.VisibilityOptions.Public, joinCode);
        lobbyOptions.Data.Add("Join Code Key", dataObject);

        currentLobby = await Lobbies.Instance.CreateLobbyAsync(lobbyData.lobbyName, lobbyData.maxPlayers, lobbyOptions);
    }

    public async void QuickJoinLobby()
    {
        currentLobby = await Lobbies.Instance.QuickJoinLobbyAsync();
        string relayJoinCode = currentLobby.Data["Join Code Key"].Value;

        RelayManager.Instance.JoinRelayGame(relayJoinCode);
    }
}

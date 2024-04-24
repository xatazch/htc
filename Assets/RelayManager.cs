using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.Events;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;

public class RelayManager : MonoBehaviour
{
    public static RelayManager Instance;
    public string joinCode;
    private UnityTransport transport;

    public UnityEvent OnConnectionChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        transport = GetComponent<UnityTransport>();
    }

    public async Task<string> CreateRelayGame(int maxPlayer)
    {
        try
        {
            //Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayer);

            //joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            //Debug.Log("THE JOIN CODE IS : " + joinCode);
            //LogManager.Instance.AddLog("THE JOIN CODE IS : " + joinCode);

            //transport.SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes,
            //    allocation.Key, allocation.ConnectionData);

            //NetworkManager.Singleton.StartHost();

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayer);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            LogManager.Instance.AddLog("THE JOIN CODE IS : " + joinCode);

            
            OnConnectionChanged.Invoke();

            return NetworkManager.Singleton.StartHost() ? joinCode : null;
        }
        catch (System.Exception ex)
        {
           LogManager.Instance.AddLog(ex.Message);
        }
        return null;
    }


    public async void JoinRelayGame(string joinCode)
    {
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        //transport.SetClientRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes,
        //    allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
        NetworkManager.Singleton.StartClient();
    }
}

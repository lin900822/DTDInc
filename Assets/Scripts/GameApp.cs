using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class GameApp : MonoBehaviour
{
    public static GameApp Instance { get; private set; }

    public NetworkRunner Runner => networkRunner;
    public NetworkEvents Event  => networkEvents;

    [HideInInspector] public string playerName = "";
        
    [SerializeField] private NetworkRunner networkRunner = null;
    [SerializeField] private NetworkEvents networkEvents = null;

    [SerializeField] private PlayerNetworkData playerNetworkDataPrefab = null;
        
    public Dictionary<PlayerRef, PlayerNetworkData> PlayerNetworkDataList { get; } = new Dictionary<PlayerRef, PlayerNetworkData>();

    private float joinRoomTime = -999f;
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
                    
            networkEvents.PlayerJoined.AddListener(OnPlayerJoined);
            networkEvents.PlayerLeft.AddListener(OnPlayerLeft);
            networkEvents.OnShutdown.AddListener(OnShutdown);
            networkEvents.OnDisconnectedFromServer.AddListener(OnDisconnectedFromServer);
            networkEvents.OnConnectFailed.AddListener(OnConnectFailed);
                
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (joinRoomTime > 0 && Time.time - joinRoomTime >= 10)
        {
            Runner.Shutdown();
        }
    }

    public void StartGame()
    {
        if (Runner.IsServer)
        {
            if (CheckIfAllPlayerReady())
            {
                Runner.SetActiveScene("Wilson");
            }
        }
    }
    
    private bool CheckIfAllPlayerReady()
    {
        if (PlayerNetworkDataList.Count <= 0) return false;
            
        foreach (var data in PlayerNetworkDataList)
        {
            if (data.Value.IsReady == false) return false;
        }

        return true;
    }
    
    public void ResetAllPlayerData()
    {
        if (PlayerNetworkDataList.Count <= 0) return;
            
        foreach (var data in PlayerNetworkDataList)
        {
            data.Value.IsReady = false;
            data.Value.SelectedCharacterIndex = -999;
        }
    }

    public async Task<StartGameResult> CreateRoom(string roomName, int maxPlayerAmount)
    {
        networkRunner.ProvideInput = true;

        var result = await networkRunner.JoinSessionLobby(SessionLobby.ClientServer);

        if (!result.Ok) return result;
        
        result = await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = roomName,
            PlayerCount = maxPlayerAmount,
            Scene = 3,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        return result;
    }

    public async Task<StartGameResult> JoinRoom(string roomName)
    {
        networkRunner.ProvideInput = true;

        var result = await networkRunner.JoinSessionLobby(SessionLobby.ClientServer);

        if (!result.Ok) return result;

        joinRoomTime = Time.time;
        
        result = await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,
            SessionName = roomName,
            Scene = 3,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
        
        joinRoomTime = -999f;

        return result;
    }

    public void AddPlayerNetworkData(PlayerNetworkData data)
    {
        PlayerNetworkDataList.Add(data.Object.InputAuthority, data);
            
        data.transform.SetParent(transform);
    }

    public PlayerNetworkData GetPlayerNetworkData(PlayerRef player = default)
    {
        PlayerNetworkData data;
            
        if (player == default)
        {
            data = PlayerNetworkDataList.TryGetValue(networkRunner.LocalPlayer, out PlayerNetworkData obj) ? obj : null;
        }
        else
        {
            data = PlayerNetworkDataList.TryGetValue(player, out PlayerNetworkData obj) ? obj : null;
        }

        return data;
    }

    // Network Events

    private void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        runner.Spawn(playerNetworkDataPrefab, transform.position, transform.rotation, player);
    } 
        
    private void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (PlayerNetworkDataList.TryGetValue(player, out PlayerNetworkData playerNetworkData))
        {
            runner.Despawn(playerNetworkData.Object);
            PlayerNetworkDataList.Remove(player);
        }
    }

    private void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }
    
    private void OnDisconnectedFromServer(NetworkRunner runner)
    {
        print("OnDisconnectedFromServer");
    }
    private void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        print("OnConnectRequest");
    }
}
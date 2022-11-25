using System;
using System.Collections.Generic;
using Lobby;
using UnityEngine;
using Fusion;
using UnityEngine.Serialization;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private NetworkObject[] playerPrefabs = null;
    
    private Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>();
    
    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            SpawnAllPlayers();
        }
    }

    public void SpawnAllPlayers()
    {
        var gameApp = GameApp.Instance;
        var spawnPointManager = SpawnPointManager.Instance;

        foreach (var player in gameApp.PlayerNetworkDataList)
        {
            var spawnPoint = spawnPointManager.GetRandomSpawnPoint(Runner.Simulation.Tick);

            var index = player.Value.SelectedCharacterIndex - 1;

            index = Mathf.Clamp(index, 0, 3);
            
            Runner.Spawn(playerPrefabs[index], spawnPoint.position, spawnPoint.rotation, player.Key);
        }
    }

    private void Awake() 
    {
        GameApp.Instance.Event.PlayerLeft.AddListener(OnPlayerLeft);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (playerList.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            playerList.Remove(player);
        }
    }
}
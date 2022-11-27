﻿using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace GamePlay
{
    public class PlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private PlayerController[] playerPrefabs = null;
    
        public readonly Dictionary<PlayerRef, PlayerController> PlayerList = new Dictionary<PlayerRef, PlayerController>();
    
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
            
                var playerController = Runner.Spawn(playerPrefabs[index], spawnPoint.position, spawnPoint.rotation, player.Key);
                
                PlayerList.Add(player.Key, playerController);
            }
        }

        private void Awake() 
        {
            GameApp.Instance.Event.PlayerLeft.AddListener(OnPlayerLeft);
        }

        private void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (PlayerList.TryGetValue(player, out PlayerController playerController))
            {
                runner.Despawn(playerController.Object);
                PlayerList.Remove(player);
            }
        }
    }
}
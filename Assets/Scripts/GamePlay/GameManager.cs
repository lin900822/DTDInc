using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public FloorManager FloorManager => floorManager;
        public RoundManager RoundManager => roundManager;
        public GameUIManager GameUIManager => gameUIManager;
    
        [SerializeField] private FloorManager floorManager      = null;
        [SerializeField] private RoundManager roundManager      = null;
        [SerializeField] private PlayerSpawner playerSpawner    = null;
        [SerializeField] private GameUIManager gameUIManager    = null;
        
        [SerializeField] private Coin coin = null;
        
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                
                SubscribeEvents();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                UnsubscribeEvents();
            }
        }

        private void SubscribeEvents()
        {
            roundManager.OnReadyStarted += floorManager.RecoverAllCubes;
            roundManager.OnReadyStarted += coin.ResetCoin;
            roundManager.OnReadyStarted += ResetAllPlayerData;
        }

        private void UnsubscribeEvents()
        {
            roundManager.OnReadyStarted -= floorManager.RecoverAllCubes;
            roundManager.OnReadyStarted -= coin.ResetCoin;
            roundManager.OnReadyStarted -= ResetAllPlayerData;
        }

        private void ResetAllPlayerData()
        {
            var gameApp = GameApp.Instance;

            if (!gameApp.Runner.IsServer) return;

            var playerNetworkDatas = gameApp.PlayerNetworkDataList;

            foreach (var data in playerNetworkDatas)
            {
                data.Value.KillAmount = 0;
                data.Value.DeathAmount = 0;
                data.Value.HasCoin = false;
            }

            var players = playerSpawner.PlayerList;

            foreach (var player in players)
            {
                player.Value.AbilityHandler.ClearAllAbilities();
            }
        }
    }
}
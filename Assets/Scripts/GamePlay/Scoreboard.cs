using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GamePlay
{
    public class Scoreboard : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = null;
        
        [SerializeField] private PlayerInfoCell playerInfoCellPrefab = null;

        [SerializeField] private Transform contentTrans = null;

        private readonly List<GameObject> _playerInfoCellObjList = new List<GameObject>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                canvasGroup.alpha = 1f;
                ShowScoreboard();
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                canvasGroup.alpha = 0f;
            }
        }

        private void ShowScoreboard()
        {
            var gameApp = GameApp.Instance;
            var gameManager = GameManager.Instance;

            var playerDataList = gameApp.PlayerNetworkDataList;
            var localPlayer = gameApp.Runner.LocalPlayer;

            foreach (var obj in _playerInfoCellObjList)
            {
                Destroy(obj);
            }
            
            _playerInfoCellObjList.Clear();
            
            foreach (var data in playerDataList)
            {
                var cell = Instantiate(playerInfoCellPrefab, contentTrans);

                var info = new PlayerInfoCell.PlayerInfo
                {
                    PlayerName = data.Value.PlayerName,
                    Kill = data.Value.KillAmount,
                    Death = data.Value.DeathAmount,
                    HasCoin = gameManager.Coin.OwnerPlayerRef == data.Key,
                    IsLocalPlayer = localPlayer == data.Key
                };

                cell.SetInfo(info);
                
                _playerInfoCellObjList.Add(cell.gameObject);
            }
        }
    }
}
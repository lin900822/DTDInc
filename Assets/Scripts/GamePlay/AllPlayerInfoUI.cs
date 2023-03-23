using System;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay
{
    public class AllPlayerInfoUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup[] playerInfoCanvasGroups = new CanvasGroup[4];

        [SerializeField] private TMP_Text[] playerNames = new TMP_Text[4];
        [SerializeField] private Image[] icons = new Image[4];

        [SerializeField] private Image[] progressBarImgs = new Image[4];
        [SerializeField] private TMP_Text[] progressTexts = new TMP_Text[4];

        private Dictionary<PlayerRef, PlayerNetworkData> playerNetworkDatas = null;

        private void Start()
        {
            if (GameApp.Instance == null) return;

            playerNetworkDatas = GameApp.Instance.PlayerNetworkDataList;

            int i = 0;
            foreach (var playerData in playerNetworkDatas)
            {
                SetPlayerInfo(i, playerData.Value.PlayerName, 0);
                i++;
            }

            for (int j = playerNetworkDatas.Count; j < 4; j++)
            {
                playerInfoCanvasGroups[j].alpha = 0f;
            }
        }

        private void Update()
        {
            float allPlayerKeepCoinTime = 0f;
            foreach (var playerData in playerNetworkDatas)
            {
                allPlayerKeepCoinTime += playerData.Value.KeepCoinTime;
            }

            int i = 0;
            foreach (var playerData in playerNetworkDatas)
            {
                if (allPlayerKeepCoinTime == 0f)
                {
                    SetPlayerProgress(i, 0f);
                }
                else
                {
                    SetPlayerProgress(i, (playerData.Value.KeepCoinTime / allPlayerKeepCoinTime) * 100);
                }
                i++;
            }
        }

        public void SetPlayerInfo(int index, string playerName, int selectedCharacter)
        {
            playerNames[index].text = playerName;
        }

        public void SetPlayerProgress(int index, float ratio)
        {
            progressBarImgs[index].fillAmount = ratio / 100f;
            progressTexts[index].text = ratio.ToString("0") + "%";
        }
    }
}
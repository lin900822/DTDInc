using System;
using System.Collections.Generic;
using Fusion;
using SO;
using UnityEngine;
using UnityEngine.UI;

namespace Character_Selection
{
    public class CharacterSelectionController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup selectionPanel = null;
        [SerializeField] private CanvasGroup readyPanel = null;

        [SerializeField] private GameObject mainCamera = null;
        [SerializeField] private GameObject[] virtualCameras = new GameObject[5];

        [SerializeField] private Button readyBtn = null;
        [SerializeField] private Button[] buttons = new Button[5];

        [Space(35)] [SerializeField] private CharacterInfoSO characterInfoSo = null;
        
        [SerializeField] private PlayerInfo[] playerInfos = new PlayerInfo[4];
        
        [SerializeField] private Button startBtn = null;
        
        private int selectedIndex = 0;

        private Dictionary<PlayerRef, PlayerNetworkData> playerNetworkDataList = null;
        
        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            for (int i = 0; i < buttons.Length; i++)
            {
                var i1 = i;
                buttons[i].onClick.AddListener(() => { OnCharacterClicked(i1); });
            }
            
            readyBtn.onClick.AddListener(OnReadyBtnClicked);

            SetPanel(selectionPanel, true);
            SetPanel(readyPanel, false);

            foreach (var info in playerInfos)
            {
                info.gameObject.SetActive(false);
            }
            
            startBtn.gameObject.SetActive(false);
            
            startBtn.onClick.AddListener(OnStartBtnClicked);
        }

        private void SetPanel(CanvasGroup canvasGroup, bool value)
        {
            canvasGroup.alpha = value ? 1 : 0;
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }

        private void OnDestroy()
        {
            foreach (var b in buttons)
            {
                b.onClick.RemoveAllListeners();
            }

            readyBtn.onClick.RemoveAllListeners();
            startBtn.onClick.RemoveAllListeners();
        }

        private void SetSelectedIndex(int value)
        {
            selectedIndex = value;

            mainCamera.SetActive(false);
            
            for (int i = 0; i < virtualCameras.Length; i++)
            {
                virtualCameras[i].SetActive(i == selectedIndex);
            }
        }

        private void OnCharacterClicked(int index)
        {
            SetSelectedIndex(index);
        }

        private void OnReadyBtnClicked()
        {
            foreach (var c in virtualCameras)
            {
                c.SetActive(false);
            }

            mainCamera.SetActive(true);
            
            SetPanel(selectionPanel, false);
            SetPanel(readyPanel, true);
            
            var playerNetworkData = GameApp.Instance.GetPlayerNetworkData();

            playerNetworkData.SetIsReady_RPC(true);
            playerNetworkData.SetSelectCharacterIndex_RPC(selectedIndex + 1);
            
            playerNetworkDataList = GameApp.Instance.PlayerNetworkDataList;
            int i = 0;
            foreach (var entry in playerNetworkDataList)
            {
                playerInfos[i].gameObject.SetActive(true);
                playerInfos[i].playerName.text = entry.Value.PlayerName;
                playerInfos[i].readyHint.SetActive(entry.Value.IsReady);

                Sprite icon = characterInfoSo.getIconById(entry.Value.SelectedCharacterIndex - 1);
                if (icon != null)
                {
                    playerInfos[i].icon.gameObject.SetActive(true);
                    playerInfos[i].icon.sprite = icon;
                }
                else
                {
                    playerInfos[i].icon.gameObject.SetActive(false);
                }

                i++;
            }

            if (GameApp.Instance.Runner.IsServer)
            {
                startBtn.gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            if (playerNetworkDataList != null)
            {
                int i = 0;
                foreach (var entry in playerNetworkDataList)
                {
                    playerInfos[i].gameObject.SetActive(true);
                    playerInfos[i].playerName.text = entry.Value.PlayerName;
                    playerInfos[i].readyHint.SetActive(entry.Value.IsReady);

                    Sprite icon = characterInfoSo.getIconById(entry.Value.SelectedCharacterIndex - 1);
                    if (icon != null)
                    {
                        playerInfos[i].icon.gameObject.SetActive(true);
                        playerInfos[i].icon.sprite = icon;
                    }
                    else
                    {
                        playerInfos[i].icon.gameObject.SetActive(false);
                    }

                    i++;
                }
            }
        }

        private void OnStartBtnClicked()
        {
            GameApp.Instance.StartGame();
        }
    }
}
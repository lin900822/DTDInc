using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GamePlay
{
    public class PlayerInfoCell : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText        = null;
        [SerializeField] private TMP_Text killText              = null;
        [SerializeField] private TMP_Text deathText             = null;
        [SerializeField] private GameObject hasCoinImage        = null;
        [SerializeField] private GameObject localPlayerBarImage  = null;

        private PlayerInfo _info;
        
        public void SetInfo(PlayerInfo info)
        {
            _info = info;

            playerNameText.text = _info.PlayerName;
            killText.text = _info.Kill.ToString();
            deathText.text = _info.Death.ToString();
            hasCoinImage.SetActive(_info.HasCoin);
            localPlayerBarImage.SetActive(_info.IsLocalPlayer);
        }
        
        public struct PlayerInfo
        {
            public string PlayerName;
            public int Kill;
            public int Death;
            public bool HasCoin;
            public bool IsLocalPlayer;
        }
    }
}
using TMPro;
using UnityEngine;

namespace Lobby
{
    public class JoinRoomPanel : Panel
    {
        [SerializeField] private TMP_InputField roomNameInputField = null;

        public void OnBackBtnClicked()
        {
            menuManager.SwitchPanel(0);
        }

        public async void OnConfirmBtnClicked()
        {
            menuManager.StartLoading();

            var roomName = roomNameInputField.text;

            var result = await GameApp.Instance.JoinRoom(roomName);

            if (result.Ok)
            {
                //menuManager.SwitchPanel(1);
            }
            else
            {
                print("無法加入房間，請再試一次!");
            }
        }
    }
}

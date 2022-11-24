using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateRoomPanel : Panel
{
    [SerializeField] private TMP_InputField roomNameInputField = null;

    public void OnBackBtnClicked()
    {
        menuManager.SwitchPanel(0);
    }

    public async void OnConfirmBtnClicked()
    {
        menuManager.StartLoading();

        string roomName = roomNameInputField.text;

        var result = await GameApp.Instance.CreateRoom(roomName, 4);

        if (result.Ok)
        {
            menuManager.SwitchPanel(2);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchPanel : Panel
{
    public void OnJoinRoomBtnClicked()
    {
        menuManager.SwitchPanel(1);
    }

    public void OnCreateRoomBtnClicked()
    {
        menuManager.SwitchPanel(2);
    }

    public void Exit()
    {

    }
}

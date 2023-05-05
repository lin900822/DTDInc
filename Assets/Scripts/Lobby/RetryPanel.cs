using System.Collections;
using System.Collections.Generic;
using Lobby;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryPanel : Panel
{
    public void OnRetryBtnClicked()
    {
        SceneManager.LoadScene(0);
    }
}

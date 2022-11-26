using System.Collections;
using System.Collections.Generic;
using Lobby;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Launcher : MonoBehaviour
{
    [SerializeField] private GameApp gameAppPrefab = null;

    [SerializeField] private TMP_InputField nameInputField = null;
    
    [SerializeField] private string nextScene = "";

    public void LaunchGame()
    {
        var playerName = nameInputField.text;
        
        var gameApp = Instantiate(gameAppPrefab, Vector3.zero, Quaternion.identity);

        gameApp.PlayerName = playerName;
        
        SceneManager.LoadScene(nextScene);
    }
    
}

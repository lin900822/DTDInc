using System.Collections;
using System.Collections.Generic;
using Lobby;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour
{
    [SerializeField] private GameApp gameApp = null;

    [SerializeField] private string nextScene = "";

    private void Start()
    {
        Instantiate(gameApp, Vector3.zero, Quaternion.identity);

        SceneManager.LoadScene(nextScene);
    }
}

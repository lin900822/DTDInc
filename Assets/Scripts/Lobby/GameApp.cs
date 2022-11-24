using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameApp : MonoBehaviour
{
    public static GameApp Instance { get; private set; }

    [SerializeField] private NetworkRunner runner = null;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task<StartGameResult> CreateRoom(string roomName, int maxPlayerAmount)
    {
        runner.ProvideInput = true;

        var result = await runner.JoinSessionLobby(SessionLobby.ClientServer);

        if (!result.Ok) return result;

        result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = roomName,
            PlayerCount = maxPlayerAmount,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        return result;
    }

    public async Task<StartGameResult> JoinRoom(string roomName)
    {
        runner.ProvideInput = true;

        var result = await runner.JoinSessionLobby(SessionLobby.ClientServer);

        if (!result.Ok) return result;

        result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,
            SessionName = roomName,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        return result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// 1. �إ߳s�u���ж�
/// 2. ���s���a�[�J����A�ͦ��@�Ӫ��a����
/// 3. �O�s�o�Ǫ��a����
/// </summary>
public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner networkRunner = null;

    [SerializeField] private NetworkObject playerPrefab = null;

    [SerializeField] private float mouseSensitivity = 100f;

    private Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>();

    private void Start()
    {
        StartGame(GameMode.AutoHostOrClient);
    }

    async void StartGame(GameMode mode)
    {
        networkRunner.ProvideInput = true;

        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "New Room",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Vector3 spawnPosition = Vector3.one;
        NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);

        playerList.Add(player, networkPlayerObject);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (playerList.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            playerList.Remove(player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
            data.MovementInput += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            data.MovementInput += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            data.MovementInput += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            data.MovementInput += Vector3.right;

        data.Buttons.Set(InputButtons.JUMP, Input.GetKey(KeyCode.Space));
        data.Buttons.Set(InputButtons.FIRE, Input.GetKey(KeyCode.Mouse0));

        data.Yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        data.Pitch += Input.GetAxis("Mouse Y") * mouseSensitivity;

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

}

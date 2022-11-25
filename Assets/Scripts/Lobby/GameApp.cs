using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Lobby
{
    public class GameApp : MonoBehaviour
    {
        public static GameApp Instance { get; private set; }

        [SerializeField] private NetworkRunner networkRunner = null;
        [SerializeField] private NetworkEvents networkEvents = null;

        [SerializeField] private PlayerNetworkData playerNetworkDataPrefab = null;
        
        public Dictionary<PlayerRef, PlayerNetworkData> PlayerNetworkDataList { get; } = new Dictionary<PlayerRef, PlayerNetworkData>();

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                    
                networkEvents.PlayerJoined.AddListener(OnPlayerJoined);
                networkEvents.PlayerLeft.AddListener(OnPlayerLeft);
                
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public async Task<StartGameResult> CreateRoom(string roomName, int maxPlayerAmount)
        {
            networkRunner.ProvideInput = true;

            var result = await networkRunner.JoinSessionLobby(SessionLobby.ClientServer);

            if (!result.Ok) return result;

            result = await networkRunner.StartGame(new StartGameArgs()
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
            networkRunner.ProvideInput = true;

            var result = await networkRunner.JoinSessionLobby(SessionLobby.ClientServer);

            if (!result.Ok) return result;

            result = await networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Client,
                SessionName = roomName,
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });

            return result;
        }

        public void AddPlayerNetworkData(PlayerNetworkData data)
        {
            PlayerNetworkDataList.Add(data.Object.InputAuthority, data);
            
            data.transform.SetParent(transform);
        }
        
        // Events

        private void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            runner.Spawn(playerNetworkDataPrefab, transform.position, transform.rotation, player);
        } 
        
        private void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (PlayerNetworkDataList.TryGetValue(player, out PlayerNetworkData playerNetworkData))
            {
                runner.Despawn(playerNetworkData.Object);
                PlayerNetworkDataList.Remove(player);
            }
        }
    }
}

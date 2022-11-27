using UnityEngine;

namespace GamePlay
{
    public class SpawnPointManager : MonoBehaviour
    {
        public static SpawnPointManager Instance { get; private set; }

        [SerializeField] private Transform[] spawnPoints = null;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public Transform GetRandomSpawnPoint(int randomSeed)
        {
            Random.InitState(randomSeed);
            return spawnPoints[Random.Range(0, spawnPoints.Length)];
        }
    }
}

using UnityEngine;

namespace GamePlay
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public FloorManager FloorManager => floorManager;
        public MatchManager MatchManager => matchManager;
    
        [SerializeField] private FloorManager floorManager = null;
        [SerializeField] private MatchManager matchManager = null;
    
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
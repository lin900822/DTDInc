using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay
{
    public class FloorManager : NetworkBehaviour
    {
        [SerializeField] private float timeToRecoverCubes = 5f;
        [SerializeField] private Vector2 recoverAmountRange = new Vector2();

        [SerializeField] private GameObject[] cubes = new GameObject[4900];
        [Networked] private TickTimer RecoverCubesTimer { get; set; }

        private GameManager _gameManager = null;
    
        private readonly Queue<int> _destroyedCubesIndex = new Queue<int>();

        private readonly List<int> _cubesToRecover = new List<int>();

        private readonly Queue<int> _batchDestroyCubesIndexBuffer = new Queue<int>();
        private readonly List<int> _batchDestroyCubesIndex = new List<int>();

        private void Start()
        {
            _gameManager = GameManager.Instance;
        }

        public override void FixedUpdateNetwork()
        {
            if (RecoverCubesTimer.ExpiredOrNotRunning(Runner))
            {
                AutoRecoverCubes();
            }
            else
            {
                BatchDestroyCubes();
            }
        }
        
        private void AutoRecoverCubes()
        {
            if (!Object.HasStateAuthority) return;
            
            RecoverCubesTimer = TickTimer.CreateFromSeconds(Runner, timeToRecoverCubes);

            var recoverAmount = (int)Random.Range(recoverAmountRange.x, recoverAmountRange.y);

            RecoverCubes(recoverAmount);
        }

        public int GetCubeIndex(GameObject cubeObj)
        {
            for(int i = 0; i < cubes.Length; i++)
            {
                if (cubeObj == cubes[i])
                    return i;
            }

            return -1;
        }
        
        private void RecoverCubes(int recoverAmount)
        {
            if (!Object.HasStateAuthority) return;

            _cubesToRecover.Clear();

            for (var i = 0; i < recoverAmount; i++)
            {
                if(_destroyedCubesIndex.Count > 0)
                {
                    _cubesToRecover.Add(_destroyedCubesIndex.Dequeue());
                }
            }

            RecoverCubes_RPC(_cubesToRecover.ToArray());
        }

        public void RecoverAllCubes()
        {
            if (!Object.HasStateAuthority) return;

            _destroyedCubesIndex.Clear();

            RecoverAllCubes_RPC();
        }

        public void DestroyCubes(int[] indexes)
        {
            foreach (var t in indexes)
            {
                _batchDestroyCubesIndexBuffer.Enqueue(t);
            }
        }
        
        private void BatchDestroyCubes()
        {
            if (!Object.HasStateAuthority) return;
            
            _batchDestroyCubesIndex.Clear();
            
            for (int i = 0; i < 10; i++)
            {
                if(_batchDestroyCubesIndexBuffer.Count <= 0) break;
                    
                _batchDestroyCubesIndex.Add(_batchDestroyCubesIndexBuffer.Dequeue());
            }
                
            DestroyCubes_RPC(_batchDestroyCubesIndex.ToArray());
        }

        // RPCs
    
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void DestroyOneCube_RPC(int index)
        {
            if(_gameManager.RoundManager.Stage != RoundStage.InGame) return;
        
            if (index < 0 || index >= cubes.Length) return;

            cubes[index].SetActive(false);

            if (Object.HasStateAuthority)
            {
                _destroyedCubesIndex.Enqueue(index);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void DestroyCubes_RPC(int[] indexes)
        {
            if(_gameManager.RoundManager.Stage != RoundStage.InGame) return;
        
            foreach(var index in indexes)
            {
                if (index < 0 || index >= cubes.Length) return;

                cubes[index].SetActive(false);

                if (Object.HasStateAuthority)
                {
                    _destroyedCubesIndex.Enqueue(index);
                }
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RecoverCubes_RPC(int[] indexes)
        {
            foreach (var index in indexes)
            {
                if (index < 0 || index >= cubes.Length) return;

                cubes[index].SetActive(true);
            }
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RecoverAllCubes_RPC()
        {
            foreach (var cube in cubes)
            {
                cube.SetActive(true);
            }
        }

        #region - Add Cube To Array -
        [ContextMenu("AddCube")]
        public void AddCube()
        {
            int i = 0;

            foreach(Transform child in transform)
            {
                cubes[i] = child.gameObject;
                child.gameObject.GetComponent<Cube>().Index = i;
                i++;
            }
        }
        #endregion
    }
}

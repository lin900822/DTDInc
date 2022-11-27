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

        private void Start()
        {
            _gameManager = GameManager.Instance;
        }

        public override void FixedUpdateNetwork()
        {
            AutoRecoverCubes();
        }

        private void AutoRecoverCubes()
        {
            if (!Object.HasStateAuthority) return;
            if (!RecoverCubesTimer.ExpiredOrNotRunning(Runner)) return;
        
            RecoverCubesTimer = TickTimer.CreateFromSeconds(Runner, timeToRecoverCubes);

            var recoverAmount = (int)Random.Range(recoverAmountRange.x, recoverAmountRange.y);

            RecoverCubes(recoverAmount);
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

        public int GetCubeIndex(GameObject cubeObj)
        {
            for(int i = 0; i < cubes.Length; i++)
            {
                if (cubeObj == cubes[i])
                    return i;
            }

            return -1;
        }

        public void RecoverAllCubes()
        {
            if (!Object.HasStateAuthority) return;
            
            RecoverCubes(_destroyedCubesIndex.Count);
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
        private void RecoverCubes_RPC(int[] indexs)
        {
            foreach (var index in indexs)
            {
                if (index < 0 || index >= cubes.Length) return;

                cubes[index].SetActive(true);
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

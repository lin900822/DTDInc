using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class FloorManager : NetworkBehaviour
{
    public static FloorManager Instance = null;

    [SerializeField] private float timeToRecoverCubes = 5f;
    [SerializeField] private Vector2 recoverAmountRange = new Vector2();

    [SerializeField] private GameObject[] cubes = new GameObject[4900];

    [SerializeField] private Queue<int> destroyedCubesIndex = new Queue<int>();

    [Networked] private TickTimer recoverCubesTimer { get; set; }

    private List<int> cubesToRecover = new List<int>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            if (recoverCubesTimer.ExpiredOrNotRunning(Runner))
            {
                recoverCubesTimer = TickTimer.CreateFromSeconds(Runner, timeToRecoverCubes);

                int recoverAmount = (int)Random.Range(recoverAmountRange.x, recoverAmountRange.y);

                RecoverCubes(recoverAmount);
            }
        }
    }

    private void RecoverCubes(int recoverAmount)
    {
        if (!Object.HasStateAuthority) return;

        cubesToRecover.Clear();

        for (int i = 0; i < recoverAmount; i++)
        {
            if(destroyedCubesIndex.Count > 0)
            {
                cubesToRecover.Add(destroyedCubesIndex.Dequeue());
            }
        }

        RecoverCubes_RPC(cubesToRecover.ToArray());
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

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void DestroyOneCube_RPC(int index)
    {
        if (index < 0 || index >= cubes.Length) return;

        cubes[index].SetActive(false);

        if (Object.HasStateAuthority)
        {
            destroyedCubesIndex.Enqueue(index);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void DestroyCubes_RPC(int[] indexs)
    {
        foreach(var index in indexs)
        {
            if (index < 0 || index >= cubes.Length) return;

            cubes[index].SetActive(false);

            if (Object.HasStateAuthority)
            {
                destroyedCubesIndex.Enqueue(index);
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

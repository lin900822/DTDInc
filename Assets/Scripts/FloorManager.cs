using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class FloorManager : NetworkBehaviour
{
    public static FloorManager Instance = null;

    [SerializeField] private GameObject[] cubes = new GameObject[4900];

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void DestroyCubes_RPC(int[] indexs)
    {
        foreach(var index in indexs)
        {
            if (index < 0 || index >= cubes.Length) return;

            cubes[index].SetActive(false);
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

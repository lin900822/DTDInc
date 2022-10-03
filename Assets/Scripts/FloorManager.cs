using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class FloorManager : NetworkBehaviour
{
    // 單例模式
    public static FloorManager Instance = null;

    [SerializeField] private GameObject[] cubes = new GameObject[4900];

    private void Awake()
    {
        // 單例模式
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // 判斷傳入的物體，是否有在Cubes陣列裡面，有的話回傳該Cube的Index，沒有的話回傳-1
    public int GetCubeIndex(GameObject cubeObj)
    {
        for(int i = 0; i < cubes.Length; i++)
        {
            if (cubeObj == cubes[i])
                return i;
        }

        return -1;
    }

    // 傳入Cube的Index，並SetActive(false)該Cube。只能從Server呼叫，並讓所有人執行此方法。
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void DestroyOneCube_RPC(int index)
    {
        if (index < 0 || index >= cubes.Length) return;

        cubes[index].SetActive(false);
    }

    #region - Add Cube -
    [ContextMenu("AddCube")]
    public void AddCube()
    {
        int i = 0;

        foreach(Transform child in transform)
        {
            cubes[i] = child.gameObject;
            i++;
        }
    }
    #endregion
}

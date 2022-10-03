using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class FloorManager : NetworkBehaviour
{
    // ��ҼҦ�
    public static FloorManager Instance = null;

    [SerializeField] private GameObject[] cubes = new GameObject[4900];

    private void Awake()
    {
        // ��ҼҦ�
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // �P�_�ǤJ������A�O�_���bCubes�}�C�̭��A�����ܦ^�Ǹ�Cube��Index�A�S�����ܦ^��-1
    public int GetCubeIndex(GameObject cubeObj)
    {
        for(int i = 0; i < cubes.Length; i++)
        {
            if (cubeObj == cubes[i])
                return i;
        }

        return -1;
    }

    // �ǤJCube��Index�A��SetActive(false)��Cube�C�u��qServer�I�s�A�����Ҧ��H���榹��k�C
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

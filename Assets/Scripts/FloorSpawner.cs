using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cube = null;
    [SerializeField] private GameObject cube1 = null;

    [SerializeField] private int width = 70;

    [ContextMenu("SpawnFloor")]
    public void SpawnFloor()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < width; j++)
            {
                if ((i + j) % 2 == 0)
                    Instantiate(cube, new Vector3(i, 0, j), Quaternion.identity, transform);
                else
                    Instantiate(cube1, new Vector3(i, 0, j), Quaternion.identity, transform);
            }
        }
    }
}

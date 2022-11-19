using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cube = null;
    [SerializeField] private GameObject cube1 = null;

    [SerializeField] private int radius = 70;

    [ContextMenu("SpawnFloor")]
    public void SpawnFloor()
    {
        for(int i = -radius; i < radius; i++)
        {
            for(int j = -radius; j < radius; j++)
            {
                if (i * i + j * j > radius * radius) continue;

                if ((i + j) % 2 == 0)
                    Instantiate(cube, new Vector3(i, 0, j), Quaternion.identity, transform);
                else
                    Instantiate(cube1, new Vector3(i, 0, j), Quaternion.identity, transform);
            }
        }
    }
}

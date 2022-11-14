using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class BlackHole : NetworkBehaviour
{
    [SerializeField] private float radius = 10f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float destroyCubesTime = 5f;
    [SerializeField] private LayerMask affectLayerMask = default;

    private Collider[] hitColliders = new Collider[50];
    private List<int> hitCubesIndex = new List<int>();

    [Networked] private TickTimer lifeTimer { get; set; }
    [Networked] private TickTimer destroyCubesTimer { get; set; }

    public override void Spawned()
    {
        lifeTimer = TickTimer.CreateFromSeconds(Runner, lifeTime);
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;

        transform.Translate(Vector3.forward * moveSpeed * Runner.DeltaTime);

        if(destroyCubesTimer.ExpiredOrNotRunning(Runner))
        {
            destroyCubesTimer = TickTimer.CreateFromSeconds(Runner, destroyCubesTime);
            DestroyCubes();
        }
        
        if (lifeTimer.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
    }

    private void DestroyCubes()
    {
        hitCubesIndex.Clear();

        Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders, affectLayerMask);

        foreach (var collider in hitColliders)
        {
            if (collider == null) continue;

            if (collider.TryGetComponent<Cube>(out var cube))
            {
                hitCubesIndex.Add(cube.Index);
            }
        }

        FloorManager.Instance.DestroyCubes_RPC(hitCubesIndex.ToArray());
    }
}

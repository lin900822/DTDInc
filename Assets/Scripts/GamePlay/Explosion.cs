using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace GamePlay
{
    public class Explosion : NetworkBehaviour
    {
        [SerializeField] private GameObject prepareEffect = null;
        [SerializeField] private GameObject explosionEffect = null;
        
        [SerializeField] private float radius = 7.5f;
        [SerializeField] private float prepareTime = 1.3f;
        [SerializeField] private float lifeTime = 5f;
        
        [SerializeField] private LayerMask affectLayerMask = default;

        private readonly Collider[] _hitColliders = new Collider[1000];
        private readonly List<int> _hitCubesIndex = new List<int>();

        [Networked] private TickTimer LifeTimer     { get; set; }
        [Networked] private TickTimer PrepareTimer  { get; set; }

        public override void Spawned()
        {
            LifeTimer = TickTimer.CreateFromSeconds(Runner, lifeTime);
            PrepareTimer = TickTimer.CreateFromSeconds(Runner, prepareTime);
            
            Instantiate(prepareEffect, transform);
        }

        public override void FixedUpdateNetwork()
        {
            if (!Runner.IsForward) return;
            
            if (PrepareTimer.Expired(Runner))
            {
                PrepareTimer = TickTimer.None;
                Instantiate(explosionEffect, transform);
                DestroyCubes();
            }
            if (LifeTimer.Expired(Runner))
            {
                PrepareTimer = TickTimer.None;
                Runner.Despawn(Object);
            }
        }

        private void DestroyCubes()
        {
            if (!Object.HasStateAuthority) return;
            
            _hitCubesIndex.Clear();

            Physics.OverlapSphereNonAlloc(transform.position, radius, _hitColliders, affectLayerMask);

            foreach (var collider in _hitColliders)
            {
                if (collider == null) continue;

                if (collider.TryGetComponent<Cube>(out var cube))
                {
                    _hitCubesIndex.Add(cube.Index);
                }
            }

            GameManager.Instance.FloorManager.DestroyCubes_RPC(_hitCubesIndex.ToArray());
        }
    }
}
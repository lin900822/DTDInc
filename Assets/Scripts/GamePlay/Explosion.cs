using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace GamePlay
{
    public class Explosion : NetworkBehaviour
    {
        [SerializeField] private GameObject prepareEffect = null;
        [SerializeField] private GameObject explosionEffect = null;
        
        [SerializeField] private float radius = 9f;
        [SerializeField] private float detectPlayerRadius = 35f;
        [SerializeField] private float prepareTime = 1.3f;
        [SerializeField] private float lifeTime = 5f;
        
        [SerializeField] private LayerMask floorLayerMask = default;
        [SerializeField] private LayerMask playerLayerMask = default;

        [SerializeField] private AudioSource audioSource = null;
        
        private readonly Collider[] _hitColliders = new Collider[350];
        private readonly List<short> _hitCubesIndex = new List<short>();

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
                
                audioSource.Play();
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

            for (int i = 0; i < _hitColliders.Length; i++)
            {
                _hitColliders[i] = null;
            }
            
            Physics.OverlapSphereNonAlloc(transform.position, radius, _hitColliders, floorLayerMask);

            foreach (var hit in _hitColliders)
            {
                if (hit == null) continue;

                if (hit.TryGetComponent<Cube>(out var cube))
                {
                    _hitCubesIndex.Add(cube.Index);
                }
            }
            
            GameManager.Instance.FloorManager.DestroyCubes(_hitCubesIndex.ToArray());
            print(_hitCubesIndex.Count);
            
            DoCameraShake();
        }

        private void DoCameraShake()
        {
            _hitCubesIndex.Clear();

            for (int i = 0; i < _hitColliders.Length; i++)
            {
                _hitColliders[i] = null;
            }

            Physics.OverlapSphereNonAlloc(transform.position, detectPlayerRadius, _hitColliders, playerLayerMask);

            foreach (var hit in _hitColliders)
            {
                if (hit == null) continue;

                if (hit.TryGetComponent<PlayerController>(out var player))
                {
                    if (player.Object.InputAuthority == 99) continue;
                    player.CameraHandler.Explosion_RPC();
                }
            }
        }
    }
}
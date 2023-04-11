using System.Collections.Generic;
using Fusion;
using GamePlay;
using UnityEngine;

namespace Ability
{
    public class LaserAbility : Ability
    {
        [SerializeField] private Vector3 destroyBound = new Vector3(1, 1, 50);
        [SerializeField] private Vector3 detectPlayerBound = new Vector3(10, 1, 50);

        [SerializeField] private GameObject effect = null;

        [SerializeField] private LayerMask floorLayerMask = default;
        [SerializeField] private LayerMask playerLayerMask = default;

        private readonly Collider[] _hitColliders = new Collider[350];
        private readonly List<short> _hitCubesIndex = new List<short>();

        private Vector3 _destroyCenter;
        private Quaternion _destroyRotation;
        
        public override void OnPrepare()
        {
            if (!Object.HasStateAuthority) return;
            
            PlayEffect_RPC();

            _destroyCenter = playerController.transform.position + playerController.transform.forward * (destroyBound.z + 3f);
            _destroyRotation = playerController.transform.rotation;
        }

        public override void OnExecute()
        {
            if (!Object.HasStateAuthority) return;

            DestroyCubes();
        }

        private void DestroyCubes()
        {
            _hitCubesIndex.Clear();

            for (int i = 0; i < _hitColliders.Length; i++)
            {
                _hitColliders[i] = null;
            }

            Physics.OverlapBoxNonAlloc(_destroyCenter, destroyBound, _hitColliders, _destroyRotation, floorLayerMask);

            foreach (var collider in _hitColliders)
            {
                if (collider == null) continue;

                if (collider.TryGetComponent<Cube>(out var cube))
                {
                    _hitCubesIndex.Add(cube.Index);
                }
            }
            
            print(_hitCubesIndex.Count);
            
            GameManager.Instance.FloorManager.DestroyCubes(_hitCubesIndex.ToArray());
            
            DoCameraShake();
        }

        private void DoCameraShake()
        {
            _hitCubesIndex.Clear();

            for (int i = 0; i < _hitColliders.Length; i++)
            {
                _hitColliders[i] = null;
            }

            Physics.OverlapBoxNonAlloc(_destroyCenter, detectPlayerBound, _hitColliders, _destroyRotation, playerLayerMask);

            foreach (var hit in _hitColliders)
            {
                if (hit == null) continue;

                if (hit.TryGetComponent<PlayerController>(out var player))
                {
                    player.CameraHandler.Laser_RPC();
                }
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void PlayEffect_RPC()
        {
            Instantiate(effect, transform.position + Vector3.up * 1.5f, transform.rotation);
        }
    }
}
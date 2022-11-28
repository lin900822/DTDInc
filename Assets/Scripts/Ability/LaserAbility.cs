using System.Collections.Generic;
using Fusion;
using GamePlay;
using UnityEngine;

namespace Ability
{
    public class LaserAbility : Ability
    {
        [SerializeField] private Vector3 destroyBound = new Vector3(1, 1, 50);

        [SerializeField] private GameObject effect = null;

        [SerializeField] private LayerMask affectLayerMask = default;

        private readonly Collider[] _hitColliders = new Collider[1000];
        private readonly List<int> _hitCubesIndex = new List<int>();

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

            Physics.OverlapBoxNonAlloc(_destroyCenter, destroyBound, _hitColliders, _destroyRotation, affectLayerMask);

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
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void PlayEffect_RPC()
        {
            Instantiate(effect, transform.position + Vector3.up * 1.5f, transform.rotation);
        }
    }
}
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Ability
{
    public class BlackHoleAbility : Ability
    {
        [SerializeField] private float range = 15f;

        [SerializeField] private NetworkObject blackHolePrefab = null;

        private List<LagCompensatedHit> hits = new List<LagCompensatedHit>();

        public override void OnExecute()
        {
            if (!Object.HasStateAuthority) return;

            Runner.Spawn(blackHolePrefab, transform.position + transform.rotation * new Vector3(0, 1, 7), playerController.transform.rotation, Object.InputAuthority);
        }
    }
}
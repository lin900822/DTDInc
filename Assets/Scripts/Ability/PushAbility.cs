using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Ability
{
    public class PushAbility : Ability
    {
        [SerializeField] private float range = 15f;
        [SerializeField] private float impulseMagnitude = 10f;

        [SerializeField] private GameObject effect = null;

        [SerializeField] private LayerMask hitLayer = default;

        private List<LagCompensatedHit> hits = new List<LagCompensatedHit>();

        public override void OnExecute()
        {
            if (!Object.HasStateAuthority) return;

            PlayEffect_RPC();

            DetectCollision();

            foreach (var hit in hits)
            {
                if (hit.GameObject.TryGetComponent<PlayerController>(out var hitPlayer))
                {
                    var pushDirection = (hitPlayer.transform.position - playerController.transform.position).normalized;

                    pushDirection.y += .1f;

                    hitPlayer.KCC.AddExternalImpulse(pushDirection * impulseMagnitude);
                    hitPlayer.LastHitPlayer = Object.InputAuthority;
                    hitPlayer.LastGotHitTime = Time.time;
                    
                    hitPlayer.CameraHandler.Hit_RPC();
                }
            }
        }

        private void DetectCollision()
        {
            hits.Clear();

            Runner.LagCompensation.OverlapSphere(playerController.transform.position, range, Object.InputAuthority, hits, hitLayer, HitOptions.IgnoreInputAuthority);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void PlayEffect_RPC()
        {
            Instantiate(effect, transform);
        }
    }
}
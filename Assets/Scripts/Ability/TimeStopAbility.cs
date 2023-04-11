using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Ability
{
    public class TimeStopAbility : Ability
    {
        [SerializeField] private float range = 24f;

        [SerializeField] private GameObject effect          = null;
        [SerializeField] private GameObject cameraEffect    = null;

        [SerializeField] private AudioSource audioSource = null;
        
        [SerializeField] private LayerMask hitLayer = default;

        private readonly List<LagCompensatedHit> _hits = new List<LagCompensatedHit>();

        public override void OnExecute()
        {
            if (Object.HasInputAuthority && Runner.IsForward)
            {
                Instantiate(cameraEffect, playerController.PlayerCamera.transform);
            }
            
            if (!Object.HasStateAuthority) return;

            PlayEffect_RPC();

            DetectCollision();

            foreach (var hit in _hits)
            {
                if (hit.GameObject.TryGetComponent<PlayerController>(out var hitPlayer))
                {
                    hitPlayer.Input.InputBlocked = true;
                }
            }
        }

        public override void OnCleanUp()
        {
            foreach (var hit in _hits)
            {
                if (hit.GameObject.TryGetComponent<PlayerController>(out var hitPlayer))
                {
                    hitPlayer.Input.InputBlocked = false;
                }
            }
        }

        private void DetectCollision()
        {
            _hits.Clear();

            Runner.LagCompensation.OverlapSphere(playerController.transform.position, range, Object.InputAuthority, _hits, hitLayer, HitOptions.IgnoreInputAuthority);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void PlayEffect_RPC()
        {
            Instantiate(effect, transform.position, transform.rotation);
            audioSource.Play();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
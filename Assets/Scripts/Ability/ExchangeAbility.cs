using Fusion;
using KCC_Processor;
using UnityEngine;

namespace Ability
{
    public class ExchangeAbility : Ability
    {
        [SerializeField] private GameObject effect = null;
        [SerializeField] private GameObject cameraEffect = null;
        [SerializeField] private TeleportKCCProcessor teleportKCCProcessor = null;

        public override void Activate(PlayerController playerController, Transform aimmedTrans)
        {
            base.Activate(playerController, aimmedTrans);

            if (aimmedTrans == null) return;

            if (aimmedTrans.TryGetComponent<PlayerController>(out var hitPlayer))
            {
                Vector3 currentPosition = playerController.transform.position;

                teleportKCCProcessor.SetTarget(aimmedTrans);
                playerController.KCC.AddModifier(teleportKCCProcessor);

                hitPlayer.KCC.SetPosition(currentPosition);
                hitPlayer.LastHitPlayer = Object.InputAuthority;
                hitPlayer.LastGotHitTime = Time.time;

                if (Object.HasStateAuthority)
                {
                    PlayEffect_RPC(currentPosition);
                    Instantiate(cameraEffect, playerController.PlayerCamera.transform);
                }
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void PlayEffect_RPC(Vector3 pos)
        {
            Instantiate(effect, pos + Vector3.up * 1.5f, Quaternion.identity);
            Instantiate(cameraEffect, playerController.PlayerCamera.transform);
        }
    }
}
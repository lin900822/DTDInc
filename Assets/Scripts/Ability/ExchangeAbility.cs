using Fusion;
using KCC_Processor;
using UnityEngine;

namespace Ability
{
    public class ExchangeAbility : Ability
    {
        [SerializeField] private GameObject effect = null;
        [SerializeField] private TeleportKCCProcessor teleportKCCProcessor = null;

        public override void OnExecute()
        {
            if (aimmedTrans == null) return;

            if(aimmedTrans.TryGetComponent<PlayerController>(out var hitPlayer))
            {
                Vector3 currentPosition = playerController.transform.position;

                teleportKCCProcessor.SetTarget(aimmedTrans);
                playerController.KCC.AddModifier(teleportKCCProcessor);

                hitPlayer.KCC.SetPosition(currentPosition);
                hitPlayer.LastAttackPlayer = Object.InputAuthority;

                if (Object.HasInputAuthority)
                    PlayEffect_RPC(currentPosition);
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        private void PlayEffect_RPC(Vector3 pos)
        {
            Instantiate(effect, pos + Vector3.up * 1.5f, Quaternion.identity);
        }
    }
}
 
using Fusion;
using System.Collections;
using UnityEngine;

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

            if (Object.HasInputAuthority)
                PlayEffect_RPC(currentPosition, aimmedTrans.position);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void PlayEffect_RPC(Vector3 pos1, Vector3 pos2)
    {
        Instantiate(effect, pos1 + Vector3.up * 1.5f, Quaternion.identity);
        Instantiate(effect, pos2 + Vector3.up * 1.5f, Quaternion.identity);
    }
}
 
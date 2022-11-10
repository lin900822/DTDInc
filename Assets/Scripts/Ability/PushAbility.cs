using Fusion;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAbility : Ability
{
    [SerializeField] private float range = 15f;
    [SerializeField] private float impulseMagnitude = 10f;

    [SerializeField] private GameObject effect = null;

    [SerializeField] private PushKCCProcessor pushKCCProcessor = null;

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
                pushKCCProcessor.SetCenterPoint(playerController.transform.position, impulseMagnitude);
                hitPlayer.KCC.AddModifier(pushKCCProcessor);
            }
        }
    }

    public override void OnCleanUp()
    {
        foreach (var hit in hits)
        {
            if (hit.GameObject.TryGetComponent<PlayerController>(out var hitPlayer))
            {
                hitPlayer.KCC.RemoveModifier(pushKCCProcessor);
            }
        }
    }

    private void DetectCollision()
    {
        hits.Clear();

        Runner.LagCompensation.OverlapSphere(playerController.transform.position, range, Object.InputAuthority, hits, -1, HitOptions.IgnoreInputAuthority);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void PlayEffect_RPC()
    {
        Instantiate(effect, transform);
    }
}
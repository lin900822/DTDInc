using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Coin : NetworkBehaviour
{
    [SerializeField] private float radius = 1f;

    [SerializeField] private LayerMask hitLayers = default;

    private PlayerController playerController = null;

    private List<LagCompensatedHit> hits = new List<LagCompensatedHit>();

    [Networked] private NetworkBehaviourId ownerId { get; set; }

    public override void FixedUpdateNetwork()
    {
        if (Runner.TryFindBehaviour(ownerId, out PlayerController obj))
        {
            playerController = obj;
        }
        else
        {
            playerController = null;
        }

        if (playerController != null)
        {
            transform.position = playerController.transform.position + new Vector3(0, 2f, 0);
        }

        if (transform.position.y < 0)
        {
            ownerId = NetworkBehaviourId.None;

            transform.position = new Vector3(0, 2, 0);
        }

        DetectPlayer();
    }

    public override void Render()
    {
        if(playerController != null)
        {
            transform.position = playerController.transform.position + new Vector3(0, 2f, 0);
        }
    }

    private void DetectPlayer()
    {
        if (!Object.HasStateAuthority) return;

        hits.Clear();

        Runner.LagCompensation.OverlapSphere(transform.position, radius, Object.InputAuthority, hits, hitLayers, HitOptions.None);

        if (hits.Count > 0)
        {
            if (hits[0].GameObject.TryGetComponent<PlayerController>(out var playerController))
            {
                ownerId = playerController.Id;
            }
        }
    }
}

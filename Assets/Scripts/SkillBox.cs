using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SkillBox : NetworkBehaviour
{
    [SerializeField] private float lifeTime = 1f;
    [SerializeField] private float coolDownTime = 1f;

    [SerializeField] private List<string> abilities = new List<string>();

    [Networked] private NetworkBool IsActive { get; set; }
    [Networked] private TickTimer timer { get; set; }

    public override void Spawned()
    {
        timer = TickTimer.CreateFromSeconds(Runner, lifeTime);
    }

    public override void FixedUpdateNetwork()
    {
        if(timer.Expired(Runner) && !IsActive)
        {
            IsActive = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!Object.HasStateAuthority) return;

        if (!IsActive) return;

        if (other.CompareTag("Player"))
        {
            if(other.TryGetComponent<PlayerController>(out var playerController))
            {
                int randomValue = Random.Range(0, abilities.Count);
                string abilityName = abilities[randomValue];

                if (playerController.AbilityHandler.AddAbilityToSlots(abilityName))
                {
                    IsActive = false;
                    timer = TickTimer.CreateFromSeconds(Runner, coolDownTime);
                }
            }
        }
    }
}

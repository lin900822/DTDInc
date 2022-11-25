using Fusion;
using UnityEngine;

namespace Ability
{
    public class AbilityHolder : NetworkBehaviour
    {
        private Ability ability = null;

        public bool IsBusy => abilityState != AbilityState.Ready;

        [Networked] private AbilityState abilityState { get; set; } = AbilityState.Ready;

        [Networked] private TickTimer timer { get; set; }

        public void Activate(PlayerController playerController, Ability ability)
        {
            if (abilityState != AbilityState.Ready) return;

            this.ability = ability;

            if (this.ability.CanAim)
            {
                this.ability.Activate(playerController, GetLagcompensatedHitTransform(playerController.PlayerCamera.transform));
            }
            else
            {
                this.ability.Activate(playerController);
            }

            abilityState = AbilityState.Prepare;
            this.ability.OnPrepare();
            timer = TickTimer.CreateFromSeconds(Runner, this.ability.PrepareTime);
        }

        public override void FixedUpdateNetwork()
        {
            if (ability == null) return;

            switch (abilityState)
            {
                case AbilityState.Prepare:
                    if (timer.Expired(Runner))
                    {
                        abilityState = AbilityState.Execute;
                        ability.OnExecute();
                        timer = TickTimer.CreateFromSeconds(Runner, ability.ExcuteTime);
                    }
                    else
                    {
                        ability.PrepareUpdate();
                    }
                    break;
                case AbilityState.Execute:
                    if (timer.Expired(Runner))
                    {
                        abilityState = AbilityState.CleanUp;
                        ability.OnCleanUp();
                        timer = TickTimer.CreateFromSeconds(Runner, ability.CleanUpTime);
                    }
                    else
                    {
                        ability.ExcecuteUpdate();
                    }
                    break;
                case AbilityState.CleanUp:
                    if (timer.Expired(Runner))
                    {
                        abilityState = AbilityState.Ready;
                        ability.Deactivate();
                    }
                    else
                    {
                        ability.CleanUpUpdate();
                    }
                    break;
            }
        }

        private Transform GetLagcompensatedHitTransform(Transform shootPoint)
        {
            Transform hitTrans = null;

            if(Runner.LagCompensation.Raycast(
                   shootPoint.position,
                   shootPoint.rotation * Vector3.forward,
                   Mathf.Infinity,
                   Object.InputAuthority,
                   out LagCompensatedHit hit,
                   -1,
                   HitOptions.IgnoreInputAuthority | HitOptions.IncludePhysX))
            {
                hitTrans = hit.GameObject.transform;
            }

            return hitTrans;
        }

        private enum AbilityState
        {
            Ready,
            Prepare,
            Execute,
            CleanUp
        }
    }
}
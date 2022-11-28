using System.Collections.Generic;
using Fusion;
using GamePlay;
using Unity.Mathematics;
using UnityEngine;

namespace Ability
{
    public class ExplosionAbility : Ability
    {
        [SerializeField] private Explosion explosion = null;

        public override void OnExecute()
        {
            if (aimmedTrans == null) return;

            var pos = aimmedTrans.position;

            Runner.Spawn(explosion, pos, quaternion.identity, Object.InputAuthority);
        }
        
    }
}
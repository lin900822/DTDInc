using System.Collections.Generic;
using UnityEngine;

namespace Ability
{
    public class PlayerAbilityDatabase : MonoBehaviour
    {
        public List<Ability> AllAbilities => allAbilities;
        [SerializeField] private List<Ability> allAbilities = new List<Ability>();

        public Ability GetAbilityByName(string abilityName)
        {
            Ability temp = null;
            foreach (var ability in allAbilities)
            {
                if (ability.AbilityName == abilityName)
                {
                    temp = ability;
                }
            }

            return temp;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Ability
{
    public class PlayerAbilityHandler : NetworkBehaviour
    {
        // Events

        public event Action<int> OnSelectedAbilityIndexUpdate = null;
        public event Action<AbilitySlotUIInfo[]> OnAbilityStatusUpdate = null;

        // Components

        [SerializeField] private PlayerController playerController = null;

        [SerializeField] private PlayerAbilityDatabase abilityDatabase = null;

        [SerializeField] private AbilityHolder abilityHolder = null;

        // Network Properties

        [Networked(OnChanged = nameof(OnAbilitySlotListChanged))] [Capacity(4)] [UnitySerializeField] 
        private NetworkLinkedList<AbilitySlot> abilitySlotsList { get; }

        [Networked(OnChanged = nameof(OnSelectedAbilityIndexChanged))]
        private int selectedAbilityIndex { get; set; } = -1;

        [Networked] private float mouseScrollValue { get; set; }

        // Network Behaviour

        public override void Spawned()
        {
            selectedAbilityIndex = 0;

            if (Object.HasInputAuthority)
            {
                OnAbilityStatusUpdate += playerController.UIHandler.UpdateAbilityStatus;
                OnSelectedAbilityIndexUpdate += playerController.UIHandler.UpdateSelectedSlot;

                UpdateSlotsUI();
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            OnAbilityStatusUpdate -= playerController.UIHandler.UpdateAbilityStatus;
            OnSelectedAbilityIndexUpdate -= playerController.UIHandler.UpdateSelectedSlot;
        }

        public void ProcessInput()
        {
            ProcessUseAbility();

            UpdateSelectedIndex();
        }

        private void ProcessUseAbility()
        {
            if (playerController.Input.WasPressed(InputButtons.UseAbility))
            {
                UseAbility();
            }
        }

        // Public Methods

        public bool AddAbilityToSlots(string abilityName)
        {
            int i = 0;

            foreach (var abilitySlot in abilitySlotsList)
            {
                if (abilitySlot.AbilityName == abilityName)
                {
                    var slot = abilitySlot;
                    slot.Amount++;
                    abilitySlotsList.Set(i, slot);

                    return true;
                }

                i++;
            }

            i = 0;

            foreach (var abilitySlot in abilitySlotsList)
            {
                if (string.IsNullOrEmpty(abilitySlot.AbilityName.ToString()))
                {
                    var slot = abilitySlot;
                    slot.AbilityName = abilityName;
                    slot.Amount = 1;
                    abilitySlotsList.Set(i, slot);

                    return true;
                }

                i++;
            }

            return false;
        }

        // Private Methods

        private void UseAbility()
        {
            if (abilityHolder.IsBusy) return;
            if (selectedAbilityIndex < 0 && selectedAbilityIndex >= 3) return;
            if (abilitySlotsList[selectedAbilityIndex].Amount <= 0) return;

            ActiveSelectedAbility();

            ConsumeOneAbility();
        }

        private void ActiveSelectedAbility()
        {
            string selectedAbilityName = abilitySlotsList[selectedAbilityIndex].AbilityName.ToString();
            Ability abilitySelected = abilityDatabase.GetAbilityByName(selectedAbilityName);
            abilityHolder.Activate(playerController, abilitySelected);
        }

        private void ConsumeOneAbility()
        {
            var slot = abilitySlotsList[selectedAbilityIndex];
            slot.Amount--;

            if (slot.Amount <= 0)
            {
                slot.AbilityName = null;
            }

            abilitySlotsList.Set(selectedAbilityIndex, slot);
        }

        private void UpdateSelectedIndex()
        {
            mouseScrollValue += playerController.Input.FixedInput.MouseWheelDelta;
            mouseScrollValue = Mathf.Clamp(mouseScrollValue, 0, 100);
            selectedAbilityIndex = Mathf.Clamp((int)(mouseScrollValue / 33), 0, 2);

            OnSelectedAbilityIndexUpdate?.Invoke(selectedAbilityIndex);
        }

        // OnChanged Methods

        private static void OnAbilitySlotListChanged(Changed<PlayerAbilityHandler> changed)
        {
            changed.Behaviour.UpdateSlotsUI();
            changed.Behaviour.UpdateCrosshair(changed.Behaviour.selectedAbilityIndex);
        }

        private void UpdateSlotsUI()
        {
            AbilitySlotUIInfo[] abilityDisplayStatuses = new AbilitySlotUIInfo[3];

            int i = 0;

            foreach (var abilistySlot in abilitySlotsList)
            {
                var ability = abilityDatabase.GetAbilityByName(abilistySlot.AbilityName.ToString());

                if(ability != null)
                {
                    abilityDisplayStatuses[i].Name = ability.AbilityName;
                    abilityDisplayStatuses[i].Amount = abilistySlot.Amount;
                    abilityDisplayStatuses[i].Icon = ability.AbilityIcon;
                }
                else
                {
                    abilityDisplayStatuses[i].Name = "";
                    abilityDisplayStatuses[i].Amount = 0;
                    abilityDisplayStatuses[i].Icon = default;
                }

                i++;
            }

            OnAbilityStatusUpdate?.Invoke(abilityDisplayStatuses);
        }

        private static void OnSelectedAbilityIndexChanged(Changed<PlayerAbilityHandler> changed)
        {
            changed.Behaviour.UpdateCrosshair(changed.Behaviour.selectedAbilityIndex);
        }

        private void UpdateCrosshair(int index)
        {
            string selectedAbilityName = abilitySlotsList[selectedAbilityIndex].AbilityName.ToString();
            Ability abilitySelected = abilityDatabase.GetAbilityByName(selectedAbilityName);

            if (abilitySelected == null)
            {
                playerController.UIHandler.SetAimCrosshair(false);

                return;
            }

            bool canAim = abilitySelected.CanAim;

            playerController.UIHandler.SetAimCrosshair(canAim);
        }
    }

    [Serializable]
    public struct AbilitySlot : INetworkStruct
    {
        public NetworkString<_32> AbilityName;
        public int Amount;

        public AbilitySlot(string name, int amount)
        {
            AbilityName = name;
            Amount = amount;
        }
    }
}
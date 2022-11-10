using System.Collections;
using UnityEngine;
using Fusion;
using System.Collections.Generic;
using System;

public class PlayerAbilityHandler : NetworkBehaviour
{
    public event Action<int> OnSelectedAbilityIndexUpdate = null;
    public event Action<AbilityDisplayStatus[]> OnAbilityStatusUpdate = null;

    [SerializeField] private PlayerController playerController = null;

    [SerializeField] private AbilityHolder abilityHolder = null;

    [SerializeField] private List<Ability> abilities = new List<Ability>();

    // Network Properties

    [Networked(OnChanged = nameof(OnAbilitySlotListChanged))]
    [Capacity(4)] 
    [UnitySerializeField] 
    private NetworkLinkedList<AbilitySlot> abilitySlotsList { get; }

    [UnitySerializeField]
    [Networked] public int SelectedAbilityIndex { get; set; }

    [Networked] private float mouseScrollValue { get; set; }

    // Network Behaviour

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            OnAbilityStatusUpdate += playerController.UIHandler.UpdateAbilityStatus;
            OnSelectedAbilityIndexUpdate += playerController.UIHandler.UpdateSelectedSlot;

            UpdateSlotsUI();
        }
    }

    public void ProcessInput()
    {
        ProcessUseAbility();

        ProcessSelectedIndex();
    }

    private void ProcessUseAbility()
    {
        if (playerController.Input.WasPressed(InputButtons.UseAbility))
        {
            if (SelectedAbilityIndex < 0 && SelectedAbilityIndex >= 3) return;
            if (abilityHolder.IsBusy) return;

            if (abilitySlotsList[SelectedAbilityIndex].Amount <= 0) return;

            Ability abilityToUse = GetAbilityToUse();

            abilityHolder.Activate(playerController, abilityToUse);

            var slot = abilitySlotsList[SelectedAbilityIndex];
            slot.Amount--;

            if (slot.Amount <= 0)
                slot.AbilityName = null;

            abilitySlotsList.Set(SelectedAbilityIndex, slot);
        }
    }

    private Ability GetAbilityToUse()
    {
        string selectedAbilityName = abilitySlotsList[SelectedAbilityIndex].AbilityName.ToString();

        Ability abilityToUse = null;
        foreach (var ability in abilities)
        {
            if (ability.AbilityName == selectedAbilityName)
            {
                abilityToUse = ability;
            }
        }

        return abilityToUse;
    }

    private void ProcessSelectedIndex()
    {
        mouseScrollValue += playerController.Input.FixedInput.MouseWheelDelta;
        mouseScrollValue = Mathf.Clamp(mouseScrollValue, 0, 100);
        SelectedAbilityIndex = Mathf.Clamp((int)(mouseScrollValue / 33), 0, 2);

        OnSelectedAbilityIndexUpdate?.Invoke(SelectedAbilityIndex);
    }

    public void OnFixedUpdate()
    {

    }

    public bool AddAbility(string abilityName)
    {
        int i = 0;

        foreach(var abilitySlot in abilitySlotsList)
        {
            if(abilitySlot.AbilityName == abilityName)
            {
                var slot = abilitySlot;
                slot.Amount++;
                abilitySlotsList.Set(i, slot);

                return true;
            }
            else if(string.IsNullOrEmpty(abilitySlot.AbilityName.ToString()))
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

    private void UpdateSlotsUI()
    {
        AbilityDisplayStatus[] abilityDisplayStatuses = new AbilityDisplayStatus[3];

        int i = 0;

        foreach (var abilistySlot in abilitySlotsList)
        {
            abilityDisplayStatuses[i].Name = abilistySlot.AbilityName.ToString();
            abilityDisplayStatuses[i].Amount = abilistySlot.Amount;

            i++;
        }

        OnAbilityStatusUpdate?.Invoke(abilityDisplayStatuses);
    }

    private static void OnAbilitySlotListChanged(Changed<PlayerAbilityHandler> changed)
    {
        changed.Behaviour.UpdateSlotsUI();
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
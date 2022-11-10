using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIHandler : MonoBehaviour
{
    [SerializeField] private Image[] abilityImgs = null;
    [SerializeField] private TMP_Text[] abilityAmountTxt = null;

    public void UpdateSelectedSlot(int selectedIndex)
    {
        for (int i = 0; i < abilityImgs.Length; i++)
        {
            if (i == selectedIndex)
            {
                abilityImgs[i].color = new Color(.3f, .3f, .3f);
            }
            else
            {
                abilityImgs[i].color = Color.black;
            }
        }
    }

    public void UpdateAbilityStatus(AbilityDisplayStatus[] abilities)
    {
        for (int i = 0; i < abilityAmountTxt.Length; i++)
        {
            abilityAmountTxt[i].text = abilities[i].Amount.ToString();
        }
    }
}

public struct AbilityDisplayStatus
{
    public string Name;
    public string Desciption;
    public Texture2D Icon;

    public int Amount;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIHandler : MonoBehaviour
{
    [SerializeField] private Image[] abilityImgs = null;
    [SerializeField] private TMP_Text[] abilityAmountTxt = null;

    [SerializeField] private Image crosshairImg = null;

    [SerializeField] private Sprite[] crosshairSprites = null;

    public void UpdateSelectedSlot(int selectedIndex)
    {
        for (int i = 0; i < abilityImgs.Length; i++)
        {
            if (i == selectedIndex)
            {
                abilityImgs[i].color = new Color(.75f, .75f, .75f);
            }
            else
            {
                abilityImgs[i].color = Color.white;
            }
        }
    }

    public void UpdateAbilityStatus(AbilitySlotUIInfo[] abilities)
    {
        for (int i = 0; i < abilityAmountTxt.Length; i++)
        {
            abilityAmountTxt[i].text = abilities[i].Amount.ToString();
            abilityImgs[i].sprite = abilities[i].Icon;

            if(abilities[i].Amount > 0)
            {
                abilityImgs[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                abilityImgs[i].color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void SetCrosshair(bool canAim)
    {
        if(canAim)
        {
            crosshairImg.sprite = crosshairSprites[1]; 
        }
        else
        {
            crosshairImg.sprite = crosshairSprites[0];
        }
    }
}

public struct AbilitySlotUIInfo
{
    public string Name;
    public string Desciption;
    public Sprite Icon;

    public int Amount;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIHandler : MonoBehaviour
{
    [SerializeField] private Image[] abilitySlotImgs = null;
    [SerializeField] private Image[] abilityIconImgs = null;
    [SerializeField] private TMP_Text[] abilityAmountTxt = null;

    [SerializeField] private Image crosshairImg = null;

    [SerializeField] private Sprite[] crosshairSprites = null;

    [SerializeField] private TMP_Text playerNameTxt = null;
    
    public void UpdateSelectedSlot(int selectedIndex)
    {
        for (int i = 0; i < abilityIconImgs.Length; i++)
        {
            if (i == selectedIndex)
            {
                abilityIconImgs[i].color = new Color(1f, 1f, 1f);
                abilitySlotImgs[i].enabled = true;
            }
            else
            {
                abilityIconImgs[i].color = new Color(.5f, .5f, .5f, .5f);
                abilitySlotImgs[i].enabled = false;
            }
        }
    }

    public void UpdateAbilityStatus(AbilitySlotUIInfo[] abilities)
    {
        for (int i = 0; i < abilityAmountTxt.Length; i++)
        {
            abilityAmountTxt[i].text = abilities[i].Amount == 0 ? "" : abilities[i].Amount.ToString();
            abilityIconImgs[i].sprite = abilities[i].Icon;

            if(abilities[i].Amount > 0)
            {
                abilityIconImgs[i].gameObject.SetActive(true);
            }
            else
            {
                abilityIconImgs[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetAimCrosshair(bool canAim)
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

    public void SetPlayerName(string value)
    {
        playerNameTxt.text = value;
    }
}

public struct AbilitySlotUIInfo
{
    public string Name;
    public string Description;
    public Sprite Icon;

    public int Amount;
}
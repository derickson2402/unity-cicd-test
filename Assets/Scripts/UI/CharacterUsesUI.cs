using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TakesDamage))]
[RequireComponent(typeof(WeaponInterface))]
public class CharacterUsesUI : MonoBehaviour
{
    WeaponInterface wi; // Reference to the characters weapon interface

    public void setWeaponA(DealsDamage weaponPrefab)
    {
        Image im = GameObject.Find("WeaponAImage").GetComponent<Image>();
        if (im == null)
        {
            Debug.Log("Could not find reference to Weapon A UI element");
        }
        else
        {
            im.sprite = weaponPrefab.GetComponent<ItemUIIcon>().GetIcon();
        }
    }

    public void setWeaponB(DealsDamage weaponPrefab)
    {
        Image im = GameObject.Find("WeaponBImage").GetComponent<Image>();
        if (im == null)
        {
            Debug.Log("Could not find reference to Weapon B UI element");
        }
        else
        {
            im.sprite = weaponPrefab.GetComponent<ItemUIIcon>().GetIcon();
        }
    }

    public void setHealth(float hp)
    {
        TextMeshProUGUI text = GameObject.Find("HealthCountText").GetComponent<TextMeshProUGUI>();
        if (text == null)
        {
            Debug.Log("Could not find reference to health text UI element");
        }
        else
        {
            text.text = hp.ToString();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterWeaponUIInterface : MonoBehaviour
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
}

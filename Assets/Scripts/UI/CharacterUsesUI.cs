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

    public Sprite Sword;
    public Sprite Bow;
    public Sprite Boomerang;
    public Sprite Bomb;

    public void setWeaponA(WeaponType type)
    {
        Image im = GameObject.Find("WeaponAImage").GetComponent<Image>();
        Sprite s = WeaponTypeToSprite(type);
        if (s == null)
        {
            Debug.Log("Sprite not assigned to weapon type " + type);
            im.color = Color.black;
            im.sprite = null;
        }
        else if (im == null)
        {
            Debug.Log("Could not find reference to Weapon A UI element");
        }
        else
        {
            im.color = Color.white;
            im.sprite = s;
        }
    }

    public void setWeaponB(WeaponType type)
    {
        Image im = GameObject.Find("WeaponBImage").GetComponent<Image>();
        Sprite s = WeaponTypeToSprite(type);
        if (s == null)
        {
            Debug.Log("Sprite not assigned to weapon type " + type);
            im.color = Color.black;
            im.sprite = null;
        }
        else if (im == null)
        {
            Debug.Log("Could not find reference to Weapon A UI element");
        }
        else
        {
            im.color = Color.white;
            im.sprite = s;
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

    private Sprite WeaponTypeToSprite(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.None:
                return null;
            case WeaponType.Sword:
                return Sword;
            case WeaponType.Bomb:
                return Bomb;
            case WeaponType.Boomerang:
                return Boomerang;
            case WeaponType.Bow:
                return Bow;
            default:
                return null;
        }
    }
}

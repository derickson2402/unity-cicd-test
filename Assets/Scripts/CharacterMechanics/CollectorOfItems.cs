using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TakesDamage))]
public class CollectorOfItems : MonoBehaviour
{
    private Dictionary<WeaponType, bool> itemsUnlocked;

    private void Awake()
    {
        itemsUnlocked = new Dictionary<WeaponType, bool>();
        foreach (WeaponType item in Enum.GetValues(typeof(WeaponType)))
        {
            itemsUnlocked[item] = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Enum.GetValues(typeof(WeaponType));
    }


    public bool IsItemUnlocked(WeaponType itemType)
    {
        return itemsUnlocked[itemType];
    }
}

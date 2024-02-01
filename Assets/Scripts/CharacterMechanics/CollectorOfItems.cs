using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TakesDamage))]
public class CollectorOfItems : MonoBehaviour
{
    private Dictionary<CollectableWeaponTypes, bool> itemsUnlocked;

    private void Awake()
    {
        itemsUnlocked = new Dictionary<CollectableWeaponTypes, bool>();
        foreach (CollectableWeaponTypes item in Enum.GetValues(typeof(CollectableWeaponTypes)))
        {
            itemsUnlocked[item] = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Enum.GetValues(typeof(CollectableWeaponTypes));
    }


    public bool IsItemUnlocked(CollectableWeaponTypes itemType)
    {
        return itemsUnlocked[itemType];
    }
}

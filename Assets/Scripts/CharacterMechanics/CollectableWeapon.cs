using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableWeaponTypes
{
    BOOMERANG,
    BOMB,
    BOW,
    SWORD
}

[RequireComponent(typeof(BoxCollider))]
public class CollectableWeapon : MonoBehaviour
{
    public CollectableWeaponTypes weaponType;
    [SerializeField] protected AudioClip soundEffect;
    public DealsDamage weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider can collect items
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            // Give the collider ourselves
            pc.PickUpSecondaryWeapon(weaponType, soundEffect, weaponPrefab);
            Destroy(gameObject);
        }
    }
}

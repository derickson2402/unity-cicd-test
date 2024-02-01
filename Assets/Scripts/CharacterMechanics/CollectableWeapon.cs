using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Boomerang,
    Bomb,
    Bow,
    Sword,
    None
}

[RequireComponent(typeof(BoxCollider))]
public class CollectableWeapon : MonoBehaviour
{
    public WeaponType weaponType;
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

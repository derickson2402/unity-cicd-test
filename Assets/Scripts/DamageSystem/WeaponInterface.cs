using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody))]

public class WeaponInterface : MonoBehaviour
{
    public DealsDamage weaponAPrefab;   // Prefab to use for weaponA
    public bool weaponAUsesAmmo;        // If weapon A is out of ammo, should it be useable?
    public DealsDamage weaponBPrefab;   // Prefab to use for weaponB
    public bool weaponBUsesAmmo;        // If weapon B is out of ammo, should it be useable?

    private int weaponAAmmo = 0;
    private int weaponBAmmo = 0;

    // Use weapon A
    public void useWeaponA()
    {
        if (weaponAPrefab != null)
        {
            useWeapon(weaponAPrefab, weaponAUsesAmmo, ref weaponAAmmo);
        }
    }

    // Use weapon B
    public void useWeaponB()
    {
        if (weaponBPrefab != null)
        {
            useWeapon(weaponBPrefab, weaponBUsesAmmo, ref weaponBAmmo);
        }
    }

    // Increase ammo count for weapon slot A
    public void giveAmmoWeaponA()
    {
        ++weaponAAmmo;
    }

    // Increase ammo count for weapon slot B
    public void giveAmmoWeaponB()
    {
        ++weaponBAmmo;
    }

    // Set weapon slot A to the given parameters
    public void setWeaponA(DealsDamage weaponPrefab, bool usesAmmo, int ammoCount)
    {
        weaponAPrefab = weaponPrefab;
        weaponAUsesAmmo = usesAmmo;
        weaponAAmmo = ammoCount;
    }

    // Set weapon slot B to the given parameters
    public void setWeaponB(DealsDamage weaponPrefab, bool usesAmmo, int ammoCount)
    {
        weaponBPrefab = weaponPrefab;
        weaponBUsesAmmo = usesAmmo;
        weaponBAmmo = ammoCount;
    }


    // Base method for useA and useB
    private void useWeapon(DealsDamage weapon, bool usesAmmo, ref int ammo)
    {
        Debug.Log("Using weapon");
        // If we have ammo left, instantiate a new weapon object on screen and decrement ammo count
        DealsDamage weaponObj = null;
        if (usesAmmo)
        {
            if (ammo < 1)
            {
                return;
            }
            else
            {
                --ammo;
            }
        }
        weaponObj = Instantiate(weapon, GetComponent<Rigidbody>().position + new Vector3(0,-1), Quaternion.identity);
        Assert.IsFalse(weaponObj == null);
        // If this is a projectile, then we should call shoot
        Projectile projectile = weaponObj.GetComponent<Projectile>();
        if (projectile == null)
        {
            Debug.Log("Non-projectile weapons not yet implemented");
        }
        else
        {
            projectile.Shoot(new Vector3(0, -1));
        }
    }
}

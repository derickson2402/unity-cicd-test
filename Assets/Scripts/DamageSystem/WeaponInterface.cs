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

    private int weaponAAmmo = 0;        // Current amount of ammo in weapon A
    private int weaponBAmmo = 0;        // Current amount of ammo in weapon B
    private Dictionary<string,DealsDamage> weaponRefs; // References to any active projectiles, lookup done by DealsDamage.name

    private void Awake()
    {
        weaponRefs = new Dictionary<string,DealsDamage>();
    }

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
        Assert.IsTrue(weapon != null);
        Debug.Log("Using weapon");
        // If we are holding a projectile, check if another is allowed to be fired
        Projectile prefabProjectile = weapon.GetComponent<Projectile>();
        if (prefabProjectile != null)
        {
            // Register projectiles to the weaponRefs dict firstly
            if (!weaponRefs.ContainsKey(weapon.name))
            {
                weaponRefs.Add(weapon.name, null);
            }
            // Check if we're allowed to fire again
            if (!prefabProjectile.multiFireAllowed && weaponRefs[weapon.name] != null)
            {
                Debug.Log("Active " + weapon.name + " already on map somewhere");
                return;
            }
        }
        // Check our ammo
        Debug.Log("Checking ammo");
        DealsDamage weaponObj = null;
        if (usesAmmo)
        {
            if (ammo < 1)
            {
                Debug.Log(weapon.name + " out of ammo");
                return;
            }
            else
            {
                --ammo;
            }
        }
        // All good, instantiate the object
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
            if (!prefabProjectile.multiFireAllowed)
            {
                weaponRefs[weapon.name] = weaponObj;
            }
            projectile.Shoot(new Vector3(0, -1));
        }
    }
}

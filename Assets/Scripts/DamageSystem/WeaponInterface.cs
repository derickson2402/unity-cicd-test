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
    private DealsDamage weaponInHand;   // For non-projectiles, can only have 1 weapon actively being used
    private int inHandFrames;           // For non-projectiles, how many frames has the character been attacking
    private Dictionary<string,DealsDamage> projRefs; // References to any active projectiles, lookup done by DealsDamage.name

    private void Awake()
    {
        projRefs = new Dictionary<string,DealsDamage>();
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
        // If we are already mid attack, do not allow attack
        if (weaponInHand != null)
        {
            Debug.Log(weaponInHand + " already in hand");
            return;
        }
        Assert.IsTrue(weapon != null);
        Debug.Log("Using weapon");
        // If we are holding a projectile, check if another is allowed to be fired
        Projectile prefabProjectile = weapon.GetComponent<Projectile>();
        Debug.Log(prefabProjectile);
        if (prefabProjectile != null)
        {
            // Register projectiles to the weaponRefs dict firstly
            if (!projRefs.ContainsKey(weapon.name))
            {
                Debug.Log("Adding key " + weapon.name);
                projRefs.Add(weapon.name, null);
            }
            Debug.Log(prefabProjectile + " multifire: " + prefabProjectile.multiFireAllowed + ", and name is " + weapon.name);
            Debug.Log("Found ref " + (projRefs[weapon.name] == null));
            // Check if we're allowed to fire again
            if (!prefabProjectile.multiFireAllowed && projRefs[weapon.name] != null)
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
        Debug.Log("spawning weapon " + weapon.name);
        weaponObj = Instantiate(weapon, GetComponent<Rigidbody>().position + new Vector3(0,-1), Quaternion.identity);
        weaponObj.name = weaponObj.name.Remove(weaponObj.name.Length - 7); // get rid of (cloned) at the end of name, needed for lookups later
        Assert.IsFalse(weaponObj == null);
        // Freeze the character holding the weapon. Update() will relinquish controls once it is done with delay
        // TODO: this will change with movement system
        InputManager inputManager = GetComponent<InputManager>();
        if (inputManager != null)
        {
            Debug.Log("Freezing player controls during weapon attack");
            inputManager.controlEnabled = false;
        }

        // Special logic for master sword, which is only a projectile when at full health
        WeaponTypeSword swordType = weaponObj.GetComponent<WeaponTypeSword>();
        if (swordType != null)
        {
            TakesDamage health = GetComponent<TakesDamage>();
            if (health == null)
            {
                Debug.Log("Warning: " + gameObject + " is holding master sword but has no TakesHealth. Defaulting to projectile behavior");
            }
            else
            {
                if (health.GetHP() != health.maxHP)
                {
                    Debug.Log("Mortal player not at full health, swinging sword");
                    Destroy(weaponObj.GetComponent<Projectile>());
                }
            }
        }
        // Weapon is spawned, put it in hand and start countdown. Update() will handle the rest of the logic
        weaponInHand = weaponObj;
        inHandFrames = 0;
    }

    private void Update()
    {
        if (weaponInHand != null)
        {
            if (inHandFrames < weaponInHand.attackDelayFrames)
            {
                inHandFrames++;
            }
            else
            {
                // For projectiles, we call shoot and then clean up our local references
                // For non-projectiles, we can simply destroy them
                Projectile projectile = weaponInHand.GetComponent<Projectile>();
                if (projectile == null)
                {
                    // handheld weapon
                    Debug.Log("Destroying weapon in hand " + weaponInHand);
                    Object.Destroy(weaponInHand.gameObject);
                }
                else
                {
                    // Projectile
                    Debug.Log("Fired object " + weaponInHand.name);
                    projRefs[weaponInHand.name] = weaponInHand;
                    projectile.Shoot(new Vector3(0, -1));
                }
                InputManager inputManger = GetComponent<InputManager>();
                if (inputManger != null)
                {
                    Debug.Log("Relinquishing controls to player");
                    inputManger.controlEnabled = true;
                }
                weaponInHand = null;
                inHandFrames = 0;
            }
        }
    }
}

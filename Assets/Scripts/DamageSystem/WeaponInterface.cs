using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GenericMovement))]

public class WeaponInterface : MonoBehaviour
{
    public DealsDamage weaponAPrefab;   // Prefab to use for weaponA
    public DealsDamage weaponBPrefab;   // Prefab to use for weaponB

    private DealsDamage weaponInHand;   // For non-projectiles, can only have 1 weapon actively being used
    private int inHandFrames;           // For non-projectiles, how many frames has the character been attacking
    private Dictionary<string,DealsDamage> projRefs; // References to any active projectiles, lookup done by DealsDamage.name
    private CharacterUsesUI uiRef;      // Characters (Link specifically) will have their weapons show up in the UI

    private void Awake()
    {
        projRefs = new Dictionary<string,DealsDamage>();
    }

    private void Start()
    {
        uiRef = GetComponent<CharacterUsesUI>();
        if (uiRef != null )
        {
            // If these are null the image will just be blank
            uiRef.setWeaponA(weaponAPrefab);
            uiRef.setWeaponB(weaponBPrefab);
        }
    }

    // Use weapon A
    public void useWeaponA()
    {
        if (weaponAPrefab != null)
        {
            useWeapon(weaponAPrefab);
        }
    }

    // Use weapon B
    public void useWeaponB()
    {
        if (weaponBPrefab != null)
        {
            useWeapon(weaponBPrefab);
        }
    }

    // Set weapon slot A to the given parameters
    public void setWeaponA(DealsDamage weaponPrefab)
    {
        weaponAPrefab = weaponPrefab;
        if (uiRef != null)
        {
            uiRef.setWeaponA(weaponPrefab);
        }
    }

    // Set weapon slot B to the given parameters
    public void setWeaponB(DealsDamage weaponPrefab)
    {
        weaponBPrefab = weaponPrefab;
        if (uiRef != null)
        {
            uiRef.setWeaponB(weaponPrefab);
        }
    }


    // Base method for useA and useB
    private void useWeapon(DealsDamage weapon)
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
        // What direction are we facing?
        Vector3 dirVec = DirectionManager.DirectionToVector3(GetComponent<GenericMovement>().directionManager.current);
        Vector3 weaponOffset = weapon.spawnOffsetDistance * dirVec;
        float degreeAngle = 0;
        switch (GetComponent<GenericMovement>().directionManager.current)
        {
            case Direction.Up:
                degreeAngle = 180; break;
            case Direction.Down:
                degreeAngle = 0; break;
            case Direction.Left:
                degreeAngle = 270; break;
            case Direction.Right:
                degreeAngle = 90; break;
        }
        Quaternion weaponRot = Quaternion.AngleAxis(degreeAngle, Vector3.forward);
        // All good, instantiate the object
        Debug.Log("spawning weapon " + weapon.name);
        DealsDamage weaponObj = Instantiate(weapon, GetComponent<Rigidbody>().position + weaponOffset, weaponRot);
        weaponObj.name = weaponObj.name.Remove(weaponObj.name.Length - 7); // get rid of (cloned) at the end of name, needed for lookups later
        Assert.IsFalse(weaponObj == null);
        // Set player/enemy interactions
        if (weaponObj.name != "Bomb")
        {
            TakesDamage health = GetComponent<TakesDamage>();
            weaponObj.affectEnemy = !health.isEnemy;
            weaponObj.affectPlayer = health.isEnemy;
        }
        // Trigger attack animations if necessary
        ScriptAnim4DirectionWalkPlusAttack anim = GetComponent<ScriptAnim4DirectionWalkPlusAttack>();
        if (anim != null)
        {
            anim.BeginAttack();
        }
        // Freeze the character holding the weapon. Update() will relinquish controls once it is done with delay
        InputManager inputManager = GetComponent<InputManager>();
        if (inputManager != null)
        {
            Debug.Log("Freezing player controls during weapon attack");
            inputManager.controlEnabled = false;
        }
        GenericMovement gm = GetComponent<GenericMovement>();
        if (gm != null)
        {
            gm.movementEnabled = false;
        }
        // Special logic for special weapon mechanics
        LogicSword(weaponObj);
        LogicBoomerang(weaponObj);
        // Weapon is spawned, put it in hand and start countdown. Update() will handle the rest of the logic
        weaponInHand = weaponObj;
        inHandFrames = 0;
    }

    // Special logic for master sword, which is only a projectile when at full health
    private void LogicSword(DealsDamage weaponObj)
    {
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
    }

    // Special logic for boomerang, which needs to track the character that threw it
    private void LogicBoomerang(DealsDamage weaponObj)
    {
        WeaponTypeBoomerang boomerang = weaponObj.GetComponent<WeaponTypeBoomerang>();
        if (boomerang != null)
        {
            boomerang.RefToThrower(GetComponent<Rigidbody>());
        }
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
                    projectile.Shoot(DirectionManager.DirectionToVector3(GetComponent<GenericMovement>().directionManager.current));
                }
                // Return control back to player
                InputManager inputManger = GetComponent<InputManager>();
                if (inputManger != null)
                {
                    Debug.Log("Relinquishing controls to player");
                    inputManger.controlEnabled = true;
                }
                GenericMovement gm = GetComponent<GenericMovement>();
                if (gm != null)
                {
                    gm.movementEnabled = true;
                }
                // Handle animations
                ScriptAnim4DirectionWalkPlusAttack anim = GetComponent<ScriptAnim4DirectionWalkPlusAttack>();
                if (anim != null)
                {
                    anim.EndAttack();
                }
                weaponInHand = null;
                inHandFrames = 0;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //fields for UI
    [SerializeField] protected TextField rupeeCountText;
    [SerializeField] protected TextField keyCountText;
    [SerializeField] protected TextField bombCountText;
    [SerializeField] protected TextField heartCountText;

    // Sound Effects
    [SerializeField] protected AudioClip rupeeCollectionSoundEffect;
    [SerializeField] protected AudioClip heartCollectionSoundEffect;
    [SerializeField] protected AudioClip keyCollectionSoundEffect;
    [SerializeField] protected AudioClip bombCollectionSoundEffect;

    //hp variables
    private bool godMode = false;

    //inventory variables
    private int rupeeCount = 0;
    private int maxRupees = 255;
    private int keyCount = 0;
    private int maxKeys = 255;
    private int bombCount = 0;
    private int maxBombs = 8;
    private List<WeaponType> weaponsUnlocked;
    private List<DealsDamage> weaponPrefabs;
    private int weaponIndex = 0;

    void Start()
    {
        GetComponent<ScriptAnim4DirectionWalkPlusAttack>().active = true;
        GetComponent<GenericMovement>().movementEnabled = true;
        weaponsUnlocked = new List<WeaponType>();
        weaponPrefabs = new List<DealsDamage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckForPickups(other);
    }

    public void ReturnPlayerToStart()
    {
        GetComponent<RoomManager>().TeleportToRoom(GetComponent<RoomManager>().startX, GetComponent<RoomManager>().startY);
    }

    //-----------------------
    //  Inventory functions
    //-----------------------

    public void ModifyRupees(int num)
    {
        if (num > 0)
        {
            if (rupeeCount + num <= maxRupees)
            {
                rupeeCount += num;
            }
            else
            {
                rupeeCount = maxRupees;
            }
        }
        else
        {
            if (rupeeCount + num < 0)
            {
                rupeeCount = 0;
            }
            else
            {
                rupeeCount += num;
            }
        }
        rupeeCountText.Write('x' + rupeeCount.ToString());
    }

    public void ModifyKeys(int num)
    {
        if (num > 0)
        {
            if (keyCount + num <= maxKeys)
            {
                keyCount += num;
            }
            else
            {
                keyCount = maxKeys;
            }
        }
        else
        {
            if (keyCount + num < 0)
            {
                keyCount = 0;
            }
            else
            {
                keyCount += num;
            }
        }
        keyCountText.Write('x' + keyCount.ToString());
    }


    public void ModifyBombs(int num)
    {
        if (num > 0)
        {
            if (bombCount + num <= maxBombs)
            {
                bombCount += num;
            }
            else
            {
                bombCount = maxBombs;
            }
        }
        else
        {
            if (bombCount + num < 0)
            {
                bombCount = 0;
            }
            else
            {
                bombCount += num;
            }
        }
        bombCountText.Write('x' + bombCount.ToString());
    }

    //--------------------
    //  Pickup functions
    //--------------------

    private void CheckForPickups(Collider other)
    {
        // Grab game object that this component belongs to.
        GameObject go = other.gameObject;

        // Specialize behavior based on tag.
        if (go.CompareTag("Rupee"))
        {
            ModifyRupees(1);
            Debug.Log("Rupees x" + rupeeCount);

            // Make Rupee disappear.
            Destroy(go);

            // Play Rupee collection clip.
            AudioSource.PlayClipAtPoint(rupeeCollectionSoundEffect, Camera.main.transform.position);
        }
        else if (go.CompareTag("Key"))
        {
            ModifyKeys(1);

            Debug.Log("Keys x" + keyCount);
       

            // Make Rupee disappear.
            Destroy(go);

            // Play Rupee collection clip.
            AudioSource.PlayClipAtPoint(keyCollectionSoundEffect, Camera.main.transform.position);
        }
        else if (go.CompareTag("Heart"))
        {
            GetComponent<TakesDamage>().Heal(1);
            Destroy(go);

            // Play collection clip.
            AudioSource.PlayClipAtPoint(heartCollectionSoundEffect, Camera.main.transform.position);
        }
        else if (go.CompareTag("Bomb"))
        {
            ModifyBombs(1);
            Destroy(go);

            // Play collection clip.
            AudioSource.PlayClipAtPoint(bombCollectionSoundEffect, Camera.main.transform.position);
        }
        else if (go.CompareTag("LockedDoor"))
        {
            if (keyCount > 0)
            {
                ModifyKeys(-1);
                go.GetComponent<LockedDoor>().openDoor();
            }
        }
    }
    public void PickUpSecondaryWeapon(WeaponType weaponType, AudioClip soundEffect, DealsDamage weaponPrefab)
    {
        Debug.Log(gameObject + " picked up " + weaponType);
        AudioSource.PlayClipAtPoint(soundEffect, Camera.main.transform.position);
        if (!weaponsUnlocked.Contains(weaponType))
        {
            weaponsUnlocked.Add(weaponType);
            weaponPrefabs.Add(weaponPrefab);
        }
        if (weaponType == WeaponType.Bomb)
        {
            ModifyBombs(1);
        }
    }

    public void EquipNextSecondaryWeapon()
    {
        WeaponInterface wi = GetComponent<WeaponInterface>();
        if (wi == null)
        {
            Debug.Log("Error getting weaponinterface");
            return;
        }
        if (weaponsUnlocked.Count == 0)
        {
            return;
        }
        weaponIndex = (weaponIndex + 1) % weaponsUnlocked.Count;
        
        wi.setWeaponB(weaponsUnlocked[weaponIndex], weaponPrefabs[weaponIndex]);
    }

    public void UseSecondaryWeapon()
    {
        WeaponInterface wi = GetComponent<WeaponInterface>();
        if (wi == null)
        {
            Debug.Log("Error getting weaponinterface");
            return;
        }

        if (weaponsUnlocked.Count == 0) { return; }
        if (weaponsUnlocked[weaponIndex] == WeaponType.Bomb)
        {
            if (bombCount > 0)
            {
                ModifyBombs(-1);
            }
            else
            {
                Debug.Log("out of bombs");
                return;
            }
        }
        if (weaponsUnlocked[weaponIndex] == WeaponType.Bow)
        {
            if (rupeeCount > 0)
            {
                ModifyRupees(-1);
            }
            else
            {
                Debug.Log("out of rupees");
                return;
            }
        }
        wi.useWeaponB();
    }

    internal void UseSecondaryWeapon(Vector3 aimDirection)
    {
        WeaponInterface wi = GetComponent<WeaponInterface>();
        if (wi == null)
        {
            Debug.Log("Error getting weaponinterface");
            return;
        }

        if (weaponsUnlocked.Count == 0) { return; }
        wi.useWeaponB(aimDirection);
    }

    //-----------------------
    //  Debug/Cheat functions
    //-----------------------

    public void ActivateCheats()
    {
        if (!godMode)
        {
            Debug.Log("Activating God Mode");
            godMode = true;
            GetComponent<TakesDamage>().invincible = true;
            GetComponent<TakesDamage>().Heal(1000);
            ModifyRupees(maxRupees);
            ModifyKeys(maxKeys);
            ModifyBombs(maxBombs);
            if (!weaponsUnlocked.Contains(WeaponType.Bomb))
            {
                weaponsUnlocked.Add(WeaponType.Bomb);
                weaponPrefabs.Add(null);
            }
        }
        else
        {
            Debug.Log("Deactivating God Mode");
            godMode = false;
            GetComponent<TakesDamage>().invincible = false;
        }
    }
}

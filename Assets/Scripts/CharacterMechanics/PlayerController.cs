using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Start()
    {
        GetComponent<ScriptAnim4DirectionWalkPlusAttack>().active = true;
        GetComponent<GenericMovement>().movementEnabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckForPickups(other);
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
        else if (go.CompareTag("Bomb"))
        {
            ModifyBombs(1);

            Debug.Log("Keys x" + bombCount);

            // Make Rupee disappear.
            Destroy(go);

            // Play Rupee collection clip.
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
            ModifyRupees(maxRupees);
            ModifyKeys(maxKeys);
            ModifyBombs(maxBombs);
        }
        else
        {
            Debug.Log("Deactivating God Mode");
            godMode = false;
            GetComponent<TakesDamage>().invincible = false;
        }
    }
}

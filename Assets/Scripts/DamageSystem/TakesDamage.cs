using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TakesDamage : MonoBehaviour
{
    public float maxHP;             // Max # of health object can have
    public bool invincible;         // If true the player will not take damage
    public bool isEnemy;            // Is this object considered an enemy or a player (used by DealsDamage)
    public int iFrames;             // Number of invincibility frames after taking damage
    public bool boomerangStunOnly;  // Does this thing get killed by boomerangs or just stunned
    [SerializeField] protected AudioClip damageSoundEffect;
    [SerializeField] protected AudioClip deathSoundEffect;

    private float curHP;            // Current number of health points, < maxHP
    private bool isStunned;         // Is the object currently stunned?
    private int iFramesRemaining;   // Number of frames invincibility left, at 0 is disabled
    private int stunFramesRemaining;// Number of frames left where controls are disabled
    private CharacterUsesUI uiRef;  // Characters (Link specifically) will have their health show up in the UI

    // Start is called before the first frame update
    void Start()
    {
        curHP = maxHP;
        stunFramesRemaining = 0;
        iFramesRemaining = 0;
        uiRef = GetComponent<CharacterUsesUI>();
        if (uiRef != null )
        {
            uiRef.setHealth(curHP);
        }
    }

    void Update()
    {
        // Update iFrames and stun frames
        if (iFramesRemaining > 0)
        {
            --iFramesRemaining;
        }
        if (stunFramesRemaining > 0)
        {
            --stunFramesRemaining;
            GetComponent<GenericMovement>().movementEnabled = false;
        } else
        {
            if (isStunned)
            {
                isStunned = false;
                GetComponent<GenericMovement>().movementEnabled = true;
            }
        }
    }

    // Returns the number of health points the object has
    public float GetHP()
    {
        return curHP;
    }

    // Deal damage to the object, decreasing its health, killing it if it goes below 0
    public void Damage(float damagePoints)
    {
        if (invincible || iFramesRemaining > 0)
        {
            return;
        }
        curHP -= damagePoints;
        if (curHP < 0)
        {
            curHP = 0;
        }
        // Update UI
        if (uiRef != null)
        {
            uiRef.setHealth(curHP);
        }
        if (curHP == 0)
        {
            if (gameObject.CompareTag("Player"))
            {
                AudioSource.PlayClipAtPoint(deathSoundEffect, Camera.main.transform.position);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                AudioSource.PlayClipAtPoint(deathSoundEffect, Camera.main.transform.position);
                GameObject.FindGameObjectWithTag("Player").GetComponent<RoomManager>().recieveEnemyDeath();
                Destroy(gameObject);
            }
        } else {
            AudioSource.PlayClipAtPoint(damageSoundEffect, Camera.main.transform.position);
        }
        iFramesRemaining = iFrames;
    }

    // Deal stun frames to the object, locking controls for the duration
    public void Stun(int stunFrames)
    {
        if (stunFrames < 1 || invincible || iFramesRemaining > 0)
        {
            return;
        } else
        {
            stunFramesRemaining = stunFrames;
            isStunned = true;
            GetComponent<GenericMovement>().movementEnabled = false;
        }
    }

    // Heal the object, increasing its health (up to healthPoints which is maximum)
    public void Heal(float healPoints)
    {
        curHP += healPoints;
        if (curHP > maxHP)
        {
            curHP = maxHP;
        }
        // Update UI
        if (uiRef != null)
        {
            uiRef.setHealth(curHP);
        }
    }
}

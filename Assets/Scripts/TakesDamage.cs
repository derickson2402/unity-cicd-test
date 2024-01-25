using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakesDamage : MonoBehaviour
{
    public float maxHP;             // Max # of health object can have
    public bool invincible;         // If true the player will not take damage
    public bool isEnemy;            // Is this object considered an enemy or a player (used by DealsDamage)
    public int iFrames;             // Number of invincibility frames after taking damage
    [SerializeField] protected AudioClip damageSoundEffect;
    [SerializeField] protected AudioClip deathSoundEffect;

    private float curHP;            // Current number of health points, < maxHP
    private int curIFrames;         // Number of frames invincibility has been active for
    private bool iFrameActive;     // Are invincibility frames active?

    // TODO: allow configuration of special thing to do on death


    // Start is called before the first frame update
    void Start()
    {
        curHP = maxHP;
    }

    void Update()
    {
        // Count iframes and turn them off when finished
        if (iFrameActive)
        {
            if (curIFrames >= iFrames)
            {
                // Turn off iFrames
                iFrameActive = false;
            }
            ++curIFrames;
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
        if (invincible || iFrameActive)
        {
            return;
        }
        curHP -= damagePoints;
        if (curHP <= 0)
        {
            // We have died
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(deathSoundEffect, Camera.main.transform.position);
            // Check if we need to drop an object
            DropsItemOnDeath itemDrop = GetComponent<DropsItemOnDeath>();
            if (itemDrop != null)
            {
                itemDrop.Drop();
            }
            // TODO: allow configuration of special thing to do on death
        } else {
            AudioSource.PlayClipAtPoint(damageSoundEffect, Camera.main.transform.position);
        }
        iFrameActive = true;
        curIFrames = 0;
    }

    // Heal the object, increasing its health (up to healthPoints which is maximum)
    public void Heal(float healPoints)
    {
        curHP += healPoints;
        if (curHP > maxHP)
        {
            curHP = maxHP;
        }
    }
}

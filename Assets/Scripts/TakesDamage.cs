using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakesDamage : MonoBehaviour
{
    public float maxHP;             // Max # of health object can have
    public bool isEnemy;            // Is this object considered an enemy or a player (used by DealsDamage)
    public int iFrames;             // Number of invincibility frames after taking damage
    public float curHP;             // Current number of hit points left
    [SerializeField] protected AudioClip damageSoundEffect;
    [SerializeField] protected AudioClip deathSoundEffect;

    // TODO: allow configuration of special thing to do on death


    // Start is called before the first frame update
    void Start()
    {
        curHP = maxHP;
    }

    // Deal damage to the object, decreasing its health, killing it if it goes below 0
    public void Damage(float damagePoints)
    {
        curHP -= damagePoints;
        if (curHP <= 0)
        {
            // We have died
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(deathSoundEffect, Camera.main.transform.position);
            // TODO: allow configuration of special thing to do on death
        } else {
            AudioSource.PlayClipAtPoint(damageSoundEffect, Camera.main.transform.position);
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
    }
}

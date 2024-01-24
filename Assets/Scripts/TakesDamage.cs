using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakesDamage : MonoBehaviour
{
    public float maxHP;             // Max # of health object can have
    public bool affectedByPlayer;   // Should this take damage from players?
    public bool affectedByEnemy;    // Should this take damage from enemies?
    public int iFrames;             // Number of invincibility frames after taking damage
    // TODO: allow configuration of special thing to do on death

    private float curHP;    // Current number of hit points left

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
            // TODO: allow configuration of special thing to do on death
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

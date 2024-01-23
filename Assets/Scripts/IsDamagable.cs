using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDamagable : MonoBehaviour
{
    public int hitPoints;       // Number of hit points in half-hearts

    private int curHitPoints;   // Current number of hit points left

    // Start is called before the first frame update
    void Start()
    {
        curHitPoints = hitPoints;
    }

    // Called by projectiles and weapons to deal damage
    public void Damage(int hitPoints)
    {
        curHitPoints -= hitPoints;
        if (curHitPoints < 1)
        {
            // We have died
            Destroy(gameObject);
        }
    }
}

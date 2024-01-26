using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTypeSword : Projectile
{
    public override void PostCollision()
    {
        Debug.Log("Sword beam explosion");
    }
}

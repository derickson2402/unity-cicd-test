using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealsDamage : MonoBehaviour
{
    public float damageHP;      // How many health points this object deals to TakesDamage
    public bool affectPlayer;   // Should this deal damage to players?
    public bool affectEnemy;    // Should this deal damage to enemies?

    public void OnCollisionEnter(Collision collision)
    {
        bool collidedWithEnemy = false;
        //GameObject collisionTakesDamageObj = collision.gameObject.GetComponent<TakesDamage>();
        //if (collisionTakesDamageObj != null) {
        //    bool otherIsEnemy = collisionTakesDamageObj.isEnemy;
        //    if ((otherIsEnemy && affectEnemy) || (!otherIsEnemy && affectPlayer))
        //    {
        //        collisionTakesDamageObj.Damage(damageHP);
        //    }
        //}
    }
}

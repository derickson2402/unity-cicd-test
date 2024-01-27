using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealsDamage : MonoBehaviour
{
    public float damageHP;      // How many health points this object deals to TakesDamage
    public bool affectPlayer;   // Should this deal damage to players?
    public bool affectEnemy;    // Should this deal damage to enemies?
    public int attackDelayFrames; // How many frames to delay before firing/destroying. Used by WeaponInterface

    private void OnCollisionEnter(Collision collision)
    {
        // Deal damage to the collided object
        TakesDamage other = collision.gameObject.GetComponent<TakesDamage>();
        if (other != null)
        {
            if ((other.isEnemy && affectEnemy) || (!other.isEnemy && affectPlayer)) {
                other.Damage(damageHP);
            }
        }

        // Check if we are a projectile, in which case we should delete ourselves on impact
        Projectile projectile = GetComponent<Projectile>();
        if (projectile != null) {
            Debug.Log("Projectile destroyed");
            projectile.PostCollision();
            Destroy(gameObject);
        }
    }
}

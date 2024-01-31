using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealsDamage : MonoBehaviour
{
    public float damageHP;      // How many health points this object deals to TakesDamage
    public bool affectPlayer;   // Should this deal damage to players?
    public bool affectEnemy;    // Should this deal damage to enemies?
    public int attackDelayFrames; // How many frames to delay before firing/destroying. Used by WeaponInterface
    public float spawnOffsetDistance; // How far from the character should this be spawned on use?
    public float knockBackDistance; // How far should the affected character be knocked back when hit?
    public int stunFrames;      // How many frames of stun damage does this thing deal?

    private void OnCollisionEnter(Collision collision)
    {
        ProcessInteraction(collision.gameObject.GetComponent<TakesDamage>());
    }

    private void OnTriggerEnter(Collider collider)
    {
        ProcessInteraction(collider.gameObject.GetComponent<TakesDamage>());
    }

    private void ProcessInteraction(TakesDamage other)
    {
        if (other != null)
        {
            if ((other.isEnemy && affectEnemy) || (!other.isEnemy && affectPlayer))
            {
                // Boomerangs only stun certain things
                WeaponTypeBoomerang boomerang = GetComponent<WeaponTypeBoomerang>();
                if (boomerang == null)
                {
                    Debug.Log(gameObject + " damaged " + other);
                    other.Damage(damageHP);
                }
                else
                {
                    if (other.boomerangStunOnly)
                    {
                        Debug.Log(gameObject + " stunned " + other);
                        other.Stun(stunFrames);
                    } else
                    {
                        Debug.Log(gameObject + " damaged " + other);
                        other.Damage(damageHP);
                    }
                }
            }
        }

        // Check if we are a projectile, in which case we should delete ourselves on impact
        Projectile projectile = GetComponent<Projectile>();
        if (projectile != null)
        {
            if (projectile.InFlight())
            {
                Debug.Log("Projectile destroyed");
                projectile.PostCollision();
                Destroy(gameObject);
            }
        }
    }
}

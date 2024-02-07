using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealsDamageBouncer : DealsDamage
{
    public int bounceLimit = 6;
    private int bounceCount = 0;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        return;
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        ProcessInteraction(collider);
    }

    protected override void ProcessInteraction(Collider collider)
    {
        TakesDamage other = collider.gameObject.GetComponent<TakesDamage>();
        Projectile projectile = GetComponent<Projectile>();
        if (other != null)
        {
            if ((other.isEnemy && affectEnemy) || (!other.isEnemy && affectPlayer))
            {
                Debug.Log(gameObject + " pierced " + other);
                other.Damage(damageHP);
            }
        }
        else
        {
            // if wall bounce
            if (collider.CompareTag("Wall") && bounceCount < bounceLimit)
            {
                // bounce logic
                bounceCount++;
                Vector3 direction = (collider.gameObject.transform.position - transform.position).normalized;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction, out hit))
                {
                    Vector3 reflectedVelocity = Vector3.Reflect(rb.velocity, hit.normal);
                    rb.velocity = reflectedVelocity;
                }
            }
            else
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
}

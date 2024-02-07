using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealsDamageBouncer : DealsDamage
{
    public int bounceLimit = 6;
    public float lifetimeMax;
    private int bounceCount = 0;
    private Rigidbody rb;
    private float currentLifetime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        currentLifetime += Time.deltaTime;
        if (currentLifetime > lifetimeMax)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall") && bounceCount < bounceLimit)
        {
            // bounce logic
            Vector3 incomingVec = collision.relativeVelocity; // vector direction of incoming object
            Vector3 normalVec = collision.contacts[0].normal; // collision surface normal
            rb.velocity = Vector3.Reflect(incomingVec, normalVec).normalized * incomingVec.magnitude; //set the direction of new velocity
        }
        else if (bounceCount > bounceLimit)
        {
            Projectile projectile = GetComponent<Projectile>();
            if (projectile.InFlight())
            {
                Debug.Log("Projectile destroyed");
                projectile.PostCollision();
                Destroy(gameObject);
            }
        }
        bounceCount++;
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        TakesDamage other = collider.gameObject.GetComponent<TakesDamage>();
        if (other != null)
        {
            if ((other.isEnemy && affectEnemy) || (!other.isEnemy && affectPlayer))
            {
                Debug.Log(gameObject + " pierced " + other);
                other.Damage(damageHP);
            }
        }
    }
}

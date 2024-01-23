using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float flySpeed = 1.0f;   // Speed the projectile should move at
    public int damageAmount = 1;    // Damage (in half-hearts) the projectile will do on impact

    private bool inFlight;          // Is the projectile flying through the air?
    private Vector3 moveVec;        // Movement vector describing the projectiles motion (0 vector if not inFlight)
    private Rigidbody rb;           // Rigid body member of the projectile

    // Start is called before the first frame update
    void Start()
    {
        inFlight = false;
        moveVec = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        Debug.Log("New projectile created");
    }

    // Tells the projectile it is now in flight and to start moving in the given direction.
    // Throws exception if already in flight
    public void Shoot(Vector3 direction)
    {
        if (inFlight)
        {
            throw new UnityException("Projectile already in flight");
        } else
        {
            inFlight = true;
            // Calculate the movement vector in the given diretion at the given speed
            moveVec = direction.normalized * flySpeed;
            rb.velocity = moveVec;
            Debug.Log("Projectile fired in direction" + moveVec);
        }
    }

    // On collision, destroy ourselves. If we hit an enemy, damage them
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<IsDamagable>().Damage(damageAmount);
        }
        Destroy(gameObject);
    }
}

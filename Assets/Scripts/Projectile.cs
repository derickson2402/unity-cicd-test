using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public float flySpeed = 1.0f;   // Speed the projectile should move at
    public int damageAmount = 1;    // Damage (in half-hearts) the projectile will do on impact

    private bool inFlight;          // Is the projectile flying through the air?
    private Direction direction;    // Cardinal direction projectile will travel
    private Vector3 moveVec;        // Movement vector describing the projectiles motion (0 vector if not inFlight)
    private Rigidbody rb;           // Rigid body member of the projectile

    // Start is called before the first frame update
    void Start()
    {
        inFlight = false;
        direction = Direction.North;
        moveVec = Vector3.zero;
        rb = GetComponent<Rigidbody>();
    }

    // Tells the projectile it is now in flight and to start moving in the given direction.
    // Throws exception if already in flight
    public void Shoot(Direction direction)
    {
        if (inFlight)
        {
            throw new UnityException("Projectile already in flight");
        } else
        {
            inFlight = true;
            this.direction = direction;
            // Set the movement vector accordingly
            switch (direction)
            {
                case Direction.North:
                    moveVec = new Vector3(0, 1);
                    break;
                case Direction.South:
                    moveVec = new Vector3(0, -1);
                    break;
                case Direction.East:
                    moveVec = new Vector3(1, 0);
                    break;
                case Direction.West:
                    moveVec = new Vector3(-1, 0);
                    break;
            }
            // Scale by the speed (note since using cardinal directions, magnitude already 1
            moveVec *= flySpeed;
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

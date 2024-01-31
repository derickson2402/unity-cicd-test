using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WeaponTypeBoomerang : Projectile
{
    public float flyDistance;       // How far the boomerang will fly (in tiles) before turning around
    public int turnFrames;          // How far the boomerang will take (in frames) to turn fully around

    private Vector3 startPos;       // Start vec of the boomerang throw for tracking distance
    private int turnFramesTraversed; // How many frames has the boomerang been turning for
    private Rigidbody throwerRB;    // Rigidbody reference for the thrower
    private State state;            // State obj of the boomerang, tracks what motion it will use

    private enum State
    {
        OUT,        // Initial movement at constant flySpeed and Shoot(direction) for flyDistance tiles
        TURNING,    // Slowdown period at constant acceleration decreasing speed [flySpeed, -flySpeed] and Shoot(direction) for turnDistance tiles each way
        RETURN,      // Return period at constant flySpeed in direction of throwerRB
        OFF
    }

    void Start()
    {
        state = State.OFF;
    }

    // Special method for boomerang which must be called before throwing. Gives
    // the boomerang reference back to thrower to know how to return to them
    public void RefToThrower(Rigidbody thrower)
    {
        throwerRB = thrower;
    }

    public override void Shoot(Vector3 direction)
    {
        // Must have a reference to the thrower to call this
        if (throwerRB == null)
        {
            Debug.Log("Boomerang thrown but not given reference to thrower");
        }
        if (inFlight)
        {
            throw new UnityException("Projectile already in flight");
        }
        else
        {
            inFlight = true;
            state = State.OUT;
            // Calculate the starting move vector in the given direction at the given speed
            moveVec = direction.normalized * flySpeed;
            rb.velocity = moveVec;
            startPos = gameObject.GetComponent<Rigidbody>().position;
            Debug.Log("Projectile fired in direction" + moveVec);
        }
    }

    // Boomerang in 3 states. Initial movement at constant velocity and
    // direction for flyDistance tiles, slowdown period for turnDistance
    // tiles with decreasing velocity, speedup period for turnDistance tiles
    // with increasing velocity and now following back to thrower, and finally
    // constant velocity back toward thrower. Depending on st
    void Update()
    {
        switch (state)
        {
            case State.OFF:
                break;
            case State.OUT:
                // Constant velocity already set, track how far we've gone
                if (Vector3.Distance(startPos, rb.position) > flyDistance)
                {
                    state = State.TURNING;
                }
                break;
            case State.TURNING:
                // Check if we have reversed directions
                if (turnFrames < turnFramesTraversed)
                {
                    state = State.RETURN;
                    rb.velocity = -(moveVec.normalized) * flySpeed;
                } else
                {
                    ++turnFramesTraversed;
                    rb.velocity += -(moveVec.normalized) * (2 * flySpeed * (1 / turnFrames));
                }
                break;
            case State.RETURN:
                // Calculate how to return to the original thrower
                rb.velocity = flySpeed * (throwerRB.position - rb.position).normalized;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject == throwerRB.gameObject) && (state == State.RETURN))
        {
            // Returned to the player, destroy ourselves
            Debug.Log(throwerRB.gameObject + " caught " + gameObject);
            Destroy(gameObject);
        }
    }
}

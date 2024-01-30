using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class MovementManager : GenericMovement
{
    private int counter = 0;
    protected void FixedUpdate()
    {
        counter++;
        if (counter == 15)
        {
            Debug.Log("Current player velocity: " + rb.velocity);
            counter = 0;
        }
    }

    public override void Move(Direction input)
    {
        Debug.Log("Player direction before moving: " + directionManager.current);
        //NOTE: needs restructuring if the player is meant to have velocity and movement disabled at some point
        if (!movementEnabled || (rb.velocity.magnitude < stopThreshold && input == Direction.None))
        {
            rb.velocity = Vector3.zero;
        }
        else if (input == Direction.None)
        {
            rb.velocity /= naturalDeaccelerationFactor;
        }
        // If player is not trying to change direction, apply velocity normally
        else if (directionManager.isCurrentDirection(input))
        {
            rb.velocity = DirectionManager.DirectionToVector3(input) * movementSpeed;
        }
        else
        {
            // If player is trying to change direction, snap position and change direction
            Debug.Log("Trying to change directions to " + input);
            //only snap to grid and change direction if nearly stopped
            if (rb.velocity.magnitude < changeDirectionThreshold)
            {
                // prevent more inputs being made during coroutine
                movementEnabled = false;
                rb.velocity = Vector3.zero;
                Vector3 gridPosition = SnapPositionToGrid(rb.position);
                StartCoroutine(
                    CoroutineHelper.MoveCharacterOverTime(transform, rb.position, gridPosition, gridAlignmentDurationSeconds, input));
            }
            else
            {
                // If player is moving too fast for a direction change, slow down
                Debug.Log("Slowing down");
                rb.velocity /= activeDeaccelerationFactor;
            }
        }
        Debug.Log("Player direction after moving: " + directionManager.current);
    }
}

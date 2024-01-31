using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : GenericMovement
{
    Vector3 initial;

    private void Awake()
    {
        initial = transform.position;
    }
    public override void Move(Direction input)
    {
        if (!movementEnabled)
        {
            rb.velocity = Vector3.zero;
        }
        else if(transform.position.y - initial.y >= 2)
        {
            movementEnabled = false;
        }
        // If player is not trying to change direction, apply velocity normally
        else if (directionManager.isCurrentDirection(Direction.Up))
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
    }
}

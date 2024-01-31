using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class GenericMovement : MonoBehaviour
{
    [SerializeField] public float movementSpeed = 2f;
    [SerializeField] protected float activeDeaccelerationFactor = 10f;
    [SerializeField] protected float naturalDeaccelerationFactor = 4f;
    [SerializeField] protected float gridAlignmentDurationSeconds = 0.03f;

    [SerializeField] protected float changeDirectionThreshold = 0.17f;
    [SerializeField] protected float stopThreshold = 0.0625f;

    [SerializeField] protected float gridSize = 0.5f;

    public bool movementEnabled;
    protected Rigidbody rb;
    public DirectionManager directionManager = new();

    void Start()
    {
        movementEnabled = false;
        rb = GetComponent<Rigidbody>();
        directionManager.changeDirection(Direction.Down);
    }

    protected Vector3 SnapPositionToGrid(Vector3 position)
    {
        // Snap horizontally and vertically
        Vector3 gridPosition = new(Mathf.Round(position.x / gridSize) * gridSize, Mathf.Round(position.y / gridSize) * gridSize, 0);
        Debug.Log("Snapping position to " + gridPosition + "from " + position);
        return gridPosition;
    }

    public virtual void Move(Direction input)
    {
        if (!movementEnabled)
        {
            rb.velocity = Vector3.zero;
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
    }

    //used for applying velocity after coroutine finishes
    public void ChangeDirection(Direction input)
    {
        rb.velocity = DirectionManager.DirectionToVector3(input) * movementSpeed;
        directionManager.changeDirection(input);
        movementEnabled = true;
        // Handle animations
        ScriptAnim4DirectionWalkPlusAttack anim = GetComponent<ScriptAnim4DirectionWalkPlusAttack>();
        if (anim != null)
        {
            anim.ChangeDirection(input);
        }
    }
}

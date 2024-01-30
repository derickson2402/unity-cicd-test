using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class GenericMovement : MonoBehaviour
{
    [SerializeField] protected float movementSpeed = 0.28f;
    [SerializeField] protected float easeFactor = 0.1f;
    [SerializeField] protected float accelerationFactor = 0.3f;
    [SerializeField] protected float deaccelerationFactor = 0.8f;
    [SerializeField] protected float changeDirectionThreshold = 0.0625f;
    [SerializeField] protected float stopThreshold = 0.0625f;
    [SerializeField] protected LayerMask collisionLayer;
    [SerializeField] protected float gridSize = 0.5f;

    protected BoxCollider boxCollider;
    protected Rigidbody rb;
    protected Vector3 desiredPosition;
    protected DirectionManager directionManager = new DirectionManager();
    protected Vector3 desiredVelocity;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        directionManager.current = Direction.Down;
        //desiredPosition = transform.position;
        desiredVelocity = Vector2.zero;
    }

    protected virtual void FixedUpdate()
    {
        LerpToDestination();
    }

    protected void LerpToDestination()
    {
        // Interpolate velocity
        if (desiredVelocity.Equals(Vector3.zero))
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, desiredVelocity, Time.fixedDeltaTime * deaccelerationFactor);
        }
        if ((desiredVelocity - rb.velocity).magnitude > stopThreshold)
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, desiredVelocity, Time.fixedDeltaTime * accelerationFactor);
        }
        else
        {
            rb.velocity = Vector3.zero;
            desiredVelocity = Vector3.zero;
        }
    }

    protected Vector3 SnapVelocityToGrid(Vector3 velocity, Direction newDirection)
    {
        // Are we moving horizontally or vertically?
        if (newDirection is Direction.Left or Direction.Right)
        {
            // Snap horizontally
            Vector3 gridVelocity = new Vector3(Mathf.Round(velocity.x / gridSize) * gridSize, velocity.y, 0);
            Debug.Log("Snapping velocity to " + gridVelocity + "from " + velocity);
            return gridVelocity;
        }
        else
        {
            // Snap vertically
            Vector3 gridVelocity = new Vector3(velocity.x, Mathf.Round(velocity.y / gridSize) * gridSize, 0);
            Debug.Log("Snapping velocity to " + gridVelocity + "from " + velocity);
            return gridVelocity;
        }
    }

    protected Vector3 SnapPositionToGrid(Vector3 position, Direction newDirection)
    {
        // Are we moving horizontally or vertically?
        if (newDirection is Direction.Left or Direction.Right)
        {
            // Snap horizontally
            Vector3 gridPosition = new Vector3(position.x, Mathf.Round(position.y / gridSize) * gridSize, 0);
            Debug.Log("Snapping position to " + gridPosition + "from " + position);
            return gridPosition;
        }
        else
        {
            // Snap vertically
            Vector3 gridPosition = new Vector3(Mathf.Round(position.x / gridSize) * gridSize, position.y, 0);
            Debug.Log("Snapping position to " + gridPosition + "from " + position);
            return gridPosition;
        }
    }

    public virtual void Move(Direction input)
    {
        if (input == Direction.None)
        {
            desiredVelocity = Vector3.zero;
        }
        // If player is not trying to change direction, apply velocity normally
        else if (directionManager.isCurrentDirection(input))
        {
            desiredVelocity = DirectionManager.DirectionToVector3(input) * movementSpeed;
        }
        // If player is trying to change direction, snap position and change direction
        else
        {
            Debug.Log("Trying to change directions to " + input);
            //only snap to grid and change direction if nearly stopped
            if (rb.velocity.magnitude < changeDirectionThreshold)
            {
                rb.position = SnapPositionToGrid(rb.position, input);
                desiredVelocity = DirectionManager.DirectionToVector3(input) * movementSpeed;
                //TODO: change animations here
                directionManager.current = input;
            }
            else
            {
                Debug.Log("Slowing down");
                desiredVelocity = Vector3.zero;
            }
        }
    }

    protected void EaseToDestination()
    {
        if ((desiredPosition - transform.position).magnitude > float.Epsilon)
        {
            Vector3 difference = (desiredPosition - transform.position);
            transform.position += difference * easeFactor;
        }
    }

    //public void MoveOld2(Vector2 input)
    //{
    //    if (input == currentDirection)
    //    {
    //        rb.velocity = input * movementSpeed;
    //    }
    //    else
    //    {
    //        rb.position = SnapToGrid(rb.position);
    //        currentDirection = input;
    //        rb.velocity = input * movementSpeed;
    //    }
    //}

    //public void MoveOld(Vector2 input)
    //{
    //    int xInput = (int)input.x;
    //    int yInput = (int)input.y;
    //    Vector2 start = transform.position;

    //    Vector2 end = start;
    //    end.x += xInput * movementSpeed;
    //    end.y += yInput * movementSpeed;
    //    end = SnapToGrid(end);

    //    //disabling to prevent hitting our own collider
    //    boxCollider.enabled = false;

    //    RaycastHit2D hit = Physics2D.Linecast(start, end, collisionLayer);

    //    boxCollider.enabled = true;

    //    if (hit.transform == null)
    //    {
    //        desiredPosition = end;
    //    }
    //}
}

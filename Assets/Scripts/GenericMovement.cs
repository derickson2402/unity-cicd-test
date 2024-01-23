using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMovement : MonoBehaviour
{
    public float movementSpeed = 0.28f;
    public float easeFactor = 0.1f;
    public LayerMask collisionLayer;
    public float gridSize = 0.5f;

    protected BoxCollider boxCollider;
    protected Rigidbody rb;
    protected Vector3 desiredPosition;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        desiredPosition = transform.position;
    }

    void Update()
    {
        EaseToDestination();
    }

    //put into a function, so it can be easily switched for testing
    protected void EaseToDestination()
    {
        if ((desiredPosition - transform.position).magnitude > float.Epsilon)
        {
            Vector3 difference = (desiredPosition - transform.position);
            transform.position += difference * easeFactor;
        }
    }

    protected Vector2 SnapToGrid(Vector2 current)
    {
        return new Vector2(Mathf.Round(current.x / gridSize) * gridSize, Mathf.Round(current.y / gridSize) * gridSize);
    }

    public void Move(Vector2 input)
    {
        int xInput = (int)input.x;
        int yInput = (int)input.y;
        Vector2 start = transform.position;

        Vector2 end = start;
        end.x += xInput * movementSpeed;
        end.y += yInput * movementSpeed;
        end = SnapToGrid(end);

        //disabling to prevent hitting our own collider
        boxCollider.enabled = false;

        RaycastHit2D hit = Physics2D.Linecast(start, end, collisionLayer);

        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            desiredPosition = end;
        }
    }
}

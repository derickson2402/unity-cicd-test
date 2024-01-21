using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    /* Inspector Tunables */
    public float ease_factor = 0.1f;
    public LayerMask collisionLayer;
    public float gridSize = 0.5f;

    /* Private Data */
    private BoxCollider boxCollider;
    private Rigidbody rb;
    private Vector3 desiredPosition;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        desiredPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        desiredPosition = SnapToGrid(desiredPosition);
        transform.position += (desiredPosition - transform.position) * ease_factor;
    }

    private Vector2 SnapToGrid(Vector2 current)
    {
        return new Vector2(Mathf.Round(current.x / gridSize) * gridSize, Mathf.Round(current.y / gridSize) * gridSize);
    }

    //xInput/yInput will be 1, 0, or -1 to indicate movement direction
    public bool Move (Vector2 input, float movementSpeed)
    {
        int xInput = (int)input.x;
        int yInput = (int)input.y;
        Vector2 start = transform.position;

        Vector2 end = start;
        end.x += xInput * movementSpeed;
        end.y += yInput * movementSpeed;

        //disabling to prevent hitting our own collider
        boxCollider.enabled = false;

        RaycastHit2D hit = Physics2D.Linecast(start, end, collisionLayer);

        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            desiredPosition = end;
            return true;
        }

        //ray hit something, move cannot be made
        return false;
    }
}

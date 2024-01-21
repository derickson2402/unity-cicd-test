using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    /* Inspector Tunables */
    public float ease_factor = 0.1f;
    public LayerMask collisionLayer;

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
        transform.position += (desiredPosition - transform.position) * ease_factor;
    }

    public bool Move (Vector2 currentInput)
    {
        Vector2 start = transform.position;
        //desiredPosition = start;

        Vector2 end = start + 0.125 * (currentInput / 0.125);

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

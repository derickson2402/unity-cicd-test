using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridMovement : MonoBehaviour
{
    /* Inspector Tunables */
    public float easeFactor = 0.1f;
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
        //desiredPosition = SnapToGrid(desiredPosition);
        if ((desiredPosition - transform.position).magnitude > float.Epsilon)
        {
            transform.position += (desiredPosition - transform.position) * easeFactor;
            //rb.MovePosition(desiredPosition);
            //StartCoroutine(MoveObjectOverTime(desiredPosition));
        }
    }

    //IEnumerator MoveObjectOverTime(Vector3 desiredPosition)
    //{
    //    float initialTime = Time.time;
    //    float progress = (Time.time - initialTime) / moveTime;
    //    while (progress < 1.0f)
    //    {
    //        progress = (Time.time - initialTime) / moveTime;
    //        Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, progress);
    //        rb.MovePosition(newPosition);
    //        yield return null;
    //    }
    //}

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
        end = SnapToGrid(end);

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

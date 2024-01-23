using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public Camera mainCamera;
    public float movementSpeed = 0.28f;
    public float easeFactor = 0.1f;
    public LayerMask collisionLayer;
    public float gridSize = 0.5f;

    private InputManager inputManager;
    private BoxCollider boxCollider;
    private Rigidbody rb;
    private Vector3 desiredPosition;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        desiredPosition = transform.position;
    }

    void Update()
    {
        EaseToDestination();
    }

    //put into a function so it can be easily switched for testing
    private void EaseToDestination()
    {
        if ((desiredPosition - transform.position).magnitude > float.Epsilon)
        {
            transform.position += (desiredPosition - transform.position) * easeFactor;
        }
    }

    private Vector2 SnapToGrid(Vector2 current)
    {
        return new Vector2(Mathf.Round(current.x / gridSize) * gridSize, Mathf.Round(current.y / gridSize) * gridSize);
    }

    public bool OldMove(Vector2 input)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "West_Door")
        {
            Debug.Log("starting West transition");
            RoomTransition(other.transform.position, new Vector3(-1f, 0, 0), new Vector3(-16f, 0, 0));
        }
        else if (other.tag == "East_Door")
        {
            Debug.Log("starting East transition");
            RoomTransition(other.transform.position, new Vector3(1f, 0, 0), new Vector3(16, 0, 0));
        }
        else if (other.tag == "North_Door")
        {
            Debug.Log("starting North transition");
            RoomTransition(other.transform.position, new Vector3(0, 1f, 0), new Vector3(0, 16, 0));
        }
        else if (other.tag == "South_Door")
        {
            Debug.Log("starting South transition");
            RoomTransition(other.transform.position, new Vector3(0, -1f, 0), new Vector3(0, -16, 0));
        }
    }

    void RoomTransition(Vector3 doorPosition, Vector3 playerMovement, Vector3 cameraMovement)
    {
        rb.velocity = Vector3.zero;
        rb.gameObject.SetActive(false);
        boxCollider.enabled = false;
        inputManager.controlEnabled = false;
        Debug.Log("Player control removed");
        Debug.Log(transform.position);

        Vector3 doorFramePosition = doorPosition + (1f * playerMovement);
        StartCoroutine(CoroutineHelper.MoveObjectOverTime(transform, transform.position, doorFramePosition, 1f));
        Debug.Log("Player moved into doorframe");
        Debug.Log(transform.position);

        Vector3 newCameraPosition = mainCamera.transform.position + cameraMovement;
        StartCoroutine(CoroutineHelper.MoveObjectOverTime(mainCamera.transform, mainCamera.transform.position, newCameraPosition, 2.5f));
        Debug.Log("Camera moved into new room");
        Debug.Log(transform.position);

        Vector3 outerDoorPosition = doorPosition + (1f * playerMovement);
        StartCoroutine(CoroutineHelper.MoveObjectOverTime(transform, transform.position, outerDoorPosition, 1f));
        Debug.Log("Player moved into new room");
        Debug.Log(transform.position);

        rb.gameObject.SetActive(true);
        boxCollider.enabled = true;
        inputManager.controlEnabled = true;
        Debug.Log("Player control returned");
        Debug.Log(transform.position);
    }
}

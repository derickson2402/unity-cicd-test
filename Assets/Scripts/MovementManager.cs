using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementManager : GenericMovement
{
    public Camera mainCamera;

    private InputManager inputManager;
    private PlayerController player;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        player = GetComponent<PlayerController>();
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        desiredPosition = transform.position;
    }

    void Update()
    {
        EaseToDestination();
    }

    //doesn't work right, supposed to be a smoother version of current movement
    //come back to later if Move3 is still broken
    public void Move2(Vector2 input)
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
            StartCoroutine(CoroutineHelper.MoveObjectOverTime(transform, transform.position, end, 1f));
        }
    }

    //doesn't work right, supposed to be more accurate recreation
    //come back to later
    public void Move3(Vector2 input)
    {
        Vector2 start = transform.position;
        int xInput = (int)input.x;
        int yInput = (int)input.y;
        float xMisalignment = Mathf.Abs(start.x % gridSize);
        float yMisalignment = Mathf.Abs(start.y % gridSize);
        bool xOnGrid = xMisalignment < float.Epsilon;
        bool yOnGrid = yMisalignment < float.Epsilon;

        Debug.Log("idk man: " + (Mathf.Abs(start.x % gridSize)).ToString() + " " + (Mathf.Abs(start.y % gridSize)).ToString());

        Debug.Log("Starting movement" +
            "\ninitialPosition: " + start.ToString() +
            "\nxMisalignment: " + xMisalignment.ToString() + 
            "\nyMisalignment: " + yMisalignment.ToString() + 
            "\nxOnGrid: " + xOnGrid.ToString() + 
            "\nyOnGrid: " + yOnGrid.ToString());

        Vector2 newMovement = Vector2.zero;

        // is an x and y coordinate of 0.5 multiples
        if (xOnGrid && yOnGrid)
        {
            if (xInput != 0 && yInput != 0)
            {
                // generate random number of 0 or 1 to decide whether to move in y or x first
                int randomValue = Random.Range(0, 2);
                if (randomValue == 0)
                {
                    newMovement += new Vector2(xInput, 0);
                    Debug.Log("RandomMove, moving X");
                }
                else
                {
                    newMovement += new Vector2(0, yInput);
                    Debug.Log("RandomMove, moving Y");
                }
            }
            else if (xInput != 0)
            {
                newMovement += new Vector2(xInput, 0);
                Debug.Log("Both on grid, moving X");
            }
            else if (yInput != 0)
            {
                newMovement += new Vector2(0, yInput);
                Debug.Log("Both on grid, moving Y");
            }
        }
        // is not a x coordinate of 0.5 multiples
        else if (xOnGrid)
        {
            float distanceToGridX = Mathf.Round(start.x) - start.x;
            if (xInput != 0)
            {
                newMovement += new Vector2(xInput, 0);
                Debug.Log("X on grid, moving X");
            }
            else if (yInput != 0)
            {
                if (distanceToGridX > 0)
                {
                    newMovement += Vector2.right;
                    Debug.Log("X on grid, moving right to hit Y grid");
                }
                else
                {
                    newMovement += Vector2.left;
                    Debug.Log("X on grid, moving left to hit Y grid");
                }
            }
        }
        // is not a y coordinate of 0.5 multiples
        else if (yOnGrid)
        {
            float distanceToGridY = Mathf.Round(start.y) - start.y;
            if (yInput != 0)
            {
                newMovement += new Vector2(0, yInput);
                Debug.Log("Y on grid, moving X");
            }
            else if (xInput != 0)
            {
                if (distanceToGridY > 0)
                {
                    newMovement += Vector2.up;
                    Debug.Log("Y on grid, moving up to hit X grid");
                }
                else
                {
                    newMovement += Vector2.down;
                    Debug.Log("Y on grid, moving down to hit X grid");
                }
            }
        }
        //should not happen ideally
        else
        {
            float distanceToGridX = Mathf.Round(start.x) - start.x;
            float distanceToGridY = Mathf.Round(start.y) - start.y;
            if (Mathf.Abs(distanceToGridY) < Mathf.Abs(distanceToGridX))
            {
                if (distanceToGridY > 0)
                {
                    newMovement += Vector2.right;
                    Debug.Log("No grid, moving right to hit Y grid");
                }
                else
                {
                    newMovement += Vector2.left;
                    Debug.Log("No grid, moving left to hit Y grid");
                }
            }
            else
            {
                if (distanceToGridX > 0)
                {
                    newMovement += Vector2.up;
                    Debug.Log("No grid, moving up to hit X grid");
                }
                else
                {
                    newMovement += Vector2.down;
                    Debug.Log("No grid, moving down to hit X grid");
                }
            }
        }
        Vector3 desiredPosition = start + newMovement;
        StartCoroutine(CoroutineHelper.MoveObjectOverTime(transform, transform.position, desiredPosition, 1f));
        //rb.velocity = newMovement * movementSpeed;
        Debug.Log("Final velocity change: " + rb.velocity.ToString());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "West_Door")
        {
            Debug.Log("starting West transition");
            //RoomTransition(other.transform.position, new Vector3(-1f, 0, 0), new Vector3(-16f, 0, 0));
            StartCoroutine(RoomTransition(other.transform.position,
                new Vector3(-1f, 0, 0),
                new Vector3(-16, 0, 0)));
        }
        else if (other.tag == "East_Door")
        {
            Debug.Log("starting East transition");
            //RoomTransition(other.transform.position, new Vector3(1f, 0, 0), new Vector3(16, 0, 0));
            StartCoroutine(RoomTransition(other.transform.position,
                new Vector3(1f, 0, 0),
                new Vector3(16, 0, 0)));
        }
        else if (other.tag == "North_Door")
        {
            Debug.Log("starting North transition");
            //RoomTransition(other.transform.position, new Vector3(0, 1f, 0), new Vector3(0, 16, 0));
            StartCoroutine(RoomTransition(other.transform.position,
                new Vector3(0, 1f, 0),
                new Vector3(0, 16, 0)));
        }
        else if (other.tag == "South_Door")
        {
            Debug.Log("starting South transition");
            //RoomTransition(other.transform.position, new Vector3(0, -1f, 0), new Vector3(0, -16, 0));
            StartCoroutine(RoomTransition(other.transform.position, 
                new Vector3(0, -1f, 0),
                new Vector3(0, -16, 0)));
        }
    }

    IEnumerator RoomTransition(Vector3 doorPosition, Vector3 playerMovement, Vector3 cameraMovement)
    {
        rb.velocity = Vector3.zero;
        rb.detectCollisions = false;
        boxCollider.enabled = false;
        inputManager.controlEnabled = false;
        Debug.Log("Player control removed\n" + transform.position.ToString());

        Vector3 doorFramePosition = doorPosition + (1f * playerMovement);
        yield return StartCoroutine(CoroutineHelper.MoveObjectOverTime(transform, transform.position, doorFramePosition, 1f));
        Debug.Log("Player moved into door frame\n" + transform.position.ToString());

        Vector3 newCameraPosition = mainCamera.transform.position + cameraMovement;
        yield return StartCoroutine(CoroutineHelper.MoveObjectOverTime(mainCamera.transform, mainCamera.transform.position, newCameraPosition, 2.5f));
        Debug.Log("Camera moved into new room\n" + transform.position.ToString());

        Vector3 outerDoorPosition = doorPosition + (1f * playerMovement);
        yield return StartCoroutine(CoroutineHelper.MoveObjectOverTime(transform, transform.position, outerDoorPosition, 1f));
        Debug.Log("Player moved into new room\n" + transform.position.ToString());

        rb.detectCollisions = true;
        boxCollider.enabled = true;
        inputManager.controlEnabled = true;
        Debug.Log("Player control returned\n" + transform.position.ToString());
        yield return null;
    }
}

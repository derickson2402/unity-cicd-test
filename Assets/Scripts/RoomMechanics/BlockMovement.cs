using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    // Minimum and maximum values for x-coordinate.
    float minX = 0.0f;
    float maxX = 0.0f;

    // Minimum and maximum values for y-coordinate.
    float minY = 0.0f;
    float maxY = 0.0f;

    // Maximum displacements for coordinates.
    public float dispX = 1.0f;
    public float dispY = 1.0f;

    public bool triggerDoors;

    private Rigidbody rb;
    private GameObject room;
    private RoomManager rm;

    private void Start()
    {
        // For convenience.
        Vector3 pos = transform.position;

        // Set min and max values of coordinates.
        (minX, maxX) = (pos.x - dispX, pos.x + dispX);
        (minY, maxY) = (pos.y - dispY, pos.y + dispY);

        rm = GameObject.FindGameObjectWithTag("Player").GetComponent<RoomManager>();
        room = transform.parent.gameObject;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Update()
    {
        if (rm.GetCurrentRoom() != room || !rm.roomCleared)
        {
            return;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }

        // For convenience.
        Vector3 pos = transform.position;

        // Clamp object position between bounds.
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Restrict further movement if on boundaries.
        if (pos.x == minX)
        {
            maxX = minX;
            if (triggerDoors) OpenPuzzleDoors();
        }
        if (pos.x == maxX)
        {
            minX = maxX;
            if (triggerDoors) OpenPuzzleDoors();
        }
        if (pos.y == minY)
        {
            maxY = minY;
        }
        if (pos.y == maxY)
        {
            minY = maxY;
        }

        // Set object position.
        transform.position = pos;
    }

    private static void OpenPuzzleDoors()
    {
        GameObject.FindGameObjectWithTag("PuzzleDoor").GetComponent<PuzzleDoors>().Open();
    }
}
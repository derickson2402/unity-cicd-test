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

    // Minimum and maximum values for z-coordinate.
    float minZ = 0.0f;
    float maxZ = 0.0f;

    // Maximum displacements for coordinates.
    public float dispX = 1.0f;
    public float dispY = 1.0f;
    public float dispZ = 1.0f;


    private void Awake()
    {
        // For convenience.
        Vector3 pos = transform.position;

        // Set min and max values of coordinates.
        (minX, maxX) = (Mathf.Min(pos.x, pos.x + dispX), Mathf.Max(pos.x, pos.x + dispX));
        (minY, maxY) = (Mathf.Min(pos.y, pos.y + dispY), Mathf.Max(pos.y, pos.y + dispY));
        (minZ, maxZ) = (Mathf.Min(pos.z, pos.z + dispZ), Mathf.Max(pos.z, pos.z + dispZ));
    }

    private void Update()
    {
        // For convenience.
        Vector3 pos = transform.position;

        // Clamp object position between bounds.
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        // Restrict furether movement if on boundaries.
        if (pos.x == minX || pos.x == maxX)
        {
            minX = maxX;
        }
        if (pos.y == minY || pos.y == maxY)
        {
            minY = maxY;
        }
        if (pos.z == minZ || pos.z == maxZ)
        {
            minZ = maxZ;
        }

        // Set object position.
        transform.position = pos;
    }
}

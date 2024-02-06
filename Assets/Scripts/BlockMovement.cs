using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    public float minY = 0.0f;
    public float maxY = 0.0f;

    private void Awake()
    {
        minY = transform.position.y;
        maxY = transform.position.y + 1;
    }

    private void Update()
    {
        Vector3 pos = transform.position;

        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;

        if(pos.y  ==  maxY)
        {
            minY = maxY;
        }
    }
}

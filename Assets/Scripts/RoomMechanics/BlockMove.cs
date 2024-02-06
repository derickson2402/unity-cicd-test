using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    public float desiredXDiff;
    public float desiredYDiff;

    private Vector3 initialPosition;
    private Vector3 desiredPosition;
    private bool opened = false;
    void Start()
    {
        initialPosition = transform.position;
        desiredPosition = transform.position + new Vector3(desiredXDiff, desiredYDiff, 0);
    }

    void Update()
    {
        if ((desiredXDiff.Equals(0f) && desiredYDiff.Equals(0f)) || opened)
        {
            return;
        }
        else if (Vector3.Distance(transform.position, desiredPosition) < 0.15)
        {
            GetComponent<GenericMovement>().movementEnabled = false;
            StartCoroutine(CoroutineHelper.MoveObjectOverTime(transform, transform.position,
                new Vector3(initialPosition.x + desiredXDiff, initialPosition.y + desiredYDiff, 0), 0.05f));
            void EndRoom()
            {
                gameObject.GetComponentInParent<PuzzleDoors>().Open();
            }

            StartCoroutine(CoroutineHelper.DelayFunction(0.0f, EndRoom));
            opened = true;
        }
    }
}

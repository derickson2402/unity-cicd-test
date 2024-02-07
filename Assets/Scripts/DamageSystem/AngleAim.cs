using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleAim : MonoBehaviour
{
    private LineRenderer line;
    public float oscillationSpeed = 1.0f;
    public float oscillationAngle = 45f;
    public Vector3 aimDirection;
    public bool aiming = false;

    private float aimTime = 0f;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    void Update()
    {
        if (aiming)
        {
            if (!line.enabled) line.enabled = true;
            Vector3 playerDirection = DirectionManager.DirectionToVector3(GetComponent<GenericMovement>().directionManager.current);
            float angle = -oscillationAngle + Mathf.PingPong(aimTime, 2f * oscillationAngle);
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            aimDirection = rotation * playerDirection;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position + aimDirection * 2.5f);

            aimTime += Time.deltaTime * oscillationSpeed;
        }
        else
        {
            if (line.enabled) line.enabled = false;
            aimTime = 0;
        }
    }

}
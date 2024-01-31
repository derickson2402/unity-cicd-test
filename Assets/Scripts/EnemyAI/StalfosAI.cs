using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalfosAI : NPCController
{
    public float detectionDistance = 4.5f;

    private float curTime = 0.0f;
    private float timeBetweenChecks = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if (mover.movementEnabled)
        {
            curTime += Time.deltaTime;
            if (curTime >= timeBetweenChecks)
            {
                curTime = 0;
                IsPlayerInSight();
            }
        }
    }

    private void IsPlayerInSight()
    {
        float deltaDistance = Vector3.Distance(transform.position, player.transform.position);
        if (deltaDistance <= detectionDistance)
        {
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out var hit,
                    deltaDistance))
            {
                if (hit.collider.gameObject == player)
                {
                    state = AIState.Aggression;
                    Debug.Log("Enemy " + gameObject.name + " is turning aggressive");
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeseAI : NPCController
{
    public float detectionRadius = 2.5f;

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
                IsPlayerInRadius();
            }
        }
    }

    private void IsPlayerInRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == player)
            {
                Debug.Log("Enemy " + gameObject.name + " is turning aggressive");
                state = AIState.Aggression;
                return;
            }
            else
            {
                state = AIState.Wander;
                return;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

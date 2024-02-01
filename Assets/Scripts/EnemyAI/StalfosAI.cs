using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalfosAI : NPCController
{
    private float curTime = 0.0f;
    public float timeBetweenChecks = 0.3f;
    public float sightDistance = 4f;

    // Gizmo stuff for debugging and testing
    private BoolWrapper draw1DRaycast = new BoolWrapper(0.1f);
    private Vector3 raycastDirection;

    protected override void Update()
    {
        base.Update();
        draw1DRaycast.Update();
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
        if (deltaDistance <= sightDistance)
        {
            raycastDirection = player.transform.position - transform.position;
            draw1DRaycast.Start();
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out var hit,
                    sightDistance))
            {
                if (hit.collider.gameObject == player)
                {
                    state = AIState.Aggression;
                    Debug.Log("Enemy " + gameObject.name + " is turning aggressive");
                }
            }
        }
    }

    protected virtual void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (draw1DRaycast.value)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, raycastDirection * sightDistance);
        }
    }
}

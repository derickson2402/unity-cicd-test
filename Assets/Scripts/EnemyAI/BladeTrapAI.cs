using System.Collections;
using UnityEngine;

public class BladeTrapAI : NPCController
{
    public float sightDistance = 15f;

    // Gizmo stuff for debugging and testing
    private BoolWrapper drawSightRay = new BoolWrapper(0.15f);
    private Vector3 raycastDirection;

    private BoolWrapper playerInSight = new BoolWrapper(0.5f);
    private Direction nextMove = Direction.None;
    //private float curTime = 0.0f;
    //private float timeBetweenChecks = 0.5f;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        playerInSight.Update();
        drawSightRay.Update();
        //if (mover.movementEnabled)
        //{
        //    curTime += Time.deltaTime;
        //    if (curTime >= timeBetweenChecks)
        //    {
        //        curTime = 0;
        //        IsPlayerInSight();
        //    }
        //}
    }

    private bool IsPlayerInSight()
    {
        nextMove = Direction.None;
        float deltaDistance = Vector3.Distance(initialPosition, player.transform.position);
        if (deltaDistance <= sightDistance)
        {
            (RaycastHit hitUp, RaycastHit hitDown, RaycastHit hitLeft, RaycastHit hitRight) = FourDirectionRayCast(drawSightRay, initialPosition, sightDistance);

            if (hitUp.collider != null && hitUp.collider.gameObject == player)
            {
                nextMove = Direction.Up;
            }
            else if (hitDown.collider != null && hitDown.collider.gameObject == player)
            {
                nextMove = Direction.Down;
            }
            else if (hitLeft.collider != null && hitLeft.collider.gameObject == player)
            {
                nextMove = Direction.Left;
            }
            else if (hitRight.collider != null && hitRight.collider.gameObject == player)
            {
                nextMove = Direction.Right;
            }
            else
            {
                nextMove = Direction.None;
                return false;
            }
            raycastDirection = DirectionManager.DirectionToVector3(nextMove);
            return true;
        }

        return false;
    }

    protected override IEnumerator AIMovement()
    {
        while (true)
        {
            if (mover.movementEnabled)
            {
                while (playerInSight.value)
                {
                    mover.Move(nextMove);
                    nextMove = Direction.None;
                    yield return new WaitForSeconds(0.05f);
                }
                if (!playerInSight.value)
                {
                    if (IsPlayerInSight())
                    {
                        playerInSight.Start();
                    }
                    else
                    {
                        yield return StartCoroutine(ReturnToSpawn(0.5f));
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (drawSightRay.value)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, raycastDirection * sightDistance);
        }
    }
}

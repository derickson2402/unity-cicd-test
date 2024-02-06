using System.Collections;
using UnityEngine;

public class BladeTrapAI : NPCController
{
    public float returnToSpawnTime = 3f;
    public float sightDistance = 6f;
    public float xDeltaThreshold = 0.4f;
    public float yDeltaThreshold = 0.4f;

    // Gizmo stuff for debugging and testing
    private BoolWrapper drawSightRay = new BoolWrapper(0.15f);
    private Vector3 raycastDirection;

    private BoolWrapper playerInSight = new BoolWrapper(0.5f);
    private Direction nextMove = Direction.None;

    private BoxCollider boxCollider;
    private bool hitPlayer = false;
    //private float curTime = 0.0f;
    //private float timeBetweenChecks = 0.5f;

    protected override void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        playerInSight.Update();
        drawSightRay.Update();
    }

    private bool IsPlayerInSight()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 trapPos = transform.position;

        // Check the difference between player's position and trap's position
        float deltaX = Mathf.Abs(playerPos.x - trapPos.x);
        float deltaY = Mathf.Abs(playerPos.y - trapPos.y);

        // If the player is lined up with the trap either horizontally or vertically
        // and is within sight distance, cast a ray from trap towards the player
        if ((deltaX < xDeltaThreshold && deltaY <= sightDistance) || (deltaY < yDeltaThreshold && deltaX <= sightDistance))
        {
            RaycastHit hit;
            // If the player is to the right or left of the trap
            if (deltaX < xDeltaThreshold)
            {
                Direction move = playerPos.y > trapPos.y ? Direction.Up : Direction.Down;
                raycastDirection = DirectionManager.DirectionToVector3(move);
                drawSightRay.Start();
                if (Physics.Raycast(trapPos, raycastDirection, out hit, sightDistance))
                {
                    // If the first object hit is not the player, return false
                    if (hit.collider.gameObject != player)
                    {
                        return false;
                    }
                }

                nextMove = move;
            }
            // If the player is above or below the trap
            else if (deltaY < yDeltaThreshold)
            {
                Direction move = playerPos.x > trapPos.x ? Direction.Right : Direction.Left;
                raycastDirection = DirectionManager.DirectionToVector3(move);
                drawSightRay.Start();
                if (Physics.Raycast(trapPos, raycastDirection, out hit, sightDistance))
                {
                    // If the first object hit is not the player, return false
                    if (hit.collider.gameObject != player)
                    {
                        return false;
                    }
                }
                nextMove = move;
            }

            return true;
        }

        return false;
    }

    protected override IEnumerator ReturnToSpawn(float duration)
    {
        mover.movementEnabled = false;
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            // If the enemy spots the player during the return, it stops returning and move towards the player
            //if (IsPlayerInSight() && !hitPlayer)
            //{
            //    mover.movementEnabled = true;
            //    yield break;
            //}

            float timeSinceStarted = Time.time - startTime;
            float percentageComplete = timeSinceStarted / duration;

            transform.position = Vector3.Lerp(transform.position, initialPosition, percentageComplete);

            yield return new WaitForFixedUpdate();
        }

        mover.movementEnabled = true;
        yield return null;
    }

    protected override IEnumerator AIMovement()
    {
        while (true)
        {
            if (mover.movementEnabled)
            {
                boxCollider.enabled = false;
                bool temp = IsPlayerInSight();
                while (temp)
                {
                    mover.Move(nextMove);
                    yield return new WaitForSeconds(0.05f);
                    boxCollider.enabled = true;
                    Vector3 deltaPos = transform.position - initialPosition;
                    if (Mathf.Abs(deltaPos.x) > sightDistance || Mathf.Abs(deltaPos.y) > sightDistance)
                    {
                        temp = false;
                        yield return StartCoroutine(ReturnToSpawn(returnToSpawnTime));
                    }
                }
                yield return StartCoroutine(ReturnToSpawn(returnToSpawnTime));
            }
            yield return new WaitForSeconds(movementWaitTime);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // Check if the trap collide with wall or player
        if (collision.gameObject.tag == "Tile_WALL")
        {
            StartCoroutine(ReturnToSpawn(returnToSpawnTime));
        }
        else if (collision.gameObject.tag == "Player")
        {
            hitPlayer = true;
            StartCoroutine(ReturnToSpawn(returnToSpawnTime));
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(ReturnToSpawn(returnToSpawnTime));
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (drawSightRay.value)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(initialPosition, raycastDirection * sightDistance);
        }
    }
}

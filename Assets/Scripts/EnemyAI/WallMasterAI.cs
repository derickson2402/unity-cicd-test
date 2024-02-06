using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WallmasterAI : NPCController
{
    public Vector3 detectionBoxSize;
    public float slowInitialSpeed;
    public float normalSpeed;
    public float returnToSpawnTime = 3f;

    private float timeSinceSighting = 0;
    private bool firstMove = true;
    private float curTime = 0.0f;
    private float timeBetweenChecks = 0.5f;

    protected override void Start()
    {
        mover = GetComponent<GenericMovement>();
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        currentMovement = StartCoroutine(AIMovement());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (mover.movementEnabled)
        {
            curTime += Time.deltaTime;
            if (state == AIState.Aggression)
            {
                timeSinceSighting += Time.deltaTime;
            }
            else if (curTime >= timeBetweenChecks)
            {
                curTime = 0;
                IsPlayerInSquare();
            }
            // if it leaves the bounds of its detection box, return to spawn
            Vector3 deltaPos = (transform.position - initialPosition);
            if (Mathf.Abs(deltaPos.x) > (detectionBoxSize.x / 2) || Mathf.Abs(deltaPos.y) > (detectionBoxSize.y / 2))
            {
                StartCoroutine(ReturnToSpawn(returnToSpawnTime));
            }
        }
    }

    private bool IsPlayerInSquare()
    {
        Collider[] hitColliders = Physics.OverlapBox(initialPosition, detectionBoxSize / 2, Quaternion.identity);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == player)
            {
                Debug.Log("Enemy " + gameObject.name + " is turning aggressive");
                state = AIState.Aggression;
                return true;
            }
        }

        return false;
    }

    protected override IEnumerator ReturnToSpawn(float duration)
    {
        mover.movementEnabled = false;
        rb.velocity = Vector3.zero;
        timeSinceSighting = 0;
        state = AIState.Wander;
        firstMove = false;
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            // If the enemy spots the player during the return, it stops returning and move towards the player
            if (IsPlayerInSquare())
            {
                mover.movementEnabled = true;
                yield break;
            }

            float timeSinceStarted = Time.time - startTime;
            float percentageComplete = timeSinceStarted / duration;

            transform.position = Vector3.Lerp(transform.position, initialPosition, percentageComplete);

            yield return new WaitForFixedUpdate();
        }

        state = AIState.Wander;
        mover.movementEnabled = true;
        yield return null;
    }

    protected override Direction GenerateMoveTowardPosition(Vector3 position)
    {
        Vector3 delta = position - transform.position;
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
            {
                return Direction.Right;
            }
            else
            {
                return Direction.Left;
            }
        }
        else
        {
            if (delta.y > 0)
            {
                return Direction.Up;
            }
            else
            {
                return Direction.Down;
            }
        }
        return Direction.None;
    }

    protected override void WanderMove()
    {
        firstMove = true;
        Debug.Log("NPC " + gameObject.name + " waiting");
    }

    protected override void AggressionMove(Vector3 position)
    {
        if (!IsPlayerInSquare())
        {
            return;
        }
        else if (firstMove)
        {
            mover.movementSpeed = slowInitialSpeed;
        }
        else
        {
            mover.movementSpeed = normalSpeed;
        }

        Direction movement = GenerateMoveTowardPosition(position);
        Debug.Log("NPC " + gameObject.name + " seeking player " + movement.ToString());
        mover.Move(movement);
        firstMove = false;
    }

    protected override IEnumerator AIMovement()
    {
        while (true)
        {
            if (mover.movementEnabled)
            {
                switch (state)
                {
                    case AIState.Wander:
                        WanderMove();
                        break;
                    case AIState.Aggression:
                        AggressionMove(player.transform.position);
                        break;
                }

                if (!IsPlayerInSquare() && timeSinceSighting > 2f)
                {
                    yield return StartCoroutine(ReturnToSpawn(returnToSpawnTime));
                }
            }
            yield return new WaitForSeconds(movementWaitTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerController>().ReturnPlayerToStart();
            Destroy(gameObject);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(initialPosition, detectionBoxSize);
    }
}
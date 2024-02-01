using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMasterAI : NPCController
{
    public Vector3 detectionBoxSize;
    public float slowInitialSpeed;
    public float normalSpeed;

    private float timeSinceSighting = 0;
    private Vector3 initialPosition;
    private bool firstMove = true;
    private float curTime = 0.0f;
    private float timeBetweenChecks = 0.5f;

    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        state = AIState.Aggression;
        initialPosition = transform.position;
        currentMovement = StartCoroutine(AIMovement());
    }

    // Update is called once per frame
    void Update()
    {
        if (mover.movementEnabled)
        {
            curTime += Time.deltaTime;
            if (curTime >= timeBetweenChecks)
            {
                curTime = 0;
                IsPlayerInSquare();
            }

            if (state == AIState.Aggression)
            {
                timeSinceSighting += Time.deltaTime;
            }
        }

        if (timeSinceSighting > 2f)
        {
            mover.movementEnabled = false;
            firstMove = true;
            timeSinceSighting = 0;
            state = AIState.Wander;
            StartCoroutine(ReturnToSpawn());
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

    private IEnumerator ReturnToSpawn()
    {
        while (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, initialPosition, 1f);
        }

        mover.movementEnabled = true;
        yield return null;
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, detectionBoxSize);
    }
}
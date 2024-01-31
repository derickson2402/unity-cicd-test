using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using static System.Net.WebRequestMethods;

public enum AIState
{
    Wander,
    Aggression
}

public class NPCController : MonoBehaviour
{
    public AIState state;
    protected GenericMovement mover;
    protected Rigidbody rb;
    protected Coroutine currentMovement;
    protected GameObject player;
    private float raycastDistance = 1.0f;

    //private Vector3 playerPosition;
    //private float timeSincePlayerScan;
    //private float timeBetweenPlayerScans = 0.3f;

    void Awake()
    {
        mover = GetComponent<GenericMovement>();
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        //playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        state = AIState.Wander;
        currentMovement = StartCoroutine(AIMovement());
    }

    protected virtual Direction GenerateMoveTowardPlayer()
    {
        RaycastHit hitUp, hitDown, hitLeft, hitRight;

        Physics.Raycast(transform.position, Vector3.up, out hitUp, raycastDistance);
        Physics.Raycast(transform.position, Vector3.down, out hitDown, raycastDistance);
        Physics.Raycast(transform.position, Vector3.left, out hitLeft, raycastDistance);
        Physics.Raycast(transform.position, Vector3.right, out hitRight, raycastDistance);

        Vector3 delta = player.transform.position - transform.position;
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0 && (hitRight.collider == null || hitRight.collider.gameObject == player))
            {
                return Direction.Right;
            }
            else if (hitLeft.collider == null || hitLeft.collider.gameObject == player)
            {
                return Direction.Left;
            }
        }
        else
        {
            if (delta.y > 0 && (hitUp.collider == null || hitUp.collider.gameObject == player))
            {
                return Direction.Up;
            }
            else if (hitDown.collider == null || hitDown.collider.gameObject == player)
            {
                return Direction.Down;
            }
        }
        return Direction.None; // return an alternative or do nothing if all paths are blocked
    }

    protected virtual void WanderMove()
    {
        // 25% to move in any direction
        Direction movement = DirectionManager.directions[Random.Range(0, DirectionManager.directions.Length)];
        Debug.Log("NPC " + gameObject.name + " wandering " + movement.ToString());
        mover.Move(movement);
    }

    protected virtual void AggressionMove()
    {
        Direction movement = GenerateMoveTowardPlayer();
        Debug.Log("NPC " + gameObject.name + " seeking player " + movement.ToString());
        mover.Move(movement);
    }

    // Called to generate movement for NPC
    protected virtual IEnumerator AIMovement()
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
                        AggressionMove();
                        break;
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, Vector3.up * raycastDistance);
        Gizmos.DrawRay(transform.position, Vector3.down * raycastDistance);
        Gizmos.DrawRay(transform.position, Vector3.left * raycastDistance);
        Gizmos.DrawRay(transform.position, Vector3.right * raycastDistance);
    }
}

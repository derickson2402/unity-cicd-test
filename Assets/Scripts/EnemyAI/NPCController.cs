using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public enum AIState
{
    Wander,
    Aggression
}

public class NPCController : MonoBehaviour
{
    public AIState state;
    private GenericMovement mover;
    private Rigidbody rb;
    private Coroutine currentMovement;

    //private Vector3 playerPosition;
    //private float timeSincePlayerScan;
    //private float timeBetweenPlayerScans = 0.3f;

    void Start()
    {
        mover = GetComponent<GenericMovement>();
        mover.movementEnabled = false;
        rb = GetComponent<Rigidbody>();
        //playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        
        state = AIState.Wander;
        currentMovement = StartCoroutine(AIMovement());
    }

    protected virtual void Update()
    {
        //timeSincePlayerScan += Time.deltaTime;
        //if (timeSincePlayerScan >= timeBetweenPlayerScans)
        //{
        //    timeSincePlayerScan = 0;
        //    playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        //}
    }

    protected virtual Direction GenerateMoveTowardPlayer()
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 delta = transform.position - playerPosition;
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
            while (mover.movementEnabled)
            {
                if (state == AIState.Wander)
                {
                    WanderMove();
                }
                else if (state == AIState.Aggression)
                {
                    AggressionMove();
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
}

using System.Collections;
using UnityEngine;

public enum AIState
{
    Wander,
    Aggression
}

public class NPCController : MonoBehaviour
{
    public AIState state;
    public float movementWaitTime = 1f;
    protected Vector3 initialPosition;
    protected GenericMovement mover;
    protected Rigidbody rb;
    protected Coroutine currentMovement;
    protected GameObject player;

    // Gizmo stuff for debugging and testing
    private BoolWrapper draw4DRaycastGizmos = new BoolWrapper(0.2f);
    private float hazardCheckRaycastDistance = 1.0f;

    void Awake()
    {
        mover = GetComponent<GenericMovement>();
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
    }

    protected virtual void Update()
    {
        draw4DRaycastGizmos.Update();
    }

    protected virtual void Start()
    {
        //playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        state = AIState.Wander;
        currentMovement = StartCoroutine(AIMovement());
    }

    protected (RaycastHit hitUp, RaycastHit hitDown, RaycastHit hitLeft, RaycastHit hitRight) FourDirectionRayCast(BoolWrapper gizmoBool, Vector3 position, float raycastDistance)
    {
        gizmoBool.Start();
        
        Physics.Raycast(position, Vector3.up, out var hitUp, raycastDistance);
        Physics.Raycast(position, Vector3.down, out var hitDown, raycastDistance);
        Physics.Raycast(position, Vector3.left, out var hitLeft, raycastDistance);
        Physics.Raycast(position, Vector3.right, out var hitRight, raycastDistance);

        return (hitUp, hitDown, hitLeft, hitRight);
    }

    protected virtual Direction GenerateMoveTowardPosition(Vector3 position)
    {
        (RaycastHit hitUp, RaycastHit hitDown, RaycastHit hitLeft, RaycastHit hitRight) =
            FourDirectionRayCast(draw4DRaycastGizmos, transform.position, hazardCheckRaycastDistance);

        Vector3 delta = position - transform.position;
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0 && (hitRight.collider == null || (hitRight.collider != null && hitRight.collider.gameObject == player)))
            {
                return Direction.Right;
            }
            else if (hitLeft.collider == null || (hitLeft.collider != null && hitLeft.collider.gameObject == player))
            {
                return Direction.Left;
            }
        }
        else
        {
            if (delta.y > 0 && (hitUp.collider == null || (hitUp.collider != null && hitUp.collider.gameObject == player)))
            {
                return Direction.Up;
            }
            else if (hitDown.collider == null || (hitDown.collider != null && hitDown.collider.gameObject == player))
            {
                return Direction.Down;
            }
        }
        return Direction.None;
    }

    protected virtual void WanderMove()
    {
        // 25% to move in any direction
        Direction movement = DirectionManager.directions[Random.Range(0, DirectionManager.directions.Length)];
        Debug.Log("NPC " + gameObject.name + " wandering " + movement.ToString());
        mover.Move(movement);
    }

    protected virtual void AggressionMove(Vector3 position)
    {
        Direction movement = GenerateMoveTowardPosition(position);
        Debug.Log("NPC " + gameObject.name + " seeking position " + position + " " + movement.ToString());
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
                        AggressionMove(player.transform.position);
                        break;
                }
            }
            yield return new WaitForSeconds(movementWaitTime);
        }
    }

    protected IEnumerator ReturnToSpawn(float duration)
    {
        mover.movementEnabled = false;
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            float timeSinceStarted = Time.time - startTime;
            float percentageComplete = timeSinceStarted / duration;
            transform.position = Vector3.Lerp(transform.position, initialPosition, percentageComplete);

            yield return new WaitForFixedUpdate();
        }

        mover.movementEnabled = true;
        yield return null;
    }

    protected virtual void OnDrawGizmos()
    {
        if (draw4DRaycastGizmos.value)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, Vector3.up * hazardCheckRaycastDistance);
            Gizmos.DrawRay(transform.position, Vector3.down * hazardCheckRaycastDistance);
            Gizmos.DrawRay(transform.position, Vector3.left * hazardCheckRaycastDistance);
            Gizmos.DrawRay(transform.position, Vector3.right * hazardCheckRaycastDistance);
        }
    }
}

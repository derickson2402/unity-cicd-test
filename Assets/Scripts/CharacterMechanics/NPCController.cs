using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private GenericMovement mover;
    private Rigidbody rb;
    private Coroutine currentMovement;

    void Start()
    {
        mover = GetComponent<GenericMovement>();
        mover.movementEnabled = false;
        rb = GetComponent<Rigidbody>();
        currentMovement = StartCoroutine(RandomMovement());
    }

    // Called to generate movement for NPC
    private IEnumerator RandomMovement()
    {
        while (mover.movementEnabled)
        {
            // do nothing 80% of the time
            int randomValue = Random.Range(0, 10);
            if (randomValue < 8)
            {
                yield return new WaitForSeconds(1);
            }

            // 20% chance to move in a random direction (each direction is 25%)
            Direction movement = DirectionManager.directions[Random.Range(0, DirectionManager.directions.Length)];
            Debug.Log("NPC " + gameObject.name + " moving " + movement.ToString());
            mover.Move(movement);
            yield return new WaitForSeconds(1);
        }
    }
}

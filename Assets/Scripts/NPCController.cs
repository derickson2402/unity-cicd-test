using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : GenericHealth
{
    private GenericMovement mover;
    private Rigidbody rb;
    private Coroutine currentMovement;
    private Vector2[] directions = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

    void Start()
    {
        mover = GetComponent<GenericMovement>();
        rb = GetComponent<Rigidbody>();
        hp = maxHP;
        currentMovement = StartCoroutine(RandomMovement());
    }

    //void Update()
    //{
    //    StartCoroutine(RandomMovement());
    //}

    // Called to generate movement for NPC
    private IEnumerator RandomMovement()
    {
        while (true)
        {
            // do nothing 80% of the time
            int randomValue = Random.Range(0, 10);
            if (randomValue < 8)
            {
                yield return new WaitForSeconds(1);
            }

            // 20% chance to move in a random direction (each direction is 25%)
            Vector2 movement = directions[Random.Range(0, directions.Length)];
            Debug.Log("NPC moving " + movement.ToString());
            mover.Move(movement);
            yield return new WaitForSeconds(1);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GoriyaAI : NPCController
{
    private Direction nextMove;
    private bool newMove = false;

    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        state = AIState.Aggression;
        currentMovement = StartCoroutine(AIMovement());
        StartCoroutine(AttackWithBoomerang());
    }

    public void SetMirrorMove(Direction move)
    {
        newMove = true;
        nextMove = DirectionManager.invertDirection(move);
    }
    protected override void WanderMove()
    {
        // 25% to move in any direction
        int fiftyFifty = Random.Range(0, 2);
        if (fiftyFifty == 0)
        {
            Direction movement = DirectionManager.directions[Random.Range(0, DirectionManager.directions.Length)];
            Debug.Log("NPC " + gameObject.name + " wandering " + movement.ToString());
            mover.Move(movement);
        }
        else
        {
            mover.Move(nextMove);
        }
    }

    protected override IEnumerator AIMovement()
    {
        while (true)
        {
            if (mover.movementEnabled)
            {
                if (newMove)
                {
                    WanderMove();
                }
            }
            yield return new WaitForSeconds(2);
        }
    }

    private IEnumerator AttackWithBoomerang()
    {
        while (true)
        {
            if (mover.movementEnabled)
            {
                GetComponent<WeaponInterface>().useWeaponA();
            }
            yield return new WaitForSeconds(2);
        }
    }
}

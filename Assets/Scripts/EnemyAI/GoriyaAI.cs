using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    protected override IEnumerator AIMovement()
    {
        while (true)
        {
            if (mover.movementEnabled)
            {
                if (newMove)
                {
                    newMove = false;
                    mover.Move(nextMove);
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
                // throw boomerang at player
            }
            yield return new WaitForSeconds(3);
        }
    }
}

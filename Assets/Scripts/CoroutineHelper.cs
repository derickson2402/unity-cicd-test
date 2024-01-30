using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEditor.Progress;

public class CoroutineHelper {
    public static IEnumerator MoveObjectOverTime(Transform item, Vector3 startingPosition, Vector3 desiredPosition, float duration)
    {
        UnityEngine.Debug.Log("Starting movement");
        float initialTime = Time.time;
        float progress = (Time.time - initialTime) / duration;
        while (progress < 1.0f)
        {
            progress = (Time.time - initialTime) / duration;
            Vector3 newPosition = Vector3.Lerp(startingPosition, desiredPosition, progress);
            item.position = newPosition;
            yield return null;
        }
        item.position = desiredPosition;
        UnityEngine.Debug.Log("Finishing movement");
    }

    public static IEnumerator MoveCharacterOverTime(Transform character, Vector3 startingPosition,
        Vector3 desiredPosition, float duration, Direction newDirection)
    {
        Vector3 deltaPosition = desiredPosition - startingPosition;
        Direction deltaDirection;
        var genericMovement = character.GetComponent<GenericMovement>();
        if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
        {
            deltaDirection = (deltaPosition.x > 0) ? Direction.Right : Direction.Left;
        }
        else
        {
            deltaDirection = (deltaPosition.y > 0) ? Direction.Up : Direction.Down;
        }
        //TODO: change sprite based on deltaDirection here

        float initialTime = Time.time;
        float progress = (Time.time - initialTime) / duration;
        UnityEngine.Debug.Log("Starting movement");
        while (progress < 1.0f)
        {
            progress = (Time.time - initialTime) / duration;
            Vector3 newPosition = Vector3.Lerp(startingPosition, desiredPosition, progress);
            character.position = newPosition;
            yield return null;
        }
        character.position = desiredPosition;
        UnityEngine.Debug.Log("Finishing movement");

        genericMovement.ChangeDirection(newDirection);
    }
}
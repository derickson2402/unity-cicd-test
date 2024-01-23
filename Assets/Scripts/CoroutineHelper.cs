using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper {
    public static IEnumerator MoveObjectOverTime(Transform item, Vector3 startingPosition, Vector3 desiredPosition, float duration)
    {
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
    }
}
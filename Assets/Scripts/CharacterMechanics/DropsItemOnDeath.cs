using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DropsItemOnDeath : MonoBehaviour
{
    // Randomly drop an object based on given parameters
    public void OnDestroy()
    {
        // Determine whether we drop anything at all
        if (Random.Range(0, 3) != 0)
        {
            return;
        }
        Debug.Log("Dropping item");
        // Determine what we drop
        int randomVal = Random.Range(1,4);
        if (randomVal == 1)
        {
            Instantiate(Resources.Load("Heart"), transform.position, Quaternion.identity);
        }
        else if (randomVal == 2)
        {
            Instantiate(Resources.Load("BombPickup"), transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(Resources.Load("Rupee"), transform.position, Quaternion.identity);
        }
    }
}

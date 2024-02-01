using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DropsItemOnDeath : MonoBehaviour
{
    public int heartRatio;    // Proportion of time a Heart is dropped
    public int bombRatio;     // Proportion of time a Bomb is dropped
    public int rupeeRatio;    // Proportion of time a Rupee is dropped

    private void Awake()
    {
        Assert.IsFalse(heartRatio < 0);
        Assert.IsFalse(bombRatio < 0);
        Assert.IsFalse(rupeeRatio < 0);
        Assert.IsTrue(heartRatio + bombRatio + rupeeRatio > 0);
    }

    // Randomly drop an object based on given parameters
    public void OnDestroy()
    {
        // // Determine whether we drop anything at all
        // Debug.Log("Dropping item");
        // // Determine what we drop
        // int randomValue = Random.Range(1, heartRatio + bombRatio + rupeeRatio);
        // if (randomValue <= heartRatio)
        // {
        //     Instantiate(Resources.Load("Heart"), transform.position, Quaternion.identity);
        // }
        // else if (randomValue <= heartRatio + bombRatio)
        // {
        //     Debug.Log("Dropping bomb at " + transform.position);
        //     Instantiate(Resources.Load("BombPickup"), transform.position, Quaternion.identity);
        // }
        // else if (randomValue <= heartRatio + bombRatio + rupeeRatio)
        // {
        //     Instantiate(Resources.Load("Rupee"), transform.position, Quaternion.identity);
        // }
        // else
        // {
        //     Debug.Log("Error dropping random object");
        // }

        // Quick and dirty fix of drops
        int randomVal = Random.Range(1,3);
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

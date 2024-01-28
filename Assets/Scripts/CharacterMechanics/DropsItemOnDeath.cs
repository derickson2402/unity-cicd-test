using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DropsItemOnDeath : MonoBehaviour
{
    public int dropFreq;      // How many deaths (on avg) before any item is dropped
    public int heartRatio;    // Proportion of time a Heart is dropped
    public int bombRatio;     // Proportion of time a Bomb is dropped
    public int rupeeRatio;    // Proportion of time a Rupee is dropped

    private void Awake()
    {
        Assert.IsFalse(dropFreq < 0);
        Assert.IsFalse(heartRatio < 0);
        Assert.IsFalse(bombRatio < 0);
        Assert.IsFalse(rupeeRatio < 0);
        Assert.IsTrue(heartRatio + bombRatio + rupeeRatio > 0);
    }

    // Randomly drop an object based on given parameters
    public void OnDestroy()
    {
        Debug.Log("Dropping item");
        // Determine whether we drop anything at all
        if (dropFreq == 0)
        {
            return;
        }
        else if (Random.Range(0, dropFreq - 1) != 0)
        {
            return;
        }
        // Determine what we drop
        int randomValue = Random.Range(1, heartRatio + bombRatio + rupeeRatio);
        if (randomValue <= heartRatio)
        {
            Instantiate(Resources.Load("Heart"), transform.position, Quaternion.identity);
        }
        else if (randomValue <= heartRatio + bombRatio)
        {
            Debug.Log("Dropping bomb at " + transform.position);
            Instantiate(Resources.Load("Bomb"), transform.position, Quaternion.identity);
        }
        else if (randomValue <= heartRatio + bombRatio + rupeeRatio)
        {
            Instantiate(Resources.Load("Rupee"), transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Error dropping random object");
        }
    }
}

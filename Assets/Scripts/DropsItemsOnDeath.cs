using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsItemsOnDeath : MonoBehaviour
{
    public float heartDropRate;
    public float bombDropRate;
    public float rupeeDropRate;

    public void Die()
    {
        //random value from 0-3 (0 means no drop, 25% for heart, rupee, or bomb)
        int randomValue = Random.Range(0, 4);
        switch (randomValue)
        {
            case 1:
                Instantiate(Resources.Load("Heart"), transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(Resources.Load("Rupee"), transform.position, Quaternion.identity);
                break;
            case 3:
                Instantiate(Resources.Load("Bomb"), transform.position, Quaternion.identity);
                break;
        }
    }
}

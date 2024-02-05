using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DropsItemOnDeath : MonoBehaviour
{
    public GameObject guranteedDropItem;    // If given, character will drop this on death, otherwise random item
    public int dropRate;                    // On average, drop item every dropRate times

    private GameObject obj;                 // private member of instantiated guranteedDropItem

    private void Start()
    {
        if (guranteedDropItem != null)
        {
            obj = Instantiate(guranteedDropItem, transform.position, Quaternion.identity);
            // Disable colliders on the object so we can control its position
            BoxCollider bc = obj.GetComponent<BoxCollider>();
            if (bc != null)
            {
                bc.enabled = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (obj != null)
        {
            obj.transform.position = transform.position;
        }
    }

    // Randomly drop an object based on given parameters
    public void OnDestroy()
    {
        // Check if we have an existing item to drop, re-enable it
        if (guranteedDropItem != null)
        {
            obj.GetComponent<BoxCollider>().enabled = true;
            return;
        }
        // Determine whether we drop anything at all
        if (Random.Range(0, dropRate) != 0 || (dropRate < 1))
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Displayer : MonoBehaviour
{
    public Inventory inventory;

    public TextField rupeeCount;
    public TextField keyCount;
    public TextField bombCount;
    // Start is called before the first frame update
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        if (inventory != null && rupeeCount != null)
        {
            rupeeCount.Write (":" + inventory.GetRupees ().ToString ());
        }

        if (inventory != null && keyCount != null)
        {
            keyCount.Write(":" + inventory.GetKeys().ToString());
        }

        if (inventory != null && bombCount != null)
        {
            bombCount.Write(":" + inventory.GetBombs().ToString());
        }

    }
}
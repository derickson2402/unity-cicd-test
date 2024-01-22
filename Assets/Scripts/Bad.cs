using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RupeeDisplayer : MonoBehaviour
{
    public Inventory inventory;

    TextMeshProUGUI rupeeCount;
    TextMeshProUGUI keyCount;
    TextMeshProUGUI bombCount;

    // Start is called before the first frame update
    void Start ()
    {
        rupeeCount = GetComponent<TextMeshProUGUI> ();

        keyCount = GetComponent<TextMeshProUGUI> ();

        bombCount = GetComponent<TextMeshProUGUI> ();
    }

    // Update is called once per frame
    void Update ()
    {
        if (inventory != null && rupeeCount != null)
        {
            rupeeCount.text = ("x" + inventory.GetRupees ().ToString ());
        }

        if(inventory != null && keyCount != null)
        {
           keyCount.text = ("x" + inventory.GetKeys ().ToString ());
        }

        if (inventory != null && bombCount != null)
        {
            bombCount.text = ("x" + inventory.GetBombs().ToString());
        }

    }
}

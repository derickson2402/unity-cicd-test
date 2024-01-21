using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RupeeDisplayer : MonoBehaviour
{
    public Inventory inventory;

    TextMeshProUGUI rupeeText;

    // Start is called before the first frame update
    void Start ()
    {
        rupeeText = GetComponent<TextMeshProUGUI> ();
    }

    // Update is called once per frame
    void Update ()
    {
        if (inventory != null && rupeeText != null)
        {
            rupeeText.text = inventory.GetRupees ().ToString ();
        }

    }
}

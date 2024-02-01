using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextField : MonoBehaviour
{
    TextMeshProUGUI field;

    // Start is called before the first frame update
    void Start()
    {
        field = GetComponent<TextMeshProUGUI>();
    }

    public void Write (string text)
    {
        field.text = text;
    }
}

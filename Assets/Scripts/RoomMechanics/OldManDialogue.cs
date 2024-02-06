using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OldManDialogue : MonoBehaviour
{
    TextMeshPro text;

    public void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
    }
    public void DisplayState(bool state)
    {
        Debug.Log("Changing text state!");
        text.enabled = state;
    }
}

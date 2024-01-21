using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //
    int rupeeCount = 0;

    public void AddRupees (int num)
    {
        rupeeCount += num;
    }
    
    public int GetRupees ()
    {
        return rupeeCount;
    }
}

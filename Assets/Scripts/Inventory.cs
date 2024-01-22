using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //
    int rupeeCount = 0;

    int heartCount = 0;

    int keyCount = 0;

    int bombCount = 0;

    public void AddRupees (int num)
    {
        rupeeCount += num;
    }
    
    public int GetRupees ()
    {
        return rupeeCount;
    }


    public void AddHearts(int num)
    {
        heartCount += num;
    }

    public int GetHearts()
    {
        return heartCount;
    }


    public void AddKeys(int num)
    {
        keyCount += num;
    }

    public int GetKeys()
    {
        return keyCount;
    }

    public void AddBombs(int num)
    {
        bombCount += num;
    }

    public int GetBombs()
    {
        return bombCount;
    }
}

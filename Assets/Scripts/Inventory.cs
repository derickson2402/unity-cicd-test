using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //public fields
    public TextField rupeeCountText;
    public TextField keyCountText;
    public TextField bombCountText;

    int rupeeCount = 0;
    int heartCount = 0;
    int keyCount = 0;
    int bombCount = 0;

    public void AddRupees (int num)
    {
        rupeeCount += num;
        rupeeCountText.Write(":" + rupeeCount.ToString());
    }
    
    public int GetRupees ()
    {
        return rupeeCount;
    }


    public void AddHearts(int num)
    {
        heartCount += num;
        //heartCountText.Write(":" + GetRupees().ToString());
    }

    public int GetHearts()
    {
        return heartCount;
    }


    public void AddKeys(int num)
    {
        keyCount += num;
        keyCountText.Write(":" + keyCount.ToString());
    }

    public int GetKeys()
    {
        return keyCount;
    }

    public void AddBombs(int num)
    {
        bombCount += num;
        bombCountText.Write(":" + bombCount.ToString());
    }

    public int GetBombs()
    {
        return bombCount;
    }
}

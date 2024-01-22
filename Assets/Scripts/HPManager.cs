using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    public TextField heartCountText;

    int hp = 3;

    int maxHP = 4;
    void Start ()
    {
        hp = 3;
        heartCountText.Write ("x " + hp.ToString ());
    }

    /*
    private void Awake()
    {
        hp = 3;
        heartCountText.Write ("x " + hp.ToString ());
    }*/

    public void AddHP (int num)
    {
        if (hp + num <= maxHP)
        {
            hp += num;
            heartCountText.Write ("x " + hp.ToString ());
        }
        else
        {
            hp = maxHP;
            heartCountText.Write ("x " + hp.ToString ());
        }
    }

    public int GetHP ()
    {
        return hp;
    }
    public void SubtractHP (int num)
    {
        hp -= num;
        heartCountText.Write("x " + hp.ToString());
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenericHealth : MonoBehaviour
{
    //hp variables
    [SerializeField] protected double maxHP = 3;
    protected double hp;

    void Start()
    {
        hp = maxHP;
    }

    public virtual void ModifyHP(double num)
    {
        if (hp + num > double.Epsilon)
        {
            hp += num;
        }
        else
        {
            //die
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] protected Sprite swordImage;
    [SerializeField] protected Sprite bombImage;
    [SerializeField] protected Sprite bommerangImage;
    [SerializeField] protected Sprite bowImage;


    // Start is called before the first frame update
    void setWeaponB(Weapon w)
    {
        GameObject go = GameObject.Find("WeaponBImage");

        Image im =  go.GetComponent<Image>(); 

        if(w == Weapon.Bow)
        {
            im.sprite = bowImage;
        }
        else if(w == Weapon.Boomerang)
        {
            im.sprite = bommerangImage;
        }
        else if (w == Weapon.Bomb)
        {
            im.sprite = bombImage;
        }
        else if (w == Weapon.Sword)
        {
            im.sprite = swordImage;
        }
    }

    private void Awake()
    {
        setWeaponB(Weapon.Bomb);
    }

}

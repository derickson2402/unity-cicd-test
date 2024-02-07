using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController1 : MonoBehaviour
{
    // Rupee text.
    TextMeshProUGUI rupeeText;

    // Key text.
    TextMeshProUGUI keyText;

    // Bomb text.
    TextMeshProUGUI bombText;

    // Primary weapon image.
    SpriteRenderer primaryWeaponImage;

    //Secondary weapon image.
    SpriteRenderer secondaryWeaponImage;

    // Health text.
    TextMeshProUGUI healthText;
    void Awake()
    {
        // Initialize rupees.
        rupeeText = GameObject.Find("Rupees").GetComponent<TextMeshProUGUI>();
        //SetRupees(0);

        // Initalize keys.
        keyText = GameObject.Find("Keys").GetComponent<TextMeshProUGUI>();
        //SetKeys(0);

        // Initialize bombs
        bombText = GameObject.Find("Bombs").GetComponent<TextMeshProUGUI>();
        //SetBombs(0);

        healthText = GameObject.Find("Health").GetComponent<TextMeshProUGUI>();

        // Initialize primary weapon.
        primaryWeaponImage = GameObject.Find("PrimaryWeapon").GetComponent<SpriteRenderer>();

        // Initialize secondary weapon.
        primaryWeaponImage = GameObject.Find("SecondaryWeapon").GetComponent<SpriteRenderer>();

        // Initialize health.
        healthText = GameObject.Find("Health").GetComponent<TextMeshProUGUI>();
        //SetHealth(3);

    }
    void SetRupees(int num)
    {
        rupeeText.text = 'x' + string.Format("000", num);
    }

    void SetKeys(int num)
    {
        keyText.text = 'x' + string.Format("000", num);
    }

    void SetBombs(int num)
    {
        bombText.text = 'x' + string.Format("000", num);
    }

    void SetHealth(float num)
    {
        healthText.text = 'x' + string.Format("000", num);
    }

    void SetPrimaryWeaponImage(Sprite img)
    {
        primaryWeaponImage.sprite = img;
    }
    void SetSecondaryWeaponImage(Sprite img)
    {
        secondaryWeaponImage.sprite = img;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //public fields for UI
    public TextField rupeeCountText;
    public TextField keyCountText;
    public TextField bombCountText;
    public TextField heartCountText;

    // Sound Effects for pickups
    public AudioClip rupeeCollectionSoundEffect;
    public AudioClip heartCollectionSoundEffect;
    public AudioClip keyCollectionSoundEffect;
    public AudioClip bombCollectionSoundEffect;

    //hp variables
    int hp = 3;
    int maxHP = 3;

    //inventory variables
    int rupeeCount = 0;
    int keyCount = 0;
    int bombCount = 0;

    void Start()
    {
        //hp = 3;
        heartCountText.Write(hp.ToString());
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckForPickups(other);
    }

    //----------------
    //  HP functions
    //----------------
    public void ModifyHP(int num)
    {
        if (num > 0)
        {
            if (hp + num <= maxHP)
            {
                hp += num;
            }
            else
            {
                hp = maxHP;
            }
        }
        else
        {
            if (hp + num > 0)
            {
                hp += num;
            }
            else
            {
                GameOver();
            }
        }
        heartCountText.Write(hp.ToString());
    }
    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //-----------------------
    //  Inventory functions
    //-----------------------

    public void ModifyRupees(int num)
    {
        if (num > 0)
        {
            rupeeCount += num;
        }
        else
        {
            if (rupeeCount + num < 0)
            {
                rupeeCount = 0;
            }
            else
            {
                rupeeCount += num;
            }
        }
        rupeeCountText.Write(rupeeCount.ToString());
    }

    public void ModifyKeys(int num)
    {
        if (num > 0)
        {
            keyCount += num;
        }
        else
        {
            if (keyCount + num < 0)
            {
                keyCount = 0;
            }
            else
            {
                keyCount += num;
            }
        }
        keyCountText.Write(keyCount.ToString());
    }


    public void ModifyBombs(int num)
    {
        if (num > 0)
        {
            bombCount += num;
        }
        else
        {
            if (bombCount + num < 0)
            {
                bombCount = 0;
            }
            else
            {
                bombCount += num;
            }
        }
        bombCountText.Write(bombCount.ToString());
    }

    //--------------------
    //  Pickup functions
    //--------------------

    private void CheckForPickups(Collider other)
    {
        // Grab game object that this component belongs to.
        GameObject go = other.gameObject;

        // Specialize behavior based on tag.
        if (go.CompareTag("Rupee"))
        {
            ModifyRupees(1);
            Debug.Log("Rupees x" + rupeeCount);

            // Make Rupee disappear.
            Destroy(go);

            // Play Rupee collection clip.
            AudioSource.PlayClipAtPoint(rupeeCollectionSoundEffect, Camera.main.transform.position);
        }
        else if (go.CompareTag("Heart"))
        {
            ModifyHP(1);
            Debug.Log("Hearts x" + hp);

            // Make Rupee disappear.
            Destroy(go);

            // Play Rupee collection clip.
            AudioSource.PlayClipAtPoint(heartCollectionSoundEffect, Camera.main.transform.position);
        }
        else if (go.CompareTag("Key"))
        {
            ModifyKeys(1);

            Debug.Log("Keys x" + keyCount);
       

            // Make Rupee disappear.
            Destroy(go);

            // Play Rupee collection clip.
            AudioSource.PlayClipAtPoint(keyCollectionSoundEffect, Camera.main.transform.position);
        }
        else if (go.CompareTag("Bomb"))
        {
            ModifyBombs(1);

            Debug.Log("Keys x" + bombCount);

            // Make Rupee disappear.
            Destroy(go);

            // Play Rupee collection clip.
            AudioSource.PlayClipAtPoint(bombCollectionSoundEffect, Camera.main.transform.position);
        }
        else if (go.CompareTag("LockedDoor"))
        {
            if (keyCount > 0)
            {
                ModifyKeys(-1);

                SpriteRenderer[] sprites = go.GetComponentsInChildren<SpriteRenderer>();

                Debug.Log("Size: " +  sprites.Length);

                foreach (SpriteRenderer sprite in sprites)
                {
                    sprite.enabled = true;
                }

                go.GetComponent<BoxCollider>().enabled = false;

                Debug.Log("Unlocking Door");
            }
        }
    }
}

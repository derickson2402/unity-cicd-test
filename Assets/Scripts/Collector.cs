using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    // Represents player inventory.
    Inventory inventory;

    // Sound Effects.
    public AudioClip rupeeCollectionSoundEffect;
    public AudioClip heartCollectionSoundEffect;
    public AudioClip keyCollectionSoundEffect;
    public AudioClip bombCollectionSoundEffect;

    private void Start ()
    {
        // Grab a reference to the Inventory component in this gameobject.
        inventory = GetComponent<Inventory> ();

        // Make sure game object has an inventory.
        if (inventory == null)
        {
            Debug.LogWarning("WARNING: Gameobject with a collector has no inventory to store things in!");
        }
    }
    void OnTriggerEnter (Collider other)
    {
        // Grab game object that this component belongs to.
        GameObject go = other.gameObject;

        // Specialize behavior based on tag.
        if (go.CompareTag ("Rupee"))
        {   
            // Make sure game object has an inventory.
            if (inventory != null)
            {
                inventory.AddRupees(1);

                Debug.Log("Rupees x" + inventory.GetRupees());
            }

            // Make Rupee disappear.
            Destroy (go);

            // Play Rupee collection clip.
            AudioSource.PlayClipAtPoint (rupeeCollectionSoundEffect, Camera.main.transform.position);
        }
        else if (go.CompareTag ("Heart"))
        {
            // Make sure game object has an inventory.
            if (inventory != null)
            {
                inventory.AddHearts(1);

                Debug.Log("Hearts x" + inventory.GetHearts());
            }

            // Make Rupee disappear.
            Destroy(go);

            // Play Rupee collection clip.
            AudioSource.PlayClipAtPoint(heartCollectionSoundEffect, Camera.main.transform.position);
        }
        else if (go.CompareTag ("Key"))
        {
            // Make sure game object has an inventory.
            if (inventory != null)
            {
                inventory.AddKeys (1);

                Debug.Log("Keys x" + inventory.GetKeys ());
            }

            // Make Rupee disappear.
            Destroy(go);

            // Play Rupee collection clip.
            AudioSource.PlayClipAtPoint(keyCollectionSoundEffect, Camera.main.transform.position);
        }
        else if (go.CompareTag("Bomb"))
        {
            // Make sure game object has an inventory.
            if (inventory != null)
            {
                inventory.AddBombs (1);

                Debug.Log("Keys x" + inventory.GetBombs ());
            }

            // Make Rupee disappear.
            Destroy(go);

            // Play Rupee collection clip.
            AudioSource.PlayClipAtPoint(bombCollectionSoundEffect, Camera.main.transform.position);
        }
    }
}

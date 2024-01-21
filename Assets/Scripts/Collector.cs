using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    // Represents player inventory.
    Inventory inventory;

    // Sound Effects.
    public AudioClip rupeeCollectionSoundEffect;

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

                Debug.Log("rupees: " + inventory.GetRupees());
            }

            // Make Rupee disappear.
            Destroy (go);

            // Play Rupee collection clip.
            AudioSource.PlayClipAtPoint (rupeeCollectionSoundEffect, Camera.main.transform.position);
        }
    }
}

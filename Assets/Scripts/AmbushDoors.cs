using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushDoors : MonoBehaviour
{
    List<GameObject> ambushDoors;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            if(child.CompareTag("AmbushDoor"))
            {
                ambushDoors.Add(child.gameObject);
            }
        }
    }
    
    public void AmbushDoorState(bool state)
    {
        Debug.Log("Changing text state!");
        foreach(GameObject door in ambushDoors)
        {
            door.SetActive(state);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CharacterMechanics;
using UnityEngine;

public class AmbushDoors : RoomTrait
{
    public enum AmbushState
    {
        unentered,
        fighting,
        cleared
    }

    private AmbushState currentState = AmbushState.unentered;
    private GameObject player;
    List<GameObject> ambushDoors;

    // Start is called before the first frame update
    void Start()
    {
        ambushDoors = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
        foreach (Transform child in transform)
        {
            if(child.CompareTag("AmbushDoor"))
            {
                ambushDoors.Add(child.gameObject);
            }
        }
    }

    private void Update()
    {
        if (currentState != AmbushState.cleared)
        {
            if (currentState == AmbushState.unentered)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < 13f)
                {
                    setDoors(true);
                    currentState = AmbushState.fighting;
                }
            }
            else
            {
                if (player.GetComponent<RoomManager>().roomCleared)
                {
                    setDoors(false);
                    currentState = AmbushState.cleared;
                }
            }
        }
    }
    
    public override void setState(bool state) {}

    private void setDoors(bool state)
    {
        Debug.Log("Changing ambush door state!");
        foreach (GameObject door in ambushDoors)
        {
            door.SetActive(state);
        }
    }
}

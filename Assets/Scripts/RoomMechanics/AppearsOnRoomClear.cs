using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class AppearsOnRoomClear : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider bc;
    private GameObject room;
    private RoomManager rm;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider>();
        GameObject link = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Found 'Player' " + link);
        rm = link.GetComponent<RoomManager>();
        Debug.Log("Found room manager " + rm);
        room = transform.parent.gameObject;

        // Disable on start until the room is ready
        sr.enabled = false;
        bc.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (counter > 120)
        {
            counter = 0;
            Debug.Log("Player in room " + rm.GetCurrentRoom() + ", key in room " + room + ", room cleared: " + rm.roomCleared);
        } else
        {
            counter++;
        }
        if ((rm.GetCurrentRoom() == room) && rm.roomCleared) {
            sr.enabled = true;
            bc.enabled = true;
        }
    }
}

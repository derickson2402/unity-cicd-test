using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BowRoomPortal : MonoBehaviour
{
    public bool toBow;      // Is this portal to the bow room? else back to the blade room

    private void OnCollisionEnter(Collision collision)
    {
        RoomManager rm;
        Debug.Log("Collided with " + collision.gameObject + " with name " + collision.gameObject.name);
        if (collision.gameObject.name == "Link")
        {
            rm = collision.gameObject.GetComponent<RoomManager>();
        }
        else
        {
            return;
        }
        if (toBow)
        {
            rm.SetRoom(1, 4);
            collision.gameObject.transform.position = new Vector3(19, 53);
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Camera.main.transform.position = new Vector3(23.5f, 50.5f, -1);
        }
        else
        {
            rm.SetRoom(1, 5);
            collision.gameObject.transform.position = new Vector3(23.5f, 60f);
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Camera.main.transform.position = new Vector3(23.5f, 61.5f, -1);
        }
    }
}

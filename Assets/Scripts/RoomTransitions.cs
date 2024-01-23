using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CoroutineHelper;

public class RoomTransitions : MonoBehaviour
{
    public Camera mainCamera;
    private ArrowKeyMovement player;
    private Rigidbody rb;
    private BoxCollider boxCollider;

    private void Start()
    {
        player = GetComponent<ArrowKeyMovement>();
        rb = GetComponent<Rigidbody>();
        boxCollider = rb.GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "West_Door")
        {
            Debug.Log("starting West transition");
            RoomTransition(other.transform.position, new Vector3(-1f, 0, 0), new Vector3(-16f, 0, 0));
        }
        else if (other.tag == "East_Door")
        {
            Debug.Log("starting East transition");
            RoomTransition(other.transform.position, new Vector3(1f, 0, 0),new Vector3(16, 0, 0));
        }
        else if (other.tag == "North_Door")
        {
            Debug.Log("starting North transition");
            RoomTransition(other.transform.position, new Vector3(0, 1f, 0), new Vector3(0, 16, 0));
        }
        else if (other.tag == "South_Door")
        {
            Debug.Log("starting South transition");
            RoomTransition(other.transform.position, new Vector3(0, -1f, 0), new Vector3(0, -16, 0));
        }
    }

    void RoomTransition(Vector3 doorPosition, Vector3 playerMovement, Vector3 cameraMovement)
    {
        rb.velocity = Vector3.zero;
        rb.gameObject.SetActive(false);
        boxCollider.enabled = false;
        player.controlEnabled = false;
        Debug.Log("Player control removed");
        Debug.Log(transform.position);

        Vector3 doorFramePosition = doorPosition + (1f * playerMovement);
        StartCoroutine(CoroutineHelper.MoveObjectOverTime(transform, transform.position, doorFramePosition, 1f));
        Debug.Log("Player moved into doorframe");
        Debug.Log(transform.position);

        Vector3 newCameraPosition= mainCamera.transform.position + cameraMovement;
        StartCoroutine(CoroutineHelper.MoveObjectOverTime(mainCamera.transform, mainCamera.transform.position, newCameraPosition, 2.5f));
        Debug.Log("Camera moved into new room");
        Debug.Log(transform.position);

        Vector3 outerDoorPosition = doorPosition + (1f * playerMovement);
        StartCoroutine(CoroutineHelper.MoveObjectOverTime(transform, transform.position, outerDoorPosition, 1f));
        Debug.Log("Player moved into new room");
        Debug.Log(transform.position);

        rb.gameObject.SetActive(true);
        boxCollider.enabled = true;
        player.controlEnabled = true;
        Debug.Log("Player control returned");
        Debug.Log(transform.position);
    }
}

using System.Collections;
using UnityEngine;

public class RoomTransitions : MonoBehaviour
{
    public Camera mainCamera;
    
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private InputManager inputManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        inputManager = GetComponent<InputManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var difference = transform.position - other.transform.position;
        if (other.tag == "Horizontal_Door")
        {
            if (difference.x > 0)
            {
                Debug.Log("starting West transition");
                //RoomTransition(other.transform.position, new Vector3(-1f, 0, 0), new Vector3(-16f, 0, 0));
                StartCoroutine(RoomTransition(other.transform.position,
                    new Vector3(-1f, 0, 0),
                    new Vector3(-16, 0, 0)));
            }
            else
            {
                Debug.Log("starting East transition");
                //RoomTransition(other.transform.position, new Vector3(1f, 0, 0), new Vector3(16, 0, 0));
                StartCoroutine(RoomTransition(other.transform.position,
                    new Vector3(1f, 0, 0),
                    new Vector3(16, 0, 0)));
            }
        }
        else if (other.tag == "Vertical_Door")
        {
            if (difference.y > 0)
            {
                Debug.Log("starting South transition");
                //RoomTransition(other.transform.position, new Vector3(0, -1f, 0), new Vector3(0, -16, 0));
                StartCoroutine(RoomTransition(other.transform.position,
                    new Vector3(0, -1f, 0),
                    new Vector3(0, -11, 0)));
            }
            else
            {
                Debug.Log("starting North transition");
                //RoomTransition(other.transform.position, new Vector3(0, 1f, 0), new Vector3(0, 16, 0));
                StartCoroutine(RoomTransition(other.transform.position,
                    new Vector3(0, 1f, 0),
                    new Vector3(0, 11, 0)));
            }
        }
    }

    private IEnumerator RoomTransition(Vector3 doorPosition, Vector3 playerMovement, Vector3 cameraMovement)
    {
        //float roundedX = Mathf.Round(transform.position.x);
        rb.velocity = Vector3.zero;
        rb.detectCollisions = false;
        boxCollider.enabled = false;
        inputManager.controlEnabled = false;
        Debug.Log("Player control removed\nPlayer:" + transform.position + "\nDoor?: " + doorPosition);

        //Vector3 doorFramePosition = doorPosition + (0.5f * playerMovement);
        yield return
            StartCoroutine(CoroutineHelper.MoveObjectOverTime(transform, transform.position, doorPosition, 1f));
        Debug.Log("Player moved into door frame\n" + transform.position);

        var newCameraPosition = mainCamera.transform.position + cameraMovement;
        yield return StartCoroutine(CoroutineHelper.MoveObjectOverTime(mainCamera.transform,
            mainCamera.transform.position, newCameraPosition, 2.5f));
        Debug.Log("Camera moved into new room\n" + transform.position);

        var outerDoorPosition = doorPosition + 3f * playerMovement;
        yield return StartCoroutine(
            CoroutineHelper.MoveObjectOverTime(transform, transform.position, outerDoorPosition, 1f));
        Debug.Log("Player moved into new room\n" + transform.position);

        yield return new WaitForSeconds(2);

        rb.detectCollisions = true;
        boxCollider.enabled = true;
        inputManager.controlEnabled = true;
        Debug.Log("Player control returned\n" + transform.position);
        yield return null;
    }
}
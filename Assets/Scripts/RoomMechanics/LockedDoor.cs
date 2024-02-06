using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField]  private GameObject leftDoor;
    [SerializeField] private Sprite leftDoorOpen;
    [SerializeField]  private GameObject rightDoor;
    [SerializeField] private Sprite rightDoorOpen;
    [SerializeField]  private GameObject doorWay;

    public void openDoor()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;

        leftDoor.GetComponent<SpriteRenderer>().sprite = leftDoorOpen;
        leftDoor.GetComponent<BoxCollider>().center += new Vector3(-0.5f, 0, 0);

        rightDoor.GetComponent<SpriteRenderer>().sprite = rightDoorOpen;
        rightDoor.GetComponent<BoxCollider>().center += new Vector3(0.5f, 0, 0);

        //doorWay.SetActive(true);
        Debug.Log("Unlocking Door");
    }
}

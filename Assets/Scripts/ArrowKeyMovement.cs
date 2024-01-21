using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowKeyMovement : MonoBehaviour
{

    // Inspector field for movement speed.
    public float movementSpeed = 4.0f;
    public GridMovement movementScript;

    // RigidBody representing player.
    Rigidbody rb;

    // Start is called before the first frame update.
    void Start ()
    {
        // Grab a reference to the Rigidbody component in this gameobject.
        rb = GetComponent<Rigidbody> ();
    }

    // Update is called once per frame.
    void Update ()
    {
        // Grab current values of arrow keys.
        Vector2 currentInput = GetInput() * movementSpeed;

        if (currentInput != Vector2.zero)
        {
            movementScript.Move (currentInput);
        }

        // Set Rigidbody's velocity to currentInput.
        //rb.velocity = currentInput * movementSpeed;
    }

    Vector2 GetInput ()
    {
        // Grab the current value of the left-right arrow keys.
        float horizontalInput = Input.GetAxisRaw ("Horizontal");

        // Grab the current value of the up-down arrow keys.
        float verticalInput = Input.GetAxisRaw ("Vertical");

        // Restrict vertical movement if Rigidbody is moving horizontally.
        if (Mathf.Abs (horizontalInput) > 0.0f)  
        {
            verticalInput = 0.0f;
        }

        // Return current values of arrow keys as a Vector2.
        return new Vector2 (horizontalInput, verticalInput);
    }
}

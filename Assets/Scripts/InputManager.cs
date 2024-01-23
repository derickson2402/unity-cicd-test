using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool controlEnabled = true;
    private MovementManager mover;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<MovementManager>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set Rigidbody's velocity to currentInput.
        Vector2 currentInput = GetMovementInput();
        if (currentInput != Vector2.zero)
        {
            mover.OldMove(currentInput);
        }
    }

    Vector2 GetMovementInput()
    {
        // Grab the current value of the left-right arrow keys.
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Grab the current value of the up-down arrow keys.
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Restrict vertical movement if Rigidbody is moving horizontally.
        if (Mathf.Abs(horizontalInput) > 0.0f)
        {
            verticalInput = 0.0f;
        }

        // Return current values of arrow keys as a Vector2.
        return new Vector2(horizontalInput, verticalInput);
    }
}

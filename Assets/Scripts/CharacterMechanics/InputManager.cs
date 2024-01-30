using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool controlEnabled = true;
    private MovementManager mover;
    private Rigidbody rb;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<MovementManager>();
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // default movement to neutral
        //Vector2 currentInput = Vector2.zero;
        if (controlEnabled)
        {
            Direction movement = DirectionManager.GetCurrentInputDirection();
            Debug.Log("MovementDirection: " + movement);
            if (movement != Direction.None)
            {
                mover.Move(movement);
            }
            //mover.Move2(currentInput);
            if (Input.GetKey(KeyCode.Alpha1))
            {
                player.ActivateCheats();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                GetComponent<WeaponInterface>().useWeaponA();
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                GetComponent<WeaponInterface>().useWeaponB();
            }
        }
    }

    //Vector2 GetMovementInput()
    //{
    //    // Grab the current value of the left-right arrow keys.
    //    float horizontalInput = Input.GetAxisRaw("Horizontal");

    //    // Grab the current value of the up-down arrow keys.
    //    float verticalInput = Input.GetAxisRaw("Vertical");

    //    // Restrict vertical movement if Rigidbody is moving horizontally.
    //    if (Mathf.Abs(rb.velocity.x) > float.Epsilon)
    //    {
    //        verticalInput = 0.0f;
    //    }

    //    // Return current values of arrow keys as a Vector2.
    //    return new Vector2(horizontalInput, verticalInput);
    //}
}

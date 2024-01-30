using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool controlEnabled = true;
    private MovementManager mover;
    private Rigidbody rb;
    private PlayerController player;
    private const int MOVE_BUFFER_SIZE = 3;
    private Queue<Direction> movements;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<MovementManager>();
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerController>();
        movements = new Queue<Direction>();
    }

    void FixedUpdate()
    {
        while (movements.Count > 0)
        {
            Debug.Log("Movements Queue Count: " + movements.Count);
            Direction move = movements.Dequeue();
            Debug.Log("MovementDirection: " + move);
            mover.Move(move);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // default movement to neutral
        //Vector2 currentInput = Vector2.zero;
        if (controlEnabled)
        {
            if (movements.Count < MOVE_BUFFER_SIZE)
            {
                movements.Enqueue(DirectionManager.GetCurrentInputDirection());
            }
            else
            {
                movements.Dequeue();
                movements.Enqueue(DirectionManager.GetCurrentInputDirection());
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

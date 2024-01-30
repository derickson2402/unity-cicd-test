using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool controlEnabled = true;
    private PlayerMovement mover;
    private Rigidbody rb;
    private PlayerController player;
    private const int MOVE_BUFFER_SIZE = 3;
    private Queue<Direction> movements;

    void Start()
    {
        mover = GetComponent<PlayerMovement>();
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
        if (controlEnabled)
        {
            // Player movement (WASD) or (Arrow Keys)
            if (movements.Count < MOVE_BUFFER_SIZE)
            {
                movements.Enqueue(DirectionManager.GetCurrentInputDirection());
            }
            else
            {
                movements.Dequeue();
                movements.Enqueue(DirectionManager.GetCurrentInputDirection());
            }
            // Other controls
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
}

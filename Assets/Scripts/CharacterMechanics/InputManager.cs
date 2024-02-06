using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class InputManager : MonoBehaviour
{
    public bool controlEnabled = true;
    public bool goriyaInRoom = false;
    private PlayerMovement mover;
    private Rigidbody rb;
    private PlayerController player;
    private const int MOVE_BUFFER_SIZE = 3;
    private Queue<Direction> movements;
    private RoomManager roomManager;
    private string stringInput = "";
    private bool recievingStringInput = false;

    void Start()
    {
        mover = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerController>();
        movements = new Queue<Direction>();
        roomManager = GetComponent<RoomManager>();
    }

    void FixedUpdate()
    {
        while (movements.Count > 0)
        {
            Debug.Log("Movements Queue Count: " + movements.Count);
            Direction move = movements.Dequeue();
            Debug.Log("MovementDirection: " + move);
            mover.Move(move);
            if (goriyaInRoom)
            {
                roomManager.transmitDirectionToGoriya(move);
            }
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
            if (Input.GetKey(KeyCode.Alpha1) && !recievingStringInput)
            {
                player.ActivateCheats();
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                GetComponent<WeaponInterface>().useWeaponA();
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                GetComponent<PlayerController>().UseSecondaryWeapon();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<PlayerController>().EquipNextSecondaryWeapon();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                recievingStringInput = true;
            }
            else if (recievingStringInput)
            {
                foreach (char charInput in Input.inputString)
                {
                    if (char.IsDigit(charInput))
                        stringInput += charInput;
                    if (stringInput.Length == 2)
                    {
                        int x = int.Parse(stringInput[0].ToString());
                        int y = int.Parse(stringInput[1].ToString());

                        roomManager.TeleportToRoom(x, y);

                        // reset input
                        stringInput = "";
                        recievingStringInput = false;
                        return;
                    }
                }
            }
        }
    }
}

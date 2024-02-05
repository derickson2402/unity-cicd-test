using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CharacterMechanics;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Camera mainCamera;
    public int startX;
    public int startY;

    private Rigidbody rb;
    private BoxCollider boxCollider;
    private InputManager inputManager;


    private Dictionary<(int, int), GameObject> roomDictionary;
    private int roomX;
    private int roomY;
    private GameObject currentRoom;

    public bool roomCleared = false;
    public int currentRoomEnemyTotal;
    public int currentRoomEnemyDeaths;

    private GoriyaAI[] goriyaAIArray;

    void Start()
    {
        roomDictionary = new Dictionary<(int, int), GameObject>();
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject room in rooms)
        {
            string roomName = room.name;
            //need to minus '0' to convert from ascii to int
            int x = room.name[^4] - '0';
            int y = room.name[^2] - '0';
            roomDictionary[(x, y)] = room;
        }
        roomX = startX;
        roomY = startY;
        currentRoom = roomDictionary[(roomX, roomY)];
        SetRoomState(currentRoom, true);
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        if (roomCleared)
        {
            return;
        }
        if (currentRoomEnemyTotal > 0 && currentRoomEnemyTotal == currentRoomEnemyDeaths)
        {
            roomCleared = true;
            AmbushDoors temp;
            if (currentRoom.TryGetComponent<AmbushDoors>(out temp))
            {
                temp.setState(false);
            }
        } 
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
                    -1, 0,
                    new Vector3(-16, 0, 0)));
            }
            else
            {
                Debug.Log("starting East transition");
                //RoomTransition(other.transform.position, new Vector3(1f, 0, 0), new Vector3(16, 0, 0));
                StartCoroutine(RoomTransition(other.transform.position,
                    1, 0,
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
                    0, -1,
                    new Vector3(0, -11, 0)));
            }
            else
            {
                Debug.Log("starting North transition");
                //RoomTransition(other.transform.position, new Vector3(0, 1f, 0), new Vector3(0, 16, 0));
                StartCoroutine(RoomTransition(other.transform.position,
                    0, 1,
                    new Vector3(0, 11, 0)));
            }
        }
    }

    private IEnumerator RoomTransition(Vector3 doorPosition, int xChange, int yChange, Vector3 cameraMovement)
    {
        //float roundedX = Mathf.Round(transform.position.x);
        rb.velocity = Vector3.zero;
        rb.detectCollisions = false;
        boxCollider.enabled = false;
        inputManager.controlEnabled = false;

        SetRoomState(currentRoom, false);
        Debug.Log("Room (" + roomX + "," + roomY + ") is deactivated");

        Debug.Log("Player control removed\nPlayer:" + transform.position + "\nDoor?: " + doorPosition);

        //Vector3 doorFramePosition = doorPosition + (0.5f * playerMovement);
        yield return
            StartCoroutine(CoroutineHelper.MoveObjectOverTime(transform, transform.position, doorPosition, 1f));
        Debug.Log("Player moved into door frame\n" + transform.position);

        var newCameraPosition = mainCamera.transform.position + cameraMovement;
        yield return StartCoroutine(CoroutineHelper.MoveObjectOverTime(mainCamera.transform,
            mainCamera.transform.position, newCameraPosition, 2.5f));
        Debug.Log("Camera moved into new room\n" + transform.position);

        var outerDoorPosition = doorPosition + 2f * new Vector3(xChange, yChange, 0);
        yield return StartCoroutine(
            CoroutineHelper.MoveObjectOverTime(transform, transform.position, outerDoorPosition, 1f));
        Debug.Log("Player moved into new room\n" + transform.position);

        currentRoomEnemyTotal = 0;
        currentRoomEnemyDeaths = 0;
        roomCleared = false;
        inputManager.goriyaInRoom = false;
        roomX += xChange;
        roomY += yChange;
        currentRoom = roomDictionary[(roomX, roomY)];
        SetRoomState(currentRoom, true);
        Debug.Log("Room (" + roomX + "," + roomY + ") is active");

        rb.detectCollisions = true;
        boxCollider.enabled = true;
        inputManager.controlEnabled = true;
        Debug.Log("Player control returned\n" + transform.position);
        yield return null;
    }

    public void SetRoom(int x, int y)
    {
        SetRoomState(currentRoom, false);
        roomX = x;
        roomY = y;
        currentRoom = roomDictionary[(roomX, roomY)];
        SetRoomState(currentRoom, true);
    }

    private void SetRoomState(GameObject room, bool state)
    {
        // direct room modifications
        RoomTrait rm;
        if (room.TryGetComponent<RoomTrait>(out rm))
        {
            rm.setState(state);
        }

        // child modifications
        for (int i = 0; i < room.transform.childCount; i++)
        {
            var child = room.transform.GetChild(i);
            var gm = child.GetComponent<GenericMovement>();
            var anim1 = child.GetComponent<ScriptAnim4DirectionWalkPlusAttack>();
            var anim2 = child.GetComponent<ScriptAnim4Sprite>();
            var isEnemy = child.CompareTag("Enemy");
            if (gm != null)
            {
                gm.movementEnabled = state;
            }

            if (anim1 != null)
            {
                anim1.active = state;
            }

            if (anim2 != null)
            {
                anim2.active = state;
            }

            if (isEnemy && state)
            {
                currentRoomEnemyTotal++;
            }
        }


        goriyaAIArray = room.GetComponentsInChildren<GoriyaAI>();
        if (state && goriyaAIArray.Length > 0)
        {
            inputManager.goriyaInRoom = true;
        }
    }

    public void TeleportToRoom(int x, int y)
    {
        float xPlayer = (float)((currentRoom.transform.position.x + 7.5) + ((x - roomX) * 16f));
        float yPlayer = (float)((currentRoom.transform.position.y + 2) + ((y - roomY) * 11f));
        float xCamera = ((x - roomX) * 16f);
        float yCamera = ((y - roomY) * 11f);
        Vector3 playerOffset = new Vector3(xPlayer, yPlayer, 0);
        Vector3 cameraOffset = new Vector3(xCamera, yCamera, 0);
        transform.position = playerOffset;
        mainCamera.transform.position += cameraOffset;
        SetRoom(x, y);
    }

    public void recieveEnemyDeath()
    {
        currentRoomEnemyDeaths++;
    }

    public void transmitDirectionToGoriya(Direction move)
    {
        foreach (var goriyaAI in goriyaAIArray)
        {
            goriyaAI.SetMirrorMove(move);
        }
    }
    
    public GameObject GetCurrentRoom()
    {
        return currentRoom;
    }
}
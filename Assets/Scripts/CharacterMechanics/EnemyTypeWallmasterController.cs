using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(GenericMovement))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyTypeWallmasterController : MonoBehaviour
{
    public Sprite openHandSprite;
    public Sprite closeHandSprite;
    public float timeBetweenFrames;
    public float downSpeed;

    private GenericMovement mover;
    private Rigidbody rb;
    private SpriteRenderer sr;
    private Coroutine currentMovement;
    private bool grabbedPlayer;
    private Rigidbody playerRB;
    private float curTime;
    private bool onFrame1;

    void Start()
    {
        Assert.IsNotNull(openHandSprite);
        Assert.IsNotNull(closeHandSprite);
        mover = GetComponent<GenericMovement>();
        mover.movementEnabled = false;
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        currentMovement = StartCoroutine(RandomMovement());
    }

    private void Update()
    {
        if (!mover.movementEnabled)
        {
            return;
        }
        if (!grabbedPlayer)
        {
            // Basic cycle of sprites for animation
            curTime += Time.deltaTime;
            if (curTime >= timeBetweenFrames)
            {
                curTime = 0;
                onFrame1 = !onFrame1;
                if (onFrame1)
                {
                    sr.sprite = closeHandSprite;
                }
                else
                {
                    sr.sprite = openHandSprite;
                }
            }
        }
        else
        {
            // Move us and the player downward
            rb.velocity = Vector3.down * downSpeed;
            playerRB.position = rb.position;
        }
    }

    // Called to generate movement for NPC
    private IEnumerator RandomMovement()
    {
        while (mover.movementEnabled)
        {
            // do nothing 80% of the time
            if (!grabbedPlayer)
            {
                int randomValue = Random.Range(0, 10);
                if (randomValue < 8)
                {
                    yield return new WaitForSeconds(1);
                }
            }

            // 20% chance to move in a random direction (each direction is 25%)
            if (!grabbedPlayer)
            {
                Direction movement = DirectionManager.directions[Random.Range(0, DirectionManager.directions.Length)];
                Debug.Log("NPC " + gameObject.name + " moving " + movement.ToString());
                mover.Move(movement);
                yield return new WaitForSeconds(1);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(gameObject + " GRABBED " +  collision.gameObject);
            mover.movementEnabled = false;
            grabbedPlayer = true;
            sr.sprite = closeHandSprite;
            playerRB = collision.gameObject.GetComponent<Rigidbody>();
            collision.gameObject.GetComponent<PlayerMovement>().movementEnabled = false;
            collision.gameObject.GetComponent<ScriptAnim4DirectionWalkPlusAttack>().IdleModeOn();
        }
        else if ((collision.gameObject.name == "Tile_WALL" || collision.gameObject.name == "Tile_NONE") && grabbedPlayer)
        {
           GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().returnPlayerToStart();
           Destroy(gameObject);
        }
    }
}

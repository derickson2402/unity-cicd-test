using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ScriptAnim4DirectionWalkPlusAttack : MonoBehaviour
{
    public SpriteRenderer sr;       // Reference to characters SpriteRenderer
    public int flipFrameRate;       // How many frames to wait before flipping sprite (while walking)
    public Direction curDirection;  // What direction the character is currently facing
    public Sprite up;
    public Sprite down;
    public Sprite sideWalk;
    public Sprite sideIdle;
    public bool sideIdleAndWalkLookRight; // Are the sprites for sideWalk and sideIdle pointing right or left?
    public Sprite attackUp;
    public Sprite attackDown;
    public Sprite attackSide;
    public bool attackSideLooksRight; // Does the side attack sprite look right or left?

    private int curFrame;           // Current frame in animation, once it hits flipFrameRate flip the sprite
    private bool idleMode;          // Is the player idle right now (sits in idle frame and doesnt update)
    private bool onFrame1;          // Is the player on frame 1? (note all animations are just 2 frames going from 0 -> 1 -> 0)

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(sr);
        curFrame = 0;
    }

    // Flip the sprite to simulate walking
    void Update()
    {
        if (idleMode) { return; }
        if (curFrame < flipFrameRate)
        {
            ++curFrame;
        }
        else
        {
            curFrame = 0;
            onFrame1 = !onFrame1;
            if (curDirection == Direction.Up || curDirection == Direction.Down)
            {
                // Looking up/down, just flip x axis
                sr.flipX = !sr.flipX;
            }
            else
            {
                // Looking left/right, set flip x and alternate between walk/idle
                if (onFrame1)
                {
                    sr.sprite = sideWalk;
                }
                else
                {
                    sr.sprite = sideIdle;
                }
                sr.flipX = (sideIdleAndWalkLookRight ^ (curDirection == Direction.Right));
            }
        }
    }

    public void ChangeDirection(Direction dir)
    {
        sr.flipX = false;
        curFrame = 0;
        switch (dir)
        {
            case Direction.Left:
                sr.sprite = sideIdle;
                sr.flipX = sideIdleAndWalkLookRight;
                break;
            case Direction.Right:
                sr.sprite = sideIdle;
                sr.flipX = !sideIdleAndWalkLookRight;
                break;
            case Direction.Up:
                sr.sprite = up;
                break;
            case Direction.Down:
                sr.sprite = down;
                break;
        }

    }
    public void BeginAttack()
    {
        sr.flipX = false;
        idleMode = true;
        onFrame1 = false;
        curFrame = 0;
        switch (curDirection) {
            case Direction.Left:
                sr.sprite = attackSide;
                sr.flipX = sideIdleAndWalkLookRight;
                break;
            case Direction.Right:
                sr.sprite = attackSide;
                sr.flipX = !sideIdleAndWalkLookRight;
                break;
            case Direction.Up:
                sr.sprite = attackUp;
                break;
            case Direction.Down:
                sr.sprite = attackDown;
                break;
        }
    }

    public void EndAttack()
    {
        idleMode = false;
        ChangeDirection(curDirection);
    }
}

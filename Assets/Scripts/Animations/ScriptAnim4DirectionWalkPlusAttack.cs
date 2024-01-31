using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(SpriteRenderer))]
public class ScriptAnim4DirectionWalkPlusAttack : MonoBehaviour
{
    public float timeBetweenFrames;           // Frame rate between sprite changes
    public Sprite up0;
    public Sprite up1;
    public Sprite down0;
    public Sprite down1;
    public Sprite left0;
    public Sprite left1;
    public Sprite right0;
    public Sprite right1;
    public Sprite attackUp;
    public Sprite attackDown;
    public Sprite attackLeft;
    public Sprite attackRight;

    private Direction curDirection;     // What direction the character is currently facing
    private SpriteRenderer sr;          // Reference to this game objects sprite renderer
    private float curTime;              // Current animation frame we are on
    private bool idleMode;              // Are we currently in idle mode (will not change frames)
    private bool onFrame1;              // Are we on frame 1? otherwise on frame 0. All animations are 2 frames max

    public bool active;

    void Start()
    {
        active = false;
        sr = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(sr);
        curTime = 0;
        curDirection = GetComponent<GenericMovement>().directionManager.current;
    }

    // Change sprites for walking animation
    void Update()
    {
        if (!active)
        {
            return;
        }
        if (idleMode) { return; }
        curTime += Time.deltaTime;
        if (curTime >= timeBetweenFrames)
        {
            curTime = 0;
            onFrame1 = !onFrame1;
            switch (curDirection)
            {
                case Direction.Up:
                    sr.sprite = onFrame1 ? up1 : up0;
                    break;
                case Direction.Down:
                    sr.sprite = onFrame1 ? down1 : down0;
                    break;
                case Direction.Left:
                    sr.sprite = onFrame1 ? left1 : left0;
                    break;
                case Direction.Right:
                    sr.sprite = onFrame1 ? right1 : right0;
                    break;
            }
        }
    }

    public void ChangeDirection(Direction dir)
    {
        curTime = 0;
        onFrame1 = false;
        curDirection = dir;
        switch (curDirection)
        {
            case Direction.Up:
                sr.sprite = onFrame1 ? up1 : up0;
                break;
            case Direction.Down:
                sr.sprite = onFrame1 ? down1 : down0;
                break;
            case Direction.Left:
                sr.sprite = onFrame1 ? left1 : left0;
                break;
            case Direction.Right:
                sr.sprite = onFrame1 ? right1 : right0;
                break;
        }
    }
    public void BeginAttack()
    {
        idleMode = true;
        onFrame1 = false;
        curTime = 0;
        switch (curDirection) {
            case Direction.Left:
                sr.sprite = attackLeft;
                break;
            case Direction.Right:
                sr.sprite = attackRight;
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

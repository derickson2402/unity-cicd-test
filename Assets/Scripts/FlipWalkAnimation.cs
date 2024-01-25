using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FlipWalkAnimation : MonoBehaviour
{
    public int frames;    // How many frames in between each flip

    public SpriteRenderer sprite;  // SpriteRenderer component for the object
    public int curFrame;       // What frame we are on now in the cycle, resets on flip

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        curFrame = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (curFrame >= frames)
        {
            sprite.flipX = !sprite.flipX;
            curFrame = 0;
        } else {
            ++curFrame;
        }
    }
}

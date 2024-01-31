using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ScriptAnim4Sprite : MonoBehaviour
{
    public float timeBetweenFrames;         // How many seconds in between each flip
    public Sprite a;                        // Add any number of frames here, on Start they 
    public Sprite b;                        // will be checked and only the filled ones will be used
    public Sprite c;
    public Sprite d;
    public Sprite e;
    public Sprite f;
    public Sprite g;
    public Sprite h;

    private SpriteRenderer sr;      // SpriteRenderer component for the object
    private int curSpriteIndex;     // What sprite are we currently on?
    private float curTime;          // What time it is currently, resets on flip
    private Sprite[] spriteArr;     // Array object holding the a,b,c,d sprites

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        curTime = 0;
        int numSprites = 0;
        numSprites = (a != null) ? numSprites + 1 : numSprites;
        numSprites = (b != null) ? numSprites + 1 : numSprites;
        numSprites = (c != null) ? numSprites + 1 : numSprites;
        numSprites = (d != null) ? numSprites + 1 : numSprites;
        numSprites = (e != null) ? numSprites + 1 : numSprites;
        numSprites = (f != null) ? numSprites + 1 : numSprites;
        numSprites = (g != null) ? numSprites + 1 : numSprites;
        numSprites = (h != null) ? numSprites + 1 : numSprites;
        spriteArr = new Sprite[numSprites];
        curSpriteIndex = 0;
        if (a != null) { spriteArr[curSpriteIndex++] = a; }
        if (b != null) { spriteArr[curSpriteIndex++] = b; }
        if (c != null) { spriteArr[curSpriteIndex++] = c; }
        if (d != null) { spriteArr[curSpriteIndex++] = d; }
        if (e != null) { spriteArr[curSpriteIndex++] = e; }
        if (f != null) { spriteArr[curSpriteIndex++] = f; }
        if (g != null) { spriteArr[curSpriteIndex++] = g; }
        if (h != null) { spriteArr[curSpriteIndex++] = h; }
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime >= timeBetweenFrames)
        {
            curTime = 0;
            curSpriteIndex = (curSpriteIndex + 1) % spriteArr.Length;
            sr.sprite = spriteArr[curSpriteIndex];
        }
    }
}

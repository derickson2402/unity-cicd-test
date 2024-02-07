using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CharacterMechanics;
using UnityEngine;

public class PuzzleDoors : RoomTrait
{
    public Sprite openDoor;
    private BoxCollider boxCollider;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void setState(bool state) {}

    public void Open()
    {
        boxCollider.enabled = false;
        spriteRenderer.sprite = openDoor;
    }
}

using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CharacterMechanics;
using UnityEngine;

public class PuzzleDoors : RoomTrait
{
    public override void setState(bool state) {}

    public void Open()
    {
        Debug.Log("Changing ambush door state!");
        GetComponentInChildren<LockedDoor>().openDoor();
    }
}

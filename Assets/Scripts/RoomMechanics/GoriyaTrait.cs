using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CharacterMechanics;
using UnityEngine;

public class GoriyaTrait : RoomTrait
{
    private GoriyaAI[] goriyaAIArray;

    // Start is called before the first frame update
    void Start()
    {
        goriyaAIArray = gameObject.GetComponentsInChildren<GoriyaAI>();
    }

    public override void setState(bool state) {}

    public GoriyaAI[] getGoriyaAIArray() {  return goriyaAIArray; }
}
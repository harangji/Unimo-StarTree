using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using Random = UnityEngine.Random;

public class Mon005State_Preaction_ST2 : MonsterState_Preaction
{
    [SerializeField] private float moveSpeed = 4.2f;
    [SerializeField] private int moveDir_0x1y2z = 1;
    [SerializeField] private int moveFrontBack = -1;
    
    private Vector3 moveVec = Vector3.down;

    public override void UpdateAction()
    {
        controller.transform.position += moveSpeed * Time.deltaTime * moveVec;

        if (controller.transform.position.y <= -3f)
        {
            InvokeDisappearState();
        }
    }

    private void InvokeDisappearState()
    {
        if (!hasTransit)
        {
            hasTransit = true;
            controller.EnemyDisappear();
        }
    }
}
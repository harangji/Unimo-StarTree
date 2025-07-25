using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mon005State_Preaction : MonsterState_Preaction
{
    [SerializeField] private float moveSpeed = 4.2f;
    [SerializeField] private int moveDir_0x1y2z = 1;
    [SerializeField] private int moveFrontBack = -1;
    protected float existTime = 6f;
    private Vector3 moveVec;

    public override void TransitionAction(MonsterController controller)
    {
        this.controller = controller;
        setMoveDirVector();
    }

    public override void UpdateAction()
    {
        existTime -= Time.deltaTime;
        if (existTime < 0)
        {
            InvokeDisappearState();
        }
        else //이동 로직 처리 =>
        {
            switch (controller.pattern)
            {
                case Patterns.Pattern1:
                    controller.transform.position += moveSpeed * Time.deltaTime * moveVec;

                    if (controller.transform.position.y < -2f * controller.transform.localScale.y) //이거 왜 있는 거지
                    {
                        controller.EnemyExplode();
                    }
                    break;
                case Patterns.Pattern2:
                    break;
                case Patterns.Pattern3:
                    break;
            }
        }
    }

    private void setMoveDirVector()
    {
        float angle = Random.Range(0f, 360f);
        Vector3 moveDir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));
        
        moveVec = moveFrontBack * moveDir;
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
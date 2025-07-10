using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinWheelAnimator : MonoBehaviour
{
    public FBX_Animator[] animator;

    private void Update()
    {
        for(int i = 0; i< Base_Manager.Data.UserData.BuffFloating.Length; i++)
        {
            if (Base_Manager.Data.UserData.BuffFloating[i] > 0.0f)
            {
                animator[i].speed = 150.0f;
            }
            else
            {
                animator[i].speed = 10.0f;
            }
        }
    }
}

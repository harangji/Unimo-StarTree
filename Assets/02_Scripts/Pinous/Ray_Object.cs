using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Ray_Object : MonoBehaviour
{
    public bool CameraMover;
    public CameraMoveState state;
    public string ResourcesName;
    public void GetRayCheck()
    {
        if (CameraMover)
        {
            Camera_Event.instance.GetCameraEvent(state);
        }
        if(state != CameraMoveState.BonusReward)
        {
            Canvas_Holder.instance.GetUI(ResourcesName);
        }
        else
        {
            if (Base_Manager.Data.UserData.BonusRewardCount < 900.0f)
            {
                return;
            }
            else Canvas_Holder.instance.GetUI(ResourcesName);
        }
    }
}

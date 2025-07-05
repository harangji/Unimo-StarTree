using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum checkType
{
    Disable,
    LockActive
}
public class LevelCheck : MonoBehaviour
{
    public int Level;
    public checkType typeCheck;
    public GameObject LockObj;
    public bool BonusReward;

    public GameObject LockRewardOBJ, InserParticle;
    public TextMeshProUGUI CountText;
    private void Start()
    {
        Canvas_Holder.instance.levelCheckObjs.Add(this);

        InitCheck();
    }

    private void Update()
    {
        if(BonusReward)
        {
            if(Base_Mng.Data.data.BonusRewardCount >= 900.0f)
            {
                LockRewardOBJ.SetActive(false);
                InserParticle.SetActive(true);
                CountText.text = Localization_Mng.local_Data["UI/PlusReward"].Get_Data();
            }
            else
            {
                LockRewardOBJ.SetActive(true);
                InserParticle.SetActive(false);
                CountText.text = ShowTimer(900 - Base_Mng.Data.data.BonusRewardCount);
            }
        }
    }

    public void InitCheck()
    {
        if (Base_Mng.Data.data.Level >= Level)
        {
            switch (typeCheck)
            {
                case checkType.Disable: transform.gameObject.SetActive(true); break;
                case checkType.LockActive: LockObj.SetActive(false); break;
            }
        }
        else
        {
            switch (typeCheck)
            {
                case checkType.Disable: transform.gameObject.SetActive(false); break;
                case checkType.LockActive: LockObj.SetActive(true); break;
            }
        }
    }

    public static string ShowTimer(double timer)
    {
        TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(timer));

        string answer = string.Format("{0:D2}:{1:D2}",

                              t.Minutes,

                              t.Seconds);

        return answer;
    }
}

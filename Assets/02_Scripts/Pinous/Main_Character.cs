using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Character : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Get_NeglectReward());     // ¹æÄ¡ ÀçÈ­ È¹µæ ÀÓ½Ã·Î ¸·¾Æ³ù½À´Ï´Ù.
    }

    IEnumerator Get_NeglectReward()
    {
        yield return new WaitForSeconds(0.5f);

        //double valueCount = (Base_Mng.Data.data.BestScoreGameOne + Base_Mng.Data.data.BestScoreGameTwo) / 10.0f;
        //Debug.Log(valueCount);
        var yfReward = RewardCalculator.GetYfByAltaLevel();
        var ofReward = RewardCalculator.GetOfByAltaLevel();
        if (Base_Manager.Data.UserData.BuffFloating[0]>0.0f)
        {
            yfReward *= 1.3f;
            ofReward *= 1.3f;
        }
        Base_Manager.Data.AssetPlus(Asset_State.Yellow, yfReward);
        Base_Manager.Data.AssetPlus(Asset_State.Red, ofReward);
        
        Debug.Log($"ÇöÀç ·¹º§ ::: {Base_Manager.Data.UserData.Level+1} ::: È¹µæ ÀçÈ­ ::: ³ë¶û {yfReward}, ÁÖÈ² {ofReward}");

        if (yfReward >= 1)
        {
            Get_TEXT go = Instantiate(Resources.Load<Get_TEXT>("TextMESH"));
            go.Init(transform.position, yfReward);
        }
        
        yield return new WaitForSeconds(0.5f);

        if (ofReward >= 1)
        {
            Get_TEXT go = Instantiate(Resources.Load<Get_TEXT>("TextMESH_Orange"));
            go.Init(transform.position, ofReward);
        }
        
        StartCoroutine(Get_NeglectReward());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

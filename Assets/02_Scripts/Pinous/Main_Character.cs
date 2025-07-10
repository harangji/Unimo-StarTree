using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Character : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Get_Yellow());
    }

    IEnumerator Get_Yellow()
    {
        yield return new WaitForSeconds(1.0f);

        //double valueCount = (Base_Mng.Data.data.BestScoreGameOne + Base_Mng.Data.data.BestScoreGameTwo) / 10.0f;
        //Debug.Log(valueCount);
        double reward = Base_Manager.Data.UserData.Second_Base;
        if (Base_Manager.Data.UserData.BuffFloating[0]>0.0f)
        {
            reward *= 1.3f;
        }
        Base_Manager.Data.AssetPlus(Asset_State.Yellow, reward);

        var go = Instantiate(Resources.Load<Get_TEXT>("TextMESH"));
        go.Init(transform.position, reward);

        StartCoroutine(Get_Yellow());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

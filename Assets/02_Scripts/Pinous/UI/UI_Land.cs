using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Land : UI_Base
{
    public int[] Level;
    public GameObject[] Lock;
    private void Start()
    {
        for(int i = 0; i< Level.Length; i++)
        {
            if (Base_Manager.Data.UserData.Level >= Level[i])
            {
                Lock[i].SetActive(false);
            }
        }
    }

    public void GetInButton(string temp)
    {
        Canvas_Holder.instance.GetUI(temp);
    }
}

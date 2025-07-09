using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cosume_Base_Panel : MonoBehaviour
{
    public int index;
    public CharCostumer costumer;
    private GameObject LockObj;
    private Button ButtonObj;
    private void Start()
    {
        ButtonObj = GetComponent<Button>();
        LockObj = transform.GetChild(1).gameObject;
        LockObj.GetComponent<Button>().onClick.AddListener(() => GetLockDisable());
        switch(costumer)
        {
            case CharCostumer.Charcater:
                if (Base_Manager.Data.UserData.GetCharacterData[index])
                {
                    Destroy(LockObj);
                }
                else
                {
                    ButtonObj.onClick.RemoveAllListeners();
                }
                break;
            case CharCostumer.EQ:
                if (Base_Manager.Data.UserData.GetEQData[index])
                {
                    Destroy(LockObj);
                }
                else
                {
                    ButtonObj.onClick.RemoveAllListeners();
                }
                break;
        }
    }

    public void GetLockDisable()
    {
        if(costumer == CharCostumer.Charcater)
        {
            switch(index)
            {
                case 1: case 2: case 4: case 7: case 9: case 11:
                case 5:
                    Canvas_Holder.instance.GetUI("##Shop");
                    break;
                case 3: case 10:
                case 12:
                    Canvas_Holder.instance.GetUI("##Mission");
                    break;
            }
        }
        else if(costumer == CharCostumer.EQ)
        {
            switch(index)
            {
                case 1: case 2: case 4: case 7: case 9: case 11: case 5: case 12:
                case 16:
                case 17:
                case 19:
                case 20:
                    Canvas_Holder.instance.GetUI("##Shop");
                    break;
                case 3: case 10:
                case 13:
                case 14:
                case 15:
                case 18:
                case 21:
                case 22:
                    Canvas_Holder.instance.GetUI("##Mission");
                    break;
            }
        }
    }
}

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
        
        CosumePanelManager.Instance?.Register(this); // <- 매니저에 등록
        
        switch(costumer)
        {
            
            case CharCostumer.Charcater:
                if (Base_Manager.Data.UserData.HasCharacterData[index])
                {
                    Destroy(LockObj);
                }
                else
                {
                    ButtonObj.onClick.RemoveAllListeners();
                }
                break;
            case CharCostumer.EQ:
                if (Base_Manager.Data.UserData.HasEnginData[index])
                {
                    Destroy(LockObj);
                }
                else
                {
                    ButtonObj.onClick.RemoveAllListeners();
                }
                break;
        }
        Debug.Log($"[{name}] CosumePanelManager에 등록됨");
    }

    public void ForceUnlock()
    {
        if (LockObj != null)
        {
            Debug.Log($"[{name}] LockObj 해제됨");
            LockObj.SetActive(false);  // 눈에 보이는 즉시 해제
            Destroy(LockObj);          // 필요 시 파괴
            LockObj = null;
        }
    }
    
    public void GetLockDisable()
    {
        if(costumer == CharCostumer.Charcater)
        {
            switch(index)
            {
                case 1: 
                case 3:
                case 4: 
                case 5:
                case 7:
                case 8:
                case 9:
                case 11:
                    Canvas_Holder.instance.GetUI("##Shop");
                    break;
                case 2:
                case 6:
                case 10:
                case 12:
                    Canvas_Holder.instance.GetUI("##Mission");
                    break;
            }
        }
        else if(costumer == CharCostumer.EQ)
        {
            switch(index)
            {
                case 1: 
                case 3:
                case 4: 
                case 5: 
                case 7: 
                case 8:
                case 9: 
                case 11: 
                case 16:
                case 17:
                case 19:
                case 20:
                    Canvas_Holder.instance.GetUI("##Shop");
                    break;
                case 2:
                case 6:
                case 10:
                case 12:
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

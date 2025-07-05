using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum CharCostumer
{
    Charcater,
    EQ
}

public class CurrentCharReward : MonoBehaviour
{

    public static CurrentCharReward instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private int currentChar = 1;
    private int currentEq = 1;
    public GameObject eqObj;
    public GameObject chObj;
    CharCostumer costumer;
    public void Init(CharCostumer m_State, int charCnt = -1, int eqCnt = -1)
    {
        costumer = m_State;

        if (chObj != null) chObj.SetActive(false);
        if (eqObj != null) eqObj.SetActive(false);

        currentEq = eqCnt;
        if (costumer == CharCostumer.Charcater)
        {
            currentChar = charCnt;
            Debug.Log(charCnt);
            currentEq = charCnt;
        }
        makePreviewObj(costumer);
    }
    public void ChangeCharacter(int diff)
    {
        currentChar = diff;
        makePreviewObj(costumer);
    }
    public void ChangeEquip(int diff)
    {
        currentEq = diff;
        makePreviewObj(costumer);
    }
    public void makePreviewObj(CharCostumer m_State)
    {
        if (chObj != null) { Addressables.ReleaseInstance(chObj); }
        if (eqObj != null) { Addressables.ReleaseInstance(eqObj); }
        Addressables.InstantiateAsync(AddressableKeyCtrl.EqAssetKey_Lobby(currentEq), transform).Completed += (op) =>
        {
            if (op.Status != AsyncOperationStatus.Succeeded) { return; }

            eqObj = op.Result.gameObject;
            eqObj.GetComponent<Equip_ChaSetter>().InstCharacterAsync(AddressableKeyCtrl.ChaAssetKey_Lobby(currentChar), null, null);

            switch(m_State)
            {
                case CharCostumer.EQ: eqObj.transform.GetChild(1).gameObject.SetActive(false); break;
            }
        };
    }
}

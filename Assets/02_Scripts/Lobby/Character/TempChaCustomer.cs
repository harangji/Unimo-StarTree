using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TempChaCustomer : MonoBehaviour
{
    private int currentChar = 1;
    private int currentEq = 1;
    private GameObject eqObj;
    private GameObject chObj;
    private void Start()
    {
        currentChar = Base_Manager.Data.UserData.CharCount;
        currentEq = Base_Manager.Data.UserData.EQCount;

        GameManager.Instance.ChaIdx = currentChar;
        GameManager.Instance.EqIdx = currentEq;
        makePreviewObj();
    }
    public void ChangeCharacter(int diff)
    {
        currentChar = diff;
        Base_Manager.Data.UserData.CharCount = currentChar;
        GameManager.Instance.ChaIdx = currentChar;
        makePreviewObj();
    }
    public void ChangeEquip(int diff)
    {
        currentEq = diff;
        Base_Manager.Data.UserData.EQCount = currentEq;
        GameManager.Instance.EqIdx = currentEq;
        makePreviewObj();
    }
    public void makePreviewObj()
    {
        if (chObj != null) { Addressables.ReleaseInstance(chObj); }
        if (eqObj != null) { Addressables.ReleaseInstance(eqObj); }
        Addressables.InstantiateAsync(AddressableKeyCtrl.EqAssetKey_Lobby(currentEq), transform).Completed += (op) =>
        {
            if (op.Status != AsyncOperationStatus.Succeeded) { return; }

            eqObj = op.Result.gameObject;
            eqObj.GetComponent<Equip_ChaSetter>().InstCharacterAsync(AddressableKeyCtrl.ChaAssetKey_Lobby(currentChar), null, null);
        };
    }
}

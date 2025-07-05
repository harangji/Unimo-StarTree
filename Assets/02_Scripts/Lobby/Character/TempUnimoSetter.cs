using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TempUnimoSetter : MonoBehaviour
{
    public int currentChar = 1;
    public int currentEq = 1;
    private GameObject eqObj;
    private GameObject chObj;
    private void Start()
    {
        setUnimoModeling();
    }
    private void setUnimoModeling()
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

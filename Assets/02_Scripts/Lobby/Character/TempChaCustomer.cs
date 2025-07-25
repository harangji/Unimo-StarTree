using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TempChaCustomer : MonoBehaviour
{
    private int currentcharacter;
    private int currentengine;
    private GameObject eqObj;
    private GameObject chObj;
    private void Start()
    {
        currentcharacter = Base_Manager.Data.UserData.selectCharacter;
        currentengine = Base_Manager.Data.UserData.selectEngine;

        int selectedUnimoID = PlayerPrefs.GetInt("LastSelectedUnimoID", 10101);
        int selectedEngineID = PlayerPrefs.GetInt("LastSelectedEngineID", 21101);

        Debug.Log($"[TempChaCustomer] 마지막 선택된 캐릭터 ID : {selectedUnimoID}");
        Debug.Log($"[TempChaCustomer] 마지막 선택된 엔진 ID : {selectedEngineID}");

        GameManager.Instance.SelectedUnimoID = selectedUnimoID;
        GameManager.Instance.SelectedEngineID = selectedEngineID;

        GameManager.Instance.ChaIdx = currentcharacter;
        GameManager.Instance.EqIdx = currentengine;

        makePreviewObj();
    }
    
    public void ChangeCharacter(int diff, bool isPreviewOnly = false)
    {
        currentcharacter = diff;

        if (!isPreviewOnly)
        {
            Base_Manager.Data.UserData.selectCharacter = currentcharacter;
            GameManager.Instance.ChaIdx = currentcharacter;

            int unitID = UnitIDMapping.GetUnitID(currentcharacter);
            PlayerPrefs.SetInt("LastSelectedUnimoID", unitID);
            GameManager.Instance.SelectedUnimoID = unitID;

            makePreviewObj();
        }
    }
    
    public void ChangeEquip(int diff, bool isPreviewOnly = false)
    {
        currentengine = diff;

        if (!isPreviewOnly)
        {
            Base_Manager.Data.UserData.selectEngine = currentengine;
            GameManager.Instance.EqIdx = currentengine;

            int engineID = EngineIDMapping.GetEngineID(currentengine);
            PlayerPrefs.SetInt("LastSelectedEngineID", engineID);
            GameManager.Instance.SelectedEngineID = engineID;

            makePreviewObj();
        }
    }
    
    public void makePreviewObj()
    {
        if (chObj != null) { Addressables.ReleaseInstance(chObj); }
        if (eqObj != null) { Addressables.ReleaseInstance(eqObj); }

        Addressables.InstantiateAsync(AddressableKeyCtrl.EqAssetKey_Lobby(currentengine), transform).Completed += (op) =>
        {
            if (op.Status != AsyncOperationStatus.Succeeded) { return; }

            eqObj = op.Result.gameObject;
            eqObj.GetComponent<Equip_ChaSetter>().InstCharacterAsync(AddressableKeyCtrl.ChaAssetKey_Lobby(currentcharacter), null, null);
        };
    }
}

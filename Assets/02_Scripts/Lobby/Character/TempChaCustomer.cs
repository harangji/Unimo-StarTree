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

        // 마지막 선택된 캐릭터 ID 불러오기
        int selectedUnimoID = PlayerPrefs.GetInt("LastSelectedUnimoID", 10101);

        Debug.Log($"[TempChaCustomer] 마지막 선택된 캐릭터 ID : {selectedUnimoID}");

        // GameManager로 넘겨서 인게임에서 사용할 수 있도록만 설정
        GameManager.Instance.SelectedUnimoID = selectedUnimoID;

        GameManager.Instance.ChaIdx = currentcharacter;
        GameManager.Instance.EqIdx = currentengine;

        makePreviewObj();
    }
    
    public void ChangeCharacter(int diff)
    {
        currentcharacter = diff;
        Base_Manager.Data.UserData.selectCharacter = currentcharacter;
        GameManager.Instance.ChaIdx = currentcharacter;
        
        //  인덱스를 유닛ID로 변환 후 저장해야 합니다.
        int unitID = UnitIDMapping.GetUnitID(currentcharacter);
        
        // 선택 인덱스를 실제 유닛 ID로 변환하여 저장해야 합니다.
        PlayerPrefs.SetInt("LastSelectedUnimoID", unitID); 
        GameManager.Instance.SelectedUnimoID = unitID; // 유닛 ID 저장

        
        makePreviewObj();
    }
    public void ChangeEquip(int diff)
    {
        currentengine = diff;
        Base_Manager.Data.UserData.selectEngine = currentengine;
        GameManager.Instance.EqIdx = currentengine;

        int engineID = EngineIDMapping.GetEngineID(currentengine);
        GameManager.Instance.SelectedEngineID = engineID;  // 실제 엔진 ID 저장

        makePreviewObj();
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

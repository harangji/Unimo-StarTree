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

        // ������ ���õ� ĳ���� ID �ҷ�����
        int selectedUnimoID = PlayerPrefs.GetInt("LastSelectedUnimoID", 10101);

        Debug.Log($"[TempChaCustomer] ������ ���õ� ĳ���� ID : {selectedUnimoID}");

        // GameManager�� �Ѱܼ� �ΰ��ӿ��� ����� �� �ֵ��ϸ� ����
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
        
        //  �ε����� ����ID�� ��ȯ �� �����ؾ� �մϴ�.
        int unitID = UnitIDMapping.GetUnitID(currentcharacter);
        
        // ���� �ε����� ���� ���� ID�� ��ȯ�Ͽ� �����ؾ� �մϴ�.
        PlayerPrefs.SetInt("LastSelectedUnimoID", unitID); 
        GameManager.Instance.SelectedUnimoID = unitID; // ���� ID ����

        
        makePreviewObj();
    }
    public void ChangeEquip(int diff)
    {
        currentengine = diff;
        Base_Manager.Data.UserData.selectEngine = currentengine;
        GameManager.Instance.EqIdx = currentengine;

        int engineID = EngineIDMapping.GetEngineID(currentengine);
        GameManager.Instance.SelectedEngineID = engineID;  // ���� ���� ID ����

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

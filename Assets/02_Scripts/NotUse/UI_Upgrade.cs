using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UI_Upgrade : UI_Base
{
    private TempChaCustomer Costumer;

    public GameObject[] SelectObj;
    public GameObject[] Objs;
    

    [SerializeField] private GameObject upgradePopupPrefab;  // ������ ���� ����
    [SerializeField] private GameObject engineUpgradePopupPrefab;

    private UI_UpgradePopup upgradePopupInstance;
    private UI_EngineUpgradePopup engineUpgradePopupInstance;

    public override void Start()
    {
        Costumer = Costume_Finder.instance.transform.GetComponent<TempChaCustomer>();
        Costume_Finder.instance.GetStartHandler = true;
        base.Start();
        
    }

    public override void Update()
    {
        return;
        base.Update();
    }

    private void ShowUpgradePopup(int unitID)
    {
        if (upgradePopupInstance == null)
        {
            // �������� �ν��Ͻ�ȭ
            GameObject obj = Instantiate(upgradePopupPrefab, transform.parent);  // Canvas ������ ����
            upgradePopupInstance = obj.GetComponent<UI_UpgradePopup>();
        }

        upgradePopupInstance.OpenUpgradeUI(unitID);
    }
    
    private void ShowEngineUpgradePopup(int engineID)
    {
        if (engineUpgradePopupInstance == null)
        {
            GameObject obj = Instantiate(engineUpgradePopupPrefab, transform.parent);
            engineUpgradePopupInstance = obj.GetComponent<UI_EngineUpgradePopup>();
        }
        engineUpgradePopupInstance.OpenUpgradeUI(engineID);
    }

    public void CharacterChange(int value)
    {
        int charIndex = value + 1;
        Costumer.ChangeCharacter(charIndex, isPreviewOnly: true); // ������ �� �ٲ�
        int unitID = UnitIDMapping.GetUnitID(charIndex);
        ShowUpgradePopup(unitID);
    }

    public void EQChange(int value)
    {
        int engineIndex = value + 1;
        Costumer.ChangeEquip(engineIndex, isPreviewOnly: true); // ������ �� �ٲ�
        int engineID = EngineIDMapping.GetEngineID(engineIndex);
        ShowEngineUpgradePopup(engineID);
    }

    public void GetUnimo()
    {
        SelectObj[0].SetActive(true);
        SelectObj[1].SetActive(false);
        Objs[0].SetActive(true);
        Objs[1].SetActive(false);
    }

    public void GetEQ()
    {
        SelectObj[0].SetActive(false);
        SelectObj[1].SetActive(true);
        Objs[0].SetActive(false);
        Objs[1].SetActive(true);
    }
    
    public override void DisableOBJ()
    {
        Costume_Finder.instance.ReturnCamera();
        base.DisableOBJ();
    }
    
}

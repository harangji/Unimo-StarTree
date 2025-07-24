using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UI_Upgrade : UI_Base
{
    private TempChaCustomer Costumer;

    public GameObject[] SelectObj;
    public GameObject[] Objs;
    

    [SerializeField] private GameObject upgradePopupPrefab;  // 프리팹 에셋 연결
    private UI_UpgradePopup upgradePopupInstance;

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
            // 동적으로 인스턴스화
            GameObject obj = Instantiate(upgradePopupPrefab, transform.parent);  // Canvas 하위에 생성
            upgradePopupInstance = obj.GetComponent<UI_UpgradePopup>();
        }

        upgradePopupInstance.OpenUpgradeUI(unitID);
    }
    

    public override void DisableOBJ()
    {
        Costume_Finder.instance.ReturnCamera();
        base.DisableOBJ();
    }

    public void CharacterChange(int value)
    {
        int charIndex = value + 1;
        Costumer.ChangeCharacter(charIndex, isPreviewOnly: true); // 프리뷰 안 바뀜
        int unitID = UnitIDMapping.GetUnitID(charIndex);
        ShowUpgradePopup(unitID);
    }

    public void EQChange(int value)
    {
        int equipIndex = value + 1;
        Costumer.ChangeEquip(equipIndex, isPreviewOnly: true); // 프리뷰 안 바뀜
        int unitID = UnitIDMapping.GetUnitID(equipIndex);
        ShowUpgradePopup(unitID);
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
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Costume : UI_Base
{
    TempChaCustomer Costumer;

    public GameObject[] SelectObj;
    public GameObject[] Objs;

    public TextMeshProUGUI TitleText;

    public override void Start()
    {
        Costumer = Costume_Finder.instance.transform.GetComponent<TempChaCustomer>();
        Costume_Finder.instance.GetStartHandler = true;
        TitleText.text = Localization_Manager.local_Data["UI/Unimo"].Get_Data();
        base.Start();
    }

    public override void Update()
    {
        return;
        base.Update();
    }

    public override void DisableOBJ()
    {
        Costume_Finder.instance.ReturnCamera();
        base.DisableOBJ();
    }

    public void CharacterChange(int value)
    {
        Costumer.ChangeCharacter(value + 1);
    }

    public void EQChange(int value)
    {
        Costumer.ChangeEquip(value + 1);
    }

    public void GetUnimo()
    {
        TitleText.text = Localization_Manager.local_Data["UI/Unimo"].Get_Data();

        SelectObj[0].SetActive(true);
        SelectObj[1].SetActive(false);

        Objs[0].SetActive(true);
        Objs[1].SetActive(false);
    }

    public void GetEQ()
    {
        TitleText.text = Localization_Manager.local_Data["UI/B-B-Engine"].Get_Data();
        SelectObj[0].SetActive(false);
        SelectObj[1].SetActive(true);

        Objs[0].SetActive(false);
        Objs[1].SetActive(true);
    }
}

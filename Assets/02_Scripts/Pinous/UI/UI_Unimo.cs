using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Unimo : UI_Base
{
    public string Key;
    public TextMeshProUGUI Name, Description;
    public override void Start()
    {
        Debug.Log(Key);
        Name.text = Localization_Manager.local_Data["Character/" + Key.ToString()].Get_Data();
        Description.text = Localization_Manager.local_Data["Character/" + Key.ToString() + "_Text"].Get_Data();
        base.Start();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) DisableOBJ();
    }

    public override void DisableOBJ()
    {
        Canvas_Holder.instance.GetLock(true, false);
        Camera_Event.instance.ReturnCamera();
        base.DisableOBJ();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUP_Toast : MonoBehaviour
{
    public TextMeshProUGUI TMP_Text;

    public void GetPopUp(string temp)
    {
        TMP_Text.text = Localization_Manager.local_Data["Popup/" + temp].Get_Data();
        Destroy(this.gameObject, 2.0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupTextObj : MonoBehaviour
{
    [SerializeField] private GameObject popupObj;
    private TextMeshProUGUI popupMsg;
    private RectTransform popupTransform;

    // Start is called before the first frame update
    void Start()
    {
        popupMsg = popupObj.GetComponentInChildren<TextMeshProUGUI>();
        popupTransform = popupObj.GetComponent <RectTransform>();
    }
    public void SetPopupProperties(string txt, Vector2 pos, bool isActive)
    {
        popupMsg.text = txt;
        popupTransform.anchoredPosition = pos;
        if (isActive) { popupObj.SetActive(true); }
        else { popupObj.SetActive(false); }
    }
}

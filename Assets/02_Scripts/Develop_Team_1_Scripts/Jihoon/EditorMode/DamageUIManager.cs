using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageUIManager : MonoBehaviour
{
    public static DamageUIManager instance;
    
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject damageUIPrefab;

    private Queue<GameObject> uiPool = new();
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        MakeUIPooling();
    }

    private void MakeUIPooling()
    {
        for (int i = 0; i < 10; i++)
        {
            var ui = Instantiate(damageUIPrefab, parent);
            ui.SetActive(false);
        
            uiPool.Enqueue(ui);
        }
    }

    public void GetUI(float dmg)
    {
        var ui = uiPool.Dequeue();

        ui.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 68f);
        ui.GetComponent<TMP_Text>().text = $"DMG. {dmg}";

        ui.SetActive(true); // È°¼ºÈ­
    }

    public void ReturnUI(GameObject thisUI)
    {
        thisUI.SetActive(false);
        
        uiPool.Enqueue(thisUI);
    }
}

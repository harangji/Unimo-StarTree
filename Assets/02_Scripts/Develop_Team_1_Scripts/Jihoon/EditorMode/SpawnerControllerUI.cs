using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnerControllerUI : MonoBehaviour
{
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private TMP_Text[] buttonsText;
    
    public void Initialize()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            buttonsText[i].text = spawners[i].activeSelf ? "V" : $"{i + 1}"; 
        }
    }
    
    public void OnClickSpawnerController(int idx)
    {
        spawners[idx].SetActive(!spawners[idx].activeSelf);
        buttonsText[idx].text = spawners[idx].activeSelf ? "V" : $"{idx + 1}"; 
        
    }
}

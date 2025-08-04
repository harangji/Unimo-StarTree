using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnerControllerUI : MonoBehaviour
{
    [SerializeField] private MonsterSpawner monsterSpawner;
    [SerializeField] private TMP_Text[] buttonsText;
    
    public void Initialize()
    {
        for (int i = 0; i < monsterSpawner.isStop.Length; i++)
        {
            buttonsText[i].text = monsterSpawner.isStop[i] ? "V" : $"{i + 1}"; 
        }
    }
    
    public void OnClickSpawnerController(int idx)
    {
        monsterSpawner.isStop[idx] = !monsterSpawner.isStop[idx];
        buttonsText[idx].text = monsterSpawner.isStop[idx] ? "V" : $"{idx + 1}"; 
        
        monsterSpawner.isAllStop = monsterSpawner.isStop.All(x => x);
        Debug.Log($"[Stop Check] {monsterSpawner.isAllStop}]");
    }
}

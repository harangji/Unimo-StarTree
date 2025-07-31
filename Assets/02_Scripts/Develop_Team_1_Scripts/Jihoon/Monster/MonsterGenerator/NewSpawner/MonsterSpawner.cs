using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private SpawnSO spawnData;

    private void Awake()
    {
        var dataArray = spawnData.GetDataAll();
        
        
    }

    private void FindSpawnData(SpawnData[] dataArray)
    {
        dataArray.key;
    }
}
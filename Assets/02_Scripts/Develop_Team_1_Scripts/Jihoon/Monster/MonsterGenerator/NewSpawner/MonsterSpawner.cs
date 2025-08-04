using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    public bool[] isStop = { false, false, false, false, false };
    public bool isAllStop;
    
    [SerializeField] private SpawnSO spawnData;

    [SerializeField] private MonsterFactory[] monsterFactories;

    private SpawnData currentSpawnData;
    private WaitForSeconds spawnDelay = new WaitForSeconds(1f);

    private void Awake()
    {
        FindSpawnData();

        string str = "";

        foreach (var monsterSpawnRate in currentSpawnData.MonsterSpawnRates)
        {
            str += $"{monsterSpawnRate}, ";
        }

        Debug.Log($"[Ȯ��]: {str}");

        StartCoroutine(StartSpawner());
    }

    private void FindSpawnData()
    {
        int stage = StageLoader.CurrentStageNumber;
        string key;

        if (stage % 10 == 0)
        {
            key = stage.ToString(); // ��: "10"
        }
        else
        {
            int min = (stage / 10) * 10 + 1;
            int max = ((stage / 10) + 1) * 10 - 1;
            key = $"{min}~{max}"; // ��: "1~9", "11~19"
        }

        currentSpawnData = spawnData.GetData(key);
    }

    private IEnumerator StartSpawner()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            var rate = Random.Range(0, 100);

            if (rate < currentSpawnData.MonsterSpawnRates[0])
            {
                monsterFactories[0].SpawnMonster(currentSpawnData.Pattern);
            }
            else if (rate < currentSpawnData.MonsterSpawnRates[1])
            {
                monsterFactories[1].SpawnMonster(currentSpawnData.Pattern);
            }
            else if (rate < currentSpawnData.MonsterSpawnRates[2])
            {
                monsterFactories[2].SpawnMonster(currentSpawnData.Pattern);
            }
            else if (rate < currentSpawnData.MonsterSpawnRates[3])
            {
                monsterFactories[3].SpawnMonster(currentSpawnData.Pattern);
            }
            else if (rate < currentSpawnData.MonsterSpawnRates[4])
            {
                monsterFactories[4].SpawnMonster(currentSpawnData.Pattern);
            }
            else if (rate < currentSpawnData.MonsterSpawnRates[5])
            {
                Debug.Log($"[Spawner] ������ ��ȯ");
                // monsterFactories[5].SpawnMonster(currentSpawnData.Pattern);
            }
            else
            {
                Debug.Log($"[Spawner] �߰� ��ȯ");
                // monsterFactories[6].SpawnMonster(currentSpawnData.Pattern);
            }

            yield return spawnDelay;
        }
    }
}
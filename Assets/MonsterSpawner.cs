using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private List<SpawnData> stagePatterns = new List<SpawnData>();

    void Awake()
    {
        LoadStagePatternData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SpawnData pattern = GetPatternForStage(StageLoader.CurrentStageNumber);
            Debug.Log($"Current Stage �� Species: {pattern.Species}, Pattern: {pattern.Pattern}");
        }
    }

    /// <summary>
    /// Resources/Datas/SpawnInfo.tsv ������ �о�鿩 ���� �����͸� stagePatterns ����Ʈ�� �����մϴ�.
    /// </summary>
    private void LoadStagePatternData()
    {
        TextAsset tsvFile = Resources.Load<TextAsset>("Datas/SpawnInfo"); // Ȯ���� ����
        if (tsvFile == null)
        {
            Debug.LogError("SpawnInfo.tsv ������ Resources/Datas ������ �־����� Ȯ���ϼ���.");
            return;
        }

        string[] lines = tsvFile.text.Split('\n');

        // ù ���� ����̹Ƿ� ��ŵ
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] tokens = line.Split('\t');
            if (tokens.Length != 3)
            {
                Debug.LogWarning($"���� �Ľ� ����: {line}");
                continue;
            }

            SpawnData data = new SpawnData
            {
                Key = tokens[0],
                Species = int.Parse(tokens[1]),
                Pattern = int.Parse(tokens[2])
            };

            stagePatterns.Add(data);
        }

        Debug.Log($"�� {stagePatterns.Count}���� ���� �����͸� �ҷ��Խ��ϴ�.");
    }

    /// <summary>
    /// �־��� �������� ��ȣ�� �ش��ϴ� ���� �����͸� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="stage">ã���� �ϴ� �������� ��ȣ</param>
    /// <returns>��ġ�ϴ� SpawnData �Ǵ� null</returns>
    public SpawnData GetPatternForStage(int stage)
    {
        foreach (var data in stagePatterns)
        {
            // if (data.IsMatch(stage))
            //     return data;
        }

        Debug.LogWarning($"�������� {stage}�� �ش��ϴ� ���� �����Ͱ� �����ϴ�.");
        return null;
    }
}
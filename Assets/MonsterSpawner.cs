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
            Debug.Log($"Current Stage → Species: {pattern.Species}, Pattern: {pattern.Pattern}");
        }
    }

    /// <summary>
    /// Resources/Datas/SpawnInfo.tsv 파일을 읽어들여 스폰 데이터를 stagePatterns 리스트에 저장합니다.
    /// </summary>
    private void LoadStagePatternData()
    {
        TextAsset tsvFile = Resources.Load<TextAsset>("Datas/SpawnInfo"); // 확장자 제외
        if (tsvFile == null)
        {
            Debug.LogError("SpawnInfo.tsv 파일을 Resources/Datas 폴더에 넣었는지 확인하세요.");
            return;
        }

        string[] lines = tsvFile.text.Split('\n');

        // 첫 줄은 헤더이므로 스킵
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] tokens = line.Split('\t');
            if (tokens.Length != 3)
            {
                Debug.LogWarning($"라인 파싱 실패: {line}");
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

        Debug.Log($"총 {stagePatterns.Count}개의 스폰 데이터를 불러왔습니다.");
    }

    /// <summary>
    /// 주어진 스테이지 번호에 해당하는 스폰 데이터를 반환합니다.
    /// </summary>
    /// <param name="stage">찾고자 하는 스테이지 번호</param>
    /// <returns>일치하는 SpawnData 또는 null</returns>
    public SpawnData GetPatternForStage(int stage)
    {
        foreach (var data in stagePatterns)
        {
            // if (data.IsMatch(stage))
            //     return data;
        }

        Debug.LogWarning($"스테이지 {stage}에 해당하는 스폰 데이터가 없습니다.");
        return null;
    }
}
using System.Collections.Generic;
using UnityEngine;

public class UI_BonusStage : UI_Base
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject slotPrefab;

    private const int MaxBonusStage = 995;
    
    private void OnEnable()
    {
        LoadBonusStageSlots();
    }

    private void LoadBonusStageSlots()
    {
        // 기존 슬롯 제거
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        var userData = Base_Manager.Data.UserData;
        var starDict = userData.BonusStageStars;
        int bestStage = userData.BestStage;

        var unlockedBonusStages = new List<int>();

        for (int stage = 5; stage <= MaxBonusStage; stage += 10)
        {
            if (stage <= bestStage)
            {
                unlockedBonusStages.Add(stage);
            }
        }
        
        // 정렬: 별 3개 미만이 위로
        unlockedBonusStages.Sort((a, b) =>
        {
            int aStars = GetStarCount(starDict, a);
            int bStars = GetStarCount(starDict, b);

            if (aStars == 3 && bStars < 3) return 1;
            if (aStars < 3 && bStars == 3) return -1;

            return a.CompareTo(b); // 번호 순 정렬
        });

        foreach (int stage in unlockedBonusStages)
        {
            int starFlag = 0;
            starDict.TryGetValue(stage, out starFlag);
            
            Debug.Log($"Stage {stage} : Flag = {starFlag} (Binary: {System.Convert.ToString(starFlag, 2).PadLeft(3, '0')})");

            GameObject slotGO = Instantiate(slotPrefab, content);
            var slot = slotGO.GetComponent<BonusStageSlot>();
            slot.SetSlot(stage, starFlag);
        }
    }

    private int GetStarCount(Dictionary<int, int> dict, int stage)
    {
        int value;
        if (!dict.TryGetValue(stage, out value)) return 0;

        int count = 0;
        for (int i = 0; i < 3; i++)
        {
            if ((value & (1 << i)) != 0) count++;
        }
        return count;
    }
}

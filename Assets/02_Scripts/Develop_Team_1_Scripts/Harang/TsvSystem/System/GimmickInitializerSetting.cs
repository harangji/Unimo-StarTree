using System;
using System.Collections.Generic;
using UnityEngine;

public class GimmickInitializerSetting : MonoBehaviour
{
    private static readonly Dictionary<string, eGimmickType> gimmickIdMap = new()
    {
        { "G_TRP_002", eGimmickType.BlackHole },
        { "G_INT_003", eGimmickType.RedZone },
        // { "G_ENV_004", eGimmickType.Environment },
        // { "G_PZL_005", eGimmickType.Puzzle },
        // { "G_TRG_006", eGimmickType.Trigger }
    };
    
    private void Start()
    {
        if (GameDataSOHolder.Instance != null)
        {
           ToTsvGimmickData[] toTsvGimmickDatas = GameDataSOHolder.Instance.GimmicParsingDataSo.GetDataAll();
           for (int i = 0; i < toTsvGimmickDatas.Length; i++)
           {
               if (gimmickIdMap.TryGetValue(toTsvGimmickDatas[i].Key, out var type))
               {
                   switch (type)
                   {
                       // 블랙홀
                       case eGimmickType.BlackHole:
                           break;
                       // // 특수한 꽃
                       // case "G_INT_003":
                       //     break;
                       // // 레드존
                       // case "G_ENV_004":
                       //     break;
                       // // 장판 밟기
                       // case "G_PZL_005":
                       //     break;
                       // // 지역 활성화
                       // case "G_TRG_006":
                       //     break;
                       default:
                           break;
                   }
               }
           }
        }
    }
}

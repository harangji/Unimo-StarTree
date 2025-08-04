using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoomBoomEngineData
{
    public int EngineID;
    public string Name;
    public string Rarity;
    public string ModelID;
    public string DescriptionFormat;
    public float[] GrowthTable;
    public string ButtonText;
    public int SkillID;
    public SCharacterStat StatBonus;
    public System.Func<int, float[], SCharacterStat, SCharacterStat> GrowthStatCalculator;
    public bool IsUniqueType;
    public ETriggerCondition TriggerCondition;
}

public enum ETriggerCondition
{
    None,
    OnTakeDamage,
    OnDeath,
    OnStart,
    OnOrangeFlowerCollected,
    OnYellowFlowerCollected,
    OnInactivityTimer,
    OnFixedInterval,
    OnStunReleased,
    Manual,
    OnStunEnd,
    OnShieldMissing
}

public static class BoomBoomEngineDatabase
{
    private static List<BoomBoomEngineData> engineDataList = new List<BoomBoomEngineData>();

    private static readonly Dictionary<int, BoomBoomEngineData> mEngineTable = new()
    {
        { 20101, CreateEngine_BeeTail() },
        { 20102, CreateEngine_HoneyJar() },
        { 20201, CreateEngine_Toaster() },
        { 20202, CreateEngine_Sack() },
        { 20203, CreateEngine_Vase() },
        { 20204, CreateEngine_StoneMortar() },
        { 20205, CreateEngine_HatchedEgg() },
        { 20301, CreateEngine_CleanBucket() },
        { 20302, CreateEngine_Scootus() },
        { 20303, CreateEngine_Box() },
        { 20401, CreateEngine_MonoPlane() },
        { 20402, CreateEngine_EmperorPenguin() },
        { 20403, CreateEngine_DogHouse() },
        { 21101, CreateEngine_OakTong() },
        { 21102, CreateEngine_WitchSot() },
        { 21103, CreateEngine_Icecream() },
        { 21201, CreateEngine_MagicHat() },
        { 21202, CreateEngine_DogBowl() },
        { 21301, CreateEngine_ElfCup() },
        { 21302, CreateEngine_TrashCan() },
        { 20411, CreateEngine_Cloud() },
        { 20412, CreateEngine_Bathtub() },
        { 20413, CreateEngine_SandCastle() },
    };
    public static BoomBoomEngineData GetEngineData(int id)
    {
        if (mEngineTable.TryGetValue(id, out var data))
            return data;

        Debug.LogError($"붕붕엔진 ID {id} 데이터 없음");
        return null;
    }

    public static List<BoomBoomEngineData> GetAllEngines()
    {
        return engineDataList;
    }

    public static SCharacterStat GetBonusStatWithLevel(int engineID)
    {
        var data = GetEngineData(engineID);
        if (data == null) return default;

        if (data.IsUniqueType)
        {
            int level = EngineLevelSystem.GetUniqueLevel(engineID);
            return data.GrowthStatCalculator != null
                ? data.GrowthStatCalculator(level, data.GrowthTable, data.StatBonus)
                : data.StatBonus;
        }

        return data.StatBonus;
    }

    // 기존 메서드 생략 (301 ~ 313)
    private static BoomBoomEngineData CreateEngine_BeeTail()
    {
        var data = new BoomBoomEngineData
        {
            EngineID = 20101,
            Name = "리비",
            Rarity = "Normal",
            ModelID = "EQC001_BeeTail",
            SkillID = 301,
            IsUniqueType = true, //  고유 타입 적용
            DescriptionFormat = "피격 당했을 경우, {0}초 동안 몬스터로부터 피격을 받지 않음",
            GrowthTable = Enumerable.Range(0, 51).Select(i => 1.0f + i * 0.1f).ToArray(),
            StatBonus = new SCharacterStat { },
            GrowthStatCalculator = (level, table, stat) => stat,
            TriggerCondition = ETriggerCondition.OnTakeDamage
        };

        engineDataList.Add(data);
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_HoneyJar()
    {
        var data = new BoomBoomEngineData
        {
            EngineID = 20102,
            Name = "곰곰",
            Rarity = "Normal",
            ModelID = "EQC002_HoneyJar",
            SkillID = 302,
            DescriptionFormat = "노란 별꽃 획득량 {0:P0} 증가",
            GrowthTable = Enumerable.Range(0, 51).Select(i => 0.10f + i * ((0.35f - 0.10f) / 50f)).ToArray(),
            StatBonus = new SCharacterStat { },
            IsUniqueType = true,

            GrowthStatCalculator = (level, table, stat) =>
            {
                float gainMult = table[Mathf.Clamp(level, 0, table.Length - 1)];
                stat.YFGainMult = gainMult;
                Debug.Log($"[곰곰엔진] YFGainMult 적용: {gainMult} (레벨 {level})");
                return stat;
            }
        };

        engineDataList.Add(data);
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_Toaster()
    {
        var data = new BoomBoomEngineData
        {
            // 5초 동안 Aura_Range n% 증가
            EngineID = 20201,
            Name = "토스터",
            Rarity = "Rare",
            ModelID = "EQC003_Toaster",
            SkillID = 303,
            IsUniqueType = true,
            DescriptionFormat = "주황 별꽃 10개 획득 시, 5초 동안 아우라 {0:P0} 증가",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.05f + i * ((0.50f - 0.05f) / 50f)) // 5% ~ 50%
                .ToArray(),
            StatBonus = new SCharacterStat { },
            GrowthStatCalculator = (level, table, stat) => stat,
            TriggerCondition = ETriggerCondition.OnOrangeFlowerCollected
        };

        engineDataList.Add(data); 
        return data;
    }

    private static BoomBoomEngineData CreateEngine_Sack()
    {
        var data = new BoomBoomEngineData
        {
            // 10초 동안 유니모의 Critical_Chance 100%
            EngineID = 20202,
            Name = "너구리",
            Rarity = "Rare",
            ModelID = "EQC004_Sack",
            SkillID = 304,
            IsUniqueType = true,
            DescriptionFormat = " 노랑 별꽃{0}개 획득 시, 10초동안 수확 크리티컬 확률 100% 증가",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => Mathf.Lerp(70f, 20f, i / 50f))  // 0레벨 70개 → 50레벨 1개
                .ToArray(),
            StatBonus = new SCharacterStat { },
            GrowthStatCalculator = (level, table, stat) => stat,
            TriggerCondition = ETriggerCondition.OnYellowFlowerCollected
        };

        engineDataList.Add(data); 
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_Vase()
    {
        var data = new BoomBoomEngineData
        {
            // 피해를 한 번 무시하는 방어막 생성
            EngineID = 20203,
            Name = "알",
            Rarity = "Rare",
            ModelID = "EQC005_Vase",
            SkillID = 305,
            IsUniqueType = true,
            DescriptionFormat = "현재 보호막이 없을 경우, {0}초 후 보호막 1개 생성(중복 불가)",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => Mathf.Lerp(65f, 15f, i / 50f)).ToArray(),
            StatBonus = new SCharacterStat { },
            GrowthStatCalculator = (level, table, stat) => stat,
            TriggerCondition = ETriggerCondition.OnStart
        };

        engineDataList.Add(data);
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_StoneMortar()
    {
        var data = new BoomBoomEngineData
        {
            EngineID = 20204,
            Name = "점분이",
            Rarity = "Rare",
            ModelID = "EQC005_StoneMortar",
            SkillID = 306,
            IsUniqueType = true, // 고유 효과로 취급 시 필요
            DescriptionFormat = "오라 강도 {0:P0} 증가",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.10f + i * ((0.70f - 0.10f) / 50f))
                .ToArray(),

            StatBonus = new SCharacterStat { }, // 기본값은 0
            GrowthStatCalculator = (level, table, stat) =>
            {
                float bonusPercent = table[Mathf.Clamp(level, 0, table.Length - 1)];
                stat.AuraStr = bonusPercent;  // 직접 비율로 넣기
                Debug.Log($"[점분이엔진] AuraStr 보정률 적용: {bonusPercent * 100f}% (레벨 {level})");
                return stat;
            }
        };

        engineDataList.Add(data);
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_HatchedEgg()
    {
        var data = new BoomBoomEngineData
        {
            // YF_Gainmult, OF_Gainmult n%증가
            EngineID = 20205,
            Name = "둠둠",
            Rarity = "Rare",
            ModelID = "EQC005_HatchedEgg",
            SkillID = 307,
            StatBonus = new SCharacterStat
            {
                YFGainMult = 1f,
                OFGainMult = 1f
            }
        };

        engineDataList.Add(data); 
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_CleanBucket()
    {
        var data = new BoomBoomEngineData
        {
            // 매초마다 주변 별꽃들의 개화도 n% 증가
            EngineID = 20301,
            Name = "고등어",
            Rarity = "Unique",
            ModelID = "EQC005_CleanBucket",
            SkillID = 308,
            DescriptionFormat = "제작 못함",
            StatBonus = new SCharacterStat { }
        };

        // 제작 못함
        engineDataList.Add(data); 
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_Scootus()
    {
        var data = new BoomBoomEngineData
        {
            // 미사일 3개 발사, 미사일에 피격된 별꽃 개화도 50% 증가
            EngineID = 20302,
            Name = "아우구스투스",
            Rarity = "Unique",
            ModelID = "EQC005_Scootus",
            SkillID = 309,
            DescriptionFormat = "제작 못함",
            StatBonus = new SCharacterStat { }
        };

        
        // 제작 못함
        engineDataList.Add(data); 
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_Box()
    {
        var data = new BoomBoomEngineData
        {
            // n초 동안 몬스터로부터 피격 무효
            EngineID = 20303,
            Name = "소포",
            Rarity = "Unique",
            ModelID = "EQC005_Box",
            SkillID = 310,
            IsUniqueType = true, // 고유 레벨 기반
            DescriptionFormat = "{0}초 마다, 몬스터로부터 5초간 피격 무효",
            StatBonus = new SCharacterStat { },
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => Mathf.Lerp(30f, 21f, i / 50f)) // 0레벨: 30초 → 50레벨: 21초
                .ToArray(),
            GrowthStatCalculator = (level, table, stat) => stat,
            TriggerCondition = ETriggerCondition.OnStart // 게임 시작 시 자동 발동
        };

        engineDataList.Add(data); 
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_MonoPlane()
    {
        var data = new BoomBoomEngineData
        {
            // 주황 별꽃을 얻을 때 n%로 파랑 별꽃으로 대체해서 ?득
            EngineID = 20401,
            Name = "쿠",
            Rarity = "Legend",
            ModelID = "EQC005_MonoPlane",
            SkillID = 311,
            DescriptionFormat = "제작 못함",
            StatBonus = new SCharacterStat { }
        };

        // 제작 못함
        engineDataList.Add(data);
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_EmperorPenguin()
    {
        var data = new BoomBoomEngineData
        {
            // 맵 상의 모든 몬스터가 3초간 정지
            EngineID = 20402,
            Name = "프리모",
            Rarity = "Legend",
            ModelID = "EQC005_EmperorPenguin",
            SkillID = 312,
            DescriptionFormat = "제작 못함",
            StatBonus = new SCharacterStat { }
        };

        //제작 못함
        engineDataList.Add(data); 
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_DogHouse()
    {
        var data = new BoomBoomEngineData
        {
            // 체력이 0 이하가 되었을 시, 최대체력의 n%를 가지고 부활
            EngineID = 20403,
            Name = "도베르만",
            Rarity = "Legend",
            ModelID = "EQC005_DogHouse",
            SkillID = 313,
            DescriptionFormat = "체력이 전부 소모됐을 경우, 최대체력 {0:P0}가지고 부활",
            StatBonus = new SCharacterStat { },
            IsUniqueType = true,
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.05f + i * ((0.45f - 0.05f) / 50f))
                .ToArray(),
            GrowthStatCalculator = (level, table, stat) => stat,
            TriggerCondition = ETriggerCondition.OnDeath,
        };

        engineDataList.Add(data); 
        return data;
    }

    
    private static BoomBoomEngineData CreateEngine_OakTong()
    {
        var data = new BoomBoomEngineData
        {
            // Health n% 증가
            EngineID = 21101,
            Name = "오크통",
            Rarity = "Normal",
            ModelID = "EQC001_OakTong",
            SkillID = 314,
            IsUniqueType = true, // 고유 효과로 취급 시 필요
            DescriptionFormat = "체력 {0:P0} 증가",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.30f + i * ((2.30f - 0.30f) / 50f))
                .ToArray(),

            StatBonus = new SCharacterStat { }, // 기본값은 0
            GrowthStatCalculator = (level, table, stat) =>
            {
                float hpPercent = table[Mathf.Clamp(level, 0, table.Length - 1)];
                stat.Health = hpPercent;  // 직접 비율로 넣기
                Debug.Log($"[오크통엔진] Health 보정률 적용: {hpPercent * 100f}% (레벨 {level})");
                return stat;
            }
        };

        engineDataList.Add(data); 
        return data;
    }

    private static BoomBoomEngineData CreateEngine_WitchSot()
    {
        var data = new BoomBoomEngineData
        {
            // Aura_Range n% 증가
            EngineID = 21102,
            Name = "마녀솥",
            Rarity = "Normal",
            ModelID = "EQC003_WitchSot",
            SkillID = 315,
            IsUniqueType = true, // 고유 효과로 취급 시 필요
            DescriptionFormat = "아우라 범위 {0:P0} 증가",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.10f + i * ((0.60f - 0.10f) / 50f))
                .ToArray(),

            StatBonus = new SCharacterStat { }, // 기본값은 0
            GrowthStatCalculator = (level, table, stat) =>
            {
                float AuraRangePercent = table[Mathf.Clamp(level, 0, table.Length - 1)];
                stat.AuraRange = AuraRangePercent;  // 직접 비율로 넣기
                Debug.Log($"[마녀솥엔진] AuraRange 보정률 적용: {AuraRangePercent * 100f}% (레벨 {level})");
                return stat;
            }
        };

        engineDataList.Add(data);
        return data;
    }

    private static BoomBoomEngineData CreateEngine_Icecream()
    {
        var data = new BoomBoomEngineData
        {
            // Move_Spd n%증가
            EngineID = 21103,
            Name = "아이스크림",
            Rarity = "Normal",
            ModelID = "EQC002_Icecream",
            SkillID = 316,
            IsUniqueType = true, // 고유 효과로 취급 시 필요
            DescriptionFormat = "이동속도 {0:P0} 증가",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.02f + i * (0.08f / 50f))  // 2% ~ 10%
                .ToArray(),

            StatBonus = new SCharacterStat { }, // 기본값은 0
            GrowthStatCalculator = (level, table, stat) =>
            {
                float MoveSpdPercent = table[Mathf.Clamp(level, 0, table.Length - 1)];
                stat.MoveSpd = MoveSpdPercent;
                Debug.Log($"[아이스크림엔진] MoveSpd 보정률 적용: {MoveSpdPercent * 100f}% (레벨 {level})");
                return stat;
            }
        };

        engineDataList.Add(data); 
        return data;
    }

    private static BoomBoomEngineData CreateEngine_MagicHat()
    {
        var data = new BoomBoomEngineData
        {
            // 다음 3개 중 하나 랜덤으로 7초 동안 적용
            // Move_Spd n% 증가, Aura_Range n% 증가, Health_Regen n%증가 
            EngineID = 21201,
            Name = "마술모자",
            Rarity = "Rare",
            ModelID = "EQC009_MagicHat",
            SkillID = 317,
            DescriptionFormat = "다음 중 하나의 효과를 7초마다 얻음." +
                                "이동속도, 아우라 크기, 체력 재생 중 하나 {0:P0} 상승",
            IsUniqueType = true, // 고유 효과로 취급 시 필요
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.10f + i * ((0.30f - 0.10f) / 50f)) // 0레벨 10% → 50레벨 30%
                .ToArray(),
            StatBonus = new SCharacterStat { },
            TriggerCondition = ETriggerCondition.OnStart //  추가: 시작 시 발동 조건
        };

        engineDataList.Add(data); 
        return data;
    }

    private static BoomBoomEngineData CreateEngine_DogBowl()
    {
        var data = new BoomBoomEngineData
        {
            // 오라 범위, 오라 강도, 이동속도를 갖고 있는 강아지 소환
            EngineID = 21202,
            Name = "개밥그릇",
            Rarity = "Rare",
            ModelID = "EQC004_DogBowl",
            SkillID = 318,
            DescriptionFormat = "{0} 오라 범위, {1}오라 강도, {2}의 이동속도가지고 " +
                                "플레어 주위를 회전하는 강아지 소환",
            StatBonus = new SCharacterStat { }
        };

        engineDataList.Add(data); 
        return data;
    }

    private static BoomBoomEngineData CreateEngine_ElfCup()
    {
        var data = new BoomBoomEngineData
        {
            // 스테이지 클리어 게이지 1% 증가 (판정크기: n*n)
            EngineID = 21301,
            Name = "엘프컵",
            Rarity = "Unique",
            ModelID = "EQC008_ElfCup",
            SkillID = 320,
            DescriptionFormat = "제작 못함",
            StatBonus = new SCharacterStat { }
        };
        //제작 못함
        engineDataList.Add(data);
        return data;
    }

    private static BoomBoomEngineData CreateEngine_TrashCan()
    {
        var data = new BoomBoomEngineData
        {
            // N%의 확률로 피격을 무시함
            EngineID = 21302,
            Name = "쓰레기통",
            Rarity = "Unique",
            ModelID = "EQC005_TrashCan",
            SkillID = 321,
            IsUniqueType = true, // 고유 효과로 취급 시 필요
            DescriptionFormat = "{0:P0}로 피격 무시",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.05f + i * ((0.35f - 0.05f) / 50f)) // 0.05 ~ 0.35
                .ToArray(),

            StatBonus = new SCharacterStat { }, // 기본값은 0
            GrowthStatCalculator = (level, table, stat) =>
            {
                float StunIgnoreChancePercent = table[Mathf.Clamp(level, 0, table.Length - 1)];
                stat.StunIgnoreChance = StunIgnoreChancePercent;  // 직접 비율로 넣기
                Debug.Log($"[쓰레기통엔진] StunIgnoreChance 적용: {StunIgnoreChancePercent * 100f}% (레벨 {level})");
                return stat;
            }
        };

        engineDataList.Add(data); 
        return data;
    }

    private static BoomBoomEngineData CreateEngine_Cloud()
    {
        var data = new BoomBoomEngineData
        {
            // 랜덤으로 맵을 돌아다니는 구름 소환 (판정크기: n*n)
            EngineID = 20411,
            Name = "구름",
            Rarity = "Unique",
            ModelID = "EQC010_Cloud",
            SkillID = 319,
            DescriptionFormat = "맵을 돌아다는 {0:P0} 아우라 크기를 가진 구름 소환",
            StatBonus = new SCharacterStat { }
        };

        engineDataList.Add(data); 
        return data;
    }

    private static BoomBoomEngineData CreateEngine_Bathtub()
    {
        var data = new BoomBoomEngineData
        {
            // Critical_Mult n% 증가
            EngineID = 20412,
            Name = "욕조",
            Rarity = "Legend",
            ModelID = "EQC007_Bathtub",
            SkillID = 322,
            IsUniqueType = true, // 고유 효과로 취급 시 필요
            DescriptionFormat = "크리티컬 배수 {0:P0} 증가",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.10f + i * ((1.70f - 0.10f) / 50f)) // 0.10 ~ 1.70
                .ToArray(),

            StatBonus = new SCharacterStat { }, // 기본값은 0
            GrowthStatCalculator = (level, table, stat) =>
            {
                float CriticalMultPercent = table[Mathf.Clamp(level, 0, table.Length - 1)];
                stat.CriticalMult = CriticalMultPercent;  //  직접 비율로 넣기
                Debug.Log($"[욕조엔진] CriticalMult 보정률 적용: {CriticalMultPercent * 100f}% (레벨 {level})");
                return stat;
            }
        };

        engineDataList.Add(data);
        return data;
    }

    private static BoomBoomEngineData CreateEngine_SandCastle()
    {
        var data = new BoomBoomEngineData
        {
            // 20초에 걸쳐 Aura_Range n% 까지 증가
            EngineID = 20413,
            Name = "모래성",
            Rarity = "Legend",
            ModelID = "EQC006_SandCastle",
            SkillID = 323,
            DescriptionFormat = "20초에 걸쳐서 아우라 크기 {0:P0} 까지 증가, 피격 시 초기화",
            StatBonus = new SCharacterStat { }
        };

        engineDataList.Add(data); 
        return data;
    }
    
    
    
}

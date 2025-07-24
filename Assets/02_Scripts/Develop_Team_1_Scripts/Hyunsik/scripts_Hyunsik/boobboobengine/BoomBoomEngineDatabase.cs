using System.Collections.Generic;
using UnityEngine;

public class BoomBoomEngineData
{
    public int EngineID;
    public string Name;
    public string Rarity;
    public string ModelID;
    public int SkillID;
    public SCharacterStat StatBonus;
}

public static class BoomBoomEngineDatabase
{
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

    // 기존 메서드 생략 (301 ~ 313)
    private static BoomBoomEngineData CreateEngine_BeeTail() => new BoomBoomEngineData
    {
        // 몬스터한테 타격 받은 이후 3초 동안 몬스터로 부터 피격을 받지 않음
        EngineID = 20101,
        Name = "리비",
        Rarity = "Normal",
        ModelID = "EQC001_BeeTail",
        SkillID = 301,
        StatBonus = new SCharacterStat {  }
    };
    
    private static BoomBoomEngineData CreateEngine_HoneyJar() => new BoomBoomEngineData
    {
        // YF_Gainmult n% 증가
        EngineID = 20102,
        Name = "곰곰",
        Rarity = "Normal",
        ModelID = "EQC002_HoneyJar",
        SkillID = 302,
        StatBonus = new SCharacterStat { YFGainMult = 10f }
    };
    
    private static BoomBoomEngineData CreateEngine_Toaster() => new BoomBoomEngineData
    {
        // 5초 동안 Aura_Range n% 증가
        EngineID = 20201,
        Name = "토스터",
        Rarity = "Rare",
        ModelID = "EQC003_Toaster",
        SkillID = 303,
        StatBonus = new SCharacterStat { }
    };
    
    private static BoomBoomEngineData CreateEngine_Sack() => new BoomBoomEngineData
    {
        // 10초 동안 유니모의 Critical_Chance 100%
        EngineID = 20202,
        Name = "너구리",
        Rarity = "Rare",
        ModelID = "EQC004_Sack",
        SkillID = 304,
        StatBonus = new SCharacterStat { }
    };
    
    private static BoomBoomEngineData CreateEngine_Vase() => new BoomBoomEngineData
    {
        // 피해를 한 번 무시하는 방어막 생성
        EngineID = 20203,
        Name = "알",
        Rarity = "Rare",
        ModelID = "EQC005_Vase",
        SkillID = 305,
        StatBonus = new SCharacterStat {  }
    };
    
    private static BoomBoomEngineData CreateEngine_StoneMortar() => new BoomBoomEngineData
    {
        // Aura_Str n% 증가
        EngineID = 20204,
        Name = "점분이",
        Rarity = "Rare",
        ModelID = "EQC005_StoneMortar",
        SkillID = 306,
        StatBonus = new SCharacterStat { AuraStr = 10f }
    };
    
    private static BoomBoomEngineData CreateEngine_HatchedEgg() => new BoomBoomEngineData
    {
        // YF_Gainmult, OF_Gainmult n%증가
        EngineID = 20205,
        Name = "둠둠",
        Rarity = "Rare",
        ModelID = "EQC005_HatchedEgg",
        SkillID = 307,
        StatBonus = new SCharacterStat { YFGainMult = 1f , OFGainMult = 1f }
    };
    
    
    private static BoomBoomEngineData CreateEngine_CleanBucket() => new BoomBoomEngineData
    {
        // 매초마다 주변 별꽃들의 개화도 n% 증가
        EngineID = 20301,
        Name = "고등어",
        Rarity = "Unique",
        ModelID = "EQC005_CleanBucket",
        SkillID = 308,
        StatBonus = new SCharacterStat { }
    };
    
    private static BoomBoomEngineData CreateEngine_Scootus() => new BoomBoomEngineData
    {
        // 미사일 3개 발사, 미사일에 피격된 별꽃 개화도 50% 증가
        EngineID = 20302,
        Name = "아우구스투스",
        Rarity = "Unique",
        ModelID = "EQC005_Scootus",
        SkillID = 309,
        StatBonus = new SCharacterStat { }
    };

    private static BoomBoomEngineData CreateEngine_Box() => new BoomBoomEngineData
    {
        // n초 동안 몬스터로 부터 피격 무효
        EngineID = 20303,
        Name = "소포",
        Rarity = "Unique",
        ModelID = "EQC005_Box",
        SkillID = 310,
        StatBonus = new SCharacterStat {  }
    };
    
    private static BoomBoomEngineData CreateEngine_MonoPlane() => new BoomBoomEngineData
    {
        // 주황 별꽃을 얻을 때 n%로 파랑 별꽃으로 대체해서 ?득
        EngineID = 20401,
        Name = "쿠",
        Rarity = "Legend",
        ModelID = "EQC005_MonoPlane",
        SkillID = 311,
        StatBonus = new SCharacterStat { }
    };
    private static BoomBoomEngineData CreateEngine_EmperorPenguin() => new BoomBoomEngineData
    {
        // 맵 상의 모든 몬스터가 3초간 정지
        EngineID = 20402,
        Name = "프리모",
        Rarity = "Legend",
        ModelID = "EQC005_EmperorPenguin",
        SkillID = 312,
        StatBonus = new SCharacterStat {  }
    };

    private static BoomBoomEngineData CreateEngine_DogHouse() => new BoomBoomEngineData
    {
        // 체력이 0 이하가 되었을 시, 최대체력의 n%를 가지고 부활
        EngineID = 20403,
        Name = "도베르만",
        Rarity = "Legend",
        ModelID = "EQC005_DogHouse",
        SkillID = 313,
        StatBonus = new SCharacterStat { }
    };

    
    private static BoomBoomEngineData CreateEngine_OakTong() => new BoomBoomEngineData
    {
        // Health n% 증가
        EngineID = 21101,
        Name = "오크통",
        Rarity = "Normal",
        ModelID = "EQC001_OakTong",
        SkillID = 314,
        StatBonus = new SCharacterStat { Health = 30f }
    };

    private static BoomBoomEngineData CreateEngine_WitchSot() => new BoomBoomEngineData
    {
        // Aura_Range n% 증가
        EngineID = 21102,
        Name = "마녀솥",
        Rarity = "Normal",
        ModelID = "EQC003_WitchSot",
        SkillID = 315,
        StatBonus = new SCharacterStat { AuraRange = 10.0f }
    };

    private static BoomBoomEngineData CreateEngine_Icecream() => new BoomBoomEngineData
    {
        // Move_Spd n%증가
        EngineID = 21103,
        Name = "아이스크림",
        Rarity = "Normal",
        ModelID = "EQC002_Icecream",
        SkillID = 316,
        StatBonus = new SCharacterStat { MoveSpd = 10.0f }
    };

    private static BoomBoomEngineData CreateEngine_MagicHat() => new BoomBoomEngineData
    {
        // 다음 3개 중 하나 랜덤으로 7초 동안 적용
        // Move_Spd n% 증가, Aura_Range n% 증가, Health_Regen n%증가 
        EngineID = 21201,
        Name = "마술모자",
        Rarity = "Rare",
        ModelID = "EQC009_MagicHat",
        SkillID = 317,
        StatBonus = new SCharacterStat {  }
    };

    private static BoomBoomEngineData CreateEngine_DogBowl() => new BoomBoomEngineData
    {
        // 오라 범위, 오라 강도, 이동속도를 갖고 있는 강아지 소환
        EngineID = 21202,
        Name = "개밥그릇",
        Rarity = "Rare",
        ModelID = "EQC004_DogBowl",
        SkillID = 318,
        StatBonus = new SCharacterStat {  }
    };

    private static BoomBoomEngineData CreateEngine_ElfCup() => new BoomBoomEngineData
    {
        // 랜덤으로 맵을 돌아다니는 구름 소환 (판정크기: n*n)
        EngineID = 21301,
        Name = "엘프컵",
        Rarity = "Unique",
        ModelID = "EQC008_ElfCup",
        SkillID = 319,
        StatBonus = new SCharacterStat { }
    };

    private static BoomBoomEngineData CreateEngine_TrashCan() => new BoomBoomEngineData
    {
        // 스테이지 클리어 게이지 1% 증가 (판정크기: n*n)
        EngineID = 21302,
        Name = "쓰레기통",
        Rarity = "Unique",
        ModelID = "EQC005_TrashCan",
        SkillID = 320,
        StatBonus = new SCharacterStat { }
    };

    private static BoomBoomEngineData CreateEngine_Cloud() => new BoomBoomEngineData
    {
        // N%의 확률로 피격을 무시함
        EngineID = 20411,
        Name = "구름",
        Rarity = "Unique",
        ModelID = "EQC010_Cloud",
        SkillID = 321,
        StatBonus = new SCharacterStat { }
    };

    private static BoomBoomEngineData CreateEngine_Bathtub() => new BoomBoomEngineData
    {
        // Critical_Mult n% 증가
        EngineID = 20412,
        Name = "욕조",
        Rarity = "Legend",
        ModelID = "EQC007_Bathtub",
        SkillID = 322,
        StatBonus = new SCharacterStat { CriticalMult = 10f }
    };

    private static BoomBoomEngineData CreateEngine_SandCastle() => new BoomBoomEngineData
    {
        // 20초에 걸쳐 Aura_Range n% 까지 증가
        EngineID = 20413,
        Name = "모래성",
        Rarity = "Legend",
        ModelID = "EQC006_SandCastle",
        SkillID = 323,
        StatBonus = new SCharacterStat { }
    };
}

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
        EngineID = 20101,
        Name = "리비",
        Rarity = "Normal",
        ModelID = "EQC001_BeeTail",
        SkillID = 301,
        StatBonus = new SCharacterStat { Health = 70, Armor = 0.02f }
    };
    
    private static BoomBoomEngineData CreateEngine_HoneyJar() => new BoomBoomEngineData
    {
        EngineID = 20102,
        Name = "곰곰",
        Rarity = "Normal",
        ModelID = "EQC002_HoneyJar",
        SkillID = 302,
        StatBonus = new SCharacterStat { Health = 70, Armor = 0.02f }
    };
    
    private static BoomBoomEngineData CreateEngine_Toaster() => new BoomBoomEngineData
    {
        EngineID = 20201,
        Name = "토스터",
        Rarity = "Rare",
        ModelID = "EQC003_Toaster",
        SkillID = 303,
        StatBonus = new SCharacterStat { Health = 70, Armor = 0.02f }
    };
    
    private static BoomBoomEngineData CreateEngine_Sack() => new BoomBoomEngineData
    {
        EngineID = 20202,
        Name = "너구리",
        Rarity = "Rare",
        ModelID = "EQC004_Sack",
        SkillID = 304,
        StatBonus = new SCharacterStat { Health = 70, Armor = 0.02f }
    };
    
    private static BoomBoomEngineData CreateEngine_Vase() => new BoomBoomEngineData
    {
        EngineID = 20203,
        Name = "알",
        Rarity = "Rare",
        ModelID = "EQC005_Vase",
        SkillID = 305,
        StatBonus = new SCharacterStat { Health = 70, Armor = 0.02f }
    };
    
    private static BoomBoomEngineData CreateEngine_StoneMortar() => new BoomBoomEngineData
    {
        EngineID = 20204,
        Name = "점분이",
        Rarity = "Rare",
        ModelID = "EQC005_StoneMortar",
        SkillID = 306,
        StatBonus = new SCharacterStat { Health = 70, Armor = 0.02f }
    };
    
    private static BoomBoomEngineData CreateEngine_HatchedEgg() => new BoomBoomEngineData
    {
        EngineID = 20205,
        Name = "둠둠",
        Rarity = "Rare",
        ModelID = "EQC005_HatchedEgg",
        SkillID = 307,
        StatBonus = new SCharacterStat { Health = 70, Armor = 0.02f }
    };
    
    
    private static BoomBoomEngineData CreateEngine_CleanBucket() => new BoomBoomEngineData
    {
        EngineID = 20301,
        Name = "고등어",
        Rarity = "Unique",
        ModelID = "EQC005_CleanBucket",
        SkillID = 308,
        StatBonus = new SCharacterStat { Health = 70, Armor = 0.02f }
    };
    
    private static BoomBoomEngineData CreateEngine_Scootus() => new BoomBoomEngineData
    {
        EngineID = 20302,
        Name = "아우구스투스",
        Rarity = "Unique",
        ModelID = "EQC005_Scootus",
        SkillID = 309,
        StatBonus = new SCharacterStat { Health = 70, Armor = 0.02f }
    };

    private static BoomBoomEngineData CreateEngine_Box() => new BoomBoomEngineData
    {
        EngineID = 20303,
        Name = "소포",
        Rarity = "Unique",
        ModelID = "EQC005_Box",
        SkillID = 310,
        StatBonus = new SCharacterStat { Health = 70, Armor = 0.02f }
    };
    
    private static BoomBoomEngineData CreateEngine_MonoPlane() => new BoomBoomEngineData
    {
        EngineID = 20401,
        Name = "쿠",
        Rarity = "Legend",
        ModelID = "EQC005_MonoPlane",
        SkillID = 311,
        StatBonus = new SCharacterStat { Health = 70, Armor = 0.02f }
    };
    private static BoomBoomEngineData CreateEngine_EmperorPenguin() => new BoomBoomEngineData
    {
        EngineID = 20402,
        Name = "프리모",
        Rarity = "Legend",
        ModelID = "EQC005_EmperorPenguin",
        SkillID = 312,
        StatBonus = new SCharacterStat { Health = 70, Armor = 0.02f }
    };

    private static BoomBoomEngineData CreateEngine_DogHouse() => new BoomBoomEngineData
    {
        EngineID = 20403,
        Name = "도베르만",
        Rarity = "Legend",
        ModelID = "EQC005_DogHouse",
        SkillID = 313,
        StatBonus = new SCharacterStat { Health = 70, Armor = 0.02f }
    };

    
    private static BoomBoomEngineData CreateEngine_OakTong() => new BoomBoomEngineData
    {
        EngineID = 21101,
        Name = "오크통",
        Rarity = "Normal",
        ModelID = "EQC001_OakTong",
        SkillID = 314,
        StatBonus = new SCharacterStat { Health = 70, Armor = 0.02f }
    };

    private static BoomBoomEngineData CreateEngine_WitchSot() => new BoomBoomEngineData
    {
        EngineID = 21102,
        Name = "마녀솥",
        Rarity = "Normal",
        ModelID = "EQC003_WitchSot",
        SkillID = 315,
        StatBonus = new SCharacterStat { AuraStr = 1.05f, AuraRange = 0.5f }
    };

    private static BoomBoomEngineData CreateEngine_Icecream() => new BoomBoomEngineData
    {
        EngineID = 21103,
        Name = "아이스크림",
        Rarity = "Normal",
        ModelID = "EQC002_Icecream",
        SkillID = 316,
        StatBonus = new SCharacterStat { MoveSpd = 0.5f }
    };

    private static BoomBoomEngineData CreateEngine_MagicHat() => new BoomBoomEngineData
    {
        EngineID = 21201,
        Name = "마술모자",
        Rarity = "Rare",
        ModelID = "EQC009_MagicHat",
        SkillID = 317,
        StatBonus = new SCharacterStat { HealingMult = 1.1f, HealthRegen = 5 }
    };

    private static BoomBoomEngineData CreateEngine_DogBowl() => new BoomBoomEngineData
    {
        EngineID = 21202,
        Name = "개밥그릇",
        Rarity = "Rare",
        ModelID = "EQC004_DogBowl",
        SkillID = 318,
        StatBonus = new SCharacterStat { YFGainMult = 1.1f, Health = 30 }
    };

    private static BoomBoomEngineData CreateEngine_ElfCup() => new BoomBoomEngineData
    {
        EngineID = 21301,
        Name = "엘프컵",
        Rarity = "Unique",
        ModelID = "EQC008_ElfCup",
        SkillID = 319,
        StatBonus = new SCharacterStat { CriticalChance = 0.05f, CriticalMult = 0.2f }
    };

    private static BoomBoomEngineData CreateEngine_TrashCan() => new BoomBoomEngineData
    {
        EngineID = 21302,
        Name = "쓰레기통",
        Rarity = "Unique",
        ModelID = "EQC005_TrashCan",
        SkillID = 320,
        StatBonus = new SCharacterStat { Health = 100, Armor = 0.05f }
    };

    private static BoomBoomEngineData CreateEngine_Cloud() => new BoomBoomEngineData
    {
        EngineID = 20411,
        Name = "구름",
        Rarity = "Unique",
        ModelID = "EQC010_Cloud",
        SkillID = 321,
        StatBonus = new SCharacterStat { AuraRange = 1.2f, AuraStr = 1.1f }
    };

    private static BoomBoomEngineData CreateEngine_Bathtub() => new BoomBoomEngineData
    {
        EngineID = 20412,
        Name = "욕조",
        Rarity = "Legend",
        ModelID = "EQC007_Bathtub",
        SkillID = 322,
        StatBonus = new SCharacterStat { Health = 200, HealthRegen = 10 }
    };

    private static BoomBoomEngineData CreateEngine_SandCastle() => new BoomBoomEngineData
    {
        EngineID = 20413,
        Name = "모래성",
        Rarity = "Legend",
        ModelID = "EQC006_SandCastle",
        SkillID = 323,
        StatBonus = new SCharacterStat { Armor = 0.08f, HealingMult = 1.2f }
    };
}

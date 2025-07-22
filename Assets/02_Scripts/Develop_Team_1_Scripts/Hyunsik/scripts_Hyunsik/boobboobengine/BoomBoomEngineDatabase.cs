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

        Debug.LogError($"�غؿ��� ID {id} ������ ����");
        return null;
    }

    // ���� �޼��� ���� (301 ~ 313)
    private static BoomBoomEngineData CreateEngine_BeeTail() => new BoomBoomEngineData
    {
        // �������� Ÿ�� ���� ���� 3�� ���� ���ͷ� ���� �ǰ��� ���� ����
        EngineID = 20101,
        Name = "����",
        Rarity = "Normal",
        ModelID = "EQC001_BeeTail",
        SkillID = 301,
        StatBonus = new SCharacterStat {  }
    };
    
    private static BoomBoomEngineData CreateEngine_HoneyJar() => new BoomBoomEngineData
    {
        // YF_Gainmult n% ����
        EngineID = 20102,
        Name = "����",
        Rarity = "Normal",
        ModelID = "EQC002_HoneyJar",
        SkillID = 302,
        StatBonus = new SCharacterStat { YFGainMult = 10f }
    };
    
    private static BoomBoomEngineData CreateEngine_Toaster() => new BoomBoomEngineData
    {
        // 5�� ���� Aura_Range n% ����
        EngineID = 20201,
        Name = "�佺��",
        Rarity = "Rare",
        ModelID = "EQC003_Toaster",
        SkillID = 303,
        StatBonus = new SCharacterStat { }
    };
    
    private static BoomBoomEngineData CreateEngine_Sack() => new BoomBoomEngineData
    {
        // 10�� ���� ���ϸ��� Critical_Chance 100%
        EngineID = 20202,
        Name = "�ʱ���",
        Rarity = "Rare",
        ModelID = "EQC004_Sack",
        SkillID = 304,
        StatBonus = new SCharacterStat { }
    };
    
    private static BoomBoomEngineData CreateEngine_Vase() => new BoomBoomEngineData
    {
        // ���ظ� �� �� �����ϴ� �� ����
        EngineID = 20203,
        Name = "��",
        Rarity = "Rare",
        ModelID = "EQC005_Vase",
        SkillID = 305,
        StatBonus = new SCharacterStat {  }
    };
    
    private static BoomBoomEngineData CreateEngine_StoneMortar() => new BoomBoomEngineData
    {
        // Aura_Str n% ����
        EngineID = 20204,
        Name = "������",
        Rarity = "Rare",
        ModelID = "EQC005_StoneMortar",
        SkillID = 306,
        StatBonus = new SCharacterStat { AuraStr = 10f }
    };
    
    private static BoomBoomEngineData CreateEngine_HatchedEgg() => new BoomBoomEngineData
    {
        // YF_Gainmult, OF_Gainmult n%����
        EngineID = 20205,
        Name = "�ҵ�",
        Rarity = "Rare",
        ModelID = "EQC005_HatchedEgg",
        SkillID = 307,
        StatBonus = new SCharacterStat { YFGainMult = 1f , OFGainMult = 1f }
    };
    
    
    private static BoomBoomEngineData CreateEngine_CleanBucket() => new BoomBoomEngineData
    {
        // ���ʸ��� �ֺ� ���ɵ��� ��ȭ�� n% ����
        EngineID = 20301,
        Name = "����",
        Rarity = "Unique",
        ModelID = "EQC005_CleanBucket",
        SkillID = 308,
        StatBonus = new SCharacterStat { }
    };
    
    private static BoomBoomEngineData CreateEngine_Scootus() => new BoomBoomEngineData
    {
        // �̻��� 3�� �߻�, �̻��Ͽ� �ǰݵ� ���� ��ȭ�� 50% ����
        EngineID = 20302,
        Name = "�ƿ챸������",
        Rarity = "Unique",
        ModelID = "EQC005_Scootus",
        SkillID = 309,
        StatBonus = new SCharacterStat { }
    };

    private static BoomBoomEngineData CreateEngine_Box() => new BoomBoomEngineData
    {
        // n�� ���� ���ͷ� ���� �ǰ� ��ȿ
        EngineID = 20303,
        Name = "����",
        Rarity = "Unique",
        ModelID = "EQC005_Box",
        SkillID = 310,
        StatBonus = new SCharacterStat {  }
    };
    
    private static BoomBoomEngineData CreateEngine_MonoPlane() => new BoomBoomEngineData
    {
        // ��Ȳ ������ ���� �� n%�� �Ķ� �������� ��ü�ؼ� ?��
        EngineID = 20401,
        Name = "��",
        Rarity = "Legend",
        ModelID = "EQC005_MonoPlane",
        SkillID = 311,
        StatBonus = new SCharacterStat { }
    };
    private static BoomBoomEngineData CreateEngine_EmperorPenguin() => new BoomBoomEngineData
    {
        // �� ���� ��� ���Ͱ� 3�ʰ� ����
        EngineID = 20402,
        Name = "������",
        Rarity = "Legend",
        ModelID = "EQC005_EmperorPenguin",
        SkillID = 312,
        StatBonus = new SCharacterStat {  }
    };

    private static BoomBoomEngineData CreateEngine_DogHouse() => new BoomBoomEngineData
    {
        // ü���� 0 ���ϰ� �Ǿ��� ��, �ִ�ü���� n%�� ������ ��Ȱ
        EngineID = 20403,
        Name = "��������",
        Rarity = "Legend",
        ModelID = "EQC005_DogHouse",
        SkillID = 313,
        StatBonus = new SCharacterStat { }
    };

    
    private static BoomBoomEngineData CreateEngine_OakTong() => new BoomBoomEngineData
    {
        // Health n% ����
        EngineID = 21101,
        Name = "��ũ��",
        Rarity = "Normal",
        ModelID = "EQC001_OakTong",
        SkillID = 314,
        StatBonus = new SCharacterStat { Health = 30f }
    };

    private static BoomBoomEngineData CreateEngine_WitchSot() => new BoomBoomEngineData
    {
        // Aura_Range n% ����
        EngineID = 21102,
        Name = "�����",
        Rarity = "Normal",
        ModelID = "EQC003_WitchSot",
        SkillID = 315,
        StatBonus = new SCharacterStat { AuraRange = 10.0f }
    };

    private static BoomBoomEngineData CreateEngine_Icecream() => new BoomBoomEngineData
    {
        // Move_Spd n%����
        EngineID = 21103,
        Name = "���̽�ũ��",
        Rarity = "Normal",
        ModelID = "EQC002_Icecream",
        SkillID = 316,
        StatBonus = new SCharacterStat { MoveSpd = 10.0f }
    };

    private static BoomBoomEngineData CreateEngine_MagicHat() => new BoomBoomEngineData
    {
        // ���� 3�� �� �ϳ� �������� 7�� ���� ����
        // Move_Spd n% ����, Aura_Range n% ����, Health_Regen n%���� 
        EngineID = 21201,
        Name = "��������",
        Rarity = "Rare",
        ModelID = "EQC009_MagicHat",
        SkillID = 317,
        StatBonus = new SCharacterStat {  }
    };

    private static BoomBoomEngineData CreateEngine_DogBowl() => new BoomBoomEngineData
    {
        // ���� ����, ���� ����, �̵��ӵ��� ���� �ִ� ������ ��ȯ
        EngineID = 21202,
        Name = "����׸�",
        Rarity = "Rare",
        ModelID = "EQC004_DogBowl",
        SkillID = 318,
        StatBonus = new SCharacterStat {  }
    };

    private static BoomBoomEngineData CreateEngine_ElfCup() => new BoomBoomEngineData
    {
        // �������� ���� ���ƴٴϴ� ���� ��ȯ (����ũ��: n*n)
        EngineID = 21301,
        Name = "������",
        Rarity = "Unique",
        ModelID = "EQC008_ElfCup",
        SkillID = 319,
        StatBonus = new SCharacterStat { }
    };

    private static BoomBoomEngineData CreateEngine_TrashCan() => new BoomBoomEngineData
    {
        // �������� Ŭ���� ������ 1% ���� (����ũ��: n*n)
        EngineID = 21302,
        Name = "��������",
        Rarity = "Unique",
        ModelID = "EQC005_TrashCan",
        SkillID = 320,
        StatBonus = new SCharacterStat { }
    };

    private static BoomBoomEngineData CreateEngine_Cloud() => new BoomBoomEngineData
    {
        // N%�� Ȯ���� �ǰ��� ������
        EngineID = 20411,
        Name = "����",
        Rarity = "Unique",
        ModelID = "EQC010_Cloud",
        SkillID = 321,
        StatBonus = new SCharacterStat { }
    };

    private static BoomBoomEngineData CreateEngine_Bathtub() => new BoomBoomEngineData
    {
        // Critical_Mult n% ����
        EngineID = 20412,
        Name = "����",
        Rarity = "Legend",
        ModelID = "EQC007_Bathtub",
        SkillID = 322,
        StatBonus = new SCharacterStat { CriticalMult = 10f }
    };

    private static BoomBoomEngineData CreateEngine_SandCastle() => new BoomBoomEngineData
    {
        // 20�ʿ� ���� Aura_Range n% ���� ����
        EngineID = 20413,
        Name = "�𷡼�",
        Rarity = "Legend",
        ModelID = "EQC006_SandCastle",
        SkillID = 323,
        StatBonus = new SCharacterStat { }
    };
}

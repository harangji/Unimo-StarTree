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

        Debug.LogError($"�غؿ��� ID {id} ������ ����");
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

    // ���� �޼��� ���� (301 ~ 313)
    private static BoomBoomEngineData CreateEngine_BeeTail()
    {
        var data = new BoomBoomEngineData
        {
            EngineID = 20101,
            Name = "����",
            Rarity = "Normal",
            ModelID = "EQC001_BeeTail",
            SkillID = 301,
            IsUniqueType = true, //  ���� Ÿ�� ����
            DescriptionFormat = "�ǰ� ������ ���, {0}�� ���� ���ͷκ��� �ǰ��� ���� ����",
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
            Name = "����",
            Rarity = "Normal",
            ModelID = "EQC002_HoneyJar",
            SkillID = 302,
            DescriptionFormat = "��� ���� ȹ�淮 {0:P0} ����",
            GrowthTable = Enumerable.Range(0, 51).Select(i => 0.10f + i * ((0.35f - 0.10f) / 50f)).ToArray(),
            StatBonus = new SCharacterStat { },
            IsUniqueType = true,

            GrowthStatCalculator = (level, table, stat) =>
            {
                float gainMult = table[Mathf.Clamp(level, 0, table.Length - 1)];
                stat.YFGainMult = gainMult;
                Debug.Log($"[��������] YFGainMult ����: {gainMult} (���� {level})");
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
            // 5�� ���� Aura_Range n% ����
            EngineID = 20201,
            Name = "�佺��",
            Rarity = "Rare",
            ModelID = "EQC003_Toaster",
            SkillID = 303,
            IsUniqueType = true,
            DescriptionFormat = "��Ȳ ���� 10�� ȹ�� ��, 5�� ���� �ƿ�� {0:P0} ����",
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
            // 10�� ���� ���ϸ��� Critical_Chance 100%
            EngineID = 20202,
            Name = "�ʱ���",
            Rarity = "Rare",
            ModelID = "EQC004_Sack",
            SkillID = 304,
            IsUniqueType = true,
            DescriptionFormat = " ��� ����{0}�� ȹ�� ��, 10�ʵ��� ��Ȯ ũ��Ƽ�� Ȯ�� 100% ����",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => Mathf.Lerp(70f, 20f, i / 50f))  // 0���� 70�� �� 50���� 1��
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
            // ���ظ� �� �� �����ϴ� �� ����
            EngineID = 20203,
            Name = "��",
            Rarity = "Rare",
            ModelID = "EQC005_Vase",
            SkillID = 305,
            IsUniqueType = true,
            DescriptionFormat = "���� ��ȣ���� ���� ���, {0}�� �� ��ȣ�� 1�� ����(�ߺ� �Ұ�)",
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
            Name = "������",
            Rarity = "Rare",
            ModelID = "EQC005_StoneMortar",
            SkillID = 306,
            IsUniqueType = true, // ���� ȿ���� ��� �� �ʿ�
            DescriptionFormat = "���� ���� {0:P0} ����",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.10f + i * ((0.70f - 0.10f) / 50f))
                .ToArray(),

            StatBonus = new SCharacterStat { }, // �⺻���� 0
            GrowthStatCalculator = (level, table, stat) =>
            {
                float bonusPercent = table[Mathf.Clamp(level, 0, table.Length - 1)];
                stat.AuraStr = bonusPercent;  // ���� ������ �ֱ�
                Debug.Log($"[�����̿���] AuraStr ������ ����: {bonusPercent * 100f}% (���� {level})");
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
            // YF_Gainmult, OF_Gainmult n%����
            EngineID = 20205,
            Name = "�ҵ�",
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
            // ���ʸ��� �ֺ� ���ɵ��� ��ȭ�� n% ����
            EngineID = 20301,
            Name = "����",
            Rarity = "Unique",
            ModelID = "EQC005_CleanBucket",
            SkillID = 308,
            DescriptionFormat = "���� ����",
            StatBonus = new SCharacterStat { }
        };

        // ���� ����
        engineDataList.Add(data); 
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_Scootus()
    {
        var data = new BoomBoomEngineData
        {
            // �̻��� 3�� �߻�, �̻��Ͽ� �ǰݵ� ���� ��ȭ�� 50% ����
            EngineID = 20302,
            Name = "�ƿ챸������",
            Rarity = "Unique",
            ModelID = "EQC005_Scootus",
            SkillID = 309,
            DescriptionFormat = "���� ����",
            StatBonus = new SCharacterStat { }
        };

        
        // ���� ����
        engineDataList.Add(data); 
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_Box()
    {
        var data = new BoomBoomEngineData
        {
            // n�� ���� ���ͷκ��� �ǰ� ��ȿ
            EngineID = 20303,
            Name = "����",
            Rarity = "Unique",
            ModelID = "EQC005_Box",
            SkillID = 310,
            IsUniqueType = true, // ���� ���� ���
            DescriptionFormat = "{0}�� ����, ���ͷκ��� 5�ʰ� �ǰ� ��ȿ",
            StatBonus = new SCharacterStat { },
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => Mathf.Lerp(30f, 21f, i / 50f)) // 0����: 30�� �� 50����: 21��
                .ToArray(),
            GrowthStatCalculator = (level, table, stat) => stat,
            TriggerCondition = ETriggerCondition.OnStart // ���� ���� �� �ڵ� �ߵ�
        };

        engineDataList.Add(data); 
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_MonoPlane()
    {
        var data = new BoomBoomEngineData
        {
            // ��Ȳ ������ ���� �� n%�� �Ķ� �������� ��ü�ؼ� ?��
            EngineID = 20401,
            Name = "��",
            Rarity = "Legend",
            ModelID = "EQC005_MonoPlane",
            SkillID = 311,
            DescriptionFormat = "���� ����",
            StatBonus = new SCharacterStat { }
        };

        // ���� ����
        engineDataList.Add(data);
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_EmperorPenguin()
    {
        var data = new BoomBoomEngineData
        {
            // �� ���� ��� ���Ͱ� 3�ʰ� ����
            EngineID = 20402,
            Name = "������",
            Rarity = "Legend",
            ModelID = "EQC005_EmperorPenguin",
            SkillID = 312,
            DescriptionFormat = "���� ����",
            StatBonus = new SCharacterStat { }
        };

        //���� ����
        engineDataList.Add(data); 
        return data;
    }
    
    private static BoomBoomEngineData CreateEngine_DogHouse()
    {
        var data = new BoomBoomEngineData
        {
            // ü���� 0 ���ϰ� �Ǿ��� ��, �ִ�ü���� n%�� ������ ��Ȱ
            EngineID = 20403,
            Name = "��������",
            Rarity = "Legend",
            ModelID = "EQC005_DogHouse",
            SkillID = 313,
            DescriptionFormat = "ü���� ���� �Ҹ���� ���, �ִ�ü�� {0:P0}������ ��Ȱ",
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
            // Health n% ����
            EngineID = 21101,
            Name = "��ũ��",
            Rarity = "Normal",
            ModelID = "EQC001_OakTong",
            SkillID = 314,
            IsUniqueType = true, // ���� ȿ���� ��� �� �ʿ�
            DescriptionFormat = "ü�� {0:P0} ����",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.30f + i * ((2.30f - 0.30f) / 50f))
                .ToArray(),

            StatBonus = new SCharacterStat { }, // �⺻���� 0
            GrowthStatCalculator = (level, table, stat) =>
            {
                float hpPercent = table[Mathf.Clamp(level, 0, table.Length - 1)];
                stat.Health = hpPercent;  // ���� ������ �ֱ�
                Debug.Log($"[��ũ�뿣��] Health ������ ����: {hpPercent * 100f}% (���� {level})");
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
            // Aura_Range n% ����
            EngineID = 21102,
            Name = "�����",
            Rarity = "Normal",
            ModelID = "EQC003_WitchSot",
            SkillID = 315,
            IsUniqueType = true, // ���� ȿ���� ��� �� �ʿ�
            DescriptionFormat = "�ƿ�� ���� {0:P0} ����",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.10f + i * ((0.60f - 0.10f) / 50f))
                .ToArray(),

            StatBonus = new SCharacterStat { }, // �⺻���� 0
            GrowthStatCalculator = (level, table, stat) =>
            {
                float AuraRangePercent = table[Mathf.Clamp(level, 0, table.Length - 1)];
                stat.AuraRange = AuraRangePercent;  // ���� ������ �ֱ�
                Debug.Log($"[����ܿ���] AuraRange ������ ����: {AuraRangePercent * 100f}% (���� {level})");
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
            // Move_Spd n%����
            EngineID = 21103,
            Name = "���̽�ũ��",
            Rarity = "Normal",
            ModelID = "EQC002_Icecream",
            SkillID = 316,
            IsUniqueType = true, // ���� ȿ���� ��� �� �ʿ�
            DescriptionFormat = "�̵��ӵ� {0:P0} ����",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.02f + i * (0.08f / 50f))  // 2% ~ 10%
                .ToArray(),

            StatBonus = new SCharacterStat { }, // �⺻���� 0
            GrowthStatCalculator = (level, table, stat) =>
            {
                float MoveSpdPercent = table[Mathf.Clamp(level, 0, table.Length - 1)];
                stat.MoveSpd = MoveSpdPercent;
                Debug.Log($"[���̽�ũ������] MoveSpd ������ ����: {MoveSpdPercent * 100f}% (���� {level})");
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
            // ���� 3�� �� �ϳ� �������� 7�� ���� ����
            // Move_Spd n% ����, Aura_Range n% ����, Health_Regen n%���� 
            EngineID = 21201,
            Name = "��������",
            Rarity = "Rare",
            ModelID = "EQC009_MagicHat",
            SkillID = 317,
            DescriptionFormat = "���� �� �ϳ��� ȿ���� 7�ʸ��� ����." +
                                "�̵��ӵ�, �ƿ�� ũ��, ü�� ��� �� �ϳ� {0:P0} ���",
            IsUniqueType = true, // ���� ȿ���� ��� �� �ʿ�
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.10f + i * ((0.30f - 0.10f) / 50f)) // 0���� 10% �� 50���� 30%
                .ToArray(),
            StatBonus = new SCharacterStat { },
            TriggerCondition = ETriggerCondition.OnStart //  �߰�: ���� �� �ߵ� ����
        };

        engineDataList.Add(data); 
        return data;
    }

    private static BoomBoomEngineData CreateEngine_DogBowl()
    {
        var data = new BoomBoomEngineData
        {
            // ���� ����, ���� ����, �̵��ӵ��� ���� �ִ� ������ ��ȯ
            EngineID = 21202,
            Name = "����׸�",
            Rarity = "Rare",
            ModelID = "EQC004_DogBowl",
            SkillID = 318,
            DescriptionFormat = "{0} ���� ����, {1}���� ����, {2}�� �̵��ӵ������� " +
                                "�÷��� ������ ȸ���ϴ� ������ ��ȯ",
            StatBonus = new SCharacterStat { }
        };

        engineDataList.Add(data); 
        return data;
    }

    private static BoomBoomEngineData CreateEngine_ElfCup()
    {
        var data = new BoomBoomEngineData
        {
            // �������� Ŭ���� ������ 1% ���� (����ũ��: n*n)
            EngineID = 21301,
            Name = "������",
            Rarity = "Unique",
            ModelID = "EQC008_ElfCup",
            SkillID = 320,
            DescriptionFormat = "���� ����",
            StatBonus = new SCharacterStat { }
        };
        //���� ����
        engineDataList.Add(data);
        return data;
    }

    private static BoomBoomEngineData CreateEngine_TrashCan()
    {
        var data = new BoomBoomEngineData
        {
            // N%�� Ȯ���� �ǰ��� ������
            EngineID = 21302,
            Name = "��������",
            Rarity = "Unique",
            ModelID = "EQC005_TrashCan",
            SkillID = 321,
            IsUniqueType = true, // ���� ȿ���� ��� �� �ʿ�
            DescriptionFormat = "{0:P0}�� �ǰ� ����",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.05f + i * ((0.35f - 0.05f) / 50f)) // 0.05 ~ 0.35
                .ToArray(),

            StatBonus = new SCharacterStat { }, // �⺻���� 0
            GrowthStatCalculator = (level, table, stat) =>
            {
                float StunIgnoreChancePercent = table[Mathf.Clamp(level, 0, table.Length - 1)];
                stat.StunIgnoreChance = StunIgnoreChancePercent;  // ���� ������ �ֱ�
                Debug.Log($"[�������뿣��] StunIgnoreChance ����: {StunIgnoreChancePercent * 100f}% (���� {level})");
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
            // �������� ���� ���ƴٴϴ� ���� ��ȯ (����ũ��: n*n)
            EngineID = 20411,
            Name = "����",
            Rarity = "Unique",
            ModelID = "EQC010_Cloud",
            SkillID = 319,
            DescriptionFormat = "���� ���ƴٴ� {0:P0} �ƿ�� ũ�⸦ ���� ���� ��ȯ",
            StatBonus = new SCharacterStat { }
        };

        engineDataList.Add(data); 
        return data;
    }

    private static BoomBoomEngineData CreateEngine_Bathtub()
    {
        var data = new BoomBoomEngineData
        {
            // Critical_Mult n% ����
            EngineID = 20412,
            Name = "����",
            Rarity = "Legend",
            ModelID = "EQC007_Bathtub",
            SkillID = 322,
            IsUniqueType = true, // ���� ȿ���� ��� �� �ʿ�
            DescriptionFormat = "ũ��Ƽ�� ��� {0:P0} ����",
            GrowthTable = Enumerable.Range(0, 51)
                .Select(i => 0.10f + i * ((1.70f - 0.10f) / 50f)) // 0.10 ~ 1.70
                .ToArray(),

            StatBonus = new SCharacterStat { }, // �⺻���� 0
            GrowthStatCalculator = (level, table, stat) =>
            {
                float CriticalMultPercent = table[Mathf.Clamp(level, 0, table.Length - 1)];
                stat.CriticalMult = CriticalMultPercent;  //  ���� ������ �ֱ�
                Debug.Log($"[��������] CriticalMult ������ ����: {CriticalMultPercent * 100f}% (���� {level})");
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
            // 20�ʿ� ���� Aura_Range n% ���� ����
            EngineID = 20413,
            Name = "�𷡼�",
            Rarity = "Legend",
            ModelID = "EQC006_SandCastle",
            SkillID = 323,
            DescriptionFormat = "20�ʿ� ���ļ� �ƿ�� ũ�� {0:P0} ���� ����, �ǰ� �� �ʱ�ȭ",
            StatBonus = new SCharacterStat { }
        };

        engineDataList.Add(data); 
        return data;
    }
    
    
    
}

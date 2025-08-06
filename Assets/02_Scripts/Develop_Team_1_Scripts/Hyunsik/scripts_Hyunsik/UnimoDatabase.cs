using System;
using System.Collections.Generic;
using UnityEngine;

public static class UnimoDatabase
{
    private static readonly Dictionary<int, Func<UnimoData>> mDataTable = new()
    {
        { 10101, CreateUnimo_Ribi },
        { 10102, CreateUnimo_Gomgom },
        { 10201, CreateUnimo_Toaster },
        { 10202, CreateUnimo_Raccoon },
        { 10203, CreateUnimo_Egg },
        { 10204, CreateUnimo_Jumbuni },
        { 10205, CreateUnimo_DoomDoom },
        { 10301, CreateUnimo_Mackerel },
        { 10302, CreateUnimo_Augustus },
        { 10303, CreateUnimo_Sopo },
        { 10401, CreateUnimo_Ku },
        { 10402, CreateUnimo_Primo },
        { 10403, CreateUnimo_Doberman },
    };


    public static UnimoData GetUnimoData(int id)
    {
        if (mDataTable.TryGetValue(id, out var creator))
            return creator.Invoke();

        Debug.LogError($"Unimo ID {id} not found.");
        return null;
    }
    
    // ����
    private static UnimoData CreateUnimo_Ribi() => new UnimoData
    {
        UnimoID = 10101,
        Name = "����",
        Rarity = "Normal",
        ModelID = "CH001",
        Stat = UnimoLevelSystem.ApplyLevelBonus(new SCharacterStat
        {
            MoveSpd = 8.2f,
            CharSize = new Vector2Int(120, 120),
            Health = 250,
            HealthRegen = 1,
            HealingMult = 1f,
            Armor = 0.05f,
            StunIgnoreChance = 0f,
            StunResistanceRate = 0f,
            CriticalChance = 0f,
            CriticalMult = 1.1f,
            YFGainMult = 1.3f,
            OFGainMult = 1.5f,
            AuraRange = 6.92f,
            AuraStr = 1f
        }, 10101),

        Grade = new EStatGradeSet { MoveSpd = EStatGrade.C }
    };
    
    // ����
    private static UnimoData CreateUnimo_Gomgom() => new UnimoData
    {
        UnimoID = 10102,
        Name = "����",
        Rarity = "Normal",
        ModelID = "CH003",
        Stat =  UnimoLevelSystem.ApplyLevelBonus(new SCharacterStat
        {
            MoveSpd = 7.5f,
            CharSize = new Vector2Int(130, 130),
            Health = 350, // ����
            HealthRegen = 6, // ����
            HealingMult = 1f, // ����
            Armor = 0.05f, // ����
            StunIgnoreChance = 0f,
            StunResistanceRate = 0f,
            CriticalChance = 0f,
            CriticalMult = 1.1f, 
            YFGainMult = 1.5f,
            OFGainMult = 2f,
            AuraRange = 8.73f,
            AuraStr = 1.2f
        }, 10102),
        
        Grade = new EStatGradeSet { MoveSpd = EStatGrade.C }
    };
    
    
    // �佺��
    private static UnimoData CreateUnimo_Toaster() => new UnimoData
    {
        UnimoID = 10201,
        Name = "�佺��",
        Rarity = "Rare",
        ModelID = "CH013",
        Stat = UnimoLevelSystem.ApplyLevelBonus(new SCharacterStat
        {
            MoveSpd = 9f,
            CharSize = new Vector2Int(120, 120),
            Health = 350,
            HealthRegen = 0,
            HealingMult = 1f,
            Armor = 0.1f,
            StunIgnoreChance = 0.07f,
            StunResistanceRate = 0.3f,
            CriticalChance = 0.1f,
            CriticalMult = 1.35f,
            YFGainMult = 1.5f,
            OFGainMult = 2f,
            AuraRange = 8.73f,
            AuraStr = 1.2f
        }, 10201),
        
        Grade = new EStatGradeSet { MoveSpd = EStatGrade.B /* ���� */ }
    };
    
    // ��
    private static UnimoData CreateUnimo_Raccoon() => new UnimoData
    {
        UnimoID = 10202,
        Name = "��",
        Rarity = "Rare",
        ModelID = "CH012",
        Stat = UnimoLevelSystem.ApplyLevelBonus(new SCharacterStat
        {
            MoveSpd = 9.5f,
            CharSize = new Vector2Int(115, 115),
            Health = 170,
            HealthRegen = 3,
            HealingMult = 1f,
            Armor = 0.02f,
            StunIgnoreChance = 0f,
            StunResistanceRate = 0.2f,
            CriticalChance = 0.13f,
            CriticalMult = 1.4f,
            YFGainMult = 1.5f,
            OFGainMult = 2f,
            AuraRange = 4.20f,
            AuraStr = 1.5f
        }, 010202),
        
        Grade = new EStatGradeSet { MoveSpd = EStatGrade.B }  // ����
    };
    
    // ��
    private static UnimoData CreateUnimo_Egg() => new UnimoData
    {
        UnimoID = 10203,
        Name = "��",
        Rarity = "Rare",
        ModelID = "CH009",
        Stat = UnimoLevelSystem.ApplyLevelBonus(new SCharacterStat
        {
            MoveSpd = 7.5f,
            CharSize = new Vector2Int(120, 120),
            Health = 350,
            HealthRegen = 6,
            HealingMult = 1f,
            Armor = 0.05f,
            StunIgnoreChance = 0f,
            StunResistanceRate = 0f,
            CriticalChance = 0f,
            CriticalMult = 1.1f,
            YFGainMult = 1.5f,
            OFGainMult = 2f,
            AuraRange = 9.57f,
            AuraStr = 1.2f
        },10203),
        Grade = new EStatGradeSet { MoveSpd = EStatGrade.B }  // ���÷� Rare ĳ���ʹ� B�� ����
    };
    
    // ������
    private static UnimoData CreateUnimo_Jumbuni() => new UnimoData
    {
        UnimoID = 10204,
        Name = "������",
        Rarity = "Rare",
        ModelID = "CH004",
        Stat = UnimoLevelSystem.ApplyLevelBonus(new SCharacterStat
        {
            MoveSpd = 9.5f,
            CharSize = new Vector2Int(115, 115),
            Health = 200,
            HealthRegen = 0,
            HealingMult = 1f,
            Armor = 0.05f,
            StunIgnoreChance = 0f,
            StunResistanceRate = 0.4f,
            CriticalChance = 0.16f,
            CriticalMult = 1.35f,
            YFGainMult = 1.3f,
            OFGainMult = 2f,
            AuraRange = 5.04f,
            AuraStr = 1.2f
        },10204),
        Grade = new EStatGradeSet { MoveSpd = EStatGrade.B }  // Rare ĳ���Ͷ�� ���÷� B ����
    };
    
    // �ҵ�
    private static UnimoData CreateUnimo_DoomDoom() => new UnimoData
    {
        UnimoID = 10205,
        Name = "�ҵ�",
        Rarity = "Rare",
        ModelID = "CH007",
        Stat = UnimoLevelSystem.ApplyLevelBonus(new SCharacterStat
        {
            MoveSpd = 9.0f,
            CharSize = new Vector2Int(120, 120),
            Health = 200,
            HealthRegen = 5,
            HealingMult = 1f,
            Armor = 0.1f,
            StunIgnoreChance = 0.1f,
            StunResistanceRate = 0.3f,
            CriticalChance = 0f,
            CriticalMult = 1.1f,
            YFGainMult = 1.5f,
            OFGainMult = 2f,
            AuraRange = 8.73f,
            AuraStr = 1.2f
        },10205),
        Grade = new EStatGradeSet { MoveSpd = EStatGrade.B }  // Rare ��� ����
    };
    
    // ����
    private static UnimoData CreateUnimo_Mackerel() => new UnimoData
    {
        UnimoID = 10301,
        Name = "����",
        Rarity = "Unique",
        ModelID = "CH005",
        Stat = UnimoLevelSystem.ApplyLevelBonus(new SCharacterStat
        {
            MoveSpd = 9.0f,
            CharSize = new Vector2Int(120, 120),
            Health = 350,
            HealthRegen = 5,
            HealingMult = 1f,
            Armor = 0.07f,
            StunIgnoreChance = 0.07f,
            StunResistanceRate = 0f,
            CriticalChance = 0.1f,
            CriticalMult = 1.35f,
            YFGainMult = 1.5f,
            OFGainMult = 2f,
            AuraRange = 8.73f,
            AuraStr = 1.2f
        }, 10301),
        Grade = new EStatGradeSet { MoveSpd = EStatGrade.A }  // Unique ��� ���÷� A ����
    };
    
    //�ƿ챸������
    private static UnimoData CreateUnimo_Augustus() => new UnimoData
    {
        UnimoID = 10302,
        Name = "�ƿ챸������",
        Rarity = "Unique",
        ModelID = "CH008",
        Stat = UnimoLevelSystem.ApplyLevelBonus(new SCharacterStat
        {
            MoveSpd = 9.5f,
            CharSize = new Vector2Int(120, 120),
            Health = 250,
            HealthRegen = 4,
            HealingMult = 1.07f,
            Armor = 0.05f,
            StunIgnoreChance = 0.1f,
            StunResistanceRate = 0.3f,
            CriticalChance = 0.1f,
            CriticalMult = 1.35f,
            YFGainMult = 2f,
            OFGainMult = 3f,
            AuraRange = 6.92f,
            AuraStr = 1f
        }, 10302),
        Grade = new EStatGradeSet { MoveSpd = EStatGrade.A }  // Unique ��� ����
    };
    
    // ����
    private static UnimoData CreateUnimo_Sopo() => new UnimoData
    {
        UnimoID = 10303,
        Name = "����",
        Rarity = "Unique",
        ModelID = "CH002",
        Stat = UnimoLevelSystem.ApplyLevelBonus(new SCharacterStat
        {
            MoveSpd = 7.0f,
            CharSize = new Vector2Int(110, 110),
            Health = 250,
            HealthRegen = 8,
            HealingMult = 1.21f,
            Armor = 0.13f,
            StunIgnoreChance = 0.15f,
            StunResistanceRate = 0.4f,
            CriticalChance = 0.1f,
            CriticalMult = 1.35f,
            YFGainMult = 1.3f,
            OFGainMult = 1.5f,
            AuraRange = 5.04f,
            AuraStr = 0.8f
        },10303),
        Grade = new EStatGradeSet { MoveSpd = EStatGrade.A }  // Unique ��� ����
    };
    
    // ��ƿ��
    private static UnimoData CreateUnimo_Ku() => new UnimoData
    {
        UnimoID = 10401,
        Name = "��ƿ��",
        Rarity = "Legend",
        ModelID = "CH006",
        Stat = UnimoLevelSystem.ApplyLevelBonus(new SCharacterStat
        {
            MoveSpd = 10.0f,
            CharSize = new Vector2Int(120, 120),
            Health = 350,
            HealthRegen = 6,
            HealingMult = 1.13f,
            Armor = 0.07f,
            StunIgnoreChance = 0.15f,
            StunResistanceRate = 0.3f,
            CriticalChance = 0.13f,
            CriticalMult = 1.35f,
            YFGainMult = 1.5f,
            OFGainMult = 2f,
            AuraRange = 9.57f,
            AuraStr = 1.5f
        }, 10401),
        Grade = new EStatGradeSet { MoveSpd = EStatGrade.S }  // Legend ��� ����
    };
    
    // ������
    private static UnimoData CreateUnimo_Primo() => new UnimoData
    {
        UnimoID = 10402,
        Name = "������",
        Rarity = "Legend",
        ModelID = "CH010",
        Stat = UnimoLevelSystem.ApplyLevelBonus(new SCharacterStat
        {
            MoveSpd = 9.5f,
            CharSize = new Vector2Int(120, 120),
            Health = 350,
            HealthRegen = 6,
            HealingMult = 1.13f,
            Armor = 0.07f,
            StunIgnoreChance = 0.15f,
            StunResistanceRate = 0.3f,
            CriticalChance = 0.13f,
            CriticalMult = 1.35f,
            YFGainMult = 1.5f,
            OFGainMult = 2f,
            AuraRange = 11.84f,
            AuraStr = 2f
        }, 10402),
        Grade = new EStatGradeSet { MoveSpd = EStatGrade.S }  // Legend ��� ����
    };
 
    // ��������
    private static UnimoData CreateUnimo_Doberman() => new UnimoData
    {
        UnimoID = 10403,
        Name = "��������",
        Rarity = "Legend",
        ModelID = "CH011",
        Stat = UnimoLevelSystem.ApplyLevelBonus(new SCharacterStat
        {
            MoveSpd = 9.5f,
            CharSize = new Vector2Int(100, 100),
            Health = 250,
            HealthRegen = 5,
            HealingMult = 1.13f,
            Armor = 0.07f,
            StunIgnoreChance = 0.3f,
            StunResistanceRate = 0.3f,
            CriticalChance = 0.16f,
            CriticalMult = 1.4f,
            YFGainMult = 3f,
            OFGainMult = 4f,
            AuraRange = 8.73f,
            AuraStr = 1.2f
        }, 10403),
        Grade = new EStatGradeSet { MoveSpd = EStatGrade.S }  // Legend ��� ����
    };
    
    
    
}


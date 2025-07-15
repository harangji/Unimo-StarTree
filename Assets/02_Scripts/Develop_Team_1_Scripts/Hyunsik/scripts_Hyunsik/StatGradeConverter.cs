using UnityEngine;

public static class StatGradeConverter
{
    public static EStatGrade ConvertValueToGrade(string statName, float value)
    {
        return statName switch
        {
            "Move_Spd" => GetGrade(value, 600, 1000),
            "Health" => GetGrade(value, 150, 400),
            "Health_Regen" => GetGrade(value, 1, 10),
            "Critical_Chance" => GetGrade(value, 0.0f, 0.2f),
            _ => EStatGrade.None
        };
    }

    private static EStatGrade GetGrade(float value, float min, float max)
    {
        float range = max - min;
        if (value >= min + range * 0.9f) return EStatGrade.S;
        if (value >= min + range * 0.75f) return EStatGrade.A;
        if (value >= min + range * 0.6f) return EStatGrade.B;
        if (value >= min + range * 0.4f) return EStatGrade.C;
        if (value >= min + range * 0.2f) return EStatGrade.D;
        if (value > 0) return EStatGrade.E;
        return EStatGrade.None;
    }
}


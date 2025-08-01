using System.Collections.Generic;
using UnityEngine;

public struct SStatCost
{
    public int RequireYF;
    public int RequireOF;

    public SStatCost(int yf, int of)
    {
        RequireYF = yf;
        RequireOF = of;
    }
}

public static class StatCostDataLoader
{
    public static Dictionary<int, SStatCost> LoadCostTable(TextAsset csvText)
    {
        Dictionary<int, SStatCost> costTable = new();

        string[] lines = csvText.text.Split('\n');

        // 헤더 무시하고 1번째 줄부터 시작
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] tokens = line.Split(',');

            if (tokens.Length < 3) continue;

            if (int.TryParse(tokens[0], out int level) &&
                int.TryParse(tokens[1], out int requireYF) &&
                int.TryParse(tokens[2], out int requireOF))
            {
                costTable[level] = new SStatCost(requireYF, requireOF);
            }
        }

        return costTable;
    }
}
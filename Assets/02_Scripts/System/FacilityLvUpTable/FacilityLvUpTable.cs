using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FacilLvUpTable", menuName = "Scriptable Object/FacilLvUpTable", order = int.MaxValue)]
public class FacilityLvUpTable : ScriptableObject
{
    [SerializeField] private List<int> tierUpLevel;
    [SerializeField] private List<int> upgradeLvStandards;

    public int CalculateTier(int idleLv)
    {
        int tier = 0;
        while (tier < tierUpLevel.Count && idleLv >= tierUpLevel[tier])
        { tier++; }
        return tier;
    }
    public List<List<int>> CalculateUpAccumPerTier(int idleLv)
    {
        List<List<int>> accumList = new List<List<int>>();
        
        int tier = CalculateTier(idleLv);
        for (int i = 0; i<tier; i++)
        {
            int actLv;
            accumList.Add(new List<int>());
            if (i == tierUpLevel.Count - 1)
            {
                actLv = idleLv - tierUpLevel[i];
            }
            else
            {
                actLv = Mathf.Min(idleLv - tierUpLevel[i], tierUpLevel[i + 1]-1 - tierUpLevel[i]);
            }
            for (int j = 0; j < upgradeLvStandards.Count; j++)
            {
                int add = actLv / upgradeLvStandards[j];
                accumList[i].Add(add);
                //for (int k = 0; k < j; k++)
                //{
                //    accumList[i][k] -= add;
                //}
            }
        }
        return accumList;
    }
    public List<bool> NextUpList(int idleLv)
    {
        List<bool> checks = new List<bool>();
        int oritier = CalculateTier(idleLv);
        int newtier = CalculateTier(idleLv+1);
        if (newtier == 0)
        {
            for (int i = 0; i < upgradeLvStandards.Count; i++)
            {
                checks.Add(false);
            }
            return checks;
        }

        if (oritier != newtier) 
        { 
            checks.Add(true);
            for (int i = 0; i < upgradeLvStandards.Count; i++)
            {
                checks.Add(false);
            }
        }
        else 
        { 
            checks.Add(false);
            int actLv = idleLv+1 - tierUpLevel[newtier - 1];
            for (int i = 0; i < upgradeLvStandards.Count; i++)
            {
                checks.Add(actLv % upgradeLvStandards[i] == 0);
            }
        }
        return checks;
    }
}

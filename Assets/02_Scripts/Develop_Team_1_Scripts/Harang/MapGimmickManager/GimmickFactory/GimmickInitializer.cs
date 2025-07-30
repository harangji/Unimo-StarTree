using System;
using UnityEngine;

public abstract class GimmickInitializer : MonoBehaviour
{        
    public abstract eGimmicks eGimmick { get; }
    public string GimmickID { get; set; }
    public string GimmickName { get; set; }
    public int[] Costs { get; set; }
    public int[] Weights { get; set; }
    public float[] Durations_s { get; set; }
    public float[] EffectValue1 { get; set; }
    public float[] EffectValue2 { get; set; }
        
    private bool mGimmickInitialized = false;
    
    public void GimmickInitializerSetting(ToTsvGimmickData gimmickData)
    {
        GimmickID = gimmickData.Key;
        GimmickName = gimmickData.GimmickName;
        Costs = gimmickData.Costs;
        Weights = gimmickData.Weights;
        Durations_s = gimmickData.Durations_s;
        EffectValue1 = gimmickData.EffectValue1;
        EffectValue2 = gimmickData.EffectValue2;

        mGimmickInitialized = true;
    }
    
    public abstract Gimmick InitializeGimmick(eGimmickGrade gimmickGrade);
}

using UnityEngine;

public abstract class GimmickInitializer : MonoBehaviour
{        
    public abstract eGimmickType GimmickType { get; }
    public string GimmickName { get; set; }
    public string GimmickID { get; set; }
    public int[] Costs { get; set; }
    public int[] Weights { get; set; }
    public int[] Durations_s { get; set; }
    public float[] EffectValue1 { get; set; }
    public float[] EffectValue2 { get; set; }
        
    private bool mGimmickInitialized = false;
    
    public void InitializeGimmickInitializer(ToTsvGimmickData gimmickData)
    {
        GimmickName = gimmickData.GimmickName;
        GimmickID = gimmickData.GimmickID;
        Costs = gimmickData.Costs;
        Weights = gimmickData.Weights;
        Durations_s = gimmickData.Durations_s;
        EffectValue1 = gimmickData.EffectValue1;
        EffectValue2 = gimmickData.EffectValue2;

        mGimmickInitialized = true;
    }
    
    public abstract Gimmick InitializeGimmick(eGimmickGrade gimmickGrade);

    // protected Gimmick CreateBaseGimmick(GimmickGrade gimmickGrade)
    // {
    //     return new Gimmick(gimmickFactoryData, gimmickGrade);
    // }
}

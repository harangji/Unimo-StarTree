using UnityEngine;

public abstract class GimmickInitializer : MonoBehaviour
{        
    public class GimmickInitializerData
    {
        public string GimmickName { get; set; }
        public string GimmickID { get; set; }
        public int[] Costs { get; set; }
        public int[] Weights { get; set; }
        public int[] Durations_s { get; set; }
        public float[] EffectValue1 { get; set; }
        public float[] EffectValue2 { get; set; }
    }

    protected GimmickInitializerData gimmickFactoryData { get; set; }
        
    private bool mGimmickInitialized = false;
        
    public void InitializeGimmickInitializer(ToTsvGimmickData gimmickData)
    {
        gimmickFactoryData.GimmickName = gimmickData.GimmickName;
        gimmickFactoryData.GimmickID = gimmickData.GimmickID;
        gimmickFactoryData.Costs = gimmickData.Costs;
        gimmickFactoryData.Weights = gimmickData.Weights;
        gimmickFactoryData.Durations_s = gimmickData.Durations_s;
        gimmickFactoryData.EffectValue1 = gimmickData.EffectValue1;
        gimmickFactoryData.EffectValue2 = gimmickData.EffectValue2;

        mGimmickInitialized = true;
    }
    
    public abstract void InitializeGimmick(GimmickGrade gimmickGrade);

    // protected Gimmick CreateBaseGimmick(GimmickGrade gimmickGrade)
    // {
    //     return new Gimmick(gimmickFactoryData, gimmickGrade);
    // }
}

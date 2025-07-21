using UnityEngine;

public class RedZoneGimmickInitializer : GimmickInitializer
{
    [SerializeField] private Gimmick_BlackHole blackHole;
    
    public override void InitializeGimmick(GimmickGrade gimmickGrade)
    {
        blackHole.InitializeGimmick(gimmickFactoryData, gimmickGrade);
    }
}
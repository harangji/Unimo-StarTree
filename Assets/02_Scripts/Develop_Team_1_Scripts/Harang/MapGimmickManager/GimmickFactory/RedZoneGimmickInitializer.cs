using UnityEngine;

public class RedZoneGimmickInitializer : GimmickInitializer
{
    [SerializeField] private Gimmick_BlackHole redZone;
    
    public override eGimmickType GimmickType => eGimmickType.RedZone;
    
    public override Gimmick InitializeGimmick(eGimmickGrade gimmickGrade)
    {
        redZone.InitializeGimmick(this, gimmickGrade);
        return redZone;
    }
}
using UnityEngine;

public class RedZoneGimmickInitializer : GimmickInitializer
{
    [SerializeField] private Gimmick_RedZone redZone;
    
    public override eGimmicks eGimmick => eGimmicks.RedZone;
    
    public override Gimmick InitializeGimmick(eGimmickGrade gimmickGrade)
    {
        redZone.InitializeGimmick(this, gimmickGrade);
        return redZone;
    }
}
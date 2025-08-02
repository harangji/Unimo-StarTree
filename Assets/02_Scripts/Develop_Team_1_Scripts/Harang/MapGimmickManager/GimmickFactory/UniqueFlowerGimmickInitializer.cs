using UnityEngine;

public class UniqueFlowerGimmickInitializer : GimmickInitializer
{
    [SerializeField] private Gimmick_UniqueFlower uniqueFlower;
    
    public override eGimmicks eGimmick => eGimmicks.UniqueFlower;
    
    public override Gimmick InitializeGimmick(eGimmickGrade gimmickGrade)
    {
        uniqueFlower.InitializeGimmick(this, gimmickGrade);
        return uniqueFlower;
    }
}
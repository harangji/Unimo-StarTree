using UnityEngine;

public class BlackHoleGimmickInitializer : GimmickInitializer
{
    [SerializeField] private Gimmick_BlackHole blackHole;

    public override eGimmicks eGimmick => eGimmicks.BlackHole;

    public override Gimmick InitializeGimmick(eGimmickGrade gimmickGrade)
    {
        blackHole.InitializeGimmick(this, gimmickGrade);
        return blackHole;
    }
}

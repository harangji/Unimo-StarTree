using UnityEngine;

public class BlackHoleGimmickInitializer : GimmickInitializer
{
    [SerializeField] private Gimmick_BlackHole blackHole;

    public override eGimmickType GimmickType => eGimmickType.BlackHole;

    public override Gimmick InitializeGimmick(eGimmickGrade gimmickGrade)
    {
        blackHole.InitializeGimmick(this, gimmickGrade);
        return blackHole;
    }
}

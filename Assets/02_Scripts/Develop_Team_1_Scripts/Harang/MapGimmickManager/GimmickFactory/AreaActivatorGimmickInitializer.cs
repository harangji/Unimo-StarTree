using UnityEngine;

public class AreaActivatorGimmickInitializer : GimmickInitializer
{
    [SerializeField] private Gimmick_AreaActivator areaActivator;
    
    public override eGimmicks eGimmick => eGimmicks.AreaActivator;
    
    public override Gimmick InitializeGimmick(eGimmickGrade gimmickGrade)
    {
        areaActivator.InitializeGimmick(this, gimmickGrade);
        return areaActivator;
    }
}
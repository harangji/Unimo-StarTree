using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon001Indicator_EvadeTemp : MonsterIndicatorCtrl
{
    public override void InitIndicator()
    {
        GetIndicatorTransform().localScale = Vector3.zero;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimTriggerPlayable : PlayableBehaviour
{
    public string animTrigger;
    private bool hasSet = false;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (hasSet) { return; }
        Animator anim= playerData as Animator;
        anim.SetTrigger(animTrigger);
        hasSet = true;
    }
}

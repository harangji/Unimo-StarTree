using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimTriggerClip : PlayableAsset
{
    public string animTrigger;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AnimTriggerPlayable>.Create(graph);

        AnimTriggerPlayable eventPlayable = playable.GetBehaviour();
        eventPlayable.animTrigger = animTrigger;

        return playable;
    }
}

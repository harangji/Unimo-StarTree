using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PopupTextClip : PlayableAsset
{
    public string popupText;
    public Vector2 position;
    public bool isActive = false;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<PopupTextPlayable>.Create(graph);

        PopupTextPlayable eventPlayable = playable.GetBehaviour();
        eventPlayable.eventText = popupText;
        eventPlayable.position = position;
        eventPlayable.isActive = isActive;

        return playable;
    }
}

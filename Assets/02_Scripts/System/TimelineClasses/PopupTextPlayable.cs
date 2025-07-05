using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PopupTextPlayable : PlayableBehaviour
{
    public string eventText;
    public Vector2 position;
    public bool isActive = false;
    private bool hasSet = false;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (hasSet) { return; }
        PopupTextObj popup = playerData as PopupTextObj;
        popup.SetPopupProperties(eventText, position, isActive);
        hasSet = true;
    }
}

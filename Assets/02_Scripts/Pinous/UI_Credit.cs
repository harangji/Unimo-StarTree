using UnityEngine;

public class UI_Credit : UI_Base
{
    public override void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Canvas_Holder.CloseAllPopupUI();
        }
        return;
        base.Update();
    }
}

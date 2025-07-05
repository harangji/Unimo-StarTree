using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EXIT : UI_Base
{
    public override void Start()
    {
        base.Start();
    }
    public void GetEXIT() => Application.Quit();
}

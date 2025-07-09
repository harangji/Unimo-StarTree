using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item001_Controller : ItemController
{
    private static PlayTimeManager playTimeManager;
    private readonly float gainRatio = 0.2f;
    override public void UseItem()
    {
        if (playTimeManager == null) { playTimeManager = PlaySystemRefStorage.playTimeManager; }
        playTimeManager.ChangeTimer(gainRatio * playTimeManager.GetMaxTime());
        Base_Manager.Data.UserData.TimeItem++;
        base.UseItem();
    }
}

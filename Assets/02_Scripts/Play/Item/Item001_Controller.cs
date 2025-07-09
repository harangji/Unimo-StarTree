using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 타이머 회복 시켜주는 아이템에서 HP 회복 시켜주는 아이템으로 변경함.정현식
public class Item001_Controller : ItemController
{
    private static HPManager hpManager;
    [SerializeField] private float healAmount = 20f; // 고정 회복량

    override public void UseItem()
    {
          if (hpManager == null)
          {
              hpManager = FindAnyObjectByType<HPManager>(); // 또는 PlaySystemRefStorage.hpManager
          }

          hpManager.Heal(healAmount);
          //Base_Mng.Data.data.TimeItem++;
          base.UseItem();
    }
}

// 원본 코드  
//public class Item001_Controller : ItemController
//{
//    private static PlayTimeManager playTimeManager;
//    private readonly float gainRatio = 0.2f;
//    override public void UseItem()
//    {
//        if (playTimeManager == null) { playTimeManager = PlaySystemRefStorage.playTimeManager; }
//        playTimeManager.ChangeTimer(gainRatio * playTimeManager.GetMaxTime());
//        Base_Mng.Data.data.TimeItem++;
//        base.UseItem();
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ÿ�̸� ȸ�� �����ִ� �����ۿ��� HP ȸ�� �����ִ� ���������� ������.������
public class Item001_Controller : ItemController
{
    private static HPManager hpManager;
    [SerializeField] private float healAmount = 20f; // ���� ȸ����

    override public void UseItem()
    {
          if (hpManager == null)
          {
              hpManager = FindAnyObjectByType<HPManager>(); // �Ǵ� PlaySystemRefStorage.hpManager
          }

          hpManager.Heal(healAmount);
          //Base_Mng.Data.data.TimeItem++;
          base.UseItem();
    }
}

// ���� �ڵ�  
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

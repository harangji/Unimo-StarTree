using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ÿ�̸� ȸ�� �����ִ� �����ۿ��� HP ȸ�� �����ִ� ���������� ������.������
public class Item001_Controller : ItemController
{
    [SerializeField] private float healAmount = 20f; // ���� ȸ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerIDamageAble = GameObject.FindGameObjectWithTag("Player").GetComponent<IDamageAble>();
            
            HealEvent healEvent = new HealEvent()
            {
                Receiver = playerIDamageAble,
                Heal = healAmount,
            };
            
            CombatSystem.Instance.AddInGameEvent(healEvent);
        }
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

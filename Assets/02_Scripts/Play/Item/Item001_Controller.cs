using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ÿ�̸� ȸ�� �����ִ� �����ۿ��� HP ȸ�� �����ִ� ���������� ������.������
public class Item001_Controller : ItemController
{
    // [SerializeField] private float healAmount = 20f; // ���� ȸ����

    [SerializeField] [Range(0f, 1f)]
    private float healPercentage = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // PlayerStatManager���� �ִ� ü�� ��������
            var playerStat = other.GetComponent<PlayerStatManager>();
            if (playerStat != null)
            {
                float maxHp = playerStat.GetStat().FinalStat.Health;
                float healAmount = maxHp * healPercentage;

                HealEvent healEvent = new HealEvent()
                {
                    Receiver = playerStat,
                    Heal = healAmount,
                };

                Debug.Log("Healing ::: " + healAmount);
                CombatSystem.Instance.AddInGameEvent(healEvent);
                Base_Manager.Data.UserData.TimeItem++;
            }
            else
            {
                Debug.LogWarning("[Item001_Controller] ::: PlayerStatManager�� ã�� �� �����ϴ�.");
            }
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

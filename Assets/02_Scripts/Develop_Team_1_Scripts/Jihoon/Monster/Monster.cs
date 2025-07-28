using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Monster : MonoBehaviour, IDamageAble
{
    [SerializeField] private Collider mainCollider;
    public Collider MainCollider => mainCollider;
    public GameObject GameObject => gameObject;
    
    [SerializeField] private int defaultDamage;
    [HideInInspector] public int appliedDamage;
    
    public int[] skillDamages;
    
    //todo 나중에 여기에 IDamageable 인터페이스 붙이고 메서드 추가하면 됨
    private void Start()
    {
        CombatSystem.Instance.RegisterMonster(this);
        appliedDamage = CalculateFinalDamage(defaultDamage);

        for (int i = 0; i < skillDamages.Length; i++)
        {
            skillDamages[i] = CalculateFinalDamage(skillDamages[i]);
        }
    }

    /// <summary>
    /// 스테이지에 따라 증가하는 보정값을 반영해 최종 데미지를 계산한다.
    /// </summary>
    /// <returns>보정이 적용된 최종 데미지 (정수)</returns>
    public int CalculateFinalDamage(int baseDamage)
    {
        int stageTier = StageLoader.CurrentStageNumber / 5;
        float multiplier = 1f + 0.05f * stageTier;
        return Mathf.RoundToInt(baseDamage * multiplier);   
    }
    
    public void TakeDamage(CombatEvent combatEvent)
    {
        throw new System.NotImplementedException();
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        throw new System.NotImplementedException();
    }
}
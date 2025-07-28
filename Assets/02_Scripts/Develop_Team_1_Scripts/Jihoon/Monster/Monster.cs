using System;
using Mono.Cecil.Cil;
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

        var isBoss = GetComponent<MonsterController>().isBoss;
        
        appliedDamage = isBoss ? CalculateFinalDamage(defaultDamage, 10, 0.1f) : CalculateFinalDamage(defaultDamage, 5, 0.05f);

        for (int i = 0; i < skillDamages.Length; i++)
        {
            skillDamages[i] = isBoss ? CalculateFinalDamage(skillDamages[i], 10, 0.1f) : CalculateFinalDamage(skillDamages[i], 5, 0.05f);    
        }
    }

    /// <summary>
    /// 스테이지에 따라 증가하는 보정값을 반영해 최종 데미지를 계산한다.
    /// </summary>
    /// <returns>보정이 적용된 최종 데미지 (정수)</returns>
    private int CalculateFinalDamage(int baseDamage, int boss, float growthRate)
    {
        int stageTier = (StageLoader.CurrentStageNumber - 1) / boss;
        
        float multiplier = 1f + growthRate * stageTier;
        
        return (int)(baseDamage * multiplier + 0.5f);
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
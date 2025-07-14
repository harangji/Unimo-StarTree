using System;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageAble
{
    public int stage = 1;
    
    public int defaultDamage = 1;

    //todo 나중에 여기에 IDamageable 인터페이스 붙이고 메서드 추가하면 됨
    [SerializeField] private Collider mainCollider;
    public Collider MainCollider => mainCollider;
    public GameObject GameObject => gameObject;

    private void Start()
    {
        CombatSystem.Instance.RegisterMonster(this);
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
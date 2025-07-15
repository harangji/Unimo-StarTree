using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageAble
{
    public Collider MainCollider { get; }
    public GameObject GameObject { get; }

    public void TakeDamage(CombatEvent combatEvent);
    public void TakeHeal(HealEvent combatEvent);
}